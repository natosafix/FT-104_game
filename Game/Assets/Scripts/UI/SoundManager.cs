using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public Slider soundSlider;
    public GameObject[] objectsWithSounds;

    public void ChangeVolume()
    {
        var newSoundVolume = soundSlider.value;
        for (int i = 0; i < objectsWithSounds.Length; i++)
        {
            objectsWithSounds[i].GetComponent<AudioSource>().volume = newSoundVolume*0.01f;
        }
    }
}
