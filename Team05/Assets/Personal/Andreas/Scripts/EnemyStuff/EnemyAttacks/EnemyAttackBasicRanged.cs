﻿using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStuff.EnemyAttacks
{
    public class EnemyAttackBasicRanged : EnemyAttackProjectile
    {
        public override void OnAttacked()
        {
            base.OnAttacked();
            var prefab = FastResources.Load<GameObject>("Prefabs/Projectiles/BlobBullet");
            Shoot(prefab);
        }
    }
}