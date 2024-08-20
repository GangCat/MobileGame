using System;
using UnityEngine;
using System.Collections;

public class ScoreManager : MonoBehaviour, IPlayerMoveObserver
{
    [SerializeField]
    private float ScoreMultiplierTime = 5f;

    private Action<int> onUpdateScoreAction = null; // 점수가 갱신될 때마다 업데이트하는 액션
    private Action onScoreMultiplyStart = null;
    private Action onScoreMultiplyFinish = null;

    private int curBlockCombo = 0; // 현재까지 블록의 개수
    private int curScore = 0; // 현재까지 총 점수
    private int scoreFactor = 1;

    private bool isScoreMultiplied = false;
    private WaitForSeconds waitScoreMultiplierTime = null;

    private int curMultiplier = 1;

    public int CurScore => curScore;

    public void Init(Action<int> _onUpdateScoreAction, Action _onScoreMultiplyStart, Action _onScoreMultiplyFinish)
    {
        waitScoreMultiplierTime = new WaitForSeconds(ScoreMultiplierTime);
        onUpdateScoreAction = _onUpdateScoreAction;
        onScoreMultiplyStart = _onScoreMultiplyStart;
        onScoreMultiplyFinish = _onScoreMultiplyFinish;
        curMultiplier = 1;
    }

    public void ResetScore()
    {
        curBlockCombo = 0;
        curScore = 0;
        onUpdateScoreAction?.Invoke(curScore);
        curMultiplier = 1;
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

        curScore += curMultiplier;
        //++curBlockCombo;
        onUpdateScoreAction?.Invoke(curScore);
    }

    private IEnumerator ScoreMultiplyTimerCoroutine(int _multiplierVal)
    {
        isScoreMultiplied = true;
        curMultiplier = _multiplierVal;
        onScoreMultiplyStart?.Invoke();
        yield return waitScoreMultiplierTime;
        isScoreMultiplied = false;
        curMultiplier = 1;
        onScoreMultiplyFinish?.Invoke();
    }
}
