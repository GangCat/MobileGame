using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private PlayerManager playerMng = null;
    private WalkableBlockManager walkableBlockMng = null;
    private UIManager uiMng = null;
    private ParticleManager particleMng = null;
    private ObjectPoolManager objectPoolMng = null;
    private ScoreManager scoreMng = null;
    private AudioManager audioMng = null;
    private EnemyManager enemyMng = null;
    private FeverManager feverMng = null;
    private Cam cam = null;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;  // VSync 끄기
        Application.targetFrameRate = 120;

        playerMng = FindObjectOfType<PlayerManager>();
        walkableBlockMng = FindObjectOfType<WalkableBlockManager>();
        uiMng = FindObjectOfType<UIManager>();
        particleMng = FindObjectOfType<ParticleManager>();
        objectPoolMng = FindObjectOfType<ObjectPoolManager>();
        scoreMng = FindObjectOfType<ScoreManager>();
        audioMng = FindObjectOfType<AudioManager>();
        enemyMng = FindObjectOfType<EnemyManager>();
        feverMng = FindObjectOfType<FeverManager>();
        cam = FindObjectOfType<Cam>();

        enemyMng.Init(objectPoolMng);
        playerMng.Init(walkableBlockMng, HandleGameOver);
        walkableBlockMng.Init(objectPoolMng, enemyMng.GenEnemy);
        uiMng.Init(StartGameCountdown);
        particleMng.Init(objectPoolMng, playerMng.transform);
        scoreMng.Init(uiMng.UpdateScore, uiMng.StartSpeedLine, uiMng.FinishSpeedLine);
        audioMng.Init();
        feverMng.Init();

        playerMng.ResetPlayer();
        walkableBlockMng.ResetBlock();

        playerMng.RegisterPlayerMoveObserver(particleMng);
        playerMng.RegisterPlayerMoveObserver(scoreMng);
        playerMng.RegisterPlayerMoveObserver(audioMng);
        playerMng.RegisterPlayerMoveObserver(cam);
        playerMng.RegisterPlayerMoveObserver(feverMng);
        playerMng.RegisterPlayerMoveObserver(walkableBlockMng);

        audioMng.PlayMenuBackgroundMusic();

        uiMng.RegisterArrowClickObserver(playerMng);

        uiMng.RegisterFadeFinishObserver(playerMng);
        uiMng.RegisterFadeFinishObserver(scoreMng);
        uiMng.RegisterFadeFinishObserver(walkableBlockMng);
        uiMng.RegisterFadeFinishObserver(audioMng);
        uiMng.RegisterFadeFinishObserver(cam);

        feverMng.RegisterObserver(playerMng);
        feverMng.RegisterObserver(uiMng);
        feverMng.RegisterObserver(scoreMng);
    }

    private void Update()
    {
        var curHP = playerMng.UpdatePlayerHP();
        uiMng.UpdatePlayerHP(curHP);
    }

    private void HandleGameOver(int _goldCnt, int _diaCnt)
    {
        uiMng.GameOver(scoreMng.CalcResult());
        Debug.Log("Game Over: Handling game over in GameManager.");
        // 게임 종료 처리 로직
    }

    /// <summary>
    /// 게임시작버튼 눌렀을 때 호출
    /// </summary>
    public void StartGameCountdown()
    {
        //playerManager.ResetPlayer(playerOriginPos);
        //walkableBlockManager.ResetBlock();
        cam.MoveCamGamePos();
        uiMng.ShowGameUI();
        StartCoroutine(CountdownCoroutine());
    }

    /// <summary>
    /// 카운트다운하는 코루틴, 사운드랑 묶여있음.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CountdownCoroutine()
    {
        audioMng.FadeOutBackground();
        uiMng.Countdown(3);
        audioMng.PlayCountdownSFX(1);
        yield return new WaitForSeconds(1f);
        uiMng.Countdown(2);
        audioMng.PlayCountdownSFX(1.4f);
        yield return new WaitForSeconds(1f);
        uiMng.Countdown(1);
        audioMng.PlayCountdownSFX(1.8f);
        yield return new WaitForSeconds(1f);
        uiMng.StartGame();
        playerMng.StartGame();
        scoreMng.StartGame();
    }
}
