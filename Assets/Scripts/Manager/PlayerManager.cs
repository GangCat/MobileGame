using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour, IArrowButtonClickObserver, IFadeOutFinishObserver, IFeverObserver
{
    [SerializeField]
    private ModelPartTrail[] modelPartTrailArr;

    private PlayerMovement movement;
    private PlayerInventory inventory;
    private PlayerHealth health;
    private PlayerModelController model;
    private Action<int, int> gameOverAction;

    public void Init(IWalkableBlockManager _blockManager, Action<int, int> _gameOverAction)
    {
        movement = GetComponent<PlayerMovement>();
        inventory = GetComponent<PlayerInventory>();
        health = GetComponent<PlayerHealth>();
        model = GetComponentInChildren<PlayerModelController>();
        gameOverAction = _gameOverAction;

        model.Init();
        movement.Init(_blockManager, onGameOver, inventory.ProcessBlock, model.UpdateModelForward);
        inventory.Init();
        health.Init(100, 100, 0.5f, onGameOver);

        movement.OnBlockProcessed += health.RecoverHP;
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

    public void StartGame()
    {
        movement.IsGameStop = false;
        health.IsGameStop = false;
    }

    public float UpdatePlayerHP()
    {
        return health.DecreaseHP();
    }

    public void onGameOver()
    {
        // 이동 막는 내용
        movement.IsGameStop = true;
        health.IsGameStop = true;
        gameOverAction?.Invoke(inventory.GetGoldCount(), inventory.GetDiamondCount());
    }

    public void OnNotify(in EArrowButtonType _arrowType)
    {
        movement.OnArrowButtonpressed(_arrowType);
    }

    public void OnNotify()
    {
        ResetPlayer();
    }

    public void OnNotify(in bool _isFeverStart)
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
}
