using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 이건 플레이어 모델의 손이나 발을 따라다니는 트레일임
/// 있으면 훨씬 있어보여서 좋음
/// </summary>
public class ModelPartTrail : MonoBehaviour
{
    [SerializeField]
    private Transform targetModelPartTr = null;
    private TrailRenderer myTrail = null;

    public void StartTrail()
    {
        myTrail.enabled = true;
    }

    public void Init()
    {
        myTrail = GetComponent<TrailRenderer>();
        myTrail.enabled = false;
    }

    private void Update()
    {
        transform.position = targetModelPartTr.position;
    }

    public void ResetPos()
    {
        transform.position = targetModelPartTr.position;
        GetComponent<TrailRenderer>().Clear();
        myTrail.enabled = false;
    }
}
