using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameracontroller : MonoBehaviour
{
    public Transform target;
    public float smooth;
    public Vector3 offset;
    private Vector3 velocity = Vector3.zero;

    void Update()
    {
        if (target!=null)
        {
            Vector3 targetpos = target.position + offset;
            transform.position = Vector3.SmoothDamp(transform.position,targetpos,ref velocity,smooth);
        }
    }

}
