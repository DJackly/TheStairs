using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingDetecingBox : MonoBehaviour
{
    //��⵽��ײ��˵������ѽ��룬������ֶ���������¥��ţ��׹⣩�����жԻ�
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
