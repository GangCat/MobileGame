using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    // 적 생성
    // 적 사망처리
    // 적 풀링

    private ObjectPoolManager poolMng;

    [SerializeField]
    private string[] enemyPathArr = null;

    public void Init(ObjectPoolManager _poolManager)
    {
        poolMng = _poolManager;

        foreach(var enemyPath in enemyPathArr)
            poolMng.PrepareObjects(enemyPath, 10);
    }

    public void GenEnemy(WalkableBlock _block)
    {
        int randomNum = Random.Range(0, enemyPathArr.Length);
        var enemyGO = poolMng.GetObject(enemyPathArr[randomNum]);
        var enemyCtrl = enemyGO.GetComponent<EnemyController>();
        enemyCtrl.Init(_block, poolMng);
    }
}