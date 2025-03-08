using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavingBox : MonoBehaviour
{
    public bool isUp; //�������뿪�ļ������
    public bool locked = false;

    public void ReceiveDetecting(bool isInside)
    {
        if (!locked)    
        {
            locked = true;
            if (isInside)   //�������ڲ��������������뿪
            {
                transform.parent.GetComponent<TheStair>().Leaving(isUp);
            }
            else   //���Ϊ����ò�
            {
                transform.parent.GetComponent<TheStair>().Entering();
            }
        }
        else
        {
            locked = false; //���յ��ڶ��������
        }
    }
}
