using Photon.Pun;
using System.Collections;
using UnityEngine;

public class BoostedMilitaryBaseSpawner : MonoBehaviour
{
    public GameObject objectToSpawn; // Oluþturulacak obje
    public Transform spawnPoint;    // Spawn noktasý
    public float spawnInterval = 5f; // Spawn süresi
    PhotonView pw;
    private void Start()
    {
        pw = GetComponent<PhotonView>();
        if (pw.IsMine)
        {
            StartCoroutine(SpawnObject());
        }
        
    }

    private IEnumerator SpawnObject()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval); // 30 saniye bekle
            if (objectToSpawn != null && spawnPoint != null)
            {

                GameObject soldier =PhotonNetwork.Instantiate("BoostedMilitary", spawnPoint.position, spawnPoint.rotation);
               
                }
            }
        }
    }

