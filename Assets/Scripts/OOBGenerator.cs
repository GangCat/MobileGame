using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// OOB는 Object On Block 의 줄임말
/// 즉 WalkableBlock위에 올라가는 오브젝트를 의미함
/// </summary>
public class OOBGenerator : MonoBehaviour
{
    private ObjectPoolManager poolMng;

    [SerializeField]
    private string[] enemyPathArr = null;
    [SerializeField]
    private string feverStarPath = null;

    public void Init(ObjectPoolManager _poolManager)
    {
        poolMng = _poolManager;

        foreach (var enemyPath in enemyPathArr)
            poolMng.PrepareObjects(enemyPath, 10);

        poolMng.PrepareObjects(feverStarPath, 1);

    }

    public void GenerateObject(WalkableBlock _block)
    {
        OOBController oobCtrl = null;
        if (_block.BlockType.Equals(EBlockType.FEVER_BUFF))
        {
            var starGO = poolMng.GetObject(feverStarPath);
            oobCtrl = starGO.GetComponent<OOBController>();
        }
        else
        {

            int randomNum = Random.Range(0, enemyPathArr.Length);
            var enemyGO = poolMng.GetObject(enemyPathArr[randomNum]);
            oobCtrl = enemyGO.GetComponent<OOBController>();
        }

        oobCtrl.Init(_block, poolMng);
    }
}
