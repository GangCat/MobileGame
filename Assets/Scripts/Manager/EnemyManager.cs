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
    private string enemyPath = string.Empty;

    public void Init(ObjectPoolManager _poolManager)
    {
        poolMng = _poolManager;

        poolMng.PrepareObjects(enemyPath, 10);
    }

    public void GenEnemy(WalkableBlock _block)
    {
        var enemyGO = poolMng.GetObject(enemyPath);
        var enemyCtrl = enemyGO.GetComponent<EnemyController>();
        enemyCtrl.Init(_block, poolMng);
    }
}