using UnityEngine;

public class LetterBoxCamera : MonoBehaviour
{
    // 원하는 화면 비율
    public float targetAspect = 9.0f / 16.0f;

    void Start()
    {
        // 현재 화면 비율 계산
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // 스케일 패크터 계산
        float scaleHeight = windowAspect / targetAspect;

        // 카메라 컴포넌트 가져오기
        Camera camera = GetComponent<Camera>();

        // 화면 비율이 목표 비율보다 작을 때
        if (scaleHeight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            camera.rect = rect;
        }
        else // 화면 비율이 목표 비율보다 클 때
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = camera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }
}
