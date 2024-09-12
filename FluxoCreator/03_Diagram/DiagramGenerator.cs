using Analyzer;
using Flow;
using LowCodeCSharp;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System.Drawing;
using System.Windows.Forms;

namespace Diagram
{
    public class VisualizerScreen : Form
    {
        private GViewer? viewer;
        private List<FlowNode>? flowNodes;
        private List<FlowConnection>? flowConnections;

        public List<FlowNode>? FlowNodes { get => flowNodes; set => flowNodes = value; }

        public VisualizerScreen(List<FlowConnection>? flowConnections)
        {
            this.flowConnections = flowConnections;
        }

        public VisualizerScreen()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // Inicializa os componentes da tela
            this.SuspendLayout();

            // Cria o visualizador do diagrama
            viewer = new GViewer();
            viewer.Dock = DockStyle.Fill;
            this.Controls.Add(viewer);

            // Define o tamanho da tela
            this.ClientSize = new Size(800, 600);

            // Define o título da tela
            this.Text = "Visualizador de Código";

            // Define o layout da tela
            this.ResumeLayout(false);
        }

        // Método para carregar o código C# e gerar o diagrama
        public void LoadCode(string filePath)
        {
            // Analisa o código e gera os nós de fluxo
            var flowNodes = AnalyzerCode.Analyze(filePath);

            // Gera as conexões de fluxo
            var flowConnections = LowCodeCSharpCommunication.GenerateFlowConnections(flowNodes);

            // Gera o diagrama de fluxo
            GenerateDiagram();
        }

        // Método para gerar o diagrama de fluxo
        private void GenerateDiagram()
        {
            // Cria um novo grafo Msagl
            var graph = new Graph();
            foreach (var (node, msaglNode) in
            // Adiciona os nós ao grafo
            from node in flowNodes
            let msaglNode = graph.AddNode(node.Name)
            select (node, msaglNode))
            {
                msaglNode.Attr.Shape = Shape.Box;
                msaglNode.LabelText = node.Name;
            }

            foreach (var (sourceNode, targetNode) in
            // Adiciona as conexões ao grafo
            from connection in flowConnections
            let sourceNode = graph.FindNode(connection.Source.Name)
            let targetNode = graph.FindNode(connection.Target.Name)
            where sourceNode != null && targetNode != null
            select (sourceNode, targetNode))
            {
                graph.AddEdge(sourceNode.Id, targetNode.Id);
            }


            // Configura o visualizador do diagrama
            if (viewer != null)
            {
                viewer.Graph = graph;
            }
        }

        internal void ShowDiagram(List<FlowNode> flowNodes, List<FlowConnection> flowConnections)
        {
            throw new NotImplementedException();
        }
    }
}
