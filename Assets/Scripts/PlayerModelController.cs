using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    private Animator animator = null;

    private int blockCnt = 0;

    public void Init()
    {
        animator = GetComponentInChildren<Animator>();
    }
        
    public void UpdateModelForward(Vector2 _dir)
    {
        transform.forward = new Vector3(_dir.x, 0f, _dir.y);

        ++blockCnt;
        if(blockCnt > 4)
        {
            animator.SetTrigger("DoBounce");
            animator.SetInteger("RandomAnim", Random.Range(0, 3));
            blockCnt = 0;
        }
    }

    public void ResetModelForward()
    {
        animator.Play("Idle_A");
        transform.forward = Vector3.back;
    }
}
