using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    Camera cam;
    NavMeshAgent agent;
    public LayerMask ground;
    public bool isCommandedToMove;
    DirectionIndicator directionIndicator;
    PhotonView pw;
    private void Start()
    {
        pw = GetComponent<PhotonView>();
        cam = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        directionIndicator = GetComponent<DirectionIndicator>();

    }
    private void Update()
    {
        
            if (Input.GetMouseButton(1))
            {
                RaycastHit hit;
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
                {
                    isCommandedToMove = true;
                    agent.SetDestination(hit.point);
                    directionIndicator.DrawLine(hit);
                }
            }

            if (agent.hasPath == false || agent.remainingDistance <= agent.stoppingDistance)
            {
                isCommandedToMove = false;

            }

        }
    
       
}
