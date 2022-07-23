using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMOD_OneShot : MonoBehaviour
{
    public string soundEvent = null;

    public void PlaySoundEvent()
    {
        if (soundEvent != null)
        {
            RuntimeManager.PlayOneShot(soundEvent);
        }
    }
}
