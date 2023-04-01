namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

public readonly record struct ColumnPosition(
    Coordinate<int> X,
    Coordinate<int> Z
);

public sealed class MinecraftColumnPos : IArgument<ColumnPosition> {
    public ColumnPosition Value { get; }

    public MinecraftColumnPos(Coordinate<int> x, Coordinate<int> z) {
        Value = new ColumnPosition(x, z);
    }

    public static IArgument<ColumnPosition> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 2);
        var x = ParseCoordinate(args[index++]);
        var z = ParseCoordinate(args[index++]);
        return new MinecraftColumnPos(x, z);
    }

    private static Coordinate<int> ParseCoordinate(string arg) {
        arg = NotationUtil.HandleNotation(
            arg,
            out var notation,
            CoordinateNotation.Absolute | CoordinateNotation.Relative
        );

        var result = int.TryParse(arg, out var value);
        if (!result)
            throw new System.ArgumentException($"Expected a coordinate, but got {arg}.");

        return new Coordinate<int>(value, notation);
    }
}
