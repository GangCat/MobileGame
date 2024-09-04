using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibrationButton : ButtonBase
{
    [SerializeField]
    private Text text = null;

    public void ChangeTextColor(Color _textColor)
    {
        text.color = _textColor;
    }
}
