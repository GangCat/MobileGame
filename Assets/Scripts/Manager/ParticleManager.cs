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
            case EBlockType.FEVER_BUFF:
                StartCoroutine(nameof(SpawnFeverParticleCoroutine));
                break;
            default:
                break;
        }
    }

    public void OnNotify(in EBlockType _blockType)
    {
        SpawnParticle(_blockType);
    }

    private void SpawnParticleAndInit(string _particlePath, Vector3 _particlePos)
    {
        var greenParticleGo = poolManager.GetObject(_particlePath);
        greenParticleGo.GetComponent<ParticleObject>().Init(_particlePos, poolManager);
    }

    private IEnumerator SpawnFeverParticleCoroutine()
    {
        while (true)
        {

            yield return null;
        }
    }

    public void OnFeverNotify(in bool _isFeverStart)
    {
        if (!_isFeverStart)
        {
            StopCoroutine(nameof(SpawnFeverParticleCoroutine));
        }
    }
}
