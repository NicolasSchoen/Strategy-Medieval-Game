using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip[] clips;
    //public AudioClip nightClip;
    private AudioSource audioSource;
    private Camera cam;
    cameraControl camContr;
    public bool cameraHeightVolume = false;

    //private dayLight dL;
    private bool nightClipPlaying;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = false;

        cam = Camera.main;
        camContr = cam.GetComponent<cameraControl>();

        //dL = GameObject.Find("SunLight").GetComponent<dayLight>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (dL.isNight && !nightClipPlaying)
        //{
        //    FadeOutFadeIn();
        //    audioSource.clip = nightClip;
        //    audioSource.Play();
        //    nightClipPlaying = true;
        //    Debug.Log("Music Playing: " + audioSource.clip.name);           
        //}
        //else
        //{
        //    if (!audioSource.isPlaying || (!dL.isNight && nightClipPlaying))
        //    {
        //        nightClipPlaying = false;
        //        audioSource.clip = GetRandomClip();
        //        audioSource.Play();
        //        Debug.Log("Music Playing: " + audioSource.clip.name);
        //    }
        //}

        if (!audioSource.isPlaying)
        {
            audioSource.clip = GetRandomClip();
            audioSource.Play();
            Debug.Log("Music Playing: " + audioSource.clip.name);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            audioSource.clip = GetRandomClip();
            audioSource.Play();
            Debug.Log("Music Playing: " + audioSource.clip.name);
        }
        if (cameraHeightVolume)
        {
            audioSource.volume = Mathf.Clamp(cam.transform.parent.position.y / (camContr.maxheight * 2), 0.1f, 0.5f);
        }
    }

    AudioClip GetRandomClip()
    {
        AudioClip music = null;
        music = clips[Random.Range(0, clips.Length)];

        while (music == audioSource.clip)
        {
            music = clips[Random.Range(0, clips.Length)];
        }
        return music;
    }

    //void FadeOutFadeIn()
    //{
    //    StartCoroutine(WaitBetweenFades());       
    //}

    //public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    //{
    //    float currentTime = 0;
    //    float start = audioSource.volume;

    //    while (currentTime < duration)
    //    {
    //        currentTime += Time.deltaTime;
    //        audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
    //        yield return null;
    //    }
    //    yield break;
    //}

    //IEnumerator WaitBetweenFades()
    //{
    //    StartCoroutine(StartFade(audioSource, 1f, 0f));
    //    yield return new WaitForSeconds(1);
    //    StartCoroutine(StartFade(audioSource, 1f, 0.5f));
    //}
}
