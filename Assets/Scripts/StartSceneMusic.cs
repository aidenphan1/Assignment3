using UnityEngine;

public class StartSceneMusic : MonoBehaviour
{
    public AudioSource music;         
    public AudioClip startSceneBgm;   
    void Awake()
    {
        if (!music) music = GetComponent<AudioSource>(); 
        if (startSceneBgm != null)
        {
            music.clip = startSceneBgm;
            music.loop = true;
            music.spatialBlend = 0f;  
            music.Play();
        }
    }
}
