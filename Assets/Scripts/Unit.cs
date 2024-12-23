using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;
using TMPro;

public class Unit : MonoBehaviour
{
    public float unitHealth;
    public float maxHealth;
    public HealthTracker healthTracker;
    public Animator animator;
    public NavMeshAgent navMeshAgent;
    PhotonView pw;
    public bool isDead=false;
    
    void Start()
    {
       
        UnitSelectionManager.Instance.allUnitsList.Add(gameObject);
        unitHealth = maxHealth;
        UpdateHealthUI();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        pw = gameObject.GetComponent<PhotonView>();
        if (pw.IsMine)
        {
            gameObject.tag = "Player";
            gameObject.layer = LayerMask.NameToLayer("Clickable");
        }
        else
        {
            gameObject.tag = "Enemy";
            gameObject.layer = LayerMask.NameToLayer("Attackable");
        }
    }

    private void UpdateHealthUI()
    {
        healthTracker.UpdateSliderValue(unitHealth, maxHealth);
        if(unitHealth <= 0)
        {
            if (pw.IsMine)
            {
             
                
                PhotonNetwork.Destroy(gameObject);
            }
            
        }
    }

    private void OnDestroy()
    {
        UnitSelectionManager.Instance.allUnitsList.Remove(gameObject);
        
    }

    [PunRPC]
    internal void TakeDamage(int damageToInflict)
    {
        if (!pw.IsMine) return;

        unitHealth -= damageToInflict;
        healthTracker.SyncHealth(unitHealth, maxHealth); // Saðlýk verisini senkronize et
        UpdateHealthUI();

        if (unitHealth <= 0)
        {
            if (pw.IsMine)
            {
               
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }


    private void Update()
    {
        if (pw.IsMine)
        {
      if (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
        {
            animator.SetBool("isMoving", true);
            }
            else
            {
                animator.SetBool("isMoving", false);

            }
            
    }
        }
        
}
