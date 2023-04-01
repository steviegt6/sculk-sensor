using System.Text;

namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Brigaider;

public sealed class BrigaiderString : IArgument<string> {
    public string Value { get; }

    public BrigaiderString(string value) {
        Value = value;
    }

    public static IArgument<string> Parse(string[] args, ref int index) {
        // We know there will be at least one argument.
        ArgConditions.AssertArgumentCount(args, index + 1);

        var start = index;
        var value = args[index++];

        // If this is a standalone, unquoted word.
        if (!value.StartsWith("\""))
            return new BrigaiderString(value);

        var sb = new StringBuilder();
        sb.Append(value["\"".Length..]);

        while (true) {
            ArgConditions.AssertStringArgumentCount(args, start, index + 1);
            value = args[index++];

            // Make sure it ends with just " and not \" (escaped quote).
            if (!value.EndsWith("\\\"") && value.EndsWith("\"")) {
                sb.Append(' ');
                sb.Append(value[..^"\"".Length]);
                break;
            }

            sb.Append(' ');
            sb.Append(value);
        }

        return new BrigaiderString(sb.ToString());
    }
}
