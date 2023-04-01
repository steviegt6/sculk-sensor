using Newtonsoft.Json;

namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

public sealed class MinecraftComponent : IArgument<object> {
    public object Value { get; }

    public MinecraftComponent(object value) {
        Value = value;
    }

    public static IArgument<object> Parse(string[] args, ref int index) {
        ArgConditions.AssertArgumentCount(args, index + 1);
        var json = args[index++];
        var result = JsonConvert.DeserializeObject(json);
        if (result is null)
            throw new System.ArgumentException($"Expected a JSON object, but got {json}.");

        return new MinecraftComponent(result);
    }
}
