using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
using Flow;

namespace Analyzer
{
    public class AnalyzerCode
    {
        public static List<FlowNode> Analyze(string filePath)
        {
            // Lista para armazenar os nós de fluxo
            List<FlowNode> flowNodes = new List<FlowNode>();
            List<FlowConnection> flowConnections = new List<FlowConnection>();

            // Carrega o código do arquivo
            string code = File.ReadAllText(filePath);

            // Cria um árvore de sintaxe abstrata (AST)
            SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(code);

            // Obtém a raiz da AST
            CompilationUnitSyntax? root = syntaxTree.GetRoot() as CompilationUnitSyntax;

            // Verifica se a raiz não é nula
            if (root == null)
            {
                throw new InvalidOperationException("Não foi possível obter a raiz da árvore de sintaxe.");
            }

            // Itera pelas declarações de classe
            int lineNumber = 1; // Inicializa o contador de linhas
            foreach (ClassDeclarationSyntax classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                // Cria um nó de fluxo para a classe
                FlowNode classNode = new FlowNode(classDeclaration.Identifier.Text, FlowNodeType.Class, classDeclaration.ToString(), lineNumber);
                flowNodes.Add(classNode);

                // Itera pelos métodos da classe
                foreach (MethodDeclarationSyntax methodDeclaration in classDeclaration.Members.OfType<MethodDeclarationSyntax>())
                {
                    // Cria um nó de fluxo para o método
                    FlowNode methodNode = new FlowNode(methodDeclaration.Identifier.Text, FlowNodeType.Method, methodDeclaration.ToString(), lineNumber);
                    flowNodes.Add(methodNode);

                    // Cria uma conexão entre a classe e o método
                    flowConnections.Add(new FlowConnection(classNode, methodNode));
                }

                // Atualiza o contador de linhas para a próxima linha após a declaração da classe
                lineNumber = classDeclaration.FullSpan.End + 1; // CEndLinePosition.Line
            }

            // Itera pelas declarações de métodos (fora de classes)
            foreach (MethodDeclarationSyntax methodDeclaration in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
            {
                // Cria um nó de fluxo para o método
                FlowNode methodNode = new FlowNode(methodDeclaration.Identifier.Text, FlowNodeType.Method, methodDeclaration.ToString(), lineNumber);
                flowNodes.Add(methodNode);

                // Atualiza o contador de linhas para a próxima linha após a declaração do método
                lineNumber = methodDeclaration.FullSpan.End + 1; // Corrigido: Usando EndLinePosition.Line
            }

            // Itera pelas declarações "if"
            foreach (IfStatementSyntax ifStatement in root.DescendantNodes().OfType<IfStatementSyntax>())
            {
                // Cria um nó de fluxo para a condição "if"
                FlowNode ifNode = new FlowNode("if", FlowNodeType.If, ifStatement.Condition.ToString(), lineNumber);
                flowNodes.Add(ifNode);

                // Cria um nó de fluxo para o bloco "then"
                FlowNode thenNode = new FlowNode("then", FlowNodeType.Else, ifStatement.Statement.ToString(), lineNumber);
                flowNodes.Add(thenNode);

                // Cria uma conexão entre o nó "if" e o nó "then"
                flowConnections.Add(new FlowConnection(ifNode, thenNode));

                // Se houver um bloco "else"
                if (ifStatement.Else != null)
                {
                    // Cria um nó de fluxo para o bloco "else"
                    FlowNode elseNode = new FlowNode("else", FlowNodeType.Else, ifStatement.Else.Statement.ToString(), lineNumber);
                    flowNodes.Add(elseNode);

                    // Cria uma conexão entre o nó "if" e o nó "else"
                    flowConnections.Add(new FlowConnection(ifNode, elseNode));
                }

                // Atualiza o contador de linhas para a próxima linha após a declaração "if"
                lineNumber = ifStatement.FullSpan.End + 1; // Corrigido: Usando EndLinePosition.Line
            }

            // Itera pelas declarações "for"
            foreach (ForStatementSyntax forStatement in root.DescendantNodes().OfType<ForStatementSyntax>())
            {
                // Cria um nó de fluxo para o loop "for"
                FlowNode forNode = new FlowNode("for", FlowNodeType.For, forStatement.ToString(), lineNumber);
                flowNodes.Add(forNode);

                // Cria um nó de fluxo para o bloco do loop "for"
                FlowNode forBodyNode = new FlowNode("forBody", FlowNodeType.Else, forStatement.Statement.ToString(), lineNumber);
                flowNodes.Add(forBodyNode);

                // Cria uma conexão entre o nó "for" e o nó "forBody"
                flowConnections.Add(new FlowConnection(forNode, forBodyNode));

                // Atualiza o contador de linhas para a próxima linha após a declaração "for"
                lineNumber = forStatement.FullSpan.End + 1; // Corrigido: Usando EndLinePosition.Line
            }

            // Itera pelas declarações "while"
            foreach (WhileStatementSyntax whileStatement in root.DescendantNodes().OfType<WhileStatementSyntax>())
            {
                // Cria um nó de fluxo para o loop "while"
                FlowNode whileNode = new FlowNode("while", FlowNodeType.While, whileStatement.Condition.ToString(), lineNumber);
                flowNodes.Add(whileNode);

                // Cria um nó de fluxo para o bloco do loop "while"
                FlowNode whileBodyNode = new FlowNode("whileBody", FlowNodeType.Else, whileStatement.Statement.ToString(), lineNumber);
                flowNodes.Add(whileBodyNode);

                // Cria uma conexão entre o nó "while" e o nó "whileBody"
                flowConnections.Add(new FlowConnection(whileNode, whileBodyNode));

                // Atualiza o contador de linhas para a próxima linha após a declaração "while"
                lineNumber = whileStatement.FullSpan.End + 1; // Corrigido: Usando EndLinePosition.Line
            }

            // Itera pelas declarações "do-while"
            foreach (DoStatementSyntax doWhileStatement in root.DescendantNodes().OfType<DoStatementSyntax>())
            {
                // Cria um nó de fluxo para o loop "do-while"
                FlowNode doWhileNode = new FlowNode("doWhile", FlowNodeType.DoWhile, doWhileStatement.ToString(), lineNumber);
                flowNodes.Add(doWhileNode);

                // Cria um nó de fluxo para o bloco do loop "do-while"
                FlowNode doWhileBodyNode = new FlowNode("doWhileBody", FlowNodeType.Else, doWhileStatement.Statement.ToString(), lineNumber);
                flowNodes.Add(doWhileBodyNode);

                // Cria uma conexão entre o nó "doWhile" e o nó "doWhileBody"
                flowConnections.Add(new FlowConnection(doWhileNode, doWhileBodyNode));

                // Cria uma conexão entre o nó "doWhileBody" e o nó "doWhile"
                flowConnections.Add(new FlowConnection(doWhileBodyNode, doWhileNode));

                // Atualiza o contador de linhas para a próxima linha após a declaração "do-while"
                lineNumber = doWhileStatement.FullSpan.End + 1; // Corrigido: Usando EndLinePosition.Line
            }

            // Itera pelas declarações "switch"
            foreach (SwitchStatementSyntax switchStatement in root.DescendantNodes().OfType<SwitchStatementSyntax>())
            {
                // Cria um nó de fluxo para a estrutura "switch"
                FlowNode switchNode = new FlowNode("switch", FlowNodeType.Switch, switchStatement.Expression.ToString(), lineNumber);
                flowNodes.Add(switchNode);

                // Itera pelos casos da estrutura "switch"
                foreach (SwitchSectionSyntax switchSection in switchStatement.Sections)
                {
                    // Cria um nó de fluxo para o caso
                    string caseLabel = switchSection.Labels.FirstOrDefault()?.ToString() ?? "";
                    FlowNode caseNode = new FlowNode("case", FlowNodeType.Case, caseLabel, lineNumber);
                    flowNodes.Add(caseNode);

                    // Cria uma conexão entre o nó "switch" e o nó "case"
                    flowConnections.Add(new FlowConnection(switchNode, caseNode));

                    // Itera pelas declarações do caso
                    foreach (StatementSyntax statement in switchSection.Statements)
                    {
                        // Cria um nó de fluxo para a declaração
                        if (statement != null)
                        {
                            FlowNode statementNode = new FlowNode("statement", FlowNodeType.Else, statement.ToString(), lineNumber);
                            flowNodes.Add(statementNode);

                            // Cria uma conexão entre o nó "case" e o nó "statement"
                            flowConnections.Add(new FlowConnection(caseNode, statementNode));
                        }
                    }

                    // Atualiza o contador de linhas para a próxima linha após a declaração "case"
                    lineNumber = switchSection.FullSpan.End + 1; // Corrigido: Usando EndLinePosition.Line
                }

                // Atualiza o contador de linhas para a próxima linha após a declaração "switch"
                lineNumber = switchStatement.FullSpan.End + 1; // Corrigido: Usando EndLinePosition.Line
            }

            // Itera pelas declarações de atribuição
            foreach (AssignmentExpressionSyntax assignmentExpression in root.DescendantNodes().OfType<AssignmentExpressionSyntax>())
            {
                // Cria um nó de fluxo para a atribuição
                FlowNode assignmentNode = new FlowNode("assignment", FlowNodeType.Assignment, assignmentExpression.ToString(), lineNumber);
                flowNodes.Add(assignmentNode);

                // Atualiza o contador de linhas para a próxima linha após a declaração de atribuição
                lineNumber = assignmentExpression.FullSpan.End + 1; // Corrigido: Usando EndLinePosition.Line
            }

            // Itera pelas declarações de declaração de variável
            foreach (VariableDeclarationSyntax variableDeclaration in root.DescendantNodes().OfType<VariableDeclarationSyntax>())
            {
                // Cria um nó de fluxo para a declaração de variável
                FlowNode declarationNode = new FlowNode("declaration", FlowNodeType.Declaration, variableDeclaration.ToString(), lineNumber);
                flowNodes.Add(declarationNode);

                // Atualiza o contador de linhas para a próxima linha após a declaração de variável
                lineNumber = variableDeclaration.FullSpan.End + 1; // Corrigido: Usando EndLinePosition.Line
            }

            // Itera pelas declarações de chamada de método
            foreach (InvocationExpressionSyntax invocationExpression in root.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                // Cria um nó de fluxo para a chamada de método
                FlowNode callNode = new FlowNode("call", FlowNodeType.Call, invocationExpression.ToString(), lineNumber);
                flowNodes.Add(callNode);

                // Atualiza o contador de linhas para a próxima linha após a declaração de chamada de método
                lineNumber = invocationExpression.FullSpan.End + 1; // Corrigido: Usando EndLinePosition.Line
            }

            // Itera pelas declarações de retorno
            foreach (ReturnStatementSyntax returnStatement in root.DescendantNodes().OfType<ReturnStatementSyntax>())
            {
                // Cria um nó de fluxo para a declaração de retorno
                FlowNode returnNode = new FlowNode("return", FlowNodeType.Return, returnStatement.ToString(), lineNumber);
                flowNodes.Add(returnNode);

                // Atualiza o contador de linhas para a próxima linha após a declaração de retorno
                lineNumber = returnStatement.FullSpan.End + 1; // Corrigido: Usando EndLinePosition.Line
            }

            // Retorna a lista de nós de fluxo
            return flowNodes;
        }
    }
}
