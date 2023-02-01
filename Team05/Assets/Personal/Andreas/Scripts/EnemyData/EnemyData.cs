using UnityEngine;
using UnityEngine.Serialization;

namespace Andreas.Scripts.EnemyData
{
    [CreateAssetMenu(fileName = "new Enemy Data",
        menuName = "GP2/EnemyData", order = 0)]
    public class EnemyData : ScriptableObject
    {
        [Header("SFX")]
        public AudioClip OnDeath;
    }
}