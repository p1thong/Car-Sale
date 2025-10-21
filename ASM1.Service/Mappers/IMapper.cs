namespace ASM1.Service.Mappers
{
    /// <summary>
    /// Simple generic mapper interface
    /// </summary>
    public interface IMapper
    {
        /// <summary>
        /// Maps an object from source type to destination type
        /// </summary>
        TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : class, new();

        /// <summary>
        /// Maps a collection of objects
        /// </summary>
        IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> sources)
            where TDestination : class, new();
    }
}