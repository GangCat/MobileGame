using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour, IPlayerMoveSubject
{
    [SerializeField]
    private float moveOffset = 0f;

    private Vector2 prevDir = Vector2.zero;
    private Vector2 curDir = Vector2.zero; // 현재 방향을 저장하는 변수
    private Vector3 playerOriginPos = Vector3.zero; // 플레이어의 처음 위치 저장

    private Action onGameOverAction = null; // 게임 종료를 알리는 액션
    private Action<EBlockType> onInventoryIncreaseAction = null; // 도착한 블럭의 타입에 따라 재화를 상승시키는 액션.
    private Action<Vector2> updateModelForwardAction = null; // 모델의 정면 방향을 알려주는 액션

    private IWalkableBlockManager iBlockManager = null;

    private List<IPlayerMoveObserver> observerList = new List<IPlayerMoveObserver>();

    [SerializeField]
    private bool isFeverTime = false;



    public event Action OnBlockProcessed; // 블럭을 한 칸 이동할때마다 호출할 이벤트
    public bool IsGameStop { get; set; } = true;



    public void Init(IWalkableBlockManager _iBlockManager, Action _onGameOverAction, Action<EBlockType> _onInventoryIncreaseAction, Action<Vector2> _updateModelForwardAction)
    {
        prevDir = Vector2.zero;
        curDir = Vector2.zero;
        iBlockManager = _iBlockManager;
        onGameOverAction = _onGameOverAction;
        updateModelForwardAction = _updateModelForwardAction;
        onInventoryIncreaseAction = _onInventoryIncreaseAction;
        isFeverTime = false;

        playerOriginPos = transform.position;
    }

    public void StartFever()
    {
        isFeverTime = true;
    }

    public void StopFever()
    {
        isFeverTime = false;
    }

    public void ResetPlayer()
    {
        transform.position = playerOriginPos;
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

    public void OnArrowButtonpressed(EArrowButtonType _arrowType)
    {
        if (isFeverTime)
        {
            // 무조건 다음 블럭 방향으로 이동
            Vector3 nextDir = iBlockManager.GetNextBlockDir();
            if(nextDir == Vector3.forward)
                OnUpButtonPressed();
            else if(nextDir == Vector3.right)
                OnRightButtonPressed();
            else if(nextDir == Vector3.left)
                OnLeftButtonPressed();
            else
                OnDownButtonPressed();

            return;
        }
        switch (_arrowType)
        {
            case EArrowButtonType.UP:
                OnUpButtonPressed();
                break;
            case EArrowButtonType.LEFT:
                OnLeftButtonPressed();
                break;
            case EArrowButtonType.RIGHT:
                OnRightButtonPressed();
                break;
            case EArrowButtonType.DOWN:
                OnDownButtonPressed();
                break;
        }
    }

    // 방향 버튼이 눌렸을 때 호출되는 메서드
    public void OnUpButtonPressed()
    {
        curDir = Vector2.up * moveOffset;
        HandleMovement();
    }

    public void OnDownButtonPressed()
    {
        curDir = Vector2.down * moveOffset;
        HandleMovement();
    }

    public void OnLeftButtonPressed()
    {
        curDir = Vector2.left * moveOffset;
        HandleMovement();
    }

    public void OnRightButtonPressed()
    {
        curDir = Vector2.right * moveOffset;
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
