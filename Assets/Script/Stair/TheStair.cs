using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TheStair : MonoBehaviour
{
    //��ʼ�ĵڶ��㣬���ڱ༭��ҳ���ֶ�������ԣ�2¥��1�ţ�������isInitStair = true
    public GameObject UpLocation;
    public GameObject FloorNoObject;
    public GameObject DownWall;     //���ڷ�ֹ���������ߣ�Ĭ�������ص�
    public GameObject UpWall;
    public List<GameObject> Lights; //������ĵ�

    public int floorNum;
    public int floorNo;      //¥���Ψһ���
    public bool isNormal;
    public int abCollectionIndex;   //�༭��ҳ���ֶ����� �쳣�ռ�������
    public int enterState = 0;  // ��ʼ0�������Ϊ1���뿪��Ϊ-1���ṩ�������ű��ж�����Ƿ����¥��
    public int abnormalNo = -1;    //�쳣��ţ�δ��ֵ������¥��Ϊ-1����StairManager����ʱ��ֵ

    public static readonly int Err_Rate = 62;   //60%���ʳ��쳣
    readonly int Off_Light_Rate = 13;       //10����¥��ͣ��
    readonly int Flash_Light_Rate = 28;     //25-10����¥���еƻ���

    public static readonly float HEIGHT = 6.65f;
    public bool isInitStair;    //���Ϊ��ʼ���¥���ֻ��Ӧ�뿪�����ܽ���(����ʼ��ʾ����Ϊtrue)
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
    public void Leaving(bool upward)   //��Ҵ�ĳ�����뿪����
    {
        enterState = -1;
        StairManager.Instance.ReceiveLeaving(floorNo, upward);
        if(!isNormal && !upward){   //�����뿪�쳣¥��
            AbnormalCollected.Instance.CollectAbnormal(abCollectionIndex);
            Debug.Log("�뿪�쳣�㣬���Ϊ��"+abCollectionIndex);
        }
    }
    public void Entering()
    {
        enterState = 1;
        if (isInitStair) return;    //���Ϊ��ʼ���¥���ֻ��Ӧ�����뿪�������κν���������뿪(����ʼ��ʾ����Ϊtrue)
        StairManager.Instance.ReceiveEntering(floorNo,abnormalNo);
    }
    public void ShowWall(bool upward)   //��ʾ��ֹ���ڵ�ǽ
    {
        if(upward)UpWall.SetActive(true);
        else DownWall.SetActive(true);
    }
    public void InitLights(int floorNum)
    {
        if(floorNum == StairManager.Instance.GOAL_FLOOR_NUM) return;  //���¥�������
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

