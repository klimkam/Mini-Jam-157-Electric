using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SoundChanger : MonoBehaviour
{
    [SerializeField]
    AudioSource m_musicSource;
    [SerializeField]
    Slider m_musicSlider;

    public void ChangeMusicVolum() { 
        m_musicSource.volume = m_musicSlider.value;
    }
}
