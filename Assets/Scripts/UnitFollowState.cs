using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class UnitFollowState : StateMachineBehaviour
{
    AttackController attackController;
    NavMeshAgent agent;
    public float attackingDistance = 1f;
    PhotonView photonView;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        agent = animator.transform.GetComponent<NavMeshAgent>();
        photonView = animator.GetComponentInParent<PhotonView>();

        // Diðer istemcilere "isFollowing" durumunu bildir
        if (photonView != null && photonView.IsMine)
        {
            photonView.RPC("SyncAnimation", RpcTarget.Others, "isFollowing", true);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack == null)
        {
            animator.SetBool("isFollowing", false);

            // Diðer istemcilere "isFollowing" durumunun kapandýðýný bildir
            if (photonView != null && photonView.IsMine)
            {
                photonView.RPC("SyncAnimation", RpcTarget.Others, "isFollowing", false);
            }
        }
        else
        {
            if (animator.transform.GetComponent<Movement>().isCommandedToMove == false)
            {
                // Hedefe doðru hareket ettir
                agent.SetDestination(attackController.targetToAttack.position);
                animator.transform.LookAt(attackController.targetToAttack);

                // Saldýrý mesafesine ulaþýldý mý?
                float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
                if (distanceFromTarget < attackingDistance)
                {
                    agent.SetDestination(animator.transform.position);
                    animator.SetBool("isAttacking", true);

                    // "isAttacking" durumunu diðer istemcilere bildir
                    if (photonView != null && photonView.IsMine)
                    {
                        photonView.RPC("SyncAnimation", RpcTarget.Others, "isAttacking", true);
                    }
                }
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent.SetDestination(animator.transform.position);

        // Diðer istemcilere "isFollowing" durumunun kapandýðýný bildir
        if (photonView != null && photonView.IsMine)
        {
            photonView.RPC("SyncAnimation", RpcTarget.Others, "isFollowing", false);
        }
    }

    [PunRPC]
    private void SyncAnimation(string parameterName, bool value)
    {
        Animator animator = agent.GetComponent<Animator>();

        if (parameterName == "isFollowing" || parameterName == "isAttacking")
        {
            animator.SetBool(parameterName, value);
        }
    }
}
