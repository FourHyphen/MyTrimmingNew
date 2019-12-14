using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew
{
    class AuxiliaryLineFactory
    {
        public enum Kind
        {
            RectangleAuxiliaryWithFixedRatio
        }

        public static IAuxiliaryLine Create(Kind kind)
        {
            if (kind == Kind.RectangleAuxiliaryWithFixedRatio)
            {
                return new RectangleAuxiliaryWithFixedRatio();
            }

            return null;
        }
    }
}
