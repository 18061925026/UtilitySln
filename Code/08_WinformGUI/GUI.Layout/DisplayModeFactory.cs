using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _08_WinformGUI
{
    public class DisplayModeFactory
    {
        // Methods
        public static DisplayModeBase GetDisplayMode(View_DisplayMode mode)
        {
            switch (mode)
            {
                case View_DisplayMode.Mode_1:
                    return new DisplayMode1();

                case View_DisplayMode.Mode_2A:
                    return new DisplayMode2A();

                case View_DisplayMode.Mode_2B:
                    return new DisplayMode2B();

                case View_DisplayMode.Mode_3A:
                    return new DisplayMode3A();

                case View_DisplayMode.Mode_3B:
                    return new DisplayMode3B();

                case View_DisplayMode.Mode_4A:
                    return new DisplayMode4A();

                case View_DisplayMode.Mode_4B:
                    return new DisplayMode4B();

                case View_DisplayMode.Mode_4C:
                    return new DisplayMode4C();

                //case View_DisplayMode.Mode_5A:
                //    return new DisplayMode5A();

                //case View_DisplayMode.Mode_6A:
                //    return new DisplayMode6A();

                //case View_DisplayMode.Mode_6B:
                //    return new DisplayMode6B();

                //case View_DisplayMode.Mode_7A:
                //    return new DisplayMode7A();

                //case View_DisplayMode.Mode_7B:
                //    return new DisplayMode7B();

                //case View_DisplayMode.Mode_8A:
                //    return new DisplayMode8A();

                //case View_DisplayMode.Mode_8B:
                //    return new DisplayMode8B();

                //case View_DisplayMode.Mode_9A:
                //    return new DisplayMode9A();

                //case View_DisplayMode.Mode_9B:
                //    return new DisplayMode9B();

                //case View_DisplayMode.Mode_12A:
                //    return new DisplayMode12A();

                //case View_DisplayMode.Mode_13A:
                //    return new DisplayMode13A();
            }
            //return new DisplayMode6B();
            return new DisplayMode2A();
        }
    }
}
