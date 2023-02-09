﻿using Personal.Andreas.Scripts.Actors;
using UnityEngine;

namespace Andreas.Scripts.EnemyStuff.EnemyAttacks
{
    public class EnemyAttackBase
    {
        public float CastTimer;
        public Enemy Enemy;
        public Transform Target => Enemy.FlowAgent.Target.transform;
        
        public bool Attacked;

        public EnemyAttackData Data;

        public EnemyAttackBase() { }
        public EnemyAttackBase(Enemy enemy)
        {
            Enemy = enemy;
        }

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