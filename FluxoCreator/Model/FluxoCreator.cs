using Flow;

namespace LowCodeCSharp
{
    public class LowCodeCSharp
    {
        // Método para gerar as conexões de fluxo
        public static List<FlowConnection> GenerateFlowConnections(List<FlowNode> flowNodes)
        {
            List<FlowConnection> flowConnections = new List<FlowConnection>();

            // Encontra os nós de classe e método
            var classNodes = flowNodes.Where(n => n.Type == FlowNodeType.Class).ToList();
            var methodNodes = flowNodes.Where(n => n.Type == FlowNodeType.Method).ToList();

            // Cria conexões entre classes e métodos
            foreach (var classNode in classNodes)
            {
                foreach (var methodNode in methodNodes)
                {
                    // Verifica se o método pertence à classe
                    if (methodNode.Code.Contains(classNode.Name))
                    {
                        flowConnections.Add(new FlowConnection(classNode, methodNode));
                    }
                }
            }

            // Encontra os nós "if" e "then"
            var ifNodes = flowNodes.Where(n => n.Type == FlowNodeType.If).ToList();
            var thenNodes = flowNodes.Where(n => n.Type == FlowNodeType.Else).ToList();

            // Cria conexões entre "if" e "then"
            foreach (var ifNode in ifNodes)
            {
                foreach (var thenNode in thenNodes)
                {
                    // Verifica se o bloco "then" está dentro do bloco "if"
                    if (thenNode.Code.Contains(ifNode.Code))
                    {
                        flowConnections.Add(new FlowConnection(ifNode, thenNode));
                    }
                }
            }

            // Encontra os nós "if" e "else"
            var elseNodes = flowNodes.Where(n => n.Type == FlowNodeType.Else).ToList();

            // Cria conexões entre "if" e "else"
            foreach (var ifNode in ifNodes)
            {
                foreach (var elseNode in elseNodes)
                {
                    // Verifica se o bloco "else" está dentro do bloco "if"
                    if (elseNode.Code.Contains(ifNode.Code))
                    {
                        flowConnections.Add(new FlowConnection(ifNode, elseNode));
                    }
                }
            }

            // Encontra os nós "for" e "forBody"
            var forNodes = flowNodes.Where(n => n.Type == FlowNodeType.For).ToList();
            var forBodyNodes = flowNodes.Where(n => n.Type == FlowNodeType.Else).ToList();

            // Cria conexões entre "for" e "forBody"
            foreach (var forNode in forNodes)
            {
                foreach (var forBodyNode in forBodyNodes)
                {
                    // Verifica se o bloco "forBody" está dentro do bloco "for"
                    if (forBodyNode.Code.Contains(forNode.Code))
                    {
                        flowConnections.Add(new FlowConnection(forNode, forBodyNode));
                    }
                }
            }

            // Encontra os nós "while" e "whileBody"
            var whileNodes = flowNodes.Where(n => n.Type == FlowNodeType.While).ToList();
            var whileBodyNodes = flowNodes.Where(n => n.Type == FlowNodeType.Else).ToList();

            // Cria conexões entre "while" e "whileBody"
            foreach (var whileNode in whileNodes)
            {
                foreach (var whileBodyNode in whileBodyNodes)
                {
                    // Verifica se o bloco "whileBody" está dentro do bloco "while"
                    if (whileBodyNode.Code.Contains(whileNode.Code))
                    {
                        flowConnections.Add(new FlowConnection(whileNode, whileBodyNode));
                    }
                }
            }

            // Encontra os nós "doWhile" e "doWhileBody"
            var doWhileNodes = flowNodes.Where(n => n.Type == FlowNodeType.DoWhile).ToList();
            var doWhileBodyNodes = flowNodes.Where(n => n.Type == FlowNodeType.Else).ToList();

            // Cria conexões entre "doWhile" e "doWhileBody"
            foreach (var doWhileNode in doWhileNodes)
            {
                foreach (var doWhileBodyNode in doWhileBodyNodes)
                {
                    // Verifica se o bloco "doWhileBody" está dentro do bloco "doWhile"
                    if (doWhileBodyNode.Code.Contains(doWhileNode.Code))
                    {
                        flowConnections.Add(new FlowConnection(doWhileNode, doWhileBodyNode));
                    }
                }
            }

            // Encontra os nós "switch" e "case"
            var switchNodes = flowNodes.Where(n => n.Type == FlowNodeType.Switch).ToList();
            var caseNodes = flowNodes.Where(n => n.Type == FlowNodeType.Case).ToList();

            // Cria conexões entre "switch" e "case"
            foreach (var switchNode in switchNodes)
            {
                foreach (var caseNode in caseNodes)
                {
                    // Verifica se o bloco "case" está dentro do bloco "switch"
                    if (caseNode.Code.Contains(switchNode.Code))
                    {
                        flowConnections.Add(new FlowConnection(switchNode, caseNode));
                    }
                }
            }

            // Encontra os nós "case" e "statement"
            var statementNodes = flowNodes.Where(n => n.Type == FlowNodeType.Else).ToList();

            // Cria conexões entre "case" e "statement"
            foreach (var caseNode in caseNodes)
            {
                foreach (var statementNode in statementNodes)
                {
                    // Verifica se o bloco "statement" está dentro do bloco "case"
                    if (statementNode.Code.Contains(caseNode.Code))
                    {
                        flowConnections.Add(new FlowConnection(caseNode, statementNode));
                    }
                }
            }

            return flowConnections;
        }
    }
}
