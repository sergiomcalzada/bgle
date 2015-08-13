using System.Collections.Generic;

namespace bgle.Contracts.ObjectMapper
{
    public interface IObjectMapper
    {
        TTarget Map<TSource, TTarget>(TSource source, TTarget target);

        TTarget Map<TSource, TTarget>(TSource source);
       
        IEnumerable<TTarget> Map<TSource, TTarget>(IEnumerable<TSource> source);
        
    }
}
