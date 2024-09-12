using System.Collections.Generic;
using System.Linq;

namespace Flow
{
    public class FlowNode
    {
        public string Name { get; set; }
        public FlowNodeType Type { get; set; }
        public string Code { get; set; }
        public int LineNumber { get; set; }
        public List<FlowConnection> Connections { get; set; } = new List<FlowConnection>();

        public FlowNode(string name, FlowNodeType type, string code, int lineNumber)
        {
            Name = name;
            Type = type;
            Code = code;
            LineNumber = lineNumber;
        }

        // Método para adicionar uma conexão ao nó
        public void AddConnection(FlowConnection connection)
        {
            Connections.Add(connection);
        }

        // Método para obter as conexões de saída do nó
        public IEnumerable<FlowConnection> GetOutputConnections()
        {
            return Connections.Where(c => c.Source == this);
        }

        // Método para obter as conexões de entrada do nó
        public IEnumerable<FlowConnection> GetInputConnections()
        {
            return Connections.Where(c => c.Target == this);
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
