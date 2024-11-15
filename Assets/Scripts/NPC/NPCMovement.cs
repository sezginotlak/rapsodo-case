using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    //Animator parameter names
    const string SPEED = "speed";
    const string PICK_UP = "pickUp";

    public void Move(NavMeshAgent agent, Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void SetSpeed(Animator animator, float speed)
    {
        animator.SetFloat(SPEED, speed);
    }

    public void SetTrigger(Animator animator)
    {
        animator.SetTrigger(PICK_UP);
    }

    public bool IsPickUpOver(Animator animator)
    {
        return animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0);
    }
}
