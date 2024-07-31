using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI comboText = null;
    [SerializeField]
    private Canvas resultCanvas = null;
    [SerializeField]
    private TextMeshProUGUI clearText = null;
    [SerializeField]
    private TextMeshProUGUI failText = null;
    [SerializeField]
    private Slider slider = null;

    private uint comboCount = 0;
    private float playerHp = 200;
    private float playerMaxHp = 200;

    private Action playerHpZeroAction = null;

    public void Init(Action _playerHpZeroAction)
    {
        UpdateClickCount(0);
        slider.maxValue = playerHp;
        slider.value = playerHp;
        playerHpZeroAction = _playerHpZeroAction;

    }

    public void OnMoveSuccess()
    {
        //comboCount++;
        //UpdateClickCount();
    }

    public void OnClearAction()
    {
        resultCanvas.gameObject.SetActive(true);
        clearText.enabled = true;
    }

    public void OnFailAction()
    {
        resultCanvas.gameObject.SetActive(true);
        failText.enabled = true;
    }

    public void UpdateClickCount(float _clickCount)
    {
        comboText.text = _clickCount.ToString("F0") + "Combo";
    }

    internal void UpdateHp()
    {
        playerHp -= 0.2f;
        slider.value = playerHp;

        if (playerHp <= 0)
            playerHpZeroAction?.Invoke();
    }

    public void Heal()
    {
        playerHp = Mathf.Clamp(playerHp += 40, 0, playerMaxHp);
        
    }
}
