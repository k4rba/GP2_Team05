using System;
using System.Collections.Generic;
using UnityEngine;

namespace Personal.Andreas.Scripts
{
    public class CameraTopDownController : MonoBehaviour {
        
        [SerializeField] private Camera _camera;
        //  points of interest
        [SerializeField] private List<Transform> _transforms = new();
        [SerializeField] private float _heightOffset = 10f;

        private void Start() {
            if (_transforms == null)
                return;
        }

        public void SetTransforms(Transform player) {
            _transforms.Add(player);
        }

        /// <summary>
        /// Get a centralized position of all points of interest
        /// </summary>
        public Vector3 GetCenter()
        {
            Vector3 retSum = Vector3.zero;

            if(_transforms.Count <= 0)
                return retSum;

            for(int i = 0; i < _transforms.Count; i++)
            {
                retSum += _transforms[i].position;
            }

            var length = _transforms.Count;

            retSum = new Vector3(
                retSum.x / length, 
                retSum.y / length + _heightOffset, 
                retSum.z / length);

            return retSum;
        }

        private void Update() {

            if (_transforms == null)
                return;
            
            //  todo - smoothen

            var center = GetCenter();

            if(center == Vector3.zero)
                return;
            
            center.z -= 8f;
            _camera.transform.position = center;
            _camera.transform.LookAt(center);
            
        }
    }
}