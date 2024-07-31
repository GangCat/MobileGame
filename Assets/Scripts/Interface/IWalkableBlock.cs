using UnityEngine;

public interface IWalkableBlock
{
    public Vector2 Position { get; }
    public EBlockType BlockType { get; }
    public void OnPositioned();  // 플레이어가 해당 블럭으로 이동할 때 호출될 함수
    public void OnPositionChanged(); // 플레이어가 해당 블럭의 다음 블럭으로 이동했을 때 호출될 함수
    public void Destroy();
}