using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LowCodeCSharp.Flow;

namespace LowCodeCSharp.Analyzer
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
            CompilationUnitSyntax root = syntaxTree.GetRoot() as CompilationUnitSyntax;

            // Itera pelas declarações de classe
            foreach (ClassDeclarationSyntax classDeclaration in root.DescendantNodes().OfType<ClassDeclarationSyntax>())
            {
                // Cria um nó de fluxo para a classe
                FlowNode classNode = new FlowNode(classDeclaration.Identifier.Text, FlowNodeType.Class, classDeclaration.ToString());
                flowNodes.Add(classNode);

                // Itera pelos métodos da classe
                foreach (MethodDeclarationSyntax methodDeclaration in classDeclaration.Members.OfType<MethodDeclarationSyntax>())
                {
                    // Cria um nó de fluxo para o método
                    FlowNode methodNode = new FlowNode(methodDeclaration.Identifier.Text, FlowNodeType.Method, methodDeclaration.ToString());
                    flowNodes.Add(methodNode);

                    // Cria uma conexão entre a classe e o método
                    flowConnections.Add(new FlowConnection(classNode, methodNode));
                }
            }

            // Itera pelas declarações de métodos (fora de classes)
            foreach (MethodDeclarationSyntax methodDeclaration in root.DescendantNodes().OfType<MethodDeclarationSyntax>())
            {
                // Cria um nó de fluxo para o método
                FlowNode methodNode = new FlowNode(methodDeclaration.Identifier.Text, FlowNodeType.Method, methodDeclaration.ToString());
                flowNodes.Add(methodNode);
            }

            // Itera pelas declarações "if"
            foreach (IfStatementSyntax ifStatement in root.DescendantNodes().OfType<IfStatementSyntax>())
            {
                // Cria um nó de fluxo para a condição "if"
                FlowNode ifNode = new FlowNode("if", FlowNodeType.If, ifStatement.Condition.ToString());
                flowNodes.Add(ifNode);

                // Cria um nó de fluxo para o bloco "then"
                FlowNode thenNode = new FlowNode("then", FlowNodeType.Else, ifStatement.Statement.ToString());
                flowNodes.Add(thenNode);

                // Cria uma conexão entre o nó "if" e o nó "then"
                flowConnections.Add(new FlowConnection(ifNode, thenNode));

                // Se houver um bloco "else"
                if (ifStatement.Else != null)
                {
                    // Cria um nó de fluxo para o bloco "else"
                    FlowNode elseNode = new FlowNode("else", FlowNodeType.Else, ifStatement.Else.Statement.ToString());
                    flowNodes.Add(elseNode);

                    // Cria uma conexão entre o nó "if" e o nó "else"
                    flowConnections.Add(new FlowConnection(ifNode, elseNode));
                }
            }

            // Itera pelas declarações "for"
            foreach (ForStatementSyntax forStatement in root.DescendantNodes().OfType<ForStatementSyntax>())
            {
                // Cria um nó de fluxo para o loop "for"
                FlowNode forNode = new FlowNode("for", FlowNodeType.For, forStatement.ToString());
                flowNodes.Add(forNode);

                // Cria um nó de fluxo para o bloco do loop "for"
                FlowNode forBodyNode = new FlowNode("forBody", FlowNodeType.Else, forStatement.Statement.ToString());
                flowNodes.Add(forBodyNode);

                // Cria uma conexão entre o nó "for" e o nó "forBody"
                flowConnections.Add(new FlowConnection(forNode, forBodyNode));
            }

            // Itera pelas declarações "while"
            foreach (WhileStatementSyntax whileStatement in root.DescendantNodes().OfType<WhileStatementSyntax>())
            {
                // Cria um nó de fluxo para o loop "while"
                FlowNode whileNode = new FlowNode("while", FlowNodeType.While, whileStatement.Condition.ToString());
                flowNodes.Add(whileNode);

                // Cria um nó de fluxo para o bloco do loop "while"
                FlowNode whileBodyNode = new FlowNode("whileBody", FlowNodeType.Else, whileStatement.Statement.ToString());
                flowNodes.Add(whileBodyNode);

                // Cria uma conexão entre o nó "while" e o nó "whileBody"
                flowConnections.Add(new FlowConnection(whileNode, whileBodyNode));
            }

            // Itera pelas declarações "do-while"
            foreach (DoStatementSyntax doWhileStatement in root.DescendantNodes().OfType<DoStatementSyntax>())
            {
                // Cria um nó de fluxo para o loop "do-while"
                FlowNode doWhileNode = new FlowNode("doWhile", FlowNodeType.DoWhile, doWhileStatement.ToString());
                flowNodes.Add(doWhileNode);

                // Cria um nó de fluxo para o bloco do loop "do-while"
                FlowNode doWhileBodyNode = new FlowNode("doWhileBody", FlowNodeType.Else, doWhileStatement.Statement.ToString());
                flowNodes.Add(doWhileBodyNode);

                // Cria uma conexão entre o nó "doWhile" e o nó "doWhileBody"
                flowConnections.Add(new FlowConnection(doWhileNode, doWhileBodyNode));

                // Cria uma conexão entre o nó "doWhileBody" e o nó "doWhile"
                flowConnections.Add(new FlowConnection(doWhileBodyNode, doWhileNode));
            }

            // Itera pelas declarações "switch"
            foreach (SwitchStatementSyntax switchStatement in root.DescendantNodes().OfType<SwitchStatementSyntax>())
            {
                // Cria um nó de fluxo para a estrutura "switch"
                FlowNode switchNode = new FlowNode("switch", FlowNodeType.Switch, switchStatement.Expression.ToString());
                flowNodes.Add(switchNode);

                // Itera pelos casos da estrutura "switch"
                foreach (SwitchSectionSyntax switchSection in switchStatement.Sections)
                {
                    // Cria um nó de fluxo para o caso
                    FlowNode caseNode = new FlowNode("case", FlowNodeType.Case, switchSection.Labels.First()?.ToString() ?? ""); // Corrigido: Verifica se Labels.First() não é nulo
                    flowNodes.Add(caseNode);

                    // Cria uma conexão entre o nó "switch" e o nó "case"
                    flowConnections.Add(new FlowConnection(switchNode, caseNode));

                    // Itera pelas declarações do caso
                    foreach (StatementSyntax statement in switchSection.Statements)
                    {
                        // Cria um nó de fluxo para a declaração
                        if (statement != null) // Check if statement is not null
                        {
                            FlowNode statementNode = new FlowNode("statement", FlowNodeType.Else, statement.ToString());
                            flowNodes.Add(statementNode);

                            // Cria uma conexão entre o nó "case" e o nó "statement"
                            flowConnections.Add(new FlowConnection(caseNode, statementNode));
                        }
                    }
                }
            }

            // Itera pelas declarações de atribuição
            foreach (AssignmentExpressionSyntax assignmentExpression in root.DescendantNodes().OfType<AssignmentExpressionSyntax>())
            {
                // Cria um nó de fluxo para a atribuição
                FlowNode assignmentNode = new FlowNode("assignment", FlowNodeType.Assignment, assignmentExpression.ToString());
                flowNodes.Add(assignmentNode);
            }

            // Itera pelas declarações de declaração de variável
            foreach (VariableDeclarationSyntax variableDeclaration in root.DescendantNodes().OfType<VariableDeclarationSyntax>())
            {
                // Cria um nó de fluxo para a declaração de variável
                FlowNode declarationNode = new FlowNode("declaration", FlowNodeType.Declaration, variableDeclaration.ToString());
                flowNodes.Add(declarationNode);
            }

            // Itera pelas declarações de chamada de método
            foreach (InvocationExpressionSyntax invocationExpression in root.DescendantNodes().OfType<InvocationExpressionSyntax>())
            {
                // Cria um nó de fluxo para a chamada de método
                FlowNode callNode = new FlowNode("call", FlowNodeType.Call, invocationExpression.ToString());
                flowNodes.Add(callNode);
            }

            // Itera pelas declarações de retorno
            foreach (ReturnStatementSyntax returnStatement in root.DescendantNodes().OfType<ReturnStatementSyntax>())
            {
                // Cria um nó de fluxo para a declaração de retorno
                FlowNode returnNode = new FlowNode("return", FlowNodeType.Return, returnStatement.ToString());
                flowNodes.Add(returnNode);
            }

            // Retorna a lista de nós de fluxo e conexões
            return flowNodes;
        }
    }
}
