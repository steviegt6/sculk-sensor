namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

// TODO: Validate legal chars: a-z, 0-9, _, -, .
public readonly record struct ResourceLocation(string Namespace, string Path);
