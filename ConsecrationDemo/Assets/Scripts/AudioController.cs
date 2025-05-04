using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    public AudioClip[] BGM;
    public AudioClip[] Sound;
    public Slider volumeSlider;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        audioSource.volume = volumeSlider.value*0.4f;
    }

    public void PlayBGM(int i)
    {
        audioSource.Stop();
        audioSource.clip = BGM[i];
        audioSource.Play();
    }
    public void PlaySound(int i)
    {
        audioSource.PlayOneShot(Sound[i]);
    }
    public void StopBGM()
    {
        audioSource.Stop();
    }
}
