using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System.Windows.Forms;
using Flow;

namespace Visualizer
{
    public class VisualizerScreen
    {
        public void ShowDiagram(List<FlowNode> nodes, List<FlowConnection> connections)
        {
            // Cria um novo grafo Msagl
            var graph = new Graph();

            // Adiciona os nós ao grafo
            foreach (var node in nodes)
            {
                var msaglNode = graph.AddNode(node.Name);
                msaglNode.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Box; // Qualifique o namespace para Shape
                msaglNode.LabelText = node.Name; // Use LabelText para o texto do rótulo
            }

            // Adiciona as conexões ao grafo
            foreach (var connection in connections)
            {
                var sourceNode = graph.FindNode(connection.Source.Name);
                var targetNode = graph.FindNode(connection.Target.Name);
                if (sourceNode != null && targetNode != null)
                {
                    graph.AddEdge(sourceNode.Id, targetNode.Id); // Use os IDs dos nós
                }
            }

            // Cria um visualizador Msagl
            var viewer = new GViewer();
            viewer.Graph = graph;

            // Configurações de layout
            viewer.Graph.Attr.NodeSeparation = 10;

            // Exibe o visualizador
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form { Controls = { viewer } });
        }
    }
}
