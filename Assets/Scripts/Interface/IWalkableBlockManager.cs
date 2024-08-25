using UnityEngine;

public interface IWalkableBlockManager
{
    public bool CheckBlockExistence(Vector2 _position); // 현재 해당 위치에 블럭이 존재하는지 확인, 블럭을 밟기 전.
    public EBlockType DequeueBlockAndGetBlockInterface();
    public void DequeueBlock();
    public Vector3 GetNextBlockDir();
}