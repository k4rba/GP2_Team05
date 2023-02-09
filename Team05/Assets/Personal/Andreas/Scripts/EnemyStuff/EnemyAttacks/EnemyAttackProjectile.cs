using UnityEngine;

namespace Andreas.Scripts.EnemyStuff.EnemyAttacks
{
    public abstract class EnemyAttackProjectile : EnemyAttackBase
    {
        protected Vector3 InfrontOfEnemy()
        {
            var targetPos = Target.position;
            var enemyPos = Enemy.transform.position;
            var dir = (targetPos - enemyPos).normalized;
            return enemyPos + dir * 1f;
        }

        protected void Shoot(GameObject prefab)
        {
            var spawnPos = InfrontOfEnemy();
            var bulletObj = Object.Instantiate(prefab, spawnPos, Quaternion.identity);
            var proj = bulletObj.GetComponent<Projectile>();
            proj.Init(Target.position);
        }
    }
}