using UnityEngine;

namespace Personal.Andreas.Scripts.Util
{
    public static class RectExtensions
    {
        public static float Left(this Rect rect) => rect.xMin;
        public static float Right(this Rect rect) => rect.xMax;
        public static float Top(this Rect rect) => rect.yMin;
        public static float Bottom(this Rect rect) => rect.yMax;

        public static int LeftInt(this Rect rect) => (int)Left(rect);
        public static int RightInt(this Rect rect) => (int)Right(rect);
        public static int TopInt(this Rect rect) => (int)Top(rect);
        public static int BottomInt(this Rect rect) => (int)Bottom(rect);

    }
}