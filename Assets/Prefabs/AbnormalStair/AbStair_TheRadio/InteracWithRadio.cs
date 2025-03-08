using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithRadio : Interactable
{
    bool on = true;
    Radio myRadio;
    private void Awake()
    {
        myRadio = GetComponent<Radio>();
        InteractType = "Turn on/off";
    }
    public override void Interact()
    {
        SEManager.Instance.SwitchFlash();
        if (on)
        {
            on = false;
            myRadio.audioSource.Pause();
        }
        else
        {
            on = true;
            myRadio.audioSource.UnPause();
        }
    }
}