using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStuff
{
    public enum Attacks
    {
        None,
        BasicMelee,
        BasicRanged,
        Leap,
        StunGrenade,
    }
    
    [Serializable]
    public class AttackLibrary
    {
        public List<EnemyAttackData> Attacks;

        public EnemyAttackData GetRandomValidAttack()
        {
            var validAttacks = Attacks.Where(x => !x.IsOnCooldown).ToList();
            return validAttacks.RandomItem();
        }
        
        public void Update()
        {
            var dt = Time.deltaTime;
            
            for(int i = 0; i < Attacks.Count; i++)
            {
                var att = Attacks[i];

                if(att.IsOnCooldown)
                {
                    att.CooldownTimer -= dt;
                }
                
            }
        }
        
    }

    [Serializable]
    public class EnemyAttackData
    {
        public bool IsOnCooldown => CooldownTimer > 0f;
        
        [NonSerialized] public float CooldownTimer;
        
        public Attacks Type;
        public int Damage;
        public float Range;
        public float Cooldown;
        public float CastTime;

        public void TriggerAttack()
        {
            CooldownTimer = Cooldown;
        }
    }
    
}