using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    [SerializeField] bool randomPitch;
    [SerializeField] float minPitch;
    [SerializeField] float maxPitch;
    [SerializeField] AudioClip[] clips;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (randomPitch)
        {
            audioSource.pitch = Random.Range(minPitch, maxPitch);
        }
        audioSource.clip = clips[Random.Range(0, clips.Length)];
        audioSource.Play();
    }

}
