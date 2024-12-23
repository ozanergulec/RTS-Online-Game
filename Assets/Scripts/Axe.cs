using Photon.Pun;
using UnityEngine;

public class Axe : MonoBehaviour
{
    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tree"))
        {
            PhotonView treePhotonView = other.gameObject.GetComponent<PhotonView>();
            if (treePhotonView != null)
            {
                // Hasarý aðdaki tüm oyunculara senkronize et
                treePhotonView.RPC("dealTreeDamage", RpcTarget.AllBuffered, 10);

                // Kaynak miktarýný sadece yerel oyuncuda güncelle
                if (photonView.IsMine)
                {
                    int randomNumber = Random.Range(20, 25);
                    WoodRockController.woodCount += randomNumber;
                }
            }
        }
        else if (other.CompareTag("Rock"))
        {
            PhotonView rockPhotonView = other.gameObject.GetComponent<PhotonView>();
            if (rockPhotonView != null)
            {
                // Hasarý aðdaki tüm oyunculara senkronize et
                rockPhotonView.RPC("dealRockDamage", RpcTarget.AllBuffered, 5);

                // Kaynak miktarýný sadece yerel oyuncuda güncelle
                if (photonView.IsMine)
                {
                    int randomNumber = Random.Range(20, 25);
                    WoodRockController.rockCount += randomNumber;
                }
            }
        }
    }
}
