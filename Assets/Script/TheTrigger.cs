using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheTrigger : MonoBehaviour
{
    //用于触发beingTrigger的子类
    public BeingTriggered beingTriggered;
    public bool TriggerOnce;    //是否只能触发一次
    private int TriggerTimes = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            if (TriggerTimes != 0)
            {
                if (TriggerOnce) return;
            }

            beingTriggered.TriggerIt();
            TriggerTimes++;
        }
    }

}
