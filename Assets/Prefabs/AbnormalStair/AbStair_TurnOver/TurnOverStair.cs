using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOverStair : BeingTriggered
{
    public GameObject NormalCorridor;   //�������
    public GameObject TurnOver;     //�滻Ϊ���

    private void Awake()
    {
        NormalCorridor.SetActive(true);
        TurnOver.SetActive(false) ;
    }
    public override void TriggerIt()
    {
        Player.Instance.Blink();
        Invoke("SwitchStair", 0.2f);
    }
    private void SwitchStair()
    {
        NormalCorridor.SetActive(false);
        TurnOver.SetActive(true);
    }
}
