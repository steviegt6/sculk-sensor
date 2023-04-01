namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Brigaider; 

public sealed class BrigaiderBool : IArgument<bool> {
    public bool Value { get; }
    
    public BrigaiderBool(bool value) {
        Value = value;
    }

    public static IArgument<bool> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        
        // Technically this is actually more lenient than what Minecraft allows.
        var result = bool.TryParse(args[index++], out var value);
        if (!result)
            throw new System.ArgumentException($"Expected a bool, but got {args[index - 1]}.");

        return new BrigaiderBool(value);
    }
}
