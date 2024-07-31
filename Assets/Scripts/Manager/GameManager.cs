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
    private Cam cam = null;

    private Vector3 playerOriginPos;

    private void Start()
    {
        playerMng = FindObjectOfType<PlayerManager>();
        walkableBlockMng = FindObjectOfType<WalkableBlockManager>();
        uiMng = FindObjectOfType<UIManager>();
        particleMng = FindObjectOfType<ParticleManager>();
        objectPoolMng = FindObjectOfType<ObjectPoolManager>();
        scoreMng = FindObjectOfType<ScoreManager>();
        audioMng = FindObjectOfType<AudioManager>();
        cam = FindObjectOfType<Cam>();

        playerMng.Init(walkableBlockMng, HandleGameOver);
        walkableBlockMng.Init(objectPoolMng);
        uiMng.Init();
        particleMng.Init(objectPoolMng, playerMng.transform);
        scoreMng.Init(uiMng.UpdateScore);
        audioMng.Init();

        playerOriginPos = playerMng.getCurPos();

        playerMng.ResetPlayer(playerOriginPos);
        walkableBlockMng.ResetBlock();

        playerMng.RegisterPlayerMoveObserver(particleMng);
        playerMng.RegisterPlayerMoveObserver(scoreMng);
        playerMng.RegisterPlayerMoveObserver(audioMng);
        playerMng.RegisterPlayerMoveObserver(cam);

        audioMng.PlayMenuBackgroundMusic();
    }

    private void Update()
    {
        var curHP = playerMng.UpdatePlayerHP();
        uiMng.UpdatePlayerHP(curHP);
    }

    private void HandleGameOver(int _goldCnt, int _diaCnt)
    {
        uiMng.GameOver(_goldCnt, _diaCnt, scoreMng.CurScore);
        Debug.Log("Game Over: Handling game over in GameManager.");
        // 게임 종료 처리 로직
    }

    public void StartGameCountdown()
    {
        //playerManager.ResetPlayer(playerOriginPos);
        //walkableBlockManager.ResetBlock();

        uiMng.ShowGameUI();
        StartCoroutine(CountdownCoroutine());
    }

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
    }

    private void HandleBlockProcessed(EBlockType _blockType)
    {
        // 블럭 타입에 따른 추가 로직
    }

    public void CloseResultAndGoLobby()
    {
        uiMng.EnterLobby();
        playerMng.ResetPlayer(playerOriginPos);
        scoreMng.ResetScore();
        walkableBlockMng.ResetBlock();
        audioMng.PlayMenuBackgroundMusic();
    }
}
