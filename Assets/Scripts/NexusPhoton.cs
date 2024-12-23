using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusPhoton : MonoBehaviour
{
    PhotonView pw;
    void Start()
    {
        pw = GetComponent<PhotonView>();
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
