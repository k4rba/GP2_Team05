using UnityEngine;

namespace Util
{
    public static class OrientationExtensions
    {
        public static Vector2Int ToVector2(this Orientation ori)
        {
            switch(ori)
            {
                case Orientation.NONE: return Vector2Int.zero;
                case Orientation.UP: return new Vector2Int(0, -1);
                case Orientation.DOWN: return new Vector2Int(0, 1);
                case Orientation.LEFT: return new Vector2Int(-1, 0);
                case Orientation.RIGHT: return new Vector2Int(1, 0);
                case Orientation.UP_LEFT: return new Vector2Int(-1, -1);
                case Orientation.UP_RIGHT: return new Vector2Int(1, -1);
                case Orientation.DOWN_LEFT: return new Vector2Int(-1, 1);
                case Orientation.DOWN_RIGHT: return new Vector2Int(1, 1);
            }
            return Vector2Int.zero;
        }
    }
}