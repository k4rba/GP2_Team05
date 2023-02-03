using System;
using System.Collections.Generic;
using UnityEngine;

namespace Personal.Andreas.Scripts
{
    public class CameraTopDown : MonoBehaviour {
        
        public static CameraTopDown Get { get; private set; }
        
        [SerializeField] private Camera _camera;
        
        //  points of interest
        [SerializeField] private List<Transform> _transforms = new List<Transform>();

        [SerializeField] private float _heightOffset = 10f;

        private bool _enabled = true;

        private void Awake() {
            Get = this;
        }

        private void Start() {
            if (_transforms == null)
                return;
        }

        public void SetPlayers(Transform player) {
            _transforms.Add(player);
        }

        /// <summary>
        /// Get a centralized position of all points of interest
        /// </summary>
        public Vector3 GetCenter()
        {
            Vector3 retSum = Vector3.zero;

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
            center.z -= 5f;
            _camera.transform.position = center;
            _camera.transform.LookAt(center);
            
        }
    }
}