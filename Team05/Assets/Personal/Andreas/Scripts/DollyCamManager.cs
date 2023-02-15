using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Andreas.Scripts
{
    public class DollyCamManager : MonoBehaviour
    {
        
        [SerializeField] private GameObject _targetObj;
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private CinemachineVirtualCamera [] _cameras;
        
        private List<Transform> _transforms = new();

        private void Start()
        {
            SetActiveCamera(_camera);
        }

        public void SetActiveCamera(CinemachineVirtualCamera cam)
        {
            for (int i = 0; i < _cameras.Length; i++)
            {
                _cameras[i].gameObject.SetActive(false);
            }

            _camera = cam;
            _camera.gameObject.SetActive(true);
            _camera.Follow = _targetObj.transform;
            _camera.LookAt = _targetObj.transform;
        }
        public void AssignTargets(GameObject p1, GameObject p2)
        {
            _transforms.Add(p1.transform);
            _transforms.Add(p2.transform);

            _camera.Follow = _targetObj.transform;
            _camera.LookAt = _targetObj.transform;
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
                retSum.y / length,
                retSum.z / length);

            return retSum;
        }

        private void Update()
        {
            _targetObj.transform.position = GetCenter();
        }
    }
}