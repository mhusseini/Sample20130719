using System;
using System.Collections.Generic;

namespace Sample20130719
{
    class Executer
    {
        public void Do<T1, T2>(T1 x, Func<T1, IEnumerable<T2>> f)
        {
            var l = f(x);

            foreach (var y in l)
            {
                Console.WriteLine(y);
            }
        }
    }
}