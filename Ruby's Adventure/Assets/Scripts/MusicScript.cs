using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip backgroundMusic;
    public AudioClip winMusic;
    public AudioClip loseMusic;
    // Start is called before the first frame update
    void Start()
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyController.win == true)
        {
            if (musicSource.clip == backgroundMusic)
            {
                musicSource.clip = winMusic;
                musicSource.Play();
            }
        }
        if (RubyController.currentHealth <= 0)
        {
            if (musicSource.clip == backgroundMusic)
            {
                musicSource.clip = loseMusic;
                musicSource.Play();
            }
        }
    }
}
