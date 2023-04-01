namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

public readonly record struct Coordinates(
    Coordinate<int> X,
    Coordinate<int> Y,
    Coordinate<int> Z
);

public sealed class MinecraftBlockPos : IArgument<Coordinates> {
    public Coordinates Value { get; }

    public MinecraftBlockPos(
        Coordinate<int> x,
        Coordinate<int> y,
        Coordinate<int> z
    ) {
        Value = new Coordinates(x, y, z);
    }

    public static IArgument<Coordinates> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 3);
        var x = ParseCoordinate(args[index++]);
        var y = ParseCoordinate(args[index++]);
        var z = ParseCoordinate(args[index++]);
        return new MinecraftBlockPos(x, y, z);
    }

    private static Coordinate<int> ParseCoordinate(string arg) {
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

        return new Coordinate<int>(value, notation);
    }
}
