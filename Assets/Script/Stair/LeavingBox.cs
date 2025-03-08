using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingBox : MonoBehaviour
{
    public bool isUp; //是向上离开的检测箱吗？
    public bool locked = false;

    public void ReceiveDetecting(bool isInside)
    {
        if (!locked)    
        {
            locked = true;
            if (isInside)   //先碰到内侧再碰到外侧才是离开
            {
                transform.parent.GetComponent<TheStair>().Leaving(isUp);
            }
            else   //检测为进入该层
            {
                transform.parent.GetComponent<TheStair>().Entering();
            }
        }
        else
        {
            locked = false; //接收到第二个则解锁
        }
    }
}
