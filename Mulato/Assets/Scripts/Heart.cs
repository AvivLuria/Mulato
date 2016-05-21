using UnityEngine;
using System.Collections;

public class Heart : MonoBehaviour {
    private Animator animator;
    public void SetAnimation()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("LostLife", true);
    }
}
