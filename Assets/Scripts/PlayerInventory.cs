using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private int goldCount;
    private int diamondCount;

    public void Init()
    {
        goldCount = 0;
        diamondCount = 0;
    }

    public void ResetPlayer()
    {
        goldCount = 0;
        diamondCount = 0;
    }

    public void ProcessBlock(EBlockType _blockType)
    {
        switch (_blockType)
        {
            case EBlockType.DOUBLE_SCORE:
                goldCount++;
                Debug.Log("Gold block encountered! Total Gold: " + goldCount);
                break;
            case EBlockType.TRIPLE_SCORE:
                diamondCount++;
                Debug.Log("Diamond block encountered! Total Diamonds: " + diamondCount);
                break;
            case EBlockType.NORMAL:
                // 노멀 블럭은 특별한 처리 없음
                break;
        }
    }

    public int GetGoldCount()
    {
        return goldCount;
    }

    public int GetDiamondCount()
    {
        return diamondCount;
    }
}
