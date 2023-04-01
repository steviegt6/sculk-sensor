namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

public sealed class MinecraftDimension : IArgument<ResourceLocation> {
    public ResourceLocation Value { get; }

    public MinecraftDimension(ResourceLocation value) {
        Value = value;
    }

    public static IArgument<ResourceLocation> Parse(
        string[] args,
        ref int index
    ) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        var dimension = args[index++];
        var result = ResourceLocation.TryParse(dimension, out var rl);
        if (!result)
            throw new System.ArgumentException($"Expected a dimension, but got {dimension}.");
        
        return new MinecraftDimension(rl);
    }
}
