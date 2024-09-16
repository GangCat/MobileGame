using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarController : OOBController
{
    public override void Init(WalkableBlock _block, ObjectPoolManager _poolMng)
    {
        base.Init(_block, _poolMng);
    }

    public override void Die()
    {
        base.Die();
        Return();
    }

    public override void Return()
    {
        base.Return();
    }
}
