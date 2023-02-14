using UnityEngine;

namespace Andreas.Scripts
{
    [CreateAssetMenu(fileName = "PlayerBoundsData", menuName = "Player Bounds Data", order = 0)]
    public class PlayerBoundsData : ScriptableObject
    {
        public float MaxDistance = 5f;
        public float Power = 10f;
    }
}