using System.Collections.Generic;

namespace HoneyPot
{
    struct SelectionManager
    {
        public int Columns;

        public List<string> Values;
        
        public SelectionManager(List<string> values, int columns)
        {
            Values = values;
            Columns = columns;
        }
    }
}
