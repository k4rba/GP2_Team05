using Andreas.Scripts.EnemyData;
using UnityEngine;

namespace Andreas.Scripts.HealingZone
{
    [CreateAssetMenu(fileName = "HealingZoneData", menuName = "GP2/HealingZone", order = 0)]
    public class HealingZoneData : ScriptableObject
    {
        public float HealAmount = 0.1f;
        public float TickRate = 1f;
        public int MaxTicks = 3;

        [Header("SFX")]
        public AudioData Idle;
        public AudioData Active;
    }
}