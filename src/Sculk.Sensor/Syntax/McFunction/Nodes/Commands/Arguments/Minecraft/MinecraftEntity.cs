using System;
using System.Collections.Generic;
using Uuids;

namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

public enum EntityType {
    PlayerName,
    TargetSelector,
    Uuid,
}

public readonly record struct Entity(
    EntityType Type,
    string? PlayerName,
    TargetSelector? Selector,
    Dictionary<string, object>? Tags,
    Uuid? Uuid
);

public sealed class MinecraftEntity : IArgument<Entity> {
    public Entity Value { get; }

    public MinecraftEntity(Entity value) {
        Value = value;
    }

    public static IArgument<Entity> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        var arg = args[index++];

        if (arg.StartsWith('@')) {
            var selectorStr = arg[..2];
            var result = TargetSelectorUtil.TryParse(
                selectorStr,
                out var selector
            );

            if (!result)
                throw new ArgumentException($"Expected a target selector, but got {selectorStr}.");

            Dictionary<string, object>? tags = null;

            if (arg[3] == '[') {
                // TODO: Parse tags
                tags = new Dictionary<string, object>();
            }

            return new MinecraftEntity(
                new Entity(
                    Type: EntityType.TargetSelector,
                    PlayerName: null,
                    Selector: selector,
                    Tags: tags,
                    Uuid: null
                )
            );
        }
        else 
    }
}
