using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    [SerializeField]
    private Transform camTr = null;


    private Vector3 offset = Vector3.zero;
    private void Start()
    {
        offset = transform.position - camTr.position;
    }

    void Update()
    {
        transform.position = offset + camTr.position;
        transform.rotation = Quaternion.LookRotation(camTr.position - transform.position);
    }
}
