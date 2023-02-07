using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Andreas.Scripts
{
    public class DynamicRope : MonoBehaviour
    {
        [SerializeField] private Transform start, end;

        [SerializeField] private int segmentCount;
        [SerializeField] private GameObject prefSegment;
        [SerializeField] private GameObject prefHinge;

        private List<GameObject> _segments;

        [SerializeField] private bool generate;

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
            var pos = transform.position;
            for(int i = 0; i < segmentCount; i++)
            {
                var seg = Instantiate(prefSegment, pos, Quaternion.identity, transform);
                _segments.Add(seg);

                seg.name = $"Segment {i}";
                
            }
        }

    }
}