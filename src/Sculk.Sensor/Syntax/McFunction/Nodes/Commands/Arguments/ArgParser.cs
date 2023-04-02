using System;
using System.Collections.Generic;
using System.Linq;

namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments;

internal static class ArgParser {
    // https://stackoverflow.com/a/298990
    public static string[] Split(string arguments) {
        var inQuotes = false;
        var escaped = false;
        return arguments.Split(
                            c => {
                                if (!escaped && c == '\"')
                                    inQuotes = !inQuotes;

                                escaped = !escaped && c == '\\';
                                return !inQuotes && c == ' ';
                            }
                        )
                        .Select(x => x.Trim().TrimMatchingQuotes('\"'))
                        .Where(x => !string.IsNullOrEmpty(x))
                        .ToArray();
    }

    private static IEnumerable<string> Split(
        this string value,
        Func<char, bool> controller
    ) {
        var nextPiece = 0;

        for (var i = 0; i < value.Length; i++) {
            if (!controller(value[i]))
                continue;

            yield return value.Substring(nextPiece, i - nextPiece);

            nextPiece = i + 1;
        }

        yield return value[nextPiece..];
    }

    private static string TrimMatchingQuotes(this string input, char quote) {
        if ((input.Length >= 2) && (input[0] == quote) && input[^1] == quote)
            return input.Substring(1, input.Length - 2);

        return input;
    }

    internal static Dictionary<string, object> ParseStates(string input) {
        
    }
}
