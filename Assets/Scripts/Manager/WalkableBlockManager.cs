using UnityEngine;
using System.Collections.Generic;
using System;

public class WalkableBlockManager : MonoBehaviour, IWalkableBlockManager, IFadeOutFinishObserver, IPlayerMoveObserver, IFeverObserver
{
    [SerializeField]
    private int totalBlockCount = 10;

    private Queue<WalkableBlock> blockQueue = null;
    private IBlockGenerator iBlockGenerator = null;
    private WalkableBlock curBlock = null;
    private Action<WalkableBlock> blockGenerateAction = null;

    public void Init(ObjectPoolManager _poolManager, Action<WalkableBlock> _blockGenerateAction)
    {
        blockQueue = new Queue<WalkableBlock>();
        iBlockGenerator = GetComponent<IBlockGenerator>();
        iBlockGenerator.Init(_poolManager, CheckIsNextBlockCanGen, totalBlockCount);
        blockGenerateAction = _blockGenerateAction;
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
            List<WalkableBlock> tmpList = new List<WalkableBlock>(blockQueue);
            for (int i = 0; i < tmpList.Count; ++i)
            {
                if (tmpList[i].Position != _position)
                    continue;

                var b = tmpList[i];
                tmpList.RemoveAt(i);
                tmpList.Insert(0, b);

                // 큐를 갱신
                blockQueue = new Queue<WalkableBlock>(tmpList);
                return true;
            }
        }

        return false;
    }

    public EBlockType DequeueBlockAndGetBlockInterface()
    {
        if (blockQueue.Count < 1)
            return EBlockType.NONE;

        curBlock.OnPositionChanged();

        var block = iBlockGenerator.GenerateNextBlock();
        blockQueue.Enqueue(block);
        blockGenerateAction?.Invoke(block);
        curBlock = blockQueue.Dequeue();
        curBlock.OnPositioned();
        return curBlock.BlockType;
    }

    public void DequeueBlock()
    {
        curBlock.OnPositionChanged();
        curBlock = null;
    }

    public void ResetBlock()
    {
        curBlock?.Destroy();

        while (blockQueue.Count > 0)
        {
            blockQueue.Dequeue().Destroy();
        }
        blockQueue.Clear();
        iBlockGenerator.ResetBlock();
        GenerateStartBlock();
        GenerateBlock();
    }



    private void GenerateStartBlock()
    {
        curBlock = iBlockGenerator.GenerateStartBlock();
        curBlock.SetNewIdx(1);
        curBlock.UpdateIdx();
    }

    private void GenerateBlock()
    {
        for (int i = 0; i < totalBlockCount; ++i)
        {
            var block = iBlockGenerator.GenerateNextBlock();
            blockGenerateAction?.Invoke(block);
            blockQueue.Enqueue(block);
            block.SetNewIdx(i + 2);
            block.UpdateIdx();
        }
    }

    private bool CheckIsNextBlockCanGen(Vector2 _targetPos)
    {
        foreach(var block in blockQueue)
        {
            if (block.Position.Equals(_targetPos))
                return false;
        }

        return true;
    }

    public void OnFadeOutFinishNotify()
    {
        ResetBlock();
    }

    public Vector3 GetNextBlockDir()
    {
        return curBlock.Forward;
    }

    public void OnPlayerMoveNotify(in EBlockType _blockType)
    {
        foreach (var block in blockQueue)
            block.UpdateIdx();
    }

    public void OnFeverNotify(in bool _isFeverStart)
    {
        iBlockGenerator.SetFeverStart(_isFeverStart);
    }
}