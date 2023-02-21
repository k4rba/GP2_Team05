using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateRangedHunt : EnemyStateTarget
    {
        private List<Player> _players;

        private Timer _destinationInterval = 0.2f;
        private Timer _fleeInterval = 0.5f;

        public EnemyStateRangedHunt(GameObject target) : base(target)
        {
        }

        public override void Start()
        {
            base.Start();
            
            Enemy._animator.SetBool("Run", true);
            
            _players = GameManager.Instance.CharacterManager.Players;
            RandomizeAttack();

            if(Rng.Roll(50))
            {
                TryFlee();
                return;
            }
            
            UpdateDestination();
            
            if(!Enemy.EnteredCombat)
                Enemy.Data.Sfx.OnAggro.Play(Enemy.transform.position);
            Enemy.EnteredCombat = true;
        }

        private void UpdateDestination()
        {
            Enemy.NavAgent.isStopped = false;
            Enemy.NavAgent.destination = Target.transform.position;
        }

        private void TryFlee()
        {
            float minDistanceFromPlayerToFlee = 6f;
            var enemyPos = Enemy.transform.position;
            for(int i = 0; i < _players.Count; i++)
            {
                var playerPos = _players[i].transform.position;
                var distance = enemyPos.FastDistance(playerPos);

                if(distance < minDistanceFromPlayerToFlee)
                {
                    var dir = (enemyPos - playerPos).normalized;
                    var fleePos = enemyPos + dir * 5f;
                    Enemy.StateManager.SetState(new EnemyStateMoveDirection(fleePos));
                    Exit();
                    Debug.Log("fleeing");
                    break;
                }
                
            }
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if(_destinationInterval.UpdateTick())
            {
                UpdateDestination();
            }

            if(_fleeInterval.UpdateTick())
            {
                TryFlee();
            }

            if(IsCloseForAttack())
            {
                EnterAttack();
            }
        }

        private void EnterAttack()
        {
            Enemy.StateManager.SetState(
                new EnemyStateWait(0.25f, new EnemyStateAttack(Target, Attack)));
        }

        public override void Exit()
        {
            base.Exit();
            Enemy._animator.SetBool("Run", false);
            Enemy.NavAgent.isStopped = true;
        }
    }
}