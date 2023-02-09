using System.Collections.Generic;
using UnityEngine;

namespace Andreas.Scripts
{
    public class DynamicRope : MonoBehaviour
    {
        
        [Header("Start")]
        [SerializeField] private Transform startTf;
        [SerializeField] private Rigidbody startBody;
        [Space(5)]
        [Header("End")]
        [SerializeField] private Transform endTf;
        [SerializeField] private Rigidbody endBody;
        [Space(5)]
        
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

        private void FixedUpdate()
        {
            if(_segments.Count <= 0)
                return;
            
            var firstSeg = _segments[0];
            // firstSeg.MovePosition(startTf.position);

            var lastSeg = _segments[segmentCount - 1];
            // lastSeg.GetComponent<Rigidbody>().MovePosition(endTf.position);
            // lastSeg.GetComponent<CharacterJoint>().connectedAnchor = Vector3.zero;

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
            var parentPos = startTf.transform.position;
            
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
                    joint.connectedBody = startBody;
                }
                else
                {
                    var parent = _segments[i - 1];
                    joint.connectedBody = parent.GetComponent<Rigidbody>();
                }

                if(i == segmentCount - 1)
                {
                    // joint.connectedAnchor = Vector3.zero;
                }

                seg.transform.position = segPos;
            }
        }
    }
}