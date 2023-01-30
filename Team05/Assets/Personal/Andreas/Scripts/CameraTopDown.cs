using UnityEngine;

namespace Personal.Andreas.Scripts
{
    public class CameraTopDown : MonoBehaviour
    {
        [SerializeField] private Camera _camera;
        
        //  points of interest
        [SerializeField] private Transform[] _transforms;
        
        [SerializeField] private float _heightOffset = 10f;

        private bool _enabled;

        private void Start()
        {
            enabled = _transforms is { Length: > 0 };
        }

        /// <summary>
        /// Get a centralized position of all points of interest
        /// </summary>
        public Vector3 GetCenter()
        {
            Vector3 retSum = Vector3.zero;

            for(int i = 0; i < _transforms.Length; i++)
            {
                retSum += _transforms[i].position;
            }

            var length = _transforms.Length;

            retSum = new Vector3(
                retSum.x / length, 
                retSum.y / length + _heightOffset, 
                retSum.z / length);

            return retSum;
        }

        private void Update()
        {
            //  todo - smoothen

            var center = GetCenter();
            _camera.transform.position = center;
            _camera.transform.LookAt(center);
            
        }
    }
}