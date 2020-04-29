using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    public Slider slider;
    public AudioClip music;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.loop = true;
        audioSource.volume = PlayerPrefs.GetFloat("volume", .5f);
        audioSource.clip = music;
        audioSource.Play();
    }

    void Update()
    {
        audioSource.volume = PlayerPrefs.GetFloat("volume", 5f);
    }
}
