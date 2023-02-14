using UnityEngine;

namespace Andreas.Scripts.CheckpointSystem
{
    public class Checkpoint : MonoBehaviour
    {
        public Transform[] Spawns;
        
        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<Player>();
            if(player == null)
                return;

            var cm = GameManager.Instance.CheckpointManager;
            cm.SetCurrentCheckpoint(this);
        }

        private void OnDrawGizmos()
        {
            if(Spawns == null || Spawns.Length <= 0)
                return;

            for(int i = 0; i < Spawns.Length; i++)
            {
                Gizmos.DrawWireSphere(Spawns[i].position, 0.5f);
            }
            
        }
    }
}