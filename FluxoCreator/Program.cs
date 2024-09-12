namespace LowCodeCSharpConsole
{
    class Program
    {
        //Metodo Main
        static void Main(string[] args)
        {
            // Caminho do arquivo de código-fonte
            string filePath = "D:\\Documentos\\LowCodeCSharp\\LowCodeCSharp.cs"; // Substitua pelo caminho do seu arquivo

            // Chama o método AnalyzeCode da classe de comunicação
            string diagramData = LowCodeCSharp.LowCodeCSharpCommunication.AnalyzeCode(filePath);

            // Imprime os dados do diagrama no console
            Console.WriteLine(diagramData);

            Console.ReadKey();
        }
    }
}
