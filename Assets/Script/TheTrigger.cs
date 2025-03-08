using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheTrigger : MonoBehaviour
{
    //���ڴ���beingTrigger������
    public BeingTriggered beingTriggered;
    public bool TriggerOnce;    //�Ƿ�ֻ�ܴ���һ��
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
