using System;
using System.Collections.Generic;
using UnityEngine;

public class RealRope : MonoBehaviour
{

    [SerializeField] private GameObject _segmentPrefab;

    // [SerializeField] private int _segmentCount = 10;
    [SerializeField] private bool generate = true;


    [SerializeField] private GameObject start;
    [SerializeField] private GameObject end;
    
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

    private int CalculateSegmentCount()
    {
        var distance = Vector3.Distance(start.transform.position, end.transform.position);
        var prefabSizeY = _segmentPrefab.transform.localScale.y;
        return (int)(distance / (prefabSizeY * 2f)) + 1;
    }

    private void GenerateRope()
    {
        Clear();

        var startPos = start.transform.position;
        var endPos = end.transform.position;
        
        int count = CalculateSegmentCount();

        GameObject prev = null;
        
        for(int i = 0; i < count; i++)
        {
            var pos = Vector3.Lerp(startPos, endPos, ((float)i / count));
            var rot = Quaternion.LookRotation((startPos - endPos).normalized);

            rot = Quaternion.Euler(new Vector3(0, 0, -90));

            var seg = Instantiate(_segmentPrefab, pos, rot, transform);
            _segments.Add(seg);
            var hinge = seg.GetComponent<HingeJoint>();

            if(prev != null)
            {
                hinge.connectedBody = prev.GetComponent<Rigidbody>(); 
                hinge.anchor = Vector3.zero;
            }
            else
            {
                hinge.connectedBody = start.GetComponent<Rigidbody>();
                hinge.anchor = start.transform.InverseTransformPoint(pos);
            }

            prev = seg;

        }

        var endHinge = end.GetComponent<HingeJoint>();
        endHinge.connectedBody = prev.GetComponent<Rigidbody>();
        endHinge.anchor = Vector3.zero;

    }
    
}
