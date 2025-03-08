using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PortalCamera : MonoBehaviour
{
    public GameObject FlashLight;
    void Start()
    {
        if(FlashLight!=null){
            FlashLight.SetActive(Player.Instance.lightState);
            MyEventSystem.Instance.PlayerSwitchFlashLight += SwitchFlashLight;
            MyEventSystem.Instance.PlayerSwitchClearFlag += SwitchClearFlag;
        }
        GetComponent<Camera>().clearFlags = Camera.main.clearFlags;
        GetComponent<Camera>().backgroundColor = Camera.main.backgroundColor;
        GetComponent<Camera>().cullingMask = Camera.main.cullingMask;
    }
    private void SwitchFlashLight(){
        if(FlashLight==null)return;
        FlashLight.SetActive(Player.Instance.lightState);
        FlashLight.GetComponent<Light>().range = Player.Instance.FlashLight.GetComponent<Light>().range;
        FlashLight.GetComponent<Light>().intensity = Player.Instance.FlashLight.GetComponent<Light>().intensity;
    }
    private void SwitchClearFlag(){
        if(GetComponent<Camera>().clearFlags != Camera.main.clearFlags){
            GetComponent<Camera>().clearFlags = Camera.main.clearFlags;
        }
    }
}
