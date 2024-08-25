using System;
using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class ScoreManager : MonoBehaviour, IPlayerMoveObserver, IFadeOutFinishObserver, IFeverObserver
{
    [SerializeField]
    private float ScoreMultiplierTime = 5f;

    private Action<int> onUpdateScoreAction = null; // 점수가 갱신될 때마다 업데이트하는 액션
    private Action onScoreMultiplyStart = null;
    private Action onScoreMultiplyFinish = null;

    private int curScore = 0; // 현재까지 총 점수

    private WaitForSeconds waitScoreMultiplierTime = null;

    private int curMultiplier = 1;

    public int CurScore => curScore;

    private float startTime = 0f;

    private SResult sResult;

    private int actionCount = 0;
    private float APS = 0f;
    private float bestAPS = 0f;
    private float apsStartTime = 0f;

    private bool isMoved = false;
    private bool isFever = false;


    public void Init(Action<int> _onUpdateScoreAction, Action _onScoreMultiplyStart, Action _onScoreMultiplyFinish)
    {
        waitScoreMultiplierTime = new WaitForSeconds(ScoreMultiplierTime);
        onUpdateScoreAction = _onUpdateScoreAction;
        onScoreMultiplyStart = _onScoreMultiplyStart;
        onScoreMultiplyFinish = _onScoreMultiplyFinish;
        curMultiplier = 1;
        sResult = new SResult();
    }

    public void StartGame()
    {
        startTime = Time.time;
        StartCoroutine(nameof(CalcAPSCoroutine));
    }

    public SResult CalcResult()
    {
        sResult.time = Time.time - startTime;
        sResult.score = curScore;
        sResult.bestAPS = this.bestAPS;
        StopCoroutine(nameof(CalcAPSCoroutine));
        return sResult;
    }

    public void ResetScore()
    {
        curScore = 0;
        onUpdateScoreAction?.Invoke(curScore);
        curMultiplier = 1;
        bestAPS = 0;
    }

    public void OnNotify(in EBlockType _blockType)
    {
        // 무조건 블럭당 1점
        // 단, 더블스코어, 트리플스코어 블럭 지나가면 그때부터 시간 제서 점수 배수로 증가
        switch (_blockType)
        {
            case EBlockType.DOUBLE_SCORE:
                // 시간측정 코루틴 시작, 점수 2배 적용
                StartCoroutine(nameof(ScoreMultiplyTimerCoroutine), 2);
                break;
            case EBlockType.TRIPLE_SCORE:
                // 시간측정 코루틴 시작, 점수 3배 적용
                StartCoroutine(nameof(ScoreMultiplyTimerCoroutine), 3);
                break;
        }

        isMoved = true;
        curScore += curMultiplier;
        //++curBlockCombo;
        onUpdateScoreAction?.Invoke(curScore);
    }

    private IEnumerator ScoreMultiplyTimerCoroutine(int _multiplierVal)
    {
        curMultiplier = _multiplierVal;
        onScoreMultiplyStart?.Invoke();
        yield return waitScoreMultiplierTime;
        curMultiplier = 1;
        onScoreMultiplyFinish?.Invoke();
    }

    private IEnumerator CalcAPSCoroutine()
    {
        apsStartTime = Time.time;

        while (true)
        {
            if (isMoved is true)
            {
                actionCount++;
                isMoved = false;
            }

            CalculateAPS();
            yield return null;
        }
    }

    private bool IsTouchInputDetected()
    {
        // 모바일 터치가 발생했는지 확인합니다.
        return Input.touchCount > 0;
    }

    private void CalculateAPS()
    {
        float elapsedTime = Time.time - apsStartTime;

        if (elapsedTime == 0 || actionCount < 0)
            return;

        APS = actionCount / elapsedTime;

        if (elapsedTime < 1f)
            bestAPS = APS;

        if (APS > bestAPS)
            bestAPS = APS;
    }

    public void OnNotify()
    {
        ResetScore();
    }

    public void OnNotify(in bool _isFeverStart)
    {
        isFever = _isFeverStart;
        // 점수를 배수할지 아니면 뭐 재화를 획득하게 할지 고민중
    }
}

public struct SResult
{
    public int score;
    public float time;
    public float bestAPS;
}