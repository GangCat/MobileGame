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

    private Vector2 currentDirection = Vector2.zero;
    private bool hasTurned = true;
    private System.Random random;
    private Vector2 currentPosition = Vector2.zero;

    private Vector2[] directionArr = new Vector2[3]; // 벡터 배열을 미리 선언
    private int sideProbability = 0; // 좌우 방향 확률
    private ObjectPoolManager poolManager = null;

    // 블럭 생성할 때 이동 방향으로 화살표 그리게 하기
    // 그리고 생성된 바로 다음에도 방향 전환하게 하기
    // 그러면 생성할 위치가 이미 블럭이 있는지 예외처리 해야함.


    // 생성 알고리즘을 조금 바꿔서
    // 지금은 지금 생성되는 블럭의 위치를 지금 결정하는데
    // 그렇게되면 이전 블럭에 꺾이는 화살표를 그릴 수 없음
    // 그러니까 블럭을 생성할 때 다음 블럭의 방향까지 미리 결정하고
    // 그 방향에 따라 블럭에 그려질 화살표를 결정
    // 그러면 방향결정할때 확률로 다음 블럭은 어디로 휠지 결정하고
    // 그걸 매번 갱신
    // 이 때 그 갱신 주기는 지금블럭 생성 이후에 하도록 구현
    // 일단 그림 하지말고 대충 스프라이트 하나로 해서 색을 빨주노 이런식으로 테스트

    // hasturned 없에고 관련 예외 없에면 바로바로 회전 가능함.
    // 대신 다음 위치에 블럭 있는지 확인해야함.
    // 그럼 블럭 있는지 확인하는 내용은 언제? 다음 방향 결정할 때.



    // 1. 초기화시기에 다음 블럭 방향 무조건 정면으로 결정.
    // 2. 블럭 생성시기에 이미 결정된 방향으로 블럭 생성.
    // 3. 블럭 생성 후 다음 블럭 방향 결정.
    // 3.1. 방향이 좌/우측 방향일 경우 현재 블럭에 표기되는 화살표를 해당 방향으로 휘어지는 화살표로 설정.
    // 3.2. 방향이 정면일 경우 정면 방향 화살표로 설정.
    // 이러면 화살표는 정면, 좌회전, 우회전 2개면 됨.(이미지 빨 파 초로 임시사용)

    public void Init(ObjectPoolManager _poolManager)
    {
        currentDirection = Vector2.right; // 초기 방향 설정
        hasTurned = true; // true로 해야 처음 생기는 블럭이 무조건 오른쪽에 생성됨
        random = new System.Random();
        currentPosition = Vector2.zero;
        poolManager = _poolManager;

        poolManager.PrepareObjects(blockPrefabPath, 7);

        UpdateDirections();

        // 좌우 방향 확률 계산
        sideProbability = (100 - forwardProbability) / 2;
    }

    public void ResetBlock()
    {
        currentDirection = Vector2.right; // 초기 방향 설정
        hasTurned = true; // true로 해야 처음 생기는 블럭이 무조건 오른쪽에 생성됨
        random = new System.Random();
        currentPosition = Vector2.zero;

        goldBlockCounter = 0;
        diamondBlockCounter = 0;
        goldBlockProbability = 0f;
        diamondBlockProbability = 0f;

        UpdateDirections();

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

        block.Init(_position, _blockType, poolManager, currentDirection);

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
        directionArr[0] = currentDirection; // 진행방향
        directionArr[1].x = -currentDirection.y;
        directionArr[1].y = currentDirection.x; // 좌
        directionArr[2].x = currentDirection.y;
        directionArr[2].y = -currentDirection.x; // 우
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
        newPosition = currentPosition + directionArr[0];

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
        var walkableBlock = CreateBlock(newPosition, blockType);
        currentPosition = newPosition; // 현재 위치 업데이트

        if (blockType == EBlockType.NORMAL)
        {
            goldBlockCounter++;
            diamondBlockCounter++;
        }



        // 다음 블럭의 방향을 결정
        if (hasTurned)
        {
            // 이전에 방향 전환이 있었다면 무조건 진행방향으로 생성
            newPosition = currentPosition + directionArr[0];
            hasTurned = false;
            Debug.Log("정면");
        }
        else
        {
            int randomValue = random.Next(0, 100);
            if (randomValue < forwardProbability)
            {
                newPosition = currentPosition + directionArr[0];
                Debug.Log("정면");
            }
            else if (randomValue < forwardProbability + sideProbability)
            {
                newPosition = currentPosition + directionArr[1];
                currentDirection = directionArr[1]; // 새로운 진행 방향 설정
                hasTurned = true;
                UpdateDirections();
                Debug.Log("좌회전");
            }
            else
            {
                newPosition = currentPosition + directionArr[2];
                currentDirection = directionArr[2]; // 새로운 진행 방향 설정
                hasTurned = true;
                UpdateDirections();
                Debug.Log("우회전");
            }
        }

        return walkableBlock;
    }
}
