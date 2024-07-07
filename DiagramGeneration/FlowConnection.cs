using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LowCodeCSharp
{
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


/*
Exemplos de Uso

// Create two FlowNode instances
FlowNode classNode = new FlowNode("Person", FlowNodeType.Class);
FlowNode methodNode = new FlowNode("CalculateAge", FlowNodeType.Method);

// Create a FlowConnection between them
FlowConnection connection = new FlowConnection(classNode, methodNode);

*/