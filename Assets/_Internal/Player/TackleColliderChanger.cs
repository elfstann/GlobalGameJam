using UnityEngine;
using UnityEngine.Animations;

public class TackleColliderChanger : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<Collider2D>().enabled = false;
        animator.transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        animator.gameObject.GetComponent<Collider2D>().enabled = true;
        animator.transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
    }
}