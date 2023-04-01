using System.Text;

namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Brigaider;

public sealed class BrigaiderString : IArgument<string> {
    public string Value { get; }

    public BrigaiderString(string value) {
        Value = value;
    }

    public static IArgument<string> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        return new BrigaiderString(args[index++]);
    }
}
