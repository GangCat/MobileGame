using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BlockGenerator : MonoBehaviour, IBlockGenerator
{
    [SerializeField]
    private float forwardProbability = 70; // 진행방향 확률
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

    private int goldBlockInterval = 0; // 마지막 골드 블럭 이후 생성된 블럭 수
    private int diamondBlockInterval = 0; // 마지막 다이아 블럭 이후 생성된 블럭 수
    private float goldBlockProbability = 0f; // 현재 골드 블럭 생성 확률
    private float diamondBlockProbability = 0f; // 현재 다이아 블럭 생성 확률

    private Vector2 curBlockDir = Vector2.zero;
    private System.Random random;
    private Vector2 curBlockPosition = Vector2.zero;

    private Vector2[] blockDirArr = new Vector2[3]; // 벡터 배열을 미리 선언
    private ObjectPoolManager poolManager = null;
    private Func<Vector2, bool> checkIsBlockCanGenFunc = null;

    private float sideProbability = 0; // 좌우 방향 확률
    private float originForwardProbability = 0;
    private float originSideProbability = 0;

    private bool isNextBlockPosConfirm = false;
    private bool isFwdBlocked = false;
    private bool isRightBlocked = false;
    private bool isLeftBlocked = false;


    // 그러면 생성할 위치가 이미 블럭이 있는지 예외처리 해야함.
    // 그럼 블럭 있는지 확인하는 내용은 언제? 다음 방향 결정할 때.


    public void Init(ObjectPoolManager _poolManager, Func<Vector2, bool> _checkIsBlockCanGenFunc)
    {
        curBlockDir = Vector2.right; // 초기 방향 설정
        random = new System.Random();
        curBlockPosition = Vector2.zero;
        poolManager = _poolManager;
        checkIsBlockCanGenFunc = _checkIsBlockCanGenFunc;

        poolManager.PrepareObjects(blockPrefabPath, 7);

        UpdateDirections();

        originForwardProbability = forwardProbability;
        // 좌우 방향 확률 계산
        sideProbability = (100 - forwardProbability) * 0.5f;
        originSideProbability = sideProbability;
    }

    public void ResetBlock()
    {
        curBlockDir = Vector2.right; // 초기 방향 설정
        random = new System.Random();
        curBlockPosition = Vector2.zero;

        goldBlockInterval = 0;
        diamondBlockInterval = 0;
        goldBlockProbability = 0f;
        diamondBlockProbability = 0f;

        UpdateDirections();

        forwardProbability = originForwardProbability;
        sideProbability = (100 - forwardProbability) / 2;
    }

    public WalkableBlock CreateBlock(Vector2 _position, EBlockType _blockType)
    {
        var walkableBlockGo = poolManager.GetObject(blockPrefabPath);
        WalkableBlock block = walkableBlockGo.GetComponent<WalkableBlock>();

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
        isNextBlockPosConfirm = false;
        isFwdBlocked = false;
        isRightBlocked = false;
        isLeftBlocked = false;

        // 이전에 설정된 방향으로 블럭 생성
        newPosition = curBlockPosition + blockDirArr[0];

        EBlockType blockType = EBlockType.NORMAL;

        // 골드블럭 생성주기가 되었을 경우
        if (goldBlockInterval >= minGoldBlockInterval)
        {
            // 골드블럭 생성확률 상승
            goldBlockProbability += goldBlockProbabilityIncrement;

            if (random.NextDouble() < goldBlockProbability)
            {
                blockType = EBlockType.GOLD;
                goldBlockInterval = 0;
                goldBlockProbability = 0f;
            }
        }

        // 다이아블럭 생성주기가 되었을 경우
        if (diamondBlockInterval >= minDiamondBlockInterval)
        {
            // 다이아블럭 생성확률 상승
            diamondBlockProbability += diamondBlockProbabilityIncrement;
            if (random.NextDouble() < diamondBlockProbability)
            {
                blockType = EBlockType.DIAMOND;
                diamondBlockInterval = 0;
                diamondBlockProbability = 0f;
            }
        }

        // 블럭 생성 및 스택에 추가

        if (blockType == EBlockType.NORMAL)
        {
            goldBlockInterval++;
            diamondBlockInterval++;
        }

        //  다음 블럭 방향이 정해질때까지 순환
        while (!isNextBlockPosConfirm)
        {
            int randomValue = random.Next(0, 100);

            // 정면 방향일 경우
            if (randomValue < forwardProbability && !isFwdBlocked)
            {
                // 리스트 확인
                if (!checkIsBlockCanGenFunc.Invoke(curBlockPosition + blockDirArr[0]))
                {
                    isFwdBlocked = true;
                    continue;
                }

                isNextBlockPosConfirm = true;
                forwardProbability -= 1;
                sideProbability -= 0.5f;
            }
            // 좌측방향일 경우
            else if (randomValue < forwardProbability + sideProbability && !isLeftBlocked)
            {
                if (!checkIsBlockCanGenFunc.Invoke(curBlockPosition + blockDirArr[1]))
                {
                    isLeftBlocked = true;
                    continue;
                }

                ConfirmNextDir(blockDirArr[1]);
            }
            // 우측 방향일 경우
            else if(!isRightBlocked)
            {
                if (!checkIsBlockCanGenFunc.Invoke(curBlockPosition + blockDirArr[2]))
                {
                    isRightBlocked = true;
                    continue;
                }

                ConfirmNextDir(blockDirArr[2]);
            }
        }

        // 블럭 생성 및 스택에 추가
        var walkableBlock = CreateBlock(newPosition, blockType);
        curBlockPosition = newPosition; // 현재 위치 업데이트

        return walkableBlock;
    }

    private void ConfirmNextDir(Vector2 _nextDir)
    {
        isNextBlockPosConfirm = true;
        curBlockDir = _nextDir; // 새로운 진행 방향 설정
        forwardProbability = originForwardProbability;
        sideProbability = originSideProbability;
        UpdateDirections();
    }
}
