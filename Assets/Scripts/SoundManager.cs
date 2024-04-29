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
    private bool _readyToPlay = true;

    Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    
    void Awake()
    {
        AudioClip[] clips = Resources.LoadAll<AudioClip>(SFX) as AudioClip[];

        foreach (AudioClip clip in clips)
        {
            audioClips.Add(clip.name, clip);
        }
    }

    void Update()
    {
        if (!sfxSource.isPlaying && !_readyToPlay)
        {
            _readyToPlay = true;
        }
    }

    public void PlaySFX(string name)
    {
        if (!sfxSource.isPlaying) sfxSource.PlayOneShot(audioClips[name]);
    }
    
    public void PlayThrowRope()
    {
        if (_readyToPlay)
            
            //(Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) 
            //|| Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)))
        {
            sfxSource.PlayOneShot(audioClips[THROW_ROPE]);
            _readyToPlay = false;
        }
        else
        {
            Debug.Log("not");
        }
            
            
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