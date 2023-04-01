using System;

namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments;

internal static class ArgConditions {
    public static void AssertArgumentCount(string[] args, int count) {
        if (args.Length != count)
            throw new System.ArgumentException($"Expected {count} arguments, but got {args.Length}.");
    }

    public static void AssertStringArgumentCount(string[] args, int start, int count) {
        if (args.Length == count)
            return;

        var str = string.Join(" ", args.AsSpan().Slice(start, count).ToArray());
        throw new System.ArgumentException($"Non-terminated string '{str}' at argument {start}.");
    }
}
