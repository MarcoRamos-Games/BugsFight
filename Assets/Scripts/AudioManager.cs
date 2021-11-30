using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class AudioManager : MonoBehaviour
{
    public AudioSource[] soundEffects;

    public AudioSource menuMusic, backgroundMusic;


    public static AudioManager instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        { // the singleton instance has already been initialized
            if (instance != this)
            { // if this instance of GameManager is not the same as the initialized singleton instance, it is a second instance, so it must be destroyed!
                Destroy(gameObject); // watch out, this can cause trouble!
            }
        }
    }
    // Start is called before the first frame update

    private void Update()
    {

    }

    public void PlaySFX(int soundToPlay)
    {
        //soundEffects[soundToPlay].gameObject.SetActive(true);
        soundEffects[soundToPlay].Stop();
        soundEffects[soundToPlay].pitch = Random.Range(1.3f, 1.5f);
        soundEffects[soundToPlay].Play();

    }
    public void PlayMenuMusic()
    {
        menuMusic.Stop();
        backgroundMusic.Stop();
        menuMusic.Play();

    }


    public void PlayBackgrounMusic()
    {
        backgroundMusic.Stop();
        menuMusic.Stop();
        backgroundMusic.Play();
    }





}
