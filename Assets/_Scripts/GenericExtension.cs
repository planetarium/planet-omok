using System;
using System.Collections.Generic;

namespace Omok
{
    public static class GenericExtension
    {   
        public static void DisposeAllAndClear<T>(this List<T> list) where T : IDisposable
        {
            foreach (var item in list)
            {
                item.Dispose();
            }

            list.Clear();
        }
    }
}
