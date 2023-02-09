using System;
using Andreas.Scripts.Flowfield;
using UnityEngine;
using Util;

namespace Andreas.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class FlowFollowerAgent : MonoBehaviour
    {
        public FlowTargetAgent Target => _target;

        [NonSerialized] public Rigidbody Body;
        [NonSerialized] public Vector2 FlowDirection;
        [NonSerialized] public Timer UpdateDirectionInterval = 0.1f;

        public bool IsEnabled = true;

        private AgentMover _mover;
        private FlowTargetAgent _target;

        private void Awake()
        {
            Body = GetComponent<Rigidbody>() ?? GetComponentInChildren<Rigidbody>();
        }

        public void SetTarget(FlowTargetAgent target)
        {
            _target = target;
        }

        public void SetMover(AgentMover mover)
        {
            _mover = mover;
        }

        private void Update()
        {
            if(!IsEnabled)
            {
                return;
            }

            if(UpdateDirectionInterval.UpdateTick())
            {
                UpdateDirection();
            }
        }

        private void FixedUpdate()
        {
            if(!IsEnabled)
                return;

            Move();
        }

        public void UpdateDirection()
        {
            if(_target == null)
                return;

            var position = Body.position;
            FlowDirection = _target.GetFieldManagerManager.GetDirection(position);
        }

        public void Move()
        {
            _mover.Move();
        }
    }
}