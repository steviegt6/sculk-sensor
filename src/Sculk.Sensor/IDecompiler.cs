using Sculk.Sensor.Config;
using Sculk.Sensor.Syntax;

namespace Sculk.Sensor;

/// <summary>
///     Represents a decompiler capable of decompiling a compiled mcfunction
///     file or directory of files back to a source language.
/// </summary>
/// <typeparam name="TNodeType">The node type, denoting a language.</typeparam>
public interface IDecompiler<TNodeType> {
    /// <summary>
    ///     Defines settings for the decompiler.
    /// </summary>
    DecompilerSettings Settings { get; }
    
    /// <summary>
    ///     Decompiles the compiled mcfunction file or directory of files.
    /// </summary>
    /// <returns></returns>
    SyntaxTree<TNodeType> Decompile();
}
