namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Brigaider;

public sealed class BrigaiderFloat : IArgument<float> {
    public float Value { get; }

    public BrigaiderFloat(float value) {
        Value = value;
    }

    public static IArgument<float> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        var result = float.TryParse(args[index++], out var value);
        if (!result)
            throw new System.ArgumentException($"Expected a float, but got {args[index - 1]}.");

        return new BrigaiderFloat(value);
    }
}
