using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateChanger : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] String currState = "FisherManIdleFull";
    bool attacking = false;

    public void ChangeAnimationState(String newState) {
        if(currState == newState) {
            return;
        }

        if (attacking) {
            return;
        }
        
        currState = newState;
        Debug.Log("animating " + currState);
        animator.Play(currState);
    }

    public void StartAttack() {
        attacking = true;
    }

    public void EndAttack() {
        attacking = false;
    }
}
