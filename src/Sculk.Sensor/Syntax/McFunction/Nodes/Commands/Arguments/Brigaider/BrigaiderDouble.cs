namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Brigaider;

public sealed class BrigaiderDouble : IArgument<double> {
    public double Value { get; }

    public BrigaiderDouble(double value) {
        Value = value;
    }

    public static IArgument<double> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        var result = double.TryParse(args[index++], out var value);
        if (!result)
            throw new System.ArgumentException($"Expected a double, but got {args[index - 1]}.");

        return new BrigaiderDouble(value);
    }
}
