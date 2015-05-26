namespace bgle.Graph.Rexpro
{
    public class GrahpQuery : IGrahpQuery
    {
        private readonly string graphObjName;

        private readonly string graphName;


        public IVertexQuery V { get; private set; }

        public IEdgesQuery E { get; private set; }

        public GrahpQuery(string graphObjName, string graphName)
        {
            this.graphObjName = graphObjName;
            this.graphName = graphName;
            this.V = new VertexQuery();
            this.E = new EdgesQuery();
        }

        public IGrahpQuery AddVertex<T>(T vertex) where T : IVertex
        {
            return this;
        }

        public IGrahpQuery AddEdge<T>(T edge) where T : IEdge
        {
            return this;
        }
    }


    public interface IGemlinQuery { }

    public interface IGrahpQuery : IGemlinQuery
    {
        IGrahpQuery AddVertex<T>(T vertex) where T : IVertex;
        IGrahpQuery AddEdge<T>(T edge) where T : IEdge;
    }

    public interface IEdge
    {
    }

    public interface IVertex
    {
    }

    public interface IVertexQuery : IGemlinQuery { }

    public interface IEdgesQuery : IGemlinQuery { }
}