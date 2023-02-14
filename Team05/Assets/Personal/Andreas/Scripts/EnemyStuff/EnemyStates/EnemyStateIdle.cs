using Andreas.Scripts.Flowfield;
using UnityEngine;
using Util;

namespace Andreas.Scripts.EnemyStates
{
    public class EnemyStateIdle : EnemyState
    {

        private Timer _scanTargetTimer = 1f;
        
        public override void Start()
        {
            base.Start();

            // Debug.Log("IDLE");
            
        }

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
            // Debug.Log("exit hunt");
            // Enemy.FlowAgent.SetTarget(target);
            Enemy.StateManager.SetState(new EnemyStateHunt(target));
        }
        
        private void ScanForPlayer()
        {
            var players = GameManager.Instance.CharacterManager.Players;

            float aggroRange = 15f;
            
            for(int i = 0; i < players.Count; i++)
            {
                var p = players[i];

                var distance = p.transform.position.FastDistance(Enemy.transform.position);

                if(distance < aggroRange)
                {
                    // EnterHunt(p.GetComponent<FlowTargetAgent>());
                    EnterHunt(p.gameObject);
                    // Debug.Log($"aggro on player: {p.name}");
                    return;
                }
            }
        }
        
    }
}