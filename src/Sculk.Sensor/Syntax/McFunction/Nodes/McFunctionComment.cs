using System.Collections.Generic;

namespace Sculk.Sensor.Syntax.McFunction.Nodes; 

public sealed class McFunctionComment : AstNode<McFunctionNodeType> {
    public override McFunctionNodeType NodeType => McFunctionNodeType.Comment;

    public override List<AstNode<McFunctionNodeType>> Children { get; } = new();
    
    public string Text { get; }
    
    public McFunctionComment(string text) {
        Text = text;
    }
}
