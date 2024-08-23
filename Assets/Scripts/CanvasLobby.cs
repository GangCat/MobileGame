using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasLobby : MonoBehaviour
{
    public void Init()
    {
        gameObject.SetActive(true);
    }

    public void EnterLobby()
    {
        gameObject.SetActive(true);
    }

    public void ShowGameUI()
    {
        gameObject.SetActive(false);
    }
}
