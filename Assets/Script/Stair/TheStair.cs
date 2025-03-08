using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TheStair : MonoBehaviour
{
    //初始的第二层，请在编辑器页面手动添加属性：2楼，1号，正常，isInitStair = true
    public GameObject UpLocation;
    public GameObject FloorNoObject;
    public GameObject DownWall;     //用于防止反悔往回走，默认是隐藏的
    public GameObject UpWall;
    public List<GameObject> Lights; //管理本层的灯

    public int floorNum;
    public int floorNo;      //楼层的唯一编号
    public bool isNormal;
    public int abCollectionIndex;   //编辑器页面手动赋予 异常收集的索引
    public int enterState = 0;  // 初始0，进入后为1，离开后为-1，提供给其他脚本判断玩家是否进入楼层
    public int abnormalNo = -1;    //异常序号，未赋值和正常楼层为-1，由StairManager调用时赋值

    public static readonly int Err_Rate = 62;   //60%概率出异常
    readonly int Off_Light_Rate = 13;       //10概率楼层停电
    readonly int Flash_Light_Rate = 28;     //25-10概率楼层有灯会闪

    public static readonly float HEIGHT = 6.65f;
    public bool isInitStair;    //标记为初始层的楼层会只反应离开，不管进入(仅初始的示范层为true)
    void Start()
    {
        UpLocation = transform.Find("UpLocation").gameObject;
    }
    public Vector3 ReturnUpLocation()
    {
        return UpLocation.transform.position; 
    }
    public void InitStair(int floorNum, bool isNormal, int abnormalNo)
    {
        InitLights(floorNum);
        this.floorNum = floorNum;
        this.isNormal = isNormal;
        this.floorNo = StairManager.Instance.GetFloorNo();
        this.abnormalNo = abnormalNo;

        FloorNoObject.GetComponent<TextMeshPro>().text = floorNum.ToString()+"F";
        //FloorNoObject.GetComponent<TextMeshPro>().text = floorNum.ToString() + isNormal.ToString();
    }
    public void SetPos(Vector3 pos)
    {
        transform.position = pos;
    }
    public void Leaving(bool upward)   //玩家从某方向离开本层
    {
        enterState = -1;
        StairManager.Instance.ReceiveLeaving(floorNo, upward);
        if(!isNormal && !upward){   //向下离开异常楼层
            AbnormalCollected.Instance.CollectAbnormal(abCollectionIndex);
            Debug.Log("离开异常层，序号为："+abCollectionIndex);
        }
    }
    public void Entering()
    {
        enterState = 1;
        if (isInitStair) return;    //标记为初始层的楼层会只反应向上离开，不管任何进入和向下离开(仅初始的示范层为true)
        StairManager.Instance.ReceiveEntering(floorNo,abnormalNo);
    }
    public void ShowWall(bool upward)   //显示防止反悔的墙
    {
        if(upward)UpWall.SetActive(true);
        else DownWall.SetActive(true);
    }
    public void InitLights(int floorNum)
    {
        if(floorNum == StairManager.Instance.GOAL_FLOOR_NUM) return;  //结局楼层灯正常
        int s = Random.Range(0, 100);
        if(s < Off_Light_Rate)
        {
            for(int i = 0; i < Lights.Count; i++)
            {
                Lights[i].GetComponent<StairLight>().SwitchLightMode(2);
            }
        }
        else if(s < Flash_Light_Rate)
        {
            int e = Random.Range(0, Lights.Count);
            Lights[e].GetComponent<StairLight>().SwitchLightMode(1);
        }
    }
    
}

