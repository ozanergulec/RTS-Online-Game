using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] AudioSource m_AudioSource;
    public TextMeshProUGUI youlose;
    
    void Start()
    {

        m_AudioSource.Play();
        youlose.gameObject.SetActive(true);
    }

    
    
}
