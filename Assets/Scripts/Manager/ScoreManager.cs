using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour, IPlayerMoveObserver
{
    private Action<int> onUpdateScoreAction = null; // 점수가 갱신될 때마다 업데이트하는 액션

    private int curBlockCombo = 0; // 현재까지 블록의 개수
    private int curScore = 0; // 현재까지 총 점수
    private int scoreFactor = 1;

    public int CurScore => curScore;

    public void Init(Action<int> _onUpdateScoreAction)
    {
        onUpdateScoreAction = _onUpdateScoreAction;
    }

    public void ResetScore()
    {
        curBlockCombo = 0;
        curScore = 0;
        onUpdateScoreAction?.Invoke(curScore);
    }

    public void OnNotify(in EBlockType _blockType)
    {
        // 무조건 블럭당 1점
        // 단, 더블스코어, 트리플스코어 블럭 지나가면 그때부터 시간 제서 점수 배수로 증가
        switch (_blockType)
        {
            case EBlockType.GOLD:
                // 시간측정 코루틴 시작, 점수 2배 적용
                break;
            case EBlockType.DIAMOND:
                // 시간측정 코루틴 시작, 점수 3배 적용
                break;
            default:
                ++curScore;
                break;
        }
        //++curBlockCombo;
        onUpdateScoreAction?.Invoke(curScore);
    }
}
