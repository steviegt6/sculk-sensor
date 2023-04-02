namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands.Arguments.Minecraft;

public enum TargetSelector {
    /// <summary>
    ///     Nearest player.
    /// </summary>
    P,

    /// <summary>
    ///     Random player.
    /// </summary>
    R,

    /// <summary>
    ///     All players.
    /// </summary>
    A,

    /// <summary>
    ///     All entities.
    /// </summary>
    E,

    /// <summary>
    ///     Entity executing the command.
    /// </summary>
    S,

    // C,
    // V,
    // Initiator,
}

public static class TargetSelectorUtil {
    public static bool TryParse(string arg, out TargetSelector selector) {
        switch (arg) {
            case "@p":
                selector = TargetSelector.P;
                return true;

            case "@r":
                selector = TargetSelector.R;
                return true;

            case "@a":
                selector = TargetSelector.A;
                return true;

            case "@e":
                selector = TargetSelector.E;
                return true;

            case "@s":
                selector = TargetSelector.S;
                return true;
        }

        selector = default;
        return false;
    }
}
