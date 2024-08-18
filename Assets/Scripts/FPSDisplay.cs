using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText; // FPS를 표시할 UI 텍스트

    private float deltaTime = 0.0f;

    private void Start()
    {
        //QualitySettings.vSyncCount = 0;  // VSync 끄기
        //Application.targetFrameRate = 60;
    }

    void Update()
    {
        // 델타 타임을 이용해 FPS 계산
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;

        // 텍스트 업데이트
        if (fpsText != null)
        {
            fpsText.text = string.Format("FPS: {0:0.}", fps);
        }
    }
}
