using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerModelController : MonoBehaviour, IGameOverObserver
{
    private StringBuilder sb = null;

    private int prevActionNum = -1;

    [SerializeField]
    private Animator animator = null;

    public void Init()
    {
        sb = new("Action");
    }

    private void PlayFallingAnim()
    {
        animator.SetTrigger("OnFall");
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
    }

    public void ResetModelForward()
    {
        animator.Play("Idle_A");
        transform.forward = Vector3.back;
    }

    public void OnGameOverNotify()
    {
        PlayFallingAnim();
    }
}
