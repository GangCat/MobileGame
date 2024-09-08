using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VibrationButton : ButtonBase
{
    [SerializeField]
    private TextMeshProUGUI text = null;

    public void ChangeTextColor(Color _textColor)
    {
        text.color = _textColor;
    }
}
