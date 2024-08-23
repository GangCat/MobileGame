using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModelController : MonoBehaviour
{
    [SerializeField]
    private Animator anim = null;
    public void PlayDeath()
    {
        anim.SetTrigger("Death");
    }
}
