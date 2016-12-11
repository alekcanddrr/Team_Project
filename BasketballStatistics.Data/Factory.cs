using System.Collections.Generic;

namespace BasketballStatistics.Data
{
    public class Factory
    {
        private Factory() { }

        private static Factory _default;
        public static Factory Default
        {
            get
            {
                if (_default == null)
                    _default = new Factory();
                return _default;
            }
        }
    }
}