using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WispPlayer
{
    public static class WispControl
    {
        public static WispPlugManager wispPlugManager;

        public static void InitwispPlugManager(IntPtr winPtr)
        {
            wispPlugManager = new WispPlugManager(winPtr);
        }
    }
}
