using System.Collections.Generic;
using UnityEngine;

public class RealRope : MonoBehaviour
{
    [SerializeField] private GameObject _segmentPrefab;
    [SerializeField] private bool generate = true;

    [SerializeField] private GameObject _start;
    [SerializeField] private GameObject _end;
    
    private List<GameObject> _segments;

    private void Awake()
    {
        _segments = new();
    }

    public void SetRoots(GameObject start, GameObject end)
    {
        _start = start;
        _end = end;
    }

    private void Update()
    {

        if(generate)
        {
            // GenerateRope();
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
        var distance = Vector3.Distance(_start.transform.position, _end.transform.position);
        var prefabSizeY = _segmentPrefab.transform.localScale.y;
        return (int)(distance / (prefabSizeY * 2f)) + 1;
    }

    public void GenerateRope()
    {
        Clear();

        var startPos = _start.transform.position;
        var endPos = _end.transform.position;
        
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
                hinge.connectedBody = _start.GetComponent<Rigidbody>();
                hinge.anchor = _start.transform.InverseTransformPoint(pos);
            }

            prev = seg;

        }

        var endHinge = _end.GetComponent<HingeJoint>();
        endHinge.connectedBody = prev.GetComponent<Rigidbody>();
        endHinge.anchor = Vector3.zero;

    }
    
}
