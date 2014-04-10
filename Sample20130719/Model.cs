using System.Collections.Generic;

namespace Sample20130719
{
    class Model
    {
        public IEnumerable<int> Numbers { get; set; }

        public Model()
        {
            Numbers = new[] { 1, 2, 3, 4 };
        }
    }
}