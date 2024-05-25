using UnityEngine;
using Cinemachine;

public class Zoom : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;
    private float targetOrthoSize;
    public float zoomSpeed = 2f; // Speed of the zoom transition
    public float minOrthoSize = 5f; // Minimum orthographic size
    public float maxOrthoSize = 15f; // Maximum orthographic size

    void Start()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        targetOrthoSize = vcam.m_Lens.OrthographicSize;
    }

    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            targetOrthoSize -= zoomSpeed;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            targetOrthoSize += zoomSpeed;
        }

        // Clamp the target orthographic size to prevent it from going out of bounds
        targetOrthoSize = Mathf.Clamp(targetOrthoSize, minOrthoSize, maxOrthoSize);

        // Gradually interpolate towards the target orthographic size
        vcam.m_Lens.OrthographicSize = Mathf.Lerp(vcam.m_Lens.OrthographicSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
    }
}
