namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

public enum AngleNotation {
    Absolute,

    // ~
    Relative,
}

public readonly record struct Angle(float Degrees, AngleNotation Notation);

public sealed class MinecraftAngle : IArgument<Angle> {
    public Angle Value { get; }

    public MinecraftAngle(float value, AngleNotation notation) {
        Value = new Angle(value, notation);
    }

    public static IArgument<Angle> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        var angleStr = args[index++];
        var notation = AngleNotation.Absolute;

        if (angleStr.StartsWith("~")) {
            angleStr = angleStr["~".Length..];
            notation = AngleNotation.Relative;
        }

        var result = float.TryParse(angleStr, out var angle);
        if (!result)
            throw new System.ArgumentException($"Expected an angle, but got {args[index - 1]}.");

        return new MinecraftAngle(angle, notation);
    }
}
