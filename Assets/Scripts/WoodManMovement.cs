using Photon.Pun;
using UnityEngine;

public class WoodManMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Hareket h�z�
    public Animator animator;   // Animator bile�eni
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

            // Hareket giri�lerini kontrol et
            if (Input.GetKey(KeyCode.W)) // �leri
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
            if (Input.GetKey(KeyCode.D)) // Sa�
            {
                movement += Vector3.right;
                movement += Vector3.back;
            }

            // Hareket uygula
            if (movement.magnitude > 0)
            {
                // Karakteri hareket ettir
                transform.Translate(movement.normalized * moveSpeed * Time.deltaTime, Space.World);

                // Karakteri hareket y�n�ne d�nd�r
                Quaternion toRotation = Quaternion.LookRotation(movement.normalized, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.deltaTime);

                // Y�r�me animasyonunu tetikle
                animator.SetFloat("Speed", 1);
            }
            else
            {
                // Y�r�me animasyonunu durdur
                animator.SetFloat("Speed", 0);
            }

            // Sald�r� kontrol�
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
        }
        
    }

    void Attack()
    {
        // Sald�r� animasyonunu tetikle
        animator.SetTrigger("Attack");
        if (axeCollider != null)
        {
            axeCollider.enabled = true;
        }

        // Baltan�n collider'�n� belirli bir s�re sonra kapat
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
