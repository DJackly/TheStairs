using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Daytime : MonoBehaviour
{
    public GameObject Daylight;
    GameObject Moon;
    int state = 0;

    private void Start()
    {
        Daylight.SetActive(true);
        Moon = GameObject.Find("Horizon/Moon");
        if (Moon == null) Debug.LogError("‘¬¡¡ªÒ»° ß∞‹");
    }
    private void Update()
    {
        if(state != 1 && GetComponent<TheStair>().enterState == 1)
        {
            EnterStair();
        }
        else if(state != -1 && GetComponent<TheStair>().enterState == -1)
        {
            LeaveStair();
        }
    }
    public void EnterStair()
    {
        state = 1;
        Moon.SetActive(false);
        //Camera.main.clearFlags = CameraClearFlags.Skybox;
        Player.Instance.SwitchClearFlag(0);
    }
    public void LeaveStair()
    {
        state = -1;
        Moon.SetActive(true);
        //Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Player.Instance.SwitchClearFlag(1);
    }
}
