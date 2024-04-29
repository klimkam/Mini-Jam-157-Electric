using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SoundChanger : MonoBehaviour
{
    [SerializeField]
    AudioSource m_musicSource;
    Scrollbar m_slider;

    private void Start()
    {
        m_slider = gameObject.GetComponent<Scrollbar>();
        m_slider.value = m_musicSource.volume;
    }

    public void ChangeMusicVolum() { 
        m_musicSource.volume = m_slider.value;
    }
}
