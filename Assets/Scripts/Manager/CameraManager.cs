using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Transform taget;
    public float cameraPosx;
    public float cameraPosxBeginPart2 = 0f;
    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(taget.position.x, 0f, cameraPosx),
            Mathf.Clamp(taget.position.y, -1.0f, 10f),
            transform.position.z);
    }
    public void SetCamera(int part)
    {
        if (part == 1) cameraPosx = 43f;
        else if (part == 2)
        {
            cameraPosxBeginPart2 = 45f;
            cameraPosx = 72f;
        }
    }
}
