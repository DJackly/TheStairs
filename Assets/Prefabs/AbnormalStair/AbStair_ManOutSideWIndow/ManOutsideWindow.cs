using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManOutsideWindow : BeingTriggered
{
    bool flag = false;
    float timer = 0f;
    void Update()
    {
        if (flag)
        {
            timer += Time.deltaTime;
        }
        if(timer >= 0.7f && Player.Instance.SeeIt(gameObject))
        {
            gameObject.GetComponent<Animator>().Play("ManLeave");
        }
    }
    public void EndManEnter()   //由进入动画调用
    {
        flag = true;
    }
    public override void TriggerIt()
    {
        if(!flag) gameObject.GetComponent<Animator>().Play("ManEnter");
    }
}
