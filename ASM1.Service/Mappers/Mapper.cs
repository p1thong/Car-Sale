using System.Reflection;

namespace ASM1.Service.Mappers
{
    /// <summary>
    /// Simple reflection-based mapper
    /// </summary>
    public class Mapper : IMapper
    {
        public TDestination Map<TSource, TDestination>(TSource source)
            where TDestination : class, new()
        {
            if (source == null) return null;

            var destination = new TDestination();
            var sourceType = typeof(TSource);
            var destinationType = typeof(TDestination);

            var sourceProperties = sourceType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destProperties = destinationType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanWrite)
                .ToDictionary(p => p.Name, p => p);

            foreach (var sourceProp in sourceProperties)
            {
                if (destProperties.TryGetValue(sourceProp.Name, out var destProp))
                {
                    if (sourceProp.PropertyType == destProp.PropertyType ||
                        destProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                    {
                        try
                        {
                            var value = sourceProp.GetValue(source);
                            destProp.SetValue(destination, value);
                        }
                        catch
                        {
                            // Skip properties that can't be mapped
                        }
                    }
                }
            }

            return destination;
        }

        public IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> sources)
            where TDestination : class, new()
        {
            return sources?.Select(source => Map<TSource, TDestination>(source))
                          .Where(dest => dest != null) ?? Enumerable.Empty<TDestination>();
        }
    }
}