using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthTracker : MonoBehaviourPun
{
    public Slider HealthBarSlider; // Saðlýk çubuðu
    public Image sliderFill; // Çubuðun dolum rengi
    public Material greenEmission; // Yeþil malzeme (yüksek saðlýk)
    public Material yellowEmission; // Sarý malzeme (orta saðlýk)
    public Material redEmission; // Kýrmýzý malzeme (düþük saðlýk)

    private Coroutine smoothHealthChangeCoroutine; // Saðlýk çubuðunun deðiþimi için Coroutine
    PhotonView pw;
    // Saðlýk çubuðunun deðerini güncelleyerek rengini deðiþtiren fonksiyon
    private void Start()
    {
        pw = GetComponent<PhotonView>();
    }
    [PunRPC]
    public void UpdateSliderValue(float currentHealth, float maxHealth)
    {
        float healthPercentage = Mathf.Clamp01(currentHealth / maxHealth);

        // Saðlýk barý üzerinde yapýlan deðiþiklik için animasyon eklemek
        if (smoothHealthChangeCoroutine != null)
        {
            StopCoroutine(smoothHealthChangeCoroutine);
        }

        smoothHealthChangeCoroutine = StartCoroutine(SmoothHealthChange(HealthBarSlider.value, healthPercentage, 0.5f));

        // Saðlýk yüzdesine göre renk deðiþimi
        UpdateColor(healthPercentage);
    }

    // Saðlýk çubuðunun deðerini pürüzsüz þekilde deðiþtiren Coroutine
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

    // Saðlýk yüzdesine göre renk güncellemeleri
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

    // Saðlýk deðiþtiðinde bu fonksiyonla güncellenmesini saðla
    public void SyncHealth(float currentHealth, float maxHealth)
    {
        if (pw != null && pw.IsMine)
        {
            pw.RPC("UpdateSliderValue", RpcTarget.AllBuffered, currentHealth, maxHealth);
        }
    }

}
