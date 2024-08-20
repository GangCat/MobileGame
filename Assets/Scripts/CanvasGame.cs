using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasGame : MonoBehaviour
{
    [SerializeField]
    private SpeedLineImage speedLineImage = null;
    public void Init()
    {
        speedLineImage.Init();
    }

    public void StartSpeedLine()
    {
        speedLineImage.ShowSpeedLine();
    }

    public void FinishSpeedLine()
    {
        speedLineImage.HideSpeedLine();
    }
}
