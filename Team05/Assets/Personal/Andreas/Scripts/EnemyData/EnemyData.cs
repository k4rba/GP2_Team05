using UnityEngine;

namespace Andreas.Scripts.EnemyData
{
    [CreateAssetMenu(fileName = "new Enemy Data",
        menuName = "GP2/EnemyData", order = 0)]
    public class EnemyData : ScriptableObject
    {
        [Header("Stats")] 
        public string Name;
        public int Health;
        public float MoveSpeed;
        public float Damage;

        [Space(10)] 
        
        [Tooltip("The visual size scaling of the enemy")]
        public float SizeScale = 1f;

        [Space(10)] 
        
        [Tooltip("The level or difficulty of the enemy.")]
        public int Tier = 1;

        [Header("SFX")] 
        public AudioClip OnDeath;
    }
}