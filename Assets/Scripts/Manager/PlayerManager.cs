using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IArrowButtonClickObserver, IFadeOutFinishObserver, IFeverObserver, IGameOverSubject
{
    [SerializeField]
    private ModelPartTrail[] modelPartTrailArr;

    private PlayerMovement movement;
    private PlayerInventory inventory;
    private PlayerHealth health;
    private PlayerModelController model;

    private List<IGameOverObserver> gameoverObserverList = null;

    public void Init(IWalkableBlockManager _blockManager)
    {
        movement = GetComponent<PlayerMovement>();
        inventory = GetComponent<PlayerInventory>();
        health = GetComponent<PlayerHealth>();
        model = GetComponentInChildren<PlayerModelController>();
        gameoverObserverList = new();

        model.Init();
        movement.Init(_blockManager, onGameOver, inventory.ProcessBlock, model.UpdateModelForward);
        inventory.Init();
        health.Init(100, 100, 0.5f, onGameOver);

        movement.OnBlockProcessed += health.RecoverHP;

        RegisterGameOverObserver(model);
        RegisterGameOverObserver(movement);
        RegisterGameOverObserver(health);

    }

    public void RegisterPlayerMoveObserver(IPlayerMoveObserver _observer)
    {
        movement.RegisterObserver(_observer);
    }

    public void ResetPlayer()
    {
        movement.ResetPlayer();
        inventory.ResetPlayerInventory();
        health.ResetPlayer();
        model.ResetModelForward();
        foreach (var modelPartTrail in modelPartTrailArr)
            modelPartTrail.ResetPos();
    }

    public Vector3 getCurPos()
    {
        return gameObject.transform.position;
    }

    /// <summary>
    /// 이동할 수 있는 순간을 알려줌
    /// 리셋 플레이어때는 로비여서 움직이면 안됨
    /// </summary>
    public void StartGame()
    {
        movement.IsGameOver = false;
        health.IsGameStop = false;
    }

    public float UpdatePlayerHP()
    {
        return health.DecreaseHP();
    }

    public void onGameOver()
    {
        NotifyObserversGameOver();
    }

    public void OnArrowButtonClickNotify(in EArrowButtonType _arrowType)
    {
        movement.OnArrowButtonpressed(_arrowType);
    }

    public void OnFadeOutFinishNotify()
    {
        ResetPlayer();
    }

    public void OnFeverNotify(in bool _isFeverStart)
    {
        if(_isFeverStart is true)
        {
            movement.StartFever();
            health.StartFever();
        }
        else
        {
            movement.StopFever();
            health.StopFever();
        }
    }

    public void RegisterGameOverObserver(IGameOverObserver _observer)
    {
        gameoverObserverList ??= new();
        if(!gameoverObserverList.Contains(_observer))
            gameoverObserverList.Add(_observer);
    }

    public void UnregisterGameOverObserver(IGameOverObserver _observer)
    {
        if (!gameoverObserverList.Contains(_observer))
            gameoverObserverList.Remove(_observer);
    }

    public void NotifyObserversGameOver()
    {
        foreach (var observer in gameoverObserverList)
            observer.OnGameOverNotify();
    }
}
