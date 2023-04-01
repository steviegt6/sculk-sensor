namespace Sculk.Sensor.Config;

/// <summary>
///     Describes the project type.
/// </summary>
public enum ProjectType {
    /// <summary>
    ///     A (presumably full) directory of compiled Sculk-to-mcfunction files.
    /// </summary>
    Directory,

    /// <summary>
    ///     A single, compiled Sculk-to-mcfunction file.
    /// </summary>
    File,

    /// <summary>
    ///     An unknown project type.
    /// </summary>
    Unknown,
}

public delegate void NotifyProgress(DecompilationProgress progress);

/// <summary>
///     Settings for the decompiler.
/// </summary>
public sealed class DecompilerSettings {
    public FormattingSettings Formatting { get; init; } = new();

    public ProjectType ProjectType { get; init; } = ProjectType.Unknown;

    public string Input { get; init; } = string.Empty;

    public event NotifyProgress? OnProgress;

    public void NotifyProgress(DecompilationProgress progress) {
        OnProgress?.Invoke(progress);
    }
}
