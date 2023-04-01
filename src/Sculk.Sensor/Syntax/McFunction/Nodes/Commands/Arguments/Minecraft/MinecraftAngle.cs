namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

public sealed class MinecraftAngle : IArgument<Coordinate<float>> {
    public Coordinate<float> Value { get; }

    public MinecraftAngle(float value, CoordinateNotation notation) {
        Value = new Coordinate<float>(value, notation);
    }

    public static IArgument<Coordinate<float>> Parse(
        string[] args,
        ref int index
    ) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        var angleStr = args[index++];
        var notation = CoordinateNotation.Absolute;

        if (angleStr.StartsWith("~")) {
            angleStr = angleStr["~".Length..];
            notation = CoordinateNotation.Relative;
        }

        var result = float.TryParse(angleStr, out var angle);
        if (!result)
            throw new System.ArgumentException($"Expected an angle, but got {args[index - 1]}.");

        return new MinecraftAngle(angle, notation);
    }
}
