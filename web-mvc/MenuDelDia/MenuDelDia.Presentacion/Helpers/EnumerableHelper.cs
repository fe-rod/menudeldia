using System;
using System.Collections;
using System.Collections.Generic;

namespace MenuDelDia.Presentacion.Helpers
{
    public static class EnumerableHelper
    {
        public static IEnumerable<int> Range(int start, int end, Func<int, int> step = null)
        {
            if (step == null) { step = x => x + 1; }
            //check parameters
            while (start <= end)
            {
                yield return start;
                start = step(start);
            }
        }
    }
}
