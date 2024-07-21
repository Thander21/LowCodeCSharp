using System;
using System.Collections.Generic;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.GraphViewerGdi;
using System.Windows.Forms;
using LowCodeCSharp.Flow;

namespace LowCodeCSharp.Diagram
{
    public class DiagramGenerator
    {
        private readonly List<FlowNode> _nodes;
        private readonly List<FlowConnection> _connections;

        public DiagramGenerator(List<FlowNode> nodes, List<FlowConnection> connections)
        {
            _nodes = nodes;
            _connections = connections;
        }

        public void GenerateDiagram()
        {
            // Cria um novo grafo Msagl
            var graph = new Graph();

            // Adiciona os nós ao grafo
            foreach (var node in _nodes)
            {
                var msaglNode = graph.AddNode(node.Name);
                msaglNode.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Box; // Qualifique o namespace para Shape
                msaglNode.LabelText = node.Name;
            }

            // Adiciona as conexões ao grafo
            foreach (var connection in _connections)
            {
                var sourceNode = graph.FindNode(connection.Source.Name);
                var targetNode = graph.FindNode(connection.Target.Name);
                graph.AddEdge(sourceNode.Id, targetNode.Id); // Use os IDs dos nós
            }

            // Cria um visualizador Msagl
            var viewer = new GViewer();
            viewer.Graph = graph;

            // Configuração do layout (omitido EdgeRoutingSettings se não estiver disponível)
            // viewer.Graph.Attr.EdgeRoutingSettings.EdgeRoutingMode = Microsoft.Msagl.Routing.EdgeRoutingMode.Spline; // Corrigido

            viewer.Graph.LayoutAlgorithmSettings.NodeSeparation = 10;

            // Exibe o visualizador
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form { Controls = { viewer } });
        }
    }
}
