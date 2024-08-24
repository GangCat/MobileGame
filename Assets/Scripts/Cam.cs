using System.Collections;
using UnityEngine;

public class Cam : MonoBehaviour, IPlayerMoveObserver
{
    [SerializeField]
    private Transform playerTr = null;
    [SerializeField]
    private AnimationCurve camFollowCurve;
    [SerializeField]
    private Vector3 lobbyCamPos = Vector3.zero;
    [SerializeField]
    private Vector3 lobbyCamEulerAngles = Vector3.zero;
    [SerializeField]
    private Vector3 gameCamEulerAngles = Vector3.zero;

    private Vector3 camOffset = Vector3.zero;
    private Coroutine followPlayerSmoothCoroutine = null;

    private void Start()
    {
        camOffset = transform.position - playerTr.position;

        transform.position = lobbyCamPos;
        transform.eulerAngles = lobbyCamEulerAngles;
    }

    public void MoveCamGamePos(in Vector3 _playerOriginPos)
    {
        // 2초만에 러프로 이동

        StartCoroutine(nameof(MoveCamToGamePosCoroutine));

    }

    public void ResetCamPos()
    {
        if (followPlayerSmoothCoroutine != null)
        {
            StopCoroutine(followPlayerSmoothCoroutine);
        }
        transform.position = lobbyCamPos;
        transform.eulerAngles = lobbyCamEulerAngles;
        //transform.position = _playerOriginPos + camOffset;
    }

    public void OnNotify(in EBlockType _blockType)
    {
        if (followPlayerSmoothCoroutine != null)
        {
            StopCoroutine(followPlayerSmoothCoroutine);
        }
        followPlayerSmoothCoroutine = StartCoroutine(nameof(FollowPlayerSmoothCoroutine));
    }

    private IEnumerator FollowPlayerSmoothCoroutine()
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

    private IEnumerator MoveCamToGamePosCoroutine()
    {
        float elapsedTime = 0f;
        while(elapsedTime < 2)
        {
            transform.position = Vector3.Lerp(transform.position, playerTr.position + camOffset, 0.05f);
            transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, gameCamEulerAngles, 0.05f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = playerTr.position + camOffset;
        transform.eulerAngles = gameCamEulerAngles;
    }
}
