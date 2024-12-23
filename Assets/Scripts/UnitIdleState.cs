using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class UnitIdleState : StateMachineBehaviour
{
    AttackController attackController;
    PhotonView photonView;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController = animator.transform.GetComponent<AttackController>();
        photonView = animator.GetComponentInParent<PhotonView>();

        // Animasyon durumunu diðer istemcilere bildir
        if (photonView != null && photonView.IsMine)
        {
            photonView.RPC("SyncAnimation", RpcTarget.Others, "isIdle", true);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Hedef var mý kontrol et
        if (attackController.targetToAttack != null)
        {
            animator.SetBool("isFollowing", true);

            // "isFollowing" durumunu diðer istemcilere bildir
            if (photonView != null && photonView.IsMine)
            {
                photonView.RPC("SyncAnimation", RpcTarget.Others, "isFollowing", true);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Animasyon durumunun kapandýðýný bildir
        if (photonView != null && photonView.IsMine)
        {
            photonView.RPC("SyncAnimation", RpcTarget.Others, "isIdle", false);
        }
    }

    [PunRPC]
    private void SyncAnimation(string parameterName, bool value)
    {
        Animator animator = attackController.GetComponent<Animator>();

        if (parameterName == "isIdle" || parameterName == "isFollowing")
        {
            animator.SetBool(parameterName, value);
        }
    }
}
