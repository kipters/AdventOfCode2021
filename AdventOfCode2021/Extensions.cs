﻿public static class Extensions
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
}