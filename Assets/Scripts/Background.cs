using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [SerializeField]
    private Camera mainCam = null;

    private Vector3 offset = Vector3.zero;

    private void Start()
    {
        offset = transform.position - mainCam.transform.position;
    }

    private void Update()
    {
        Vector3 newPos = mainCam.transform.position + offset;
        newPos.x *= 0.2f;
        transform.position = newPos;
        transform.up = -mainCam.transform.forward;
    }
}
