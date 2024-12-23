using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;

public class UnitAttackState : StateMachineBehaviour
{
    NavMeshAgent agent;
    AttackController attackController;
    public float stopAttackingDistance = 1.2f;
    public float attackRate = 2f;
    public float attackTimer;
    public bool isAttacking = false;
    PhotonView pw;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        agent = animator.GetComponent<NavMeshAgent>();
        attackController = animator.GetComponent<AttackController>();

        // PhotonView'ý ana nesneden al
        pw = animator.GetComponentInParent<PhotonView>();

        attackController.muzzleEffect.SetActive(true);

        // Animasyon baþlangýcýný diðer istemcilere bildir
        if (pw.IsMine)
        {
            pw.RPC("SyncAnimation", RpcTarget.Others, "isAttacking", true);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (attackController.targetToAttack != null && !animator.transform.GetComponent<Movement>().isCommandedToMove)
        {
            LookAtTarget();

            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = 1f / attackRate;
            }
            else
            {
                attackTimer -= Time.deltaTime;
            }

            float distanceFromTarget = Vector3.Distance(attackController.targetToAttack.position, animator.transform.position);
            if (distanceFromTarget > stopAttackingDistance || attackController.targetToAttack == null)
            {
                animator.SetBool("isAttacking", false); // Takip durumuna geç

                // Animasyon durdurmayý diðer istemcilere bildir
                if (pw.IsMine)
                {
                    pw.RPC("SyncAnimation", RpcTarget.Others, "isAttacking", false);
                }
            }
        }
        else
        {
            animator.SetBool("isAttacking", false); // Takip durumuna geç

            // Animasyon durdurmayý diðer istemcilere bildir
            if (pw.IsMine)
            {
                pw.RPC("SyncAnimation", RpcTarget.Others, "isAttacking", false);
            }
        }
    }

    private void Attack()
    {
        var damageToInflict = attackController.unitDamage;
        SoundManager.Instance.PlayInfantryAttackSound();

        // Photon RPC ile hasar gönder
        if (attackController.targetToAttack != null)
        {
            PhotonView targetPhotonView = attackController.targetToAttack.GetComponentInParent<PhotonView>();

            if (targetPhotonView != null)
            {
                targetPhotonView.RPC("TakeDamage", RpcTarget.AllBuffered, damageToInflict);

            }
        }

        // Atak animasyonunu tetikle
        if (pw.IsMine)
        {
            pw.RPC("SyncAnimation", RpcTarget.Others, "triggerAttack", true);
        }
    }

    private void LookAtTarget()
    {
        Vector3 direction = attackController.targetToAttack.position - agent.transform.position;
        agent.transform.rotation = Quaternion.LookRotation(direction);

        var yRotation = agent.transform.eulerAngles.y;
        agent.transform.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        attackController.muzzleEffect.SetActive(false);
        isAttacking = false;

        // Animasyon durdurmayý diðer istemcilere bildir
        if (pw.IsMine)
        {
            pw.RPC("SyncAnimation", RpcTarget.Others, "isAttacking", false);
        }
    }

    [PunRPC]
    private void SyncAnimation(string parameterName, bool value)
    {
        if (parameterName == "isAttacking")
        {
            agent.GetComponent<Animator>().SetBool(parameterName, value);
        }
        else if (parameterName == "triggerAttack" && value)
        {
            agent.GetComponent<Animator>().SetTrigger(parameterName);
        }
    }
}
