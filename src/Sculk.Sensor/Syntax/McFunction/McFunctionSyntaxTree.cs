using System;
using Sculk.Sensor.Syntax.McFunction.Nodes;

namespace Sculk.Sensor.Syntax.McFunction;

public sealed class McFunctionSyntaxTree : SyntaxTree<McFunctionNodeType> {
    public override AstNode<McFunctionNodeType> Root { get; }

    public McFunctionSyntaxTree(AstNode<McFunctionNodeType> root) {
        Root = root;
    }

    public static McFunctionSyntaxTree Parse(string text) {
        var root = new McFunctionRoot();

        // Assume empty...
        if (string.IsNullOrEmpty(text))
            return new McFunctionSyntaxTree(root);

        var lines = text.Split(
            new[] {
                "\r\n",
                "\n",
            },
            StringSplitOptions.RemoveEmptyEntries
        );

        for (var i = 0; i < lines.Length; i++) {
            var line = lines[i].Trim();

            if (line.StartsWith("#")) {
                var comment = new McFunctionComment(line[1..]) {
                    Parent = root,
                };
                root.Children.Add(comment);
            }
            else {
                var command = ParseFunction(line);
                command.Parent = root;
                root.Children.Add(command);
            }
        }
        
        return new McFunctionSyntaxTree(root);
    }
    
    private static AstNode<McFunctionNodeType> ParseFunction(string text) {
        var command = new McFunctionCommand(text) {
            Parent = null,
        };
        return command;
    }
}
