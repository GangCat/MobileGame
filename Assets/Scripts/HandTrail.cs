using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandTrail : MonoBehaviour
{
    [SerializeField]
    private Transform handTr = null;

    private void Update()
    {
        transform.position = handTr.position;
    }

    public void ResetPos()
    {
        transform.position = handTr.position;
        GetComponent<TrailRenderer>().Clear();
    }
}
