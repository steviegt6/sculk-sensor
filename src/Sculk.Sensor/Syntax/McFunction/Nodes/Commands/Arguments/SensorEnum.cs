using System;

namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments;

public sealed class SensorEnum<TEnum> : IArgument<TEnum>
    where TEnum : struct {
    public SensorEnum(TEnum value) {
        Value = value;
    }

    public TEnum Value { get; }

    public static IArgument<TEnum> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        var result = Enum.TryParse<TEnum>(
            args[index++],
            ignoreCase: true,
            out var value
        );
        if (!result)
            throw new System.ArgumentException($"Expected a {typeof(TEnum).Name}, but got {args[index - 1]}.");

        return new SensorEnum<TEnum>(value);
    }
}
