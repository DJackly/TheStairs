using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyEventSystem : MonoBehaviour
{
    public static MyEventSystem Instance;
    private void Awake()
    {
        Instance = this;
    }
    public event Action PlayerSwitchFlashLight;
    public void TriggerPlayerSwitchFlashLight(){
        if(PlayerSwitchFlashLight != null) PlayerSwitchFlashLight();
    }
    public event Action PlayerSwitchClearFlag;
    public void TriggerPlayerSwitchClearFlag(){
        if(PlayerSwitchClearFlag != null) PlayerSwitchClearFlag();
    }
}
