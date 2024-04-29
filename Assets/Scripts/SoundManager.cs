using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    [SerializeField] 
    private AudioSource _musicSource;
    [FormerlySerializedAs("_sfxSource")] public AudioSource sfxSource;

    private const string SFX = "SFX";
    private const string THROW_ROPE = "ThrowRope";

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    
    void Awake()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(SFX) as AudioClip[];

        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }
    }

    public void PlaySFX(string name)
    {
            sfxSource.PlayOneShot(audioClips[name]);
    }
    
    public void PlayThrowRope()
    {
            sfxSource.PlayOneShot(audioClips[THROW_ROPE]);
    }
    
    public void PlayMusic()
    {
        _musicSource.Play(); 
    }
    
    public void StopMusic()
    {
        _musicSource.Stop(); 
    }
}