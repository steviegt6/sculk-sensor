using System;

namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

// Flags is just used for convenience in specifying allowed notations.
[Flags]
public enum CoordinateNotation {
    Absolute = 0b0001,
    Relative = 0b0010, // ~
    Local = 0b0100, // ^
}

internal static class NotationUtil {
    public static string HandleNotation(
        string arg,
        out CoordinateNotation notation,
        CoordinateNotation allowedNotations = CoordinateNotation.Absolute
                                            | CoordinateNotation.Relative
                                            | CoordinateNotation.Local
    ) {
        if (
            arg.StartsWith("~")
         && allowedNotations.HasFlag(CoordinateNotation.Relative)
        ) {
            arg = arg["~".Length..];
            notation = CoordinateNotation.Relative;
        }
        else if (
            arg.StartsWith("^")
         && allowedNotations.HasFlag(CoordinateNotation.Local)
        ) {
            arg = arg["^".Length..];
            notation = CoordinateNotation.Local;
        }
        else {
            notation = CoordinateNotation.Absolute;
        }

        return arg;
    }
}
