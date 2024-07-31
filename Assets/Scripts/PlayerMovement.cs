using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerMoveSubject
{
    private Vector2 prevDir = Vector2.zero;
    private Vector2 curDir = Vector2.zero; // 현재 방향을 저장하는 변수

    private Action onGameOverAction = null; // 게임 종료를 알리는 액션
    private Action<EBlockType> onInventoryIncreaseAction = null; // 도착한 블럭의 타입에 따라 재화를 상승시키는 액션.
    private Action<Vector2> updateModelForwardAction = null;

    private IWalkableBlockManager iBlockManager = null;

    public event Action OnBlockProcessed; // 블럭을 한 칸 이동할때마다 호출할 이벤트
    public bool IsGameStop { get; set; } = true;

    private List<IPlayerMoveObserver> observerList = new List<IPlayerMoveObserver>();

    public void Init(IWalkableBlockManager _iBlockManager, Action _onGameOverAction, Action<EBlockType> _onInventoryIncreaseAction, Action<Vector2> _updateModelForwardAction)
    {
        prevDir = Vector2.zero;
        curDir = Vector2.zero;
        iBlockManager = _iBlockManager;
        onGameOverAction = _onGameOverAction;
        updateModelForwardAction = _updateModelForwardAction;
        onInventoryIncreaseAction = _onInventoryIncreaseAction;
    }

    public void ResetPlayer(Vector3 _originPos)
    {
        transform.position = _originPos;
        prevDir = Vector2.zero;
        curDir = Vector2.zero;
    }

    public void HandleMovement()
    {
        if (IsGameStop)
            return;

        if (curDir == Vector2.zero || prevDir == -curDir)
            return;

        Vector2 _nextPosition = new Vector2(transform.position.x, transform.position.z) + curDir;

        if (iBlockManager.CheckBlockExistence(_nextPosition))
        {
            var curBlockType = iBlockManager.DequeueBlockAndGetBlockInterface(); // 현재 위치의 블럭 제거
            onInventoryIncreaseAction?.Invoke(curBlockType);
            OnBlockProcessed?.Invoke();
            transform.position = new Vector3(_nextPosition.x, transform.position.y, _nextPosition.y);
            prevDir = curDir;
            updateModelForwardAction?.Invoke(curDir);
            NotifyObservers(curBlockType);
        }
        else
        {
            iBlockManager.DequeueBlock(); // 블럭만 제거
            transform.position = new Vector3(_nextPosition.x, transform.position.y, _nextPosition.y);
            Debug.Log("Game Over: No block in the intended direction.");
            onGameOverAction?.Invoke(); // 게임 종료를 알림
        }
    }

    private void Update()
    {
        // pc테스트용
        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnUpButtonPressed();
        }
        else if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnDownButtonPressed();
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnRightButtonPressed();
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnLeftButtonPressed();
        }
    }

    // 방향 버튼이 눌렸을 때 호출되는 메서드
    public void OnUpButtonPressed()
    {
        curDir = Vector2.up;
        HandleMovement();
    }

    public void OnDownButtonPressed()
    {
        curDir = Vector2.down;
        HandleMovement();
    }

    public void OnLeftButtonPressed()
    {
        curDir = Vector2.left;
        HandleMovement();
    }

    public void OnRightButtonPressed()
    {
        curDir = Vector2.right;
        HandleMovement();
    }

    public void RegisterObserver(IPlayerMoveObserver _observer)
    {
        if (!observerList.Contains(_observer))
        {
            observerList.Add(_observer);
        }
    }

    public void UnregisterObserver(IPlayerMoveObserver _observer)
    {
        if (observerList.Contains(_observer))
        {
            observerList.Remove(_observer);
        }
    }

    public void NotifyObservers(in EBlockType _blockType)
    {
        foreach (var observer in observerList)
        {
            observer.OnNotify(_blockType);
        }
    }
}
