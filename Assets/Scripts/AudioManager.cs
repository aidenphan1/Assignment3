using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource;       // assign the Audio Source on this object
    public AudioClip introBgm;            // short stinger (≤ 3s)
    public AudioClip ghostsNormalBgm;     // main gameplay loop

    void Start()
    {
        if (!musicSource) musicSource = GetComponent<AudioSource>();
        StartCoroutine(PlayIntroThenNormal());
    }

    private IEnumerator PlayIntroThenNormal()
    {
        float introLen = introBgm ? introBgm.length : 0f;

        if (introBgm)
        {
            musicSource.clip = introBgm;
            musicSource.loop = false;
            musicSource.Play();
        }

        // Wait for the lesser of 3 seconds or the intro clip length
        yield return new WaitForSeconds(Mathf.Min(3f, introLen));

        if (ghostsNormalBgm)
        {
            musicSource.clip = ghostsNormalBgm;
            musicSource.loop = true;
            musicSource.Play();
        }
    }
}
