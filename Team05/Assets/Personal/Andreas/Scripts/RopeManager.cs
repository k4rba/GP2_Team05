using Andreas.Scripts.RopeSystem;
using UnityEngine;

namespace Andreas.Scripts
{
    public class RopeManager : MonoBehaviour
    {
        public RealRope Rope => _rope;
        
        [SerializeField] private RealRope _rope;
        
        public void SetRopeEnds(GameObject start, GameObject end)
        {
            _rope.SetRoots(start, end);
            GenerateRope();
        }

        public void GenerateRope()
        {
            _rope.GenerateRope();
        }
        
    }
}