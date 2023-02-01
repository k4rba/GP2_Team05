using UnityEngine;

namespace Util
{
    public static class VectorExtensions
    {
        public static Vector2 Clamp(this Vector2 val, Vector2 min, Vector3 max)
        {
            return new Vector2(
                Mathf.Clamp(val.x, min.x, max.x),
                Mathf.Clamp(val.y, min.y, max.y));
        }

        public static Vector2 Clamp(this Vector2 val, float min, float max)
        {
            return new Vector2(
                Mathf.Clamp(val.x, min, max),
                Mathf.Clamp(val.y, min, max));
        }

        public static Vector2 ToVector2(this Vector2Int vec)
        {
            return new Vector2(vec.x, vec.y);
        }

        public static Vector3 ToVector3XZ(this Vector2 vec)
        {
            return new Vector3(vec.x, 0, vec.y);
        }
    }
}
