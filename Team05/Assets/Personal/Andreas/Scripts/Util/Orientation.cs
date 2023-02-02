using System;

namespace Util
{
    [Flags]
    public enum Orientation
    {
        NONE        = 0,
        UP          = 1 << 0,
        DOWN        = 1 << 1,
        LEFT        = 1 << 2,
        RIGHT       = 1 << 3,
        UP_LEFT     = UP | LEFT,
        UP_RIGHT    = UP | RIGHT,
        DOWN_LEFT   = DOWN | LEFT,
        DOWN_RIGHT  = DOWN | RIGHT,
    }
}