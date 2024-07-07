using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LowCodeCSharp
{
    public enum FlowNodeType
    {
        // Estrutura básica
        Class, // Representa uma classe
        Method, // Representa um método (função)

        // Fluxo de controle
        If, // Representa uma condição "if"
        Else, // Representa uma condição "else"
        For, // Representa um loop "for"
        While, // Representa um loop "while"
        DoWhile, // Representa um loop "do-while"
        Switch, // Representa uma estrutura "switch"
        Case, // Representa um caso dentro de um "switch"

        // Operações
        Assignment, // Representa uma atribuição de valor
        Declaration, // Representa a declaração de uma variável
        Call, // Representa uma chamada de método
        Return, // Representa uma instrução "return"

        // Entrada e Saída
        Input, // Representa uma entrada de dados (ex: leitura do usuário)
        Output, // Representa uma saída de dados (ex: escrita na tela)

        // Outros
        Comment, // Representa um comentário

        // ... outros tipos de nós conforme necessário
    }
}
