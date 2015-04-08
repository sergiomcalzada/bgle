namespace bgle.CQRS.Query
{
    public interface IPagerQuery : IQuery
    {
        int PageNumber { get; set; }

        int PageSize { get; set; }
    }
}