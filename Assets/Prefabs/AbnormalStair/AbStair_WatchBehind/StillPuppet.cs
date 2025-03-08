using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StillPuppet : MonoBehaviour
{
    public GameObject theOtherPuppet;
    bool flag = false;
    void Update()
    {
        if (!flag)
        {
            if(Player.Instance.SeeIt(gameObject) || Player.Instance.SeeIt(theOtherPuppet))
            {
                flag = true;
                SEManager.Instance.PianoScary();
            }
        }
        
    }
}
