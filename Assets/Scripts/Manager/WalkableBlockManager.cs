using UnityEngine;
using System.Collections.Generic;

public class WalkableBlockManager : MonoBehaviour, IWalkableBlockManager
{
    [SerializeField]
    private int totalBlockCount = 10;

    private Queue<IWalkableBlock> blockQueue;
    private IBlockGenerator iBlockGenerator = null;
    private IWalkableBlock iCurBlock;

    private bool isFirstStep = true;

    public void Init(ObjectPoolManager _poolManager)
    {
        blockQueue = new Queue<IWalkableBlock>();
        iBlockGenerator = GetComponent<IBlockGenerator>();
        iBlockGenerator.Init(_poolManager);
    }

    public void StartBlockGenerate()
    {
        for (int i = 0; i < totalBlockCount; ++i)
        {
            blockQueue.Enqueue(iBlockGenerator.GenerateNextBlock());
        }
    }

    public void ResetBlock()
    {
        while (blockQueue.Count > 0)
        {
            blockQueue.Dequeue().Destroy();
        }
        blockQueue.Clear();
        iBlockGenerator.ResetBlock();
        isFirstStep = true;
        GenerateStartBlock();
        StartBlockGenerate();
    }

    private void GenerateStartBlock()
    {
        iCurBlock = iBlockGenerator.GenerateStartBlock();
    }

    public bool CheckBlockExistence(Vector2 _position)
    {
        if (blockQueue.Count < 1)
            return false;

        var block = blockQueue.Peek();
        if (block.Position == _position)
            return true;
        else
        {
            List<IWalkableBlock> tmpList = new List<IWalkableBlock>(blockQueue);
            for (int i = 0; i < tmpList.Count; ++i)
            {
                if (tmpList[i].Position != _position)
                    continue;

                var b = tmpList[i];
                tmpList.RemoveAt(i);
                tmpList.Insert(0, b);

                // 큐를 갱신
                blockQueue = new Queue<IWalkableBlock>(tmpList);
                return true;
            }
        }

        return false;
    }

    public EBlockType DequeueBlockAndGetBlockInterface()
    {
        //if (isFirstStep)
        //{
        //    iCurBlock = blockQueue.Dequeue();
        //    iCurBlock.OnPositioned();
        //    isFirstStep = false;
        //    return iCurBlock.BlockType;
        //}
        if (blockQueue.Count < 1)
            return EBlockType.NONE;

        iCurBlock.OnPositionChanged();

        blockQueue.Enqueue(iBlockGenerator.GenerateNextBlock());
        iCurBlock = blockQueue.Dequeue();
        iCurBlock.OnPositioned();
        return iCurBlock.BlockType;
    }

    public void DequeueBlock()
    {
        //if (!isFirstStep)
        //{
            iCurBlock.OnPositionChanged();
        //}
    }
}