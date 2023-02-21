using Andreas.Scripts.Flowfield;
using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateIdle : EnemyState
    {
        private Timer _scanTargetTimer = 1f;

        public override void Update(float dt)
        {
            base.Update(dt);

            if(_scanTargetTimer.UpdateTick())
            {
                ScanForPlayer();
            }
        }

        private void EnterHunt(GameObject target)
        {
            //  temporary
            if(Enemy.Data.Name.Equals("Gunnar"))
            {
                Enemy.StateManager.SetState(new EnemyStateHunt(target));
            }
            else
            {
                Enemy.StateManager.SetState(new EnemyStateRangedHunt(target));
            }
        }

        private void ScanForPlayer()
        {
            var players = GameManager.Instance.CharacterManager.Players;

            float aggroRange = 15f;

            for(int i = 0; i < players.Count; i++)
            {
                var p = players[i];
                if(p.isDead)
                    continue;

                var distance = p.transform.position.FastDistance(Enemy.transform.position);

                if(distance < aggroRange)
                {
                    EnterHunt(p.gameObject);
                    return;
                }
            }
        }
    }
}