using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CanvasResult : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI blockCountText;
    [SerializeField]
    private TextMeshProUGUI goldCountText;
    [SerializeField]
    private TextMeshProUGUI diamondCountText;

    public void SetActive(bool _active)
    {
        gameObject.SetActive(_active);
    }

    public void SetCount(int _blockCount, int _goldCount, int _diamondCount)
    {
        blockCountText.text = _blockCount.ToString();
        goldCountText.text = _goldCount.ToString();
        diamondCountText.text = _diamondCount.ToString();
    }
}
