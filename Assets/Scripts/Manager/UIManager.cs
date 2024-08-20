using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Slider hpSlider = null;
    [SerializeField]
    private TextMeshProUGUI textMeshProUGUI = null;
    [SerializeField]
    private TextMeshProUGUI countdownText = null;

    [SerializeField]
    private GameObject gameUI;
    [SerializeField]
    private CanvasResult canvasResult;
    [SerializeField]
    private CanvasLobby canvasLobby;
    [SerializeField]
    private CanvasGame canvasGame = null;

    public void Init()
    {
        hpSlider.value = 100;
        textMeshProUGUI.text = "0";
        canvasLobby.SetActive(true);
        gameUI.SetActive(false);
        canvasResult.SetActive(false);

        canvasGame.Init();
    }

    public void StartSpeedLine()
    {
        canvasGame.StartSpeedLine();
    }

    public void FinishSpeedLine()
    {
        canvasGame.FinishSpeedLine();
    }

    public void UpdatePlayerHP(float _curHP)
    {
        hpSlider.value = _curHP;
    }

    public void UpdateScore(int _score)
    {
        textMeshProUGUI.text = _score.ToString();
    }

    public void StartGame()
    {
        countdownText.gameObject.SetActive(false);
    }

    public void Countdown(int _count)
    {
        countdownText.text = _count.ToString();
    }

    public void ShowGameUI()
    {
        canvasLobby.SetActive(false);
        gameUI.SetActive(true);
        countdownText.gameObject.SetActive(true);
    }

    public void GameOver(int _goldCnt, int _diaCnt, int _comboCnt)
    {
        gameUI.SetActive(false);
        canvasResult.SetActive(true);
        canvasResult.SetCount(_comboCnt, _goldCnt, _diaCnt);
    }

    public void EnterLobby()
    {
        canvasResult.SetActive(false);
        canvasLobby.SetActive(true);
    }

}
