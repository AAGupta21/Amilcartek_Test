using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    [SerializeField] private AudioSource bg_source = null;

    private bool IsMusicOn = true;

    public bool SwitchMusic()
    {
        if(IsMusicOn)
        {
            bg_source.Pause();
            IsMusicOn = false;
        }
        else
        {
            bg_source.UnPause();
            IsMusicOn = true;
        }

        return IsMusicOn;
    }
}
