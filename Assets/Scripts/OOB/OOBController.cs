using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OOBController : MonoBehaviour
{
    protected ObjectPoolManager poolMng = null;

    public virtual void Init(WalkableBlock _block, ObjectPoolManager _poolMng)
    {
        transform.position = new Vector3(_block.Position.x, 0f, _block.Position.y);
        transform.forward = _block.Forward;
        poolMng = _poolMng;
        _block.OOBController = this;
    }
    public virtual void Return()
    {
        poolMng.ReturnObj(gameObject);
    }
    public virtual void Die() { }
}
