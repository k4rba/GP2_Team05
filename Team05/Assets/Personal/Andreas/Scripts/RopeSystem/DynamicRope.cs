using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Andreas.Scripts
{
    public class DynamicRope : MonoBehaviour
    {
        [SerializeField] private Rigidbody start, end;
        [SerializeField] private GameObject prefSegment;
        [SerializeField] private int segmentCount;

        [SerializeField] private float segmentSize = 1f;
        [SerializeField] private bool generate;

        private List<GameObject> _segments;
        
        private void Awake()
        {
            _segments = new();
        }

        private void Update()
        {
            if(generate)
            {
                GenerateRope();
                generate = false;
            }
        }

        private void Clear()
        {
            for(int i = 0; i < _segments.Count; i++)
            {
                Destroy(_segments[i]);
            }

            _segments.Clear();
        }

        private void GenerateRope()
        {
            Clear();

            var pos = transform.position;
            var parentPos = start.position;
            
            var startPos = parentPos;
            
            for(int i = 0; i < segmentCount; i++)
            {
                var segPos = new Vector3(startPos.x, startPos.y - (i * segmentSize), startPos.z);
                
                var seg = Instantiate(prefSegment, segPos, Quaternion.identity, transform);
                seg.transform.eulerAngles = new Vector3(0, 0, 0);
                _segments.Add(seg);

                seg.name = $"Segment {i}";

                var joint = seg.GetComponent<CharacterJoint>();
                if(i == 0)
                {
                    // Destroy(joint);
                    // seg.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    joint.connectedBody = start;
                }
                else
                {
                    var parent = _segments[i - 1];
                    joint.connectedBody = parent.GetComponent<Rigidbody>();
                }
            }
        }
    }
}