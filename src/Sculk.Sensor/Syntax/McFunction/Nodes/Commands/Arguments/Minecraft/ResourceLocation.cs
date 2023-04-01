namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

// TODO: Validate legal chars: a-z, 0-9, _, -, .
public readonly record struct ResourceLocation(string Namespace, string Path) {
    public static bool TryParse(string input, out ResourceLocation rl) {
        var parts = input.Split(':');

        if (parts.Length != 2) {
            rl = new ResourceLocation("minecraft", parts[0]);
            return false;
        }

        rl = new ResourceLocation(parts[0], parts[1]);
        return true;
    }
}
