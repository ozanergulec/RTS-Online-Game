using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Transform targetToAttack;
    public Material idleStateMaterial;
    public Material followStateMaterial;
    public Material attackStateMaterial;
    public int unitDamage;
    public bool isPlayer;
    public GameObject muzzleEffect;
    private void OnTriggerEnter(Collider other)
    {
        if(isPlayer  && other.CompareTag("Enemy")&& targetToAttack == null)
        {
            targetToAttack = other.transform; 
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (isPlayer && other.CompareTag("Enemy") && targetToAttack == null)
        {
            targetToAttack = other.transform;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isPlayer && other.CompareTag("Enemy") && targetToAttack != null)
        {
            targetToAttack =null;
        }
    }
    /*
    public void setIdleMaterial()
    {
        GetComponent<Renderer>().material = idleStateMaterial;
    }
    public void setFollowMaterial()
    {
        GetComponent<Renderer>().material= followStateMaterial;
    }
    public void setAttackMaterial()
    {
        GetComponent<Renderer>().material= attackStateMaterial;
    }
    */

    private void OnDrawGizmos()
    {
        //Follow distance / area
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position,10f*0.2f);

        //Attack distance/area
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);

        //Stop attacking distance/area
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 1.2f);
    }
}
