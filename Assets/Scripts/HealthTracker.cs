using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthTracker : MonoBehaviourPun
{
    public Slider HealthBarSlider; // Sa�l�k �ubu�u
    public Image sliderFill; // �ubu�un dolum rengi
    public Material greenEmission; // Ye�il malzeme (y�ksek sa�l�k)
    public Material yellowEmission; // Sar� malzeme (orta sa�l�k)
    public Material redEmission; // K�rm�z� malzeme (d���k sa�l�k)

    private Coroutine smoothHealthChangeCoroutine; // Sa�l�k �ubu�unun de�i�imi i�in Coroutine
    PhotonView pw;
    // Sa�l�k �ubu�unun de�erini g�ncelleyerek rengini de�i�tiren fonksiyon
    private void Start()
    {
        pw = GetComponent<PhotonView>();
    }
    [PunRPC]
    public void UpdateSliderValue(float currentHealth, float maxHealth)
    {
        float healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);

        // Sa�l�k bar� �zerinde yap�lan de�i�iklik i�in animasyon eklemek
        if (smoothHealthChangeCoroutine != null)
        {
            StopCoroutine(smoothHealthChangeCoroutine);
        }

        smoothHealthChangeCoroutine = StartCoroutine(SmoothHealthChange(HealthBarSlider.value, healthPercentage, 0.5f));

        // Sa�l�k y�zdesine g�re renk de�i�imi
        UpdateColor(healthPercentage);
    }

    // Sa�l�k �ubu�unun de�erini p�r�zs�z �ekilde de�i�tiren Coroutine
    private IEnumerator SmoothHealthChange(float startValue, float targetValue, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            HealthBarSlider.value = Mathf.Lerp(startValue, targetValue, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        HealthBarSlider.value = targetValue;

        smoothHealthChangeCoroutine = null;
    }

    // Sa�l�k y�zdesine g�re renk g�ncellemeleri
    private void UpdateColor(float healthPercentage)
    {
        if (healthPercentage >= 0.6f)
        {
            sliderFill.material = greenEmission;
        }
        else if (healthPercentage >= 0.3f)
        {
            sliderFill.material = yellowEmission;
        }
        else
        {
            sliderFill.material = redEmission;
        }
    }

    // Sa�l�k de�i�ti�inde bu fonksiyonla g�ncellenmesini sa�la
    public void SyncHealth(float currentHealth, float maxHealth)
    {
        if (pw != null && pw.IsMine)
        {
            pw.RPC("UpdateSliderValue", RpcTarget.AllBuffered, currentHealth, maxHealth);
        }
    }

}
