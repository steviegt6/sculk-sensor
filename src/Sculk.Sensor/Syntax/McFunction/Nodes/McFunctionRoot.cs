using System.Collections.Generic;

namespace Sculk.Sensor.Syntax.McFunction.Nodes;

public sealed class McFunctionRoot : AstNode<McFunctionNodeType> {
    public override McFunctionNodeType NodeType => McFunctionNodeType.Root;

    public override List<AstNode<McFunctionNodeType>> Children { get; } = new();
}
