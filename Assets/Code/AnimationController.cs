using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public bool AnimateOnStart, AnimateOnClose, AnimateOnVisible;
    
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (AnimateOnStart)
        {
            animator.SetBool("AnimateIn", true);
        }
    }
}
