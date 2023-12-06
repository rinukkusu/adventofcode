using System.Collections;

namespace AdventOfCode_2023._05;

public class LongRangeEnumerable(long start, long count) : IEnumerable<long>
{
    public IEnumerator<long> GetEnumerator()
    {
        return new LongRangeEnumerator(start, count);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    private class LongRangeEnumerator(long start, long count) : IEnumerator<long>
    {
        private readonly long _end = start + count;

        public bool MoveNext()
        {
            if (Current >= _end - 1) 
                return false;
            
            Current++;
            return true;
        }

        public void Reset()
        {
            Current = start - 1;
        }

        public long Current { get; private set; } = start - 1;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            // Not needed for this implementation
        }
    }
}