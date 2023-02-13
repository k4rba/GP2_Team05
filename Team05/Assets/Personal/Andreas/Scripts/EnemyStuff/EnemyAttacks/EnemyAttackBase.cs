using Personal.Andreas.Scripts.Actors;
using UnityEngine;

namespace Andreas.Scripts.EnemyStuff.EnemyAttacks
{
    public class EnemyAttackBase
    {
        public float CastTimer;
        public Enemy Enemy;
        public Transform Target;
        
        public bool Attacked;

        public EnemyAttackData Data;

        public virtual void Start()
        {
            CastTimer = Data.CastTime;
        }
        
        public virtual void Update()
        {
            CastTimer -= Time.deltaTime;
            if(!Attacked && CastTimer <= 0)
            {
                OnAttacked();
            }
        }

        public virtual void OnAttacked()
        {
            Attacked = true;
        }
        
    }
}