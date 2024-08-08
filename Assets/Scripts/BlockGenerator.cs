using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BlockGenerator : MonoBehaviour, IBlockGenerator
{
    [SerializeField]
    private int forwardProbability = 70; // 진행방향 확률
    [SerializeField]
    private int minGoldBlockInterval = 20; // 골드 블럭 최소 간격
    [SerializeField]
    private int minDiamondBlockInterval = 100; // 다이아 블럭 최소 간격
    [SerializeField]
    private float goldBlockProbabilityIncrement = 0.05f; // 골드 블럭 확률 상승 계수
    [SerializeField]
    private float diamondBlockProbabilityIncrement = 0.02f; // 다이아 블럭 확률 상승 계수

    [SerializeField]
    private string blockPrefabPath;
    [SerializeField]
    private string normalBlockMatPath;
    [SerializeField]
    private string goldBlockMatPath;
    [SerializeField]
    private string diamondBlockMatPath;

    private int goldBlockCounter = 0; // 마지막 골드 블럭 이후 생성된 블럭 수
    private int diamondBlockCounter = 0; // 마지막 다이아 블럭 이후 생성된 블럭 수
    private float goldBlockProbability = 0f; // 현재 골드 블럭 생성 확률
    private float diamondBlockProbability = 0f; // 현재 다이아 블럭 생성 확률

    private Vector2 curBlockDir = Vector2.zero;
    private System.Random random;
    private Vector2 curBlockPosition = Vector2.zero;

    private Vector2[] blockDirArr = new Vector2[3]; // 벡터 배열을 미리 선언
    private int sideProbability = 0; // 좌우 방향 확률
    private ObjectPoolManager poolManager = null;
    private int originForwardProbability = 0;


    // 그러면 생성할 위치가 이미 블럭이 있는지 예외처리 해야함.
    // 그럼 블럭 있는지 확인하는 내용은 언제? 다음 방향 결정할 때.


    public void Init(ObjectPoolManager _poolManager)
    {
        curBlockDir = Vector2.right; // 초기 방향 설정
        random = new System.Random();
        curBlockPosition = Vector2.zero;
        poolManager = _poolManager;
        originForwardProbability = forwardProbability;

        poolManager.PrepareObjects(blockPrefabPath, 7);

        UpdateDirections();

        // 좌우 방향 확률 계산
        sideProbability = (100 - forwardProbability) / 2;
    }

    public void ResetBlock()
    {
        curBlockDir = Vector2.right; // 초기 방향 설정
        random = new System.Random();
        curBlockPosition = Vector2.zero;

        goldBlockCounter = 0;
        diamondBlockCounter = 0;
        goldBlockProbability = 0f;
        diamondBlockProbability = 0f;

        UpdateDirections();

        forwardProbability = originForwardProbability;
        sideProbability = (100 - forwardProbability) / 2;
    }

    public WalkableBlock CreateBlock(Vector2 _position, EBlockType _blockType)
    {
        var walkableBlockGo = poolManager.GetObject(blockPrefabPath);
        WalkableBlock block = walkableBlockGo.AddComponent<WalkableBlock>();

        // 머티리얼 로드 및 적용
        string materialPath = GetMaterialPath(_blockType);
        Material loadedMaterial = LoadMaterialSynchronously(materialPath);
        if (loadedMaterial != null)
        {
            MeshRenderer _meshRenderer = walkableBlockGo.GetComponent<MeshRenderer>();
            if (_meshRenderer != null)
            {
                _meshRenderer.material = loadedMaterial;
            }
        }

        block.Init(_position, _blockType, poolManager, curBlockDir);

        return block;
    }

    private string GetMaterialPath(EBlockType _blockType)
    {
        switch (_blockType)
        {
            case EBlockType.GOLD:
                return goldBlockMatPath;
            case EBlockType.DIAMOND:
                return diamondBlockMatPath;
            default:
                return normalBlockMatPath;
        }
    }

    private Material LoadMaterialSynchronously(string _address)
    {
        // 비동기 핸들을 얻습니다.
        AsyncOperationHandle<Material> _handle = Addressables.LoadAssetAsync<Material>(_address);

        // 로드가 완료될 때까지 대기합니다.
        _handle.WaitForCompletion();

        if (_handle.Status == AsyncOperationStatus.Succeeded)
        {
            return _handle.Result;
        }
        else
        {
            Debug.LogError($"Failed to load material: {_address}");
            return null;
        }
    }

    private void UpdateDirections()
    {
        blockDirArr[0] = curBlockDir; // 진행방향
        blockDirArr[1].x = -curBlockDir.y;
        blockDirArr[1].y = curBlockDir.x; // 좌
        blockDirArr[2].x = curBlockDir.y;
        blockDirArr[2].y = -curBlockDir.x; // 우
    }

    public WalkableBlock GenerateStartBlock()
    {
        var block = CreateBlock(new Vector2(0f, 0f), EBlockType.NORMAL);
        return block;
    }

    public WalkableBlock GenerateNextBlock()
    {
        Vector2 newPosition = Vector2.zero;

        // 이전에 설정된 방향으로 블럭 생성
        newPosition = curBlockPosition + blockDirArr[0];

        EBlockType blockType = EBlockType.NORMAL;
        if (goldBlockCounter >= minGoldBlockInterval)
        {
            goldBlockProbability += goldBlockProbabilityIncrement;
            if (random.NextDouble() < goldBlockProbability)
            {
                blockType = EBlockType.GOLD;
                goldBlockCounter = 0;
                goldBlockProbability = 0f;
            }
        }

        if (diamondBlockCounter >= minDiamondBlockInterval)
        {
            diamondBlockProbability += diamondBlockProbabilityIncrement;
            if (random.NextDouble() < diamondBlockProbability)
            {
                blockType = EBlockType.DIAMOND;
                diamondBlockCounter = 0;
                diamondBlockProbability = 0f;
            }
        }

        // 블럭 생성 및 스택에 추가

        if (blockType == EBlockType.NORMAL)
        {
            goldBlockCounter++;
            diamondBlockCounter++;
        }


        int randomValue = random.Next(0, 100);

        // 정면 방향이 아닐 경우 좌우 결정
        if (randomValue < forwardProbability)
        {
            forwardProbability -= 1;
        }
        else if (randomValue < forwardProbability + sideProbability)
        {
            curBlockDir = blockDirArr[1]; // 새로운 진행 방향 설정
            forwardProbability = originForwardProbability;
            UpdateDirections();
        }
        else
        {
            curBlockDir = blockDirArr[2]; // 새로운 진행 방향 설정
            forwardProbability = originForwardProbability;
            UpdateDirections();
        }


        // 블럭 생성 및 스택에 추가
        var walkableBlock = CreateBlock(newPosition, blockType);
        curBlockPosition = newPosition; // 현재 위치 업데이트

        return walkableBlock;
    }
}
