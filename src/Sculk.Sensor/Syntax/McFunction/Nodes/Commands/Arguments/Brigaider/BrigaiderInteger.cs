namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Brigaider;

public sealed class BrigaiderInteger : IArgument<int> {
    public int Value { get; }

    public BrigaiderInteger(int value) {
        Value = value;
    }

    public static IArgument<int> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        var result = int.TryParse(args[index++], out var value);
        if (!result)
            throw new System.ArgumentException($"Expected an int, but got {args[index - 1]}.");

        return new BrigaiderInteger(value);
    }
}
