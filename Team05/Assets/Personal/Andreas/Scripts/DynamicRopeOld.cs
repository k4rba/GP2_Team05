using System;
using UnityEngine;

public class DynamicRopeOld : MonoBehaviour
{
    public Transform startPoint, endPoint;
    public int segments = 20;
    public float ropeWidth = 0.1f;
    public Material ropeMaterial;
    public GameObject ropeSegmentPrefab;

    LineRenderer lineRenderer;
    SpringJoint[] ropeJoints;
    GameObject[] ropeSegments;

    void Start()
    {
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = ropeMaterial;
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
        ropeJoints = new SpringJoint[segments];
        ropeSegments = new GameObject[segments];

        Vector3[] ropePositions = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            GameObject segment = Instantiate(ropeSegmentPrefab, Vector3.zero, Quaternion.identity);
            ropeSegments[i] = segment;

            if (i == 0)
            {
                segment.transform.position = startPoint.position;
            }
            else if (i == segments - 1)
            {
                segment.transform.position = endPoint.position;
                ropeJoints[i - 1] = segment.AddComponent<SpringJoint>();
                ropeJoints[i - 1].autoConfigureConnectedAnchor = false;
                ropeJoints[i - 1].connectedAnchor = ropeSegments[i - 1].transform.position;
                ropeJoints[i - 1].maxDistance = 0.5f;
                ropeJoints[i - 1].minDistance = 0.1f;
            }
            else
            {
                segment.transform.position = Vector3.Lerp(startPoint.position, endPoint.position, (float)i / (float)(segments - 1));
                ropeJoints[i] = segment.AddComponent<SpringJoint>();
                ropeJoints[i].autoConfigureConnectedAnchor = false;
                ropeJoints[i].connectedAnchor = ropeSegments[i - 1].transform.position;
                ropeJoints[i].maxDistance = 0.5f;
                ropeJoints[i].minDistance = 0.1f;
            }

            segment.AddComponent<SphereCollider>();
        }

        lineRenderer.positionCount = segments;

        for (int i = 0; i < segments; i++)
        {
            ropePositions[i] = ropeSegments[i].transform.position;
        }

        lineRenderer.SetPositions(ropePositions);
    }

    private void Update()
    {
        UpdateRope();
    }

    void UpdateRope()
    {
        Vector3[] ropePositions = new Vector3[segments];
        ropePositions[0] = startPoint.position;
        ropePositions[segments - 1] = endPoint.position;

        for (int i = 0; i < segments - 1; i++)
        {
            ropePositions[i + 1] = ropeSegments[i].transform.position;
        }

        lineRenderer.SetPositions(ropePositions);
    }
}