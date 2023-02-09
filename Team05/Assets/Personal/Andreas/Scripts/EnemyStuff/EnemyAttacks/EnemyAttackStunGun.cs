﻿using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStuff.EnemyAttacks
{
    public class EnemyAttackStunGun : EnemyAttackProjectile
    {
        public override void OnAttacked()
        {
            base.OnAttacked();
            var prefab = FastResources.Load<GameObject>("Prefabs/Projectiles/StunBullet/StunBullet");
            Shoot(prefab);
        }
    }
}