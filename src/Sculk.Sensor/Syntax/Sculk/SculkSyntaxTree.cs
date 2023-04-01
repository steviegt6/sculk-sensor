namespace Sculk.Sensor.Syntax.Sculk;

/// <summary>
///     The syntax tree for Sculk.
/// </summary>
public sealed class SculkSyntaxTree : SyntaxTree<SculkNodeType> {
    public override AstNode<SculkNodeType> Root { get; }

    public SculkSyntaxTree(AstNode<SculkNodeType> root) {
        Root = root;
    }
}
