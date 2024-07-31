using System.Collections;
using UnityEngine;

public class Cam : MonoBehaviour, IPlayerMoveObserver
{
    [SerializeField]
    private Transform playerTr = null;
    [SerializeField]
    private AnimationCurve camFollowCurve;

    private Vector3 camOffset = Vector3.zero;

    private void Start()
    {
        camOffset = transform.position - playerTr.position;
    }

    private void LateUpdate()
    {
        transform.position = playerTr.position + camOffset;
    }

    public void OnNotify(EBlockType _blockType)
    {
        StartCoroutine(nameof(CamFollowPlayerCoroutine));
    }

    private IEnumerator CamFollowPlayerCoroutine()
    {
        while (true)
        {


            yield return null;
        }
    }
}
