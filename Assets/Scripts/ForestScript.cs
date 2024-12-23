using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ForestScript : MonoBehaviour
{
    
    public int health;
    PhotonView pw;
    private void Start()
    {
        pw = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
                    
    }
    [PunRPC]
    public void dealTreeDamage(int damageToInflict)
    {
        if (!pw.IsMine) return;
        health -= damageToInflict;
        if (health <= 0)
        {
            if (pw.IsMine)
            { 
                PhotonNetwork.Destroy(gameObject);
            }
           
        }
    }
}