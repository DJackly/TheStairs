using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpPaper : MonoBehaviour
{
    public List<GameObject> PuppetList = new List<GameObject>();
    bool flag = false;
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && !flag)
        {
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out hit))
            {
                if(hit.collider.transform == transform)//��ʱ��������������Լ���˵����Ҳ鿴��ֽ���ˣ�Ȼ����������ż����
                {
                    flag = true;
                    for(int i=0;i< PuppetList.Count;i++)
                    {
                        PuppetList[i].SetActive(true);
                    }
                }
            }
            //��żʹ�ýű���������Ƿ񿴵��Լ������״ο���������һ���ֲ���Ϸ���еľ�һ�µ�����
        }
    }
}
