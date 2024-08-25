using System;
using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float maxHP;            // 최대 체력
    private float curHP;            // 현재 체력
    private float decreaseFactor;   // 체력 감소 계수
    private float baseDecreaseRate; // 초기 체력 감소 속도

    private int blocksTraveled;     // 이동한 블럭 수

    private bool isFeverTime = false;

    private Action onGameOverAction;


    public bool IsGameStop { get; set; } = true;

    // 초기화 메서드
    public void Init(float _maxHP, float _baseDecreaseRate, float _decreaseFactor, Action _onGameOverAction)
    {
        maxHP = _maxHP;
        curHP = _maxHP;
        baseDecreaseRate = _baseDecreaseRate;
        decreaseFactor = _decreaseFactor;
        blocksTraveled = 0;
        onGameOverAction = _onGameOverAction;
        isFeverTime = false;
    }

    public void ResetPlayer()
    {
        RecoverHP();

        isFeverTime = false;
        blocksTraveled = 0;
    }

    // 체력을 감소시키는 메서드
    public float DecreaseHP()
    {
        if (IsGameStop)
            return curHP;

        if (isFeverTime)
            return curHP;

        // 이동한 블럭 수에 따라 감소 계수 증가
        float decreaseRate = baseDecreaseRate + ((blocksTraveled / 10) * decreaseFactor);
        curHP -= decreaseRate * Time.deltaTime;

        // 체력이 0 이하가 되면 0으로 설정
        if (curHP <= 0)
        {
            curHP = 0;
            Debug.Log("Game Over: No Hp.");
            onGameOverAction?.Invoke();
        }
        return curHP;
    }

    // 체력을 최대 체력으로 회복하는 메서드
    public void RecoverHP()
    {
        curHP = maxHP;
    }

    // 현재 체력을 반환하는 메서드
    public float GetCurrentHealth()
    {
        return curHP;
    }

    // 최대 체력을 반환하는 메서드
    public float GetMaxHealth()
    {
        return maxHP;
    }

    public void StartFever()
    {
        isFeverTime = true;
    }

    public void StopFever()
    {
        isFeverTime = false;
    }
}
