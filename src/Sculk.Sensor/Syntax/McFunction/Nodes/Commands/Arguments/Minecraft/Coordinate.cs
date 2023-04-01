namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

public readonly record struct Coordinate<TValue>(
    TValue Value,
    CoordinateNotation Notation
);
