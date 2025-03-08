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
                if(hit.collider.transform == transform)//此时玩家射线碰到到自己，说明玩家查看该纸条了，然后让身后的人偶现形
                {
                    flag = true;
                    for(int i=0;i< PuppetList.Count;i++)
                    {
                        PuppetList[i].SetActive(true);
                    }
                }
            }
            //人偶使用脚本调用玩家是否看到自己，若首次看到，发出一个恐怖游戏特有的惊一下的声音
        }
    }
}
