namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Brigaider;

public sealed class BrigaiderLong : IArgument<long> {
    public long Value { get; }

    public BrigaiderLong(long value) {
        Value = value;
    }

    public static IArgument<long> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        var result = long.TryParse(args[index++], out var value);
        if (!result)
            throw new System.ArgumentException($"Expected a long, but got {args[index - 1]}.");

        return new BrigaiderLong(value);
    }
}
