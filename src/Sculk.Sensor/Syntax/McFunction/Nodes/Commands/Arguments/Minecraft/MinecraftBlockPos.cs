namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

public enum CoordinateNotation {
    Absolute,

    // ~
    Relative,

    // ^
    Local,
}

public readonly record struct Coordinate(
    int Value,
    CoordinateNotation Notation
);

public readonly record struct Coordinates(
    Coordinate X,
    Coordinate Y,
    Coordinate Z
);

public sealed class MinecraftBlockPos : IArgument<Coordinates> {
    public Coordinates Value { get; }

    public MinecraftBlockPos(Coordinate x, Coordinate y, Coordinate z) {
        Value = new Coordinates(x, y, z);
    }

    public static IArgument<Coordinates> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 3);
        var x = ParseCoordinate(args[index++]);
        var y = ParseCoordinate(args[index++]);
        var z = ParseCoordinate(args[index++]);
        return new MinecraftBlockPos(x, y, z);
    }

    private static Coordinate ParseCoordinate(string arg) {
        var notation = CoordinateNotation.Absolute;

        if (arg.StartsWith("~")) {
            arg = arg["~".Length..];
            notation = CoordinateNotation.Relative;
        }
        else if (arg.StartsWith("^")) {
            arg = arg["^".Length..];
            notation = CoordinateNotation.Local;
        }

        var result = int.TryParse(arg, out var value);
        if (!result)
            throw new System.ArgumentException($"Expected a coordinate, but got {arg}.");

        return new Coordinate(value, notation);
    }
}
