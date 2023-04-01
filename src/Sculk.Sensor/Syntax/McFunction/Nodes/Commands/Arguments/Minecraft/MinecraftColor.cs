namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

public enum TeamColor {
    Black,
    DarkBlue,
    DarkGreen,
    DarkAqua,
    DarkRed,
    DarkPurple,
    Gold,
    Gray,
    DarkGray,
    Blue,
    Green,
    Aqua,
    Red,
    LightPurple,
    Yellow,
    White,
}

public sealed class MinecraftColor : IArgument<TeamColor> {
    public MinecraftColor(TeamColor value) {
        Value = value;
    }

    public TeamColor Value { get; }

    public static IArgument<TeamColor> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        var colorStr = args[index++];
        var teamColor = colorStr switch {
            "black" => TeamColor.Black,
            "dark_blue" => TeamColor.DarkBlue,
            "dark_green" => TeamColor.DarkGreen,
            "dark_aqua" => TeamColor.DarkAqua,
            "dark_red" => TeamColor.DarkRed,
            "dark_purple" => TeamColor.DarkPurple,
            "gold" => TeamColor.Gold,
            "gray" => TeamColor.Gray,
            "dark_gray" => TeamColor.DarkGray,
            "blue" => TeamColor.Blue,
            "green" => TeamColor.Green,
            "aqua" => TeamColor.Aqua,
            "red" => TeamColor.Red,
            "light_purple" => TeamColor.LightPurple,
            "yellow" => TeamColor.Yellow,
            "white" => TeamColor.White,
            _ => throw new System.ArgumentException($"Expected a team color, but got {args[index - 1]}."),
        };

        return new MinecraftColor(teamColor);
    }
}
