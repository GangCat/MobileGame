using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private ObjectPoolManager poolMng = null;
    public void Init(WalkableBlock _block, ObjectPoolManager _poolMng)
    {
        transform.position = new Vector3(_block.Position.x, 0f, _block.Position.y);
        transform.forward = _block.Forward;
        poolMng = _poolMng;
        _block.EnemyController = this;
    }

    public void Die()
    {
        //사망
        poolMng.ReturnObj(gameObject);
        Debug.Log("사망");
    }

}
