using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class MultiScoreTextEffect : MonoBehaviour
{
    private TextMeshProUGUI effectText = null;
    private StringBuilder sb = null;
    public void Init()
    {
        effectText = GetComponentInChildren<TextMeshProUGUI>();
        gameObject.SetActive(false);
        sb = new StringBuilder();
    }

    public void StartEffect(int _multiplytext)
    {
        sb.Append("X");
        sb.Append(_multiplytext);
        effectText.text = sb.ToString();
        sb.Clear();

        gameObject.SetActive(true);
    }

    public void FinishEffect()
    {
        gameObject.SetActive(false);
    }
}
