using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerInventory inventory;
    private PlayerHealth health;
    private Action<int, int> gameOverAction;

    public void Init(IWalkableBlockManager _blockManager, Action<int, int> _gameOverAction)
    {
        movement = GetComponent<PlayerMovement>();
        inventory = GetComponent<PlayerInventory>();
        health = GetComponent<PlayerHealth>();
        gameOverAction = _gameOverAction;

        movement.Init(_blockManager, onGameOver, inventory.ProcessBlock);
        inventory.Init();
        health.Init(100, 100, 0.5f, onGameOver);

        movement.OnBlockProcessed += health.IncrementBlocksTraveled;
        movement.OnBlockProcessed += health.RecoverHP;
    }

    public void RegisterPlayerMoveObserver(IPlayerMoveObserver _observer)
    {
        movement.RegisterObserver(_observer);
    }

    public void ResetPlayer(Vector3 _originPos)
    {
        movement.ResetPlayer(_originPos);
        inventory.ResetPlayer();
        health.ResetPlayer();
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
        var curHP = health.DecreaseHP();
        if (health.GetCurrentHealth() <= 0)
        {
            Debug.Log("Game Over: Player ran out of health.");
            // 게임 패배 처리
        }
        return curHP;
    }

    public void onGameOver()
    {
        // 이동 막는 내용
        movement.IsGameStop = true;
        health.IsGameStop = true;
        gameOverAction?.Invoke(inventory.GetGoldCount(), inventory.GetDiamondCount());
    }
}