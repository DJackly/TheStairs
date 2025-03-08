using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDetecingBox : MonoBehaviour
{
    //检测到碰撞后说明玩家已进入，触发结局动画（看向楼层号，白光），还有对话
    bool flag = false;
    private void OnTriggerEnter(Collider other)
    {
        if (!flag)
        {
            flag = true;
            AnimationSystem.Instance.Ending();
        }
    }
}
