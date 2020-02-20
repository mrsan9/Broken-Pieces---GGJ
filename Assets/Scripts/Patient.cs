using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(Animator))]
public class Patient : MonoBehaviour
{
    public Animator animator;
    public SkinnedMeshRenderer skinnedMeshRenderer;

    public Material male1;
    public Material male2;

    bool isMale1;

    public void Start()
    {
        skinnedMeshRenderer.material = male1;
        isMale1 = true;
    }

     public void OnNewLevelStarted()
    {
        if(isMale1)
        {
            skinnedMeshRenderer.material = male1;
        }
        else
        {
            skinnedMeshRenderer.material = male2;
        }
        isMale1 = !isMale1;
    }
    public void SetAnimatorState(string state)
    {
        animator.Play(state);
    }
}
