using System;
using System.Collections.Generic;
using System.Text;

namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

// TODO: Actually parse block states and data tags.
public readonly record struct BlockPredicate(
    ResourceLocation BlockId,
    Dictionary<string, object>? BlockStates,
    Dictionary<string, object>? DataTags
);

public sealed class MinecraftBlockPredicate : IArgument<BlockPredicate> {
    public BlockPredicate Value { get; }

    public MinecraftBlockPredicate(
        ResourceLocation blockId,
        Dictionary<string, object>? blockStates,
        Dictionary<string, object>? dataTags) {
        Value = new BlockPredicate(blockId, blockStates, dataTags);
    }

    public static IArgument<BlockPredicate> Parse(
        string[] args,
        ref int index
    ) {
        // I believe block:id[states]{tags} won't actually contain spaces that
        // aren't wrapped in quotes, so we should be fine?
        ArgConditions.AssertArgumentCount(args, index + 1);
        var blockStr = args[index++];

        ResourceLocation rl;

        var hasStates = blockStr.Contains('[');
        var hasTags = blockStr.Contains('{');

        // Make sure [ comes before {, since tags can contain [ due to strings.
        if (hasTags)
            hasStates = blockStr.IndexOf('[') < blockStr.IndexOf('{');

        // No block states or data tags.
        if (!hasStates && !hasTags) {
            var result = ResourceLocation.TryParse(blockStr, out rl);
            
            if (!result)
                throw new ArgumentException($"Expected a block, but got {blockStr}.");

            return new MinecraftBlockPredicate(
                rl,
                null,
                null
            );
        }

        // States always come before tags, so only check based on
        if (hasStates) {
            var blockId = blockStr[..blockStr.IndexOf('[')];
            var result = ResourceLocation.TryParse(blockId, out rl);
            
            if (!result)
                throw new ArgumentException($"Expected a block, but got {blockStr}.");
        }
        else if (hasTags) {
            var blockId = blockStr[..blockStr.IndexOf('{')];
            var result = ResourceLocation.TryParse(blockId, out rl);
            
            if (!result)
                throw new ArgumentException($"Expected a block, but got {blockStr}.");
        }
        else {
            throw new InvalidOperationException();
        }

        // Parse block states.
        Dictionary<string, object>? blockStates = null;

        if (hasStates) {
            blockStates = new Dictionary<string, object>();
            var endIndex = hasTags
                ? blockStr.IndexOf('{') - 2
                : blockStr.Length - 1;
            var stateStr = blockStr[(blockStr.IndexOf('[') + 1)..endIndex];
            var stateStrParts = stateStr.Split(',');

            foreach (var part in stateStrParts) {
                var partParts = part.Split('=');
                blockStates.Add(partParts[0], partParts[1]);
            }
        }

        // Parse data tags.
        Dictionary<string, object>? dataTags = null;

        if (hasTags) {
            dataTags = new Dictionary<string, object>();
            // TODO
        }

        return new MinecraftBlockPredicate(
            rl,
            blockStates,
            dataTags
        );
    }
}
