using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }
    private AudioSource infantryAttackChanel;
    public AudioClip infantryAttackClip;
   
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        infantryAttackChanel = gameObject.AddComponent<AudioSource>();
        infantryAttackChanel.volume = 0.1f;
        infantryAttackChanel.playOnAwake = false;
    }

    public void PlayInfantryAttackSound()
    {
        if (!infantryAttackChanel.isPlaying)
        {
            infantryAttackChanel.PlayOneShot(infantryAttackClip);
        }
    }
   
}
