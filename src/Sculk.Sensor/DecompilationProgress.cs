namespace Sculk.Sensor;

/// <summary>
///     Represents the progress of a decompilation operation.
/// </summary>
/// <param name="CurrentProgress">The current progress.</param>
/// <param name="TotalProgress">The total progress.</param>
/// <param name="Name">
///     The name of this step or overall operation, if applicable.
/// </param>
/// <param name="Description">
///     The description of this step or overall operation, if applicable.
/// </param>
public readonly record struct DecompilationProgress(
    int CurrentProgress,
    int TotalProgress,
    string? Name,
    string? Description
);
