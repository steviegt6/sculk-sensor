namespace Sculk.Sensor.Config;

/// <summary>
///     Settings for formatting the decompiled output.
/// </summary>
public sealed class FormattingSettings {
    public const int DEFAULT_TAB_SIZE = 4;

    /// <summary>
    ///     The size of a tab in spaces.
    /// </summary>
    public int TabSize { get; init; } = DEFAULT_TAB_SIZE;
}
