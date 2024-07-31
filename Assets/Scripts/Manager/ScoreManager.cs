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
        // 50콤보마다 각 블럭의 점수가 1점 추가되어 계산
        curScore += 1 + curBlockCombo / 50;
        ++curBlockCombo;
        onUpdateScoreAction?.Invoke(curScore);
    }
}
