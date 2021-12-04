public static class Extensions
{
    public static IEnumerable<IEnumerable<T>> SlidingWindow<T>(this IEnumerable<T> source, int windowSize)
    {
        using var src = source.GetEnumerator();
        var buffer = new List<T>(windowSize);

        for(int i = 0; i < windowSize; i++)
        {
            if (!src.MoveNext())
                yield return buffer;
            buffer.Add(src.Current);
        }

        while (src.MoveNext())
        {
            buffer.RemoveAt(0);
            buffer.Add(src.Current);

            yield return buffer;
        }

        yield break;
    }

    public static string Stringify(this IEnumerable<char> self) => string.Join(null, self);
    public static TResult Apply<TSource, TResult>(this TSource self, Func<TSource, TResult> func) =>
        func(self);

    public static IEnumerable<IEnumerable<string>> LineChunks(this IEnumerable<string> lines)
    {
        using var src = lines.GetEnumerator();
        List<string> chunks = new();

        while (src.MoveNext())
        {
            var line = src.Current;
            if (line.Length > 0)
            {
                chunks.Add(line);
                continue;
            }

            yield return chunks;
            chunks.Clear();
        }

        if (chunks.Any())
            yield return chunks;

        yield break;
    }

    public static IEnumerable<(TParam param, IEnumerable<TSource> intermediate)> ParamTransform<TSource, TParam>(this IEnumerable<TSource> self
        , IEnumerable<TParam> parameters
        , Func<TSource, TParam, TSource> func
    )
    {
        using var paramSrc = parameters.GetEnumerator();

        var src = self;

        while (paramSrc.MoveNext())
        {
            src = src.Select(x => func(x, paramSrc.Current)).ToArray();
            yield return (paramSrc.Current, src);
        }

        yield break;
    }
}