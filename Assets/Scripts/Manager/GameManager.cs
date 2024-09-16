using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameOverObserver
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
    private VibrateManager vibMng = null;
    private AdManager adMng = null;
    private int deadCount = 0;
    private Cam cam = null;

    private void Awake()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

    private void Start()
    {
        Debug.Log("이어아" + QualitySettings.vSyncCount);
        playerMng = FindObjectOfType<PlayerManager>();
        walkableBlockMng = FindObjectOfType<WalkableBlockManager>();
        uiMng = FindObjectOfType<UIManager>();
        particleMng = FindObjectOfType<ParticleManager>();
        objectPoolMng = FindObjectOfType<ObjectPoolManager>();
        scoreMng = FindObjectOfType<ScoreManager>();
        audioMng = FindObjectOfType<AudioManager>();
        enemyMng = FindObjectOfType<EnemyManager>();
        feverMng = FindObjectOfType<FeverManager>();
        vibMng = FindObjectOfType<VibrateManager>();
        adMng = FindObjectOfType<AdManager>();
        cam = FindObjectOfType<Cam>();

        enemyMng.Init(objectPoolMng);
        playerMng.Init(walkableBlockMng);
        walkableBlockMng.Init(objectPoolMng, enemyMng.GenEnemy);
        uiMng.Init(StartGameCountdown, SetVibe);
        particleMng.Init(objectPoolMng, playerMng.transform);
        scoreMng.Init(uiMng.UpdateScore);
        audioMng.Init();
        feverMng.Init();
        vibMng.Init();
        adMng.Init(audioMng.PauseBGM, audioMng.PlayBGM);

        playerMng.ResetPlayer();
        walkableBlockMng.ResetBlock();

        playerMng.RegisterPlayerMoveObserver(particleMng);
        playerMng.RegisterPlayerMoveObserver(scoreMng);
        playerMng.RegisterPlayerMoveObserver(audioMng);
        playerMng.RegisterPlayerMoveObserver(cam);
        playerMng.RegisterPlayerMoveObserver(feverMng);
        playerMng.RegisterPlayerMoveObserver(walkableBlockMng);
        playerMng.RegisterPlayerMoveObserver(vibMng);

        audioMng.PlayMenuBackgroundMusic();

        uiMng.RegisterArrowClickObserver(playerMng);

        uiMng.RegisterVolumeChangeObserver(audioMng);

        uiMng.RegisterFadeFinishObserver(playerMng);
        uiMng.RegisterFadeFinishObserver(scoreMng);
        uiMng.RegisterFadeFinishObserver(walkableBlockMng);
        uiMng.RegisterFadeFinishObserver(audioMng);
        uiMng.RegisterFadeFinishObserver(cam);

        feverMng.RegisterFeverObserver(playerMng);
        feverMng.RegisterFeverObserver(uiMng);
        feverMng.RegisterFeverObserver(scoreMng);
        feverMng.RegisterFeverObserver(particleMng);
        feverMng.RegisterFeverObserver(audioMng);
        feverMng.RegisterFeverObserver(walkableBlockMng);

        playerMng.RegisterGameOverObserver(this);
        playerMng.RegisterGameOverObserver(cam);
        playerMng.RegisterGameOverObserver(audioMng);

        scoreMng.RegisterMultiScoreObserver(uiMng);
    }

    private void SetVibe(bool _isVibrate)
    {
        vibMng.SetIsVibrate(_isVibrate);
    }

    private void Update()
    {
        var curHP = playerMng.UpdatePlayerHP();
        uiMng.UpdatePlayerHP(curHP);
    }

    private void HandleGameOver()
    {
        ++deadCount;
        uiMng.GameOver(scoreMng.CalcResult());
        Debug.Log("Game Over: Handling game over in GameManager.");

        if(deadCount > 2)
        {
            adMng.StartAd();
            deadCount = 0;
        }
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
        audioMng.ChangeToGameBackgroundMusic();
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

    public void OnGameOverNotify()
    {
        HandleGameOver();
    }
}
