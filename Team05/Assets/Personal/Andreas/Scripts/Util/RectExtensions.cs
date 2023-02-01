using UnityEngine;

namespace Util
{
    public static class RectExtensions
    {
        public static float Left(this Rect rect) => rect.x;
        public static float Right(this Rect rect) => rect.x + rect.width;
        public static float Top(this Rect rect) => rect.y - rect.height;
        public static float Bottom(this Rect rect) => rect.y;

        public static int LeftInt(this Rect rect) => (int)Left(rect);
        public static int RightInt(this Rect rect) => (int)Right(rect);
        public static int TopInt(this Rect rect) => (int)Top(rect);
        public static int BottomInt(this Rect rect) => (int)Bottom(rect);

    }
}