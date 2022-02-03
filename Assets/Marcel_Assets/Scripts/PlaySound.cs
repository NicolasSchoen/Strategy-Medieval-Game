using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource Sound;

    public void playSound()
    {
        Sound.Play();
    }
}
