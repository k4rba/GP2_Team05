using UnityEngine;

namespace Personal.Andreas.Scripts.Util
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
    }
}
