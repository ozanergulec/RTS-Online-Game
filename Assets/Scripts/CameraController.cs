using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;  // Kameran�n hareket h�z�
    public float boundary = 0.05f; // Fare ekran�n kenar�na ne kadar yakla��rsa hareket ba�lar (0-1 aras�, 0.1 = %10)

    void Update()
    {
        Vector3 direction = Vector3.zero;

        // Fare ekran�n kenar�na yak�n m� kontrol et
        if (Input.mousePosition.x >= Screen.width * (1 - boundary))
        {
            direction += Vector3.right;
            direction += Vector3.back;
        }
        if (Input.mousePosition.x <= Screen.width * boundary)
        {
            direction += Vector3.left;
            direction += Vector3.forward;
            
        }
        if (Input.mousePosition.y >= Screen.height * (1 - boundary))
        {
            direction += Vector3.forward; 
            direction += Vector3.right;
        }
        if (Input.mousePosition.y <= Screen.height * boundary)
        {
            direction += Vector3.back;
            direction += Vector3.left;
        }

        // Kameray� hareket ettir
        transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
    }
}
