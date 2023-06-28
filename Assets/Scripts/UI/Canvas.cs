using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    public Transform Camera;
    public GameObject player;
    public float CameraScale;
    public float playerScale;
    public float canvasSca;
    private void Start()
    {
           
    }
    void Update()
    {
        //transform.rotation = Quaternion.LookRotation(transform.position - Camera.transform.position);
        CameraScale = Camera.transform.localScale.x;
        playerScale = player.transform.localScale.x;
        canvasSca = transform.localScale.x;
        Vector3 scale = transform.localScale;
        scale.x = player.transform.localScale.x * 1;
        transform.localScale = scale;

    }
}
