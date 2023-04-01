namespace Sculk.Sensor.Syntax.McFunction.Nodes.Commands;

/// <summary>
///     Represents the arguments of a command.
/// </summary>
public interface ICommandArguments {
    /// <summary>
    ///     Converts the arguments to an array of objects.
    /// </summary>
    /// <returns>An array of objects.</returns>
    object[] ToObjectArray();
}
