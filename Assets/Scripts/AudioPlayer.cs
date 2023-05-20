using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    private static AudioPlayer _instance;
    public static AudioPlayer Instance;

    public AudioClip EggCrack, EggPour, EggLift, SoundClick;

    public AudioSource player;


void Awake()
    {
        if (_instance != null)
        {
            Destroy(this);


        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }   


    void PlaySound(AudioClip b)
    {
        player.PlayOneShot(b);
    }


    public void PlayEggcrack()
    {
        PlaySound(EggCrack);
    }

    public void PlayEggPour()
    {
        PlaySound(EggPour);
    }

    public void PlaySoundClick()
    {
        PlaySound(SoundClick);
    }
}
