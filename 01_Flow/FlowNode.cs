using System.Collections.Generic;

namespace LowCodeCSharp.Flow
{
    public class FlowNode
    {
        public string Name { get; set; }
        public FlowNodeType Type { get; set; }
        public string Code { get; set; }
        public List<FlowConnection> Connections { get; set; } = new List<FlowConnection>();

        public FlowNode(string name, FlowNodeType type, string code)
        {
            Name = name;
            Type = type;
            Code = code;
        }
    }

    public enum FlowNodeType
    {
        Class,
        Method,
        If,
        Else,
        For,
        While,
        DoWhile,
        Switch,
        Case,
        Assignment,
        Declaration,
        Call,
        Return,
        Input,
        Output,
        Comment
    }

    public class FlowConnection
    {
        public FlowNode Source { get; set; }
        public FlowNode Target { get; set; }

        public FlowConnection(FlowNode source, FlowNode target)
        {
            Source = source;
            Target = target;
        }
    }
}
