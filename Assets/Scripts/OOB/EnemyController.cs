using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : OOBController
{

    private bool isMoveRight = true;         // 사망처리시에 오른쪽으로 떨어질지 여부

    private float gravity = 40f;  // 중력

    private EnemyModelController enemyModelCtrl = null;

    public override void Init(WalkableBlock _block, ObjectPoolManager _poolMng)
    {
        base.Init(_block, _poolMng);
        enemyModelCtrl = GetComponent<EnemyModelController>();
    }

    public override void Die()
    {
        //사망
        StartCoroutine(MoveInParabola());
        enemyModelCtrl.PlayDeath();
    }

    public override void Return()
    {
        base.Return();
    }



    IEnumerator MoveInParabola()
    {
        float elapsedTime = 0f;
        Vector3 moveVec = Vector3.zero;
        float upVelocity = 5f;
        float sideVelocity = 3f;

        isMoveRight = Random.Range(0, 2) == 0;
        
        while (elapsedTime < 1f)
        {
            moveVec = Vector3.zero;

            moveVec += (isMoveRight ? transform.right : -transform.right) * (sideVelocity * Time.deltaTime);
            upVelocity -= gravity * Time.deltaTime;
            moveVec += Vector3.up * (upVelocity * Time.deltaTime);


            transform.position += moveVec;
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        poolMng.ReturnObj(gameObject);
    }

}
