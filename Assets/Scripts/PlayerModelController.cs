using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    private Animator animator = null;

    private int blockCnt = 0;

    private StringBuilder sb = null;

    private int prevActionNum = -1;

    public void Init()
    {
        animator = GetComponentInChildren<Animator>();
        sb = new("Action");
    }
        
    public void UpdateModelForward(Vector2 _dir)
    {
        transform.forward = new Vector3(_dir.x, 0f, _dir.y);

        var ranVal = Random.Range(0, 10);

        while(prevActionNum.Equals(ranVal))
            ranVal = Random.Range(0, 10);

        sb.Append(ranVal);

        animator.Play(sb.ToString());

        sb.Length -= 1;



        //++blockCnt;
        //if (blockCnt % 2 == 0)
        //    animator.Play("Punch1");
        //else
        //    animator.Play("Punch2");

        //if(blockCnt > 4)
        //{
        //    animator.SetTrigger("DoBounce");
        //    animator.SetInteger("RandomAnim", Random.Range(0, 3));
        //    blockCnt = 0;
        //}
    }

    public void ResetModelForward()
    {
        animator.Play("Idle_A");
        transform.forward = Vector3.back;
    }
}
