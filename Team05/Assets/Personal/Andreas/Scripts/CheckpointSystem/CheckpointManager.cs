using System.Linq;
using UnityEngine;

namespace Andreas.Scripts.CheckpointSystem
{
    public class CheckpointManager : MonoBehaviour
    {
        public Checkpoint CurrentCheckpoint;

        public void SetCurrentCheckpoint(Checkpoint checkpoint)
        {
            Debug.Log($"Checkpoint set: {checkpoint.name}");
            CurrentCheckpoint = checkpoint;
        }

        public Vector3[] GetCurrentSpawnPositions()
        {
            return CurrentCheckpoint.Spawns.Select(x => x.position).ToArray();
        }
        
    }
}
