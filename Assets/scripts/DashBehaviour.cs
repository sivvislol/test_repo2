using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBehaviour : StateMachineBehaviour {

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Player.dashing = true;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Rigidbody2D rbd2 = animator.gameObject.GetComponent<Rigidbody2D>();
        if (rbd2.position.x >= Player.rightBound && Player.mov.x > 0) {
            Player.mov.x = 0;
        }
        if (rbd2.position.x <= Player.leftBound && Player.mov.x < 0) {
            Player.mov.x = 0;
        }
        if (rbd2.position.y >= Player.topBound && Player.mov.y > 0) {
            Player.mov.y = 0;
        }
        if (rbd2.position.y <= Player.bottomBound && Player.mov.y < 0) {
            Player.mov.y = 0;
        }
        rbd2.transform.position += new Vector3(Player.mov.x * Player.speedDash * Time.deltaTime, Player.mov.y * Player.speedDash * Time.deltaTime, 0);
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Player.dashing = false;
        animator.ResetTrigger("dash");
    }

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
