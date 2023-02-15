using UnityEngine;

namespace Andreas.Scripts.RopeSystem
{
    [CreateAssetMenu(fileName = "Rope Data", menuName = "GP2/Rope Data", order = 0)]
    public class RopeVisualData : ScriptableObject
    {
        public float TransferSpeed = 1f;
        public float SegmentTime = 0.25f;
        public float SegmentScale = 3f;

    }
}