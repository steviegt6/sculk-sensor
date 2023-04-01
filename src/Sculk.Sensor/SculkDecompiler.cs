using Sculk.Sensor.Config;
using Sculk.Sensor.Syntax;
using Sculk.Sensor.Syntax.Sculk;

namespace Sculk.Sensor;

/// <summary>
///     The Sculk-specific implementation of
///     <see cref="IDecompiler{TNodeType}"/>.
/// </summary>
public sealed class SculkDecompiler : IDecompiler<SculkNodeType> {
    public DecompilerSettings Settings { get; }

    public SculkDecompiler(DecompilerSettings settings) {
        Settings = settings;
    }

    public SyntaxTree<SculkNodeType> Decompile() {
        throw new System.NotImplementedException();
    }
}
