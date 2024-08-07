using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelController : MonoBehaviour
{
    private Animator animator = null;

    // 애니메이션 5블럭마다 한 번씩 바뀌도록 구현

    public void Init()
    {
        animator = GetComponentInChildren<Animator>();
    }
        
    public void UpdateModelForward(Vector2 _dir)
    {
        transform.forward = new Vector3(_dir.x, 0f, _dir.y);
        animator.SetTrigger("DoBounce");
        animator.SetInteger("RandomAnim", Random.Range(0, 10));
    }

    public void ResetModelForward()
    {
        animator.Play("Idle_A");
        transform.forward = Vector3.back;
    }
}
