using System.Collections.Generic;

namespace Sculk.Sensor.Syntax;

/// <summary>
///     Represents a node in an abstract syntax tree.
/// </summary>
/// <typeparam name="TNodeType">The node type, denoting a language.</typeparam>
public abstract class AstNode<TNodeType> {
    /// <summary>
    ///     Gets the parent of this node, or <c>null</c> if this node is the
    ///     root.
    /// </summary>
    public AstNode<TNodeType>? Parent { get; set; }

    /// <summary>
    ///     Gets the type of this node.
    /// </summary>
    public abstract TNodeType NodeType { get; }

    /// <summary>
    ///     Gets the children of this node.
    /// </summary>
    public abstract List<AstNode<TNodeType>> Children { get; }
}
