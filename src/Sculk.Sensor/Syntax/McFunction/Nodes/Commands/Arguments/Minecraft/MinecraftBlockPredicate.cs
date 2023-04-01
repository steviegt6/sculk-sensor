namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

// TODO: Actually parse block states and data tags.
public readonly record struct BlockPredicate(
    ResourceLocation BlockId,
    string? BlockStates,
    string? DataTags
);

public sealed class MinecraftBlockPredicate : IArgument<BlockPredicate> {
    public BlockPredicate Value { get; }
    
    public MinecraftBlockPredicate(
        ResourceLocation blockId,
        string? blockStates,
        string? dataTags) {
        Value = new BlockPredicate(blockId, blockStates, dataTags);
    }

    public static IArgument<BlockPredicate> Parse(string[] args, ref int index) {
        
    }
}
