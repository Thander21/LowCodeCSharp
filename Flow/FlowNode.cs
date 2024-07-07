using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LowCodeCSharp
{
    public class FlowNode
    {
        public string Name { get; set; }
        public FlowNodeType Type { get; set; }
        public string Code { get; set; } // Adiciona a propriedade Code para armazenar o código do nó

        public FlowNode(string name, FlowNodeType type, string code = "")
        {
            Name = name;
            Type = type;
            Code = code;
        }
    }
}

/*
Exemplos de uso

// Criando um nó de classe
FlowNode classNode = new FlowNode("Pessoa", FlowNodeType.Class, @"
    public class Pessoa
    {
        // ... código da classe
    }
");

// Criando um nó de método
FlowNode methodNode = new FlowNode("CalcularIdade", FlowNodeType.Method, @"
    public int CalcularIdade(int anoNascimento)
    {
        // ... código do método
    }
");


*/