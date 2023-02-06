using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

namespace Personal.Andreas.Scripts
{
    public class CameraTopDownController : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        //  points of interest
        [SerializeField] private List<Transform> _transforms = new();
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _heightOffset = 10f;
        [SerializeField] private float _distance = 8f;

        private Vector3 _destination;

        private void Start()
        {
            if(_transforms == null)
                return;
        }

        public void SetTransforms(Transform player)
        {
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
                retSum.z / length - _distance);

            return retSum;
        }

        private void Update()
        {
            if(_transforms == null)
                return;

            _destination = GetCenter();

            MoveCamera();
        }

        private void MoveCamera()
        {
            if(_destination == Vector3.zero)
                return;

            var camPos = transform.position;
            var distance = _destination.FastDistance(camPos);
            
            var direction = (_destination - camPos).normalized;
            var position = camPos + direction * (distance * _speed * Time.deltaTime);

            position.y = _destination.y + _heightOffset;
            
            _camera.transform.position = position;
            // _camera.transform.LookAt(_destination);
        }
    }
}