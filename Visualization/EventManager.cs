public class EventManager
{
    private readonly Visualizer visualizer;
    private readonly Diagram diagram;

    public EventManager(Visualizer visualizer, Diagram diagram)
    {
        this.visualizer = visualizer;
        this.diagram = diagram;

        // Registrar eventos do diagrama
        diagram.OnElementSelected += OnElementSelected;
        diagram.OnZoomChanged += OnZoomChanged;
        diagram.OnFilterChanged += OnFilterChanged;
        diagram.OnSearchTextChanged += OnSearchTextChanged;
    }

    private void OnElementSelected(string elementId)
    {
        visualizer.SelectElement(diagram, elementId);
    }

    private void OnZoomChanged(double zoomLevel)
    {
        visualizer.ZoomDiagram(zoomLevel);
    }

    private void OnFilterChanged(string filterType)
    {
        visualizer.FilterDiagram(filterType);
    }

    private void OnSearchTextChanged(string searchTerm)
    {
        visualizer.SearchDiagram(searchTerm);
    }
}
