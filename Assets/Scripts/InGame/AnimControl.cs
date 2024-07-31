using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimControl : MonoBehaviour
{
    [SerializeField] Animator CharacterAnimator;
   


    public bool AnimationControl()
    {
        AnimatorStateInfo stateInfo = CharacterAnimator.GetCurrentAnimatorStateInfo(2);

        
            if (stateInfo.normalizedTime >= 0.9f)
            {
            return true;
        }
        else
        {
            return false;
        }
        
        

    }
}
