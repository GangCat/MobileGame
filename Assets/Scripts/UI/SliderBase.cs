using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBase : MonoBehaviour
{
    protected Slider slider = null;

    public float GetValue => slider.value;

    public virtual void Init()
    {
        slider = GetComponent<Slider>();
    }

    public void SetOnValueChangeAction(Action<float> _action)
    {
        slider.onValueChanged.AddListener((f) => { _action?.Invoke(f); });
    }

    public void AddValue(float _value)
    {
        slider.value += _value;
    }

}
