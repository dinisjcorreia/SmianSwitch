using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public GameObject ropeSegmentPrefab;
    public int segmentCount = 10;
    public float segmentLength = 0.5f;
    public float maxRotationAngle = -120f; // Maximum rotation angle in degrees

    private List<GameObject> ropeSegments = new List<GameObject>();

    void Start()
    {
        Vector2 segmentPosition = transform.position;

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject newSegment = Instantiate(ropeSegmentPrefab, segmentPosition, Quaternion.identity);
            newSegment.tag = "RopeSegment";
            ropeSegments.Add(newSegment);

            HingeJoint2D joint = newSegment.AddComponent<HingeJoint2D>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedBody = i > 0 ? ropeSegments[i - 1].GetComponent<Rigidbody2D>() : GetComponent<Rigidbody2D>();
            joint.connectedAnchor = new Vector2(0, -segmentLength / 2);
            joint.anchor = new Vector2(0, segmentLength / 2);

            // Set rotation limits for the hinge joint
            joint.useLimits = true;
            JointAngleLimits2D limits = joint.limits;
            limits.max = maxRotationAngle;
            joint.limits = limits;

            segmentPosition.y -= segmentLength;
        }
    }
}
