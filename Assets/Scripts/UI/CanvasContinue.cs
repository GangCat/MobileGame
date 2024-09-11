using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasContinue : MonoBehaviour
{
    [SerializeField]
    private ButtonBase adBtn = null;
    [SerializeField]
    private ButtonBase noBtn = null;
    public void Init()
    {
        adBtn.Init();
        noBtn.Init();
        gameObject.SetActive(false);
    }
}
