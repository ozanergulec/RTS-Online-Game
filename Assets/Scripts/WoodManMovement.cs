using Photon.Pun;
using UnityEngine;

public class WoodManMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Hareket hýzý
    public Animator animator;   // Animator bileþeni
    public Collider axeCollider;
    PhotonView pw;
    private void Start()
    {
        pw = GetComponent<PhotonView>();
        animator = GetComponent<Animator>();
        if (axeCollider != null)
        {
            axeCollider.enabled = false;
        }
    }
    void Update()
    {
        if (pw.IsMine)
        {
            Vector3 movement = Vector3.zero;

            // Hareket giriþlerini kontrol et
            if (Input.GetKey(KeyCode.W)) // Ýleri
            {
                movement += Vector3.forward;
                movement += Vector3.right;
            }
            if (Input.GetKey(KeyCode.S)) // Geri
            {
                movement += Vector3.back;
                movement += Vector3.left;
            }
            if (Input.GetKey(KeyCode.A)) // Sol
            {
                movement += Vector3.left;
                movement += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.D)) // Sað
            {
                movement += Vector3.right;
                movement += Vector3.back;
            }

            // Hareket uygula
            if (movement.magnitude > 0)
            {
                // Karakteri hareket ettir
                transform.Translate(movement.normalized * moveSpeed * Time.deltaTime, Space.World);

                // Karakteri hareket yönüne döndür
                Quaternion toRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);

                // Yürüme animasyonunu tetikle
                animator.SetFloat("Speed", 1);
            }
            else
            {
                // Yürüme animasyonunu durdur
                animator.SetFloat("Speed", 0);
            }

            // Saldýrý kontrolü
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
        }
        
    }

    void Attack()
    {
        // Saldýrý animasyonunu tetikle
        animator.SetTrigger("Attack");
        if (axeCollider != null)
        {
            axeCollider.enabled = true;
        }

        // Baltanýn collider'ýný belirli bir süre sonra kapat
        Invoke("DisableAxeCollider", 1f);
    }
    void DisableAxeCollider()
    {
        if (axeCollider != null)
        {
            axeCollider.enabled = false;
        }
    }
}
