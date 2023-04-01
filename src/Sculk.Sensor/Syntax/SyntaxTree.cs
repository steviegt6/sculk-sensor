namespace Sculk.Sensor.Syntax; 

/// <summary>
///     Represents a syntax tree.
/// </summary>
/// <typeparam name="TNodeType"></typeparam>
public abstract class SyntaxTree<TNodeType> {
    public abstract AstNode<TNodeType> Root { get; }
}
