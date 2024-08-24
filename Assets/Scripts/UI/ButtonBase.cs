using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBase : MonoBehaviour
{
    private Button btn = null;
    public virtual void Init()
    {
        btn = GetComponent<Button>();
    }

    public virtual void SetOnClickAction(Action action)
    {
        btn.onClick.AddListener(() => action?.Invoke());
    }
}
