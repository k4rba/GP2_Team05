using AudioSystem;
using Personal.Andreas.Scripts.Actors;
using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStuff.EnemyAttacks
{
    public class EnemyAttackBasicMelee : EnemyAttackBase
    {
        public override void OnAttacked()
        {
            base.OnAttacked();
            
            // Debug.Log("attacked");
            
            var enPos = Enemy.transform.position;
            // var unit = Enemy.FlowAgent.Target;
            var unit = Target;
            var dir = (unit.transform.position - enPos).normalized;

            var boxOffsetRange = 1.5f;

            var boxPosition = enPos + dir * boxOffsetRange;

            var prefab = FastResources.Load<GameObject>("EnemyAttackBox");
            var box = Object.Instantiate(prefab, boxPosition, Quaternion.identity);
            var hitCheck = box.GetComponent<HitCheck>();
            hitCheck.Set(unit.gameObject);
            
            AudioManager.PlaySfx("VoiceMeleeEnemyHits_mixdown", Enemy.transform.position);

        }

    }
}