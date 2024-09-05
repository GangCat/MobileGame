using System.Collections;
using UnityEngine;

public class ParticleManager : MonoBehaviour, IPlayerMoveObserver, IFeverObserver
{
    [SerializeField]
    private string greenBoxParticlePath = "";
    [SerializeField]
    private string doubleBoxParticlePath = "";
    [SerializeField]
    private string TripleBoxParticlePath = "";
    [SerializeField]
    private GameObject[] feverParticleGOArr = null;

    private ObjectPoolManager poolManager;
    private Transform playerTr;

    // 파티클 새로 만들기
    // 도착 이펙트보다 차라리 이전 위치에서 현 위치까지 빠르게 이동한 것을 보여주는 파티클(발구름, 잔상 등)이 필요할 듯

    public void Init(ObjectPoolManager _poolManager, Transform _playerTr)
    {
        poolManager = _poolManager;
        playerTr = _playerTr;

        poolManager.PrepareObjects(greenBoxParticlePath);
        poolManager.PrepareObjects(doubleBoxParticlePath);
        poolManager.PrepareObjects(TripleBoxParticlePath);

        foreach (var go in feverParticleGOArr)
            go.SetActive(false);
    }

    public void SpawnParticle(EBlockType _particleType)
    {
        switch (_particleType)
        {
            case EBlockType.NORMAL:
                SpawnParticleAndInit(greenBoxParticlePath, playerTr.position);
                break;
            case EBlockType.DOUBLE_SCORE:
                SpawnParticleAndInit(doubleBoxParticlePath, playerTr.position);
                break;
            case EBlockType.TRIPLE_SCORE:
                SpawnParticleAndInit(TripleBoxParticlePath, playerTr.position);
                break;
            default:
                break;
        }
    }

    public void OnPlayerMoveNotify(in EBlockType _blockType)
    {
        SpawnParticle(_blockType);
    }

    private void SpawnParticleAndInit(string _particlePath, Vector3 _particlePos)
    {
        var greenParticleGo = poolManager.GetObject(_particlePath);
        greenParticleGo.GetComponent<ParticleObject>().Init(_particlePos, poolManager);
    }

    public void OnFeverNotify(in bool _isFeverStart)
    {
        foreach (var feverParticleGO in feverParticleGOArr)
            feverParticleGO.SetActive(_isFeverStart);
    }
}
