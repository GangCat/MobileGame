using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BlockGenerator : MonoBehaviour, IBlockGenerator
{
    [SerializeField]
    private float forwardProbability = 70; // 진행방향 확률
    [SerializeField]
    private int minScoreBlockInterval = 20; // 스코어 블럭 최소 간격
    [SerializeField]
    private int minFeverBlockInterval = 20; // 무적버프 블럭 최소 간격
    [SerializeField]
    private float scoreBlockProbabilityIncrement = 0.05f; // 스코어 블럭 확률 상승 계수
    [SerializeField]
    private float feverBlockProbabilityIncrement = 0.05f; // 무적버프 블럭 확률 상승 계수

    [SerializeField]
    private string normalBlockPrefabPath;
    [SerializeField]
    private string doubleBlockPrefabPath;
    [SerializeField]
    private string tripleBlockPrefabPath;

    // 무적 버프는 아이템으로 하면 좋을듯?

    private int scoreBlockInterval = 0; // 마지막 스코어 블럭 이후 생성된 블럭 수
    private float scoreBlockProbability = 0f; // 현재 스코어 블럭 생성 확률
    private uint scoreblockCnt = 0; // 스코어 블럭이 생성된 개수

    private uint feverBlockInterval = 0;
    private float feverBlockProbability = 0f;

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

    private bool isTurned = false;
    private int totalBlockCount = 0;
    private bool isFeverStart = false;


    // 그러면 생성할 위치가 이미 블럭이 있는지 예외처리 해야함.
    // 그럼 블럭 있는지 확인하는 내용은 언제? 다음 방향 결정할 때.


    public void Init(ObjectPoolManager _poolManager, Func<Vector2, bool> _checkIsBlockCanGenFunc, int _totalBlockCount)
    {
        curBlockDir = Vector2.right; // 초기 방향 설정
        random = new System.Random();
        curBlockPosition = Vector2.zero;
        poolManager = _poolManager;
        checkIsBlockCanGenFunc = _checkIsBlockCanGenFunc;
        totalBlockCount = _totalBlockCount;

        scoreBlockInterval = 0;
        scoreBlockProbability = 0f;
        scoreblockCnt = 0;

        isFeverStart = false;

        feverBlockInterval = 0;
        feverBlockProbability = 0f;

        poolManager.PrepareObjects(normalBlockPrefabPath, 7);

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

        scoreBlockInterval = 0;
        scoreBlockProbability = 0f;
        scoreblockCnt = 0;

        isFeverStart = false;

        feverBlockInterval = 0;
        feverBlockProbability = 0f;

        UpdateDirections();

        forwardProbability = originForwardProbability;
        sideProbability = (100 - forwardProbability) / 2;
    }

    public WalkableBlock CreateBlock(Vector2 _position, EBlockType _blockType)
    {
        GameObject walkableBlockGo = null;
        switch (_blockType)
        {
            case EBlockType.NORMAL:
            case EBlockType.FEVER_BUFF:
                walkableBlockGo = poolManager.GetObject(normalBlockPrefabPath);
                break;
            case EBlockType.DOUBLE_SCORE:
                walkableBlockGo = poolManager.GetObject(doubleBlockPrefabPath);
                break;
            case EBlockType.TRIPLE_SCORE:
                walkableBlockGo = poolManager.GetObject(tripleBlockPrefabPath);
                break;
        }
        
        WalkableBlock block = walkableBlockGo.GetComponent<WalkableBlock>();

        // 여기서 totalBlockCount는 플레이어가 서있는 블럭을 제외한 숫자이기 때문에 총 블럭의 개수는 totalBlockCOunt + 1이 맞고
        // Idx는 0부터 시작하므로 그냥 totalBlockCount를 넣으면 될 것 같지만
        // 그리고 이렇게 생성한 뒤에 UpdateIdx를 호출하기 때문에 +1을 해줘야 한다
        block.Init(_position, _blockType, poolManager, curBlockDir, totalBlockCount + 1);

        return block;
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
        newPosition = curBlockPosition + blockDirArr[0] * 1.5f;

        // 블럭 타입 결정
        EBlockType blockType = ConfirmBlockType();

        if (blockType == EBlockType.FEVER_BUFF)
            Debug.Log("FeverBlockGen");

        // 블럭 사이 개수 증가
        // 비교문으로 노말블럭만 하려다 비용아낄겸 그냥 더해주기로 함
        ++scoreBlockInterval;
        ++feverBlockInterval;

        //  다음 블럭 방향이 정해질때까지 순환
        while (!isNextBlockPosConfirm)
        {
            int randomValue = random.Next(0, 100);

            // 정면 방향일 경우
            if (isTurned || randomValue < forwardProbability && !isFwdBlocked)
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
                isTurned = false;
            }
            // 좌측방향일 경우
            else if (randomValue < forwardProbability + sideProbability && !isLeftBlocked)
            {
                isTurned = true;
                if (!checkIsBlockCanGenFunc.Invoke(curBlockPosition + blockDirArr[1]))
                {
                    isLeftBlocked = true;
                    continue;
                }

                ConfirmNextDir(blockDirArr[1]);
            }
            // 우측 방향일 경우
            else if (!isRightBlocked)
            {
                isTurned = true;
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

    private EBlockType ConfirmBlockType()
    {
        if (isFeverStart is true)
            return EBlockType.NORMAL;

        float randomVal = UnityEngine.Random.Range(0f, 1f);
        // 스코어블럭 생성주기가 되었을 경우
        if (scoreBlockInterval >= minScoreBlockInterval)
        {
            // 스코어블럭 생성확률 상승
            scoreBlockProbability += scoreBlockProbabilityIncrement;

            if (randomVal < scoreBlockProbability)
            {
                // 5로 나눌때 나머지가 3인 경우는 5, 10번째 스코어블럭이 생성되는 순간임.
                // 이때는 3배 점수 블럭이 생성
                if (scoreblockCnt % 5 == 3)
                {
                    scoreBlockInterval = 0;
                    scoreBlockProbability = 0f;
                    ++scoreblockCnt;
                    return EBlockType.TRIPLE_SCORE;
                }
                else
                {
                    scoreBlockInterval = 0;
                    scoreBlockProbability = 0f;
                    ++scoreblockCnt;
                    return EBlockType.DOUBLE_SCORE;
                }
            }
        }

        if (feverBlockInterval >= minFeverBlockInterval)
        {
            feverBlockProbability += feverBlockProbabilityIncrement;

            if(randomVal < feverBlockProbability)
            {
                feverBlockInterval = 0;
                feverBlockProbability = 0f;
                return EBlockType.FEVER_BUFF;
            }
        }

        return EBlockType.NORMAL;
    }

    private void ConfirmNextDir(Vector2 _nextDir)
    {
        isNextBlockPosConfirm = true;
        curBlockDir = _nextDir; // 새로운 진행 방향 설정
        forwardProbability = originForwardProbability;
        sideProbability = originSideProbability;
        UpdateDirections();
    }

    public void SetFeverStart(bool _isFeverStart)
    {
        isFeverStart = _isFeverStart;
    }
}
