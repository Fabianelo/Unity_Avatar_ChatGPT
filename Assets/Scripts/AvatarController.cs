using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarController : MonoBehaviour
{
    public Animator animator;

    public void SetTalkPose(bool value)
    {
        animator.SetBool("isTalking", value);
    }
}
