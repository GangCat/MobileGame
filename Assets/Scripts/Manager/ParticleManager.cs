using UnityEngine;

public class ParticleManager : MonoBehaviour, IPlayerMoveObserver
{
    [SerializeField]
    private string greenBoxParticlePath = "";
    [SerializeField]
    private string goldBoxParticlePath = "";
    [SerializeField]
    private string diaBoxParticlePath = "";

    private ObjectPoolManager poolManager;
    private Transform playerTr;

    public void Init(ObjectPoolManager _poolManager, Transform _playerTr)
    {
        poolManager = _poolManager;
        playerTr = _playerTr;

        poolManager.PrepareObjects(greenBoxParticlePath);
        poolManager.PrepareObjects(goldBoxParticlePath);
        poolManager.PrepareObjects(diaBoxParticlePath);
    }

    public void SpawnParticle(EBlockType _particleType)
    {
        switch (_particleType)
        {
            case EBlockType.NORMAL:
                SpawnParticleAndInit(greenBoxParticlePath, playerTr.position);
                break;
            case EBlockType.GOLD:
                SpawnParticleAndInit(goldBoxParticlePath, playerTr.position);
                break;
            case EBlockType.DIAMOND:
                SpawnParticleAndInit(diaBoxParticlePath, playerTr.position);
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
}
