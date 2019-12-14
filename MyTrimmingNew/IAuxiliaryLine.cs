using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTrimmingNew
{
    interface IAuxiliaryLine
    {
        void Move();

        void ChangeSize();

        int GetLineSizeWidth();

        int GetLineSizeHeight();
    }
}
