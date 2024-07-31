using System.Collections;
using UnityEngine;

public class Cam : MonoBehaviour, IPlayerMoveObserver
{
    [SerializeField]
    private Transform playerTr = null;
    [SerializeField]
    private AnimationCurve camFollowCurve;

    private Vector3 camOffset = Vector3.zero;
    private Coroutine followPlayerSmoothCoroutine = null;

    private void Start()
    {
        camOffset = transform.position - playerTr.position;
    }

    public void OnNotify(EBlockType _blockType)
    {
        if (followPlayerSmoothCoroutine is not null)
        {
            StopCoroutine(followPlayerSmoothCoroutine);
        }
        followPlayerSmoothCoroutine = StartCoroutine(nameof(CamFollowPlayerSmoothCoroutine));
    }

    private IEnumerator CamFollowPlayerSmoothCoroutine()
    {
        float elapsedTime = 0f;
        Vector3 originPos = transform.position;
        Vector3 targetPos = playerTr.position + camOffset;
        float curveVal = 0f;
        while (curveVal < 1)
        {
            curveVal = camFollowCurve.Evaluate(elapsedTime);
            transform.position = Vector3.Lerp(originPos, targetPos, curveVal);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        transform.position = targetPos;
    }
}
