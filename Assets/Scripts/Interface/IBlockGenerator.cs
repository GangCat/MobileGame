using UnityEngine;

public interface IBlockGenerator
{
    public void Init(ObjectPoolManager _poolManager);
    public WalkableBlock GenerateNextBlock(); // 내부 로직에 맞게 다음 블럭을 생성하는 함수
    public WalkableBlock GenerateStartBlock();
    public void ResetBlock();
}
