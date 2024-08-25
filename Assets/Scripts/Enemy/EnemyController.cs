using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private bool isMoveRight = true;         // 오른쪽으로 이동할지 여부

    private float gravity = 40f;  // 중력

    private ObjectPoolManager poolMng = null;
    private EnemyModelController enemyModelCtrl = null;

    public void Init(WalkableBlock _block, ObjectPoolManager _poolMng)
    {
        transform.position = new Vector3(_block.Position.x, 0f, _block.Position.y);
        transform.forward = _block.Forward;
        poolMng = _poolMng;
        _block.EnemyController = this;
        enemyModelCtrl = GetComponent<EnemyModelController>();
    }

    public void Die()
    {
        //사망
        StartCoroutine(MoveInParabola());
        enemyModelCtrl.PlayDeath();
        Debug.Log("적 사망");
    }

    public void Return()
    {
        poolMng.ReturnObj(gameObject);
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
