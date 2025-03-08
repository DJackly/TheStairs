using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class StairManager : MonoBehaviour
{
    public static StairManager Instance;
    public List<GameObject> StairList = new List<GameObject>();     //��0��1��2����λ�ã�1Ϊ�м�㣬0Ϊ1���²㣬2Ϊ1���ϲ�
    public List<GameObject> AbnormalList = new List<GameObject>();  //�쳣¥���
    public List<int> AbnormalFrequency = new List<int>();   //��Ӧ�������쳣¥����ֵĴ��������ֹ�һ�κ��ٳ��ּ���С
    public GameObject BaseStair;    //��ʼ��ʾ����2¥
    public GameObject StairPrefab;
    public GameObject TheFirstStair;    //�ڲ��������ɾ��
    public GameObject EndingStair;
    private GameObject TempStair;   //������ʱ�洢һ��¥�㣨�Ժ�����б��У�

    public int currentFloorNum = 2;     //��2¥��ʼ�����Ŀǰ�Ľ���¥��
    public int leavingIndex = -1;
    private int tempStairIndex = -1;    //��ʱ¥���Ŀ��λ������
    private bool leavingDirection;  //upΪ��
    private int floorNo = 2;    //Stair��Ψһ��ţ�ÿ��¥�㶼�����ظ���1��Ϊ��ʼ��2¥

    public int GOAL_FLOOR_NUM = 6;     //Ŀ�ĵ�¥���

    private void Awake()
    {
        Instance = this;
        AbnormalFrequency = Enumerable.Repeat(0, AbnormalList.Count).ToList();
    }
    private void Start()    //����Ҫʵ�֣���Ϸ��ʼ��1¥����2¥������ʾ���㣩��3¥��ʼ���
    {
        GameObject empty = null;
        StairList.Add(empty);
        StairList.Add(BaseStair);   //1��λ �м��
        StairList.Add(empty);
        SwitchFloor(1);
    }
    public void SwitchFloor(int newMiddleIndex) //��Stair��Ϊ�м�λ�ã�����Stair����1����Ȼ�󰴹�����±������¡�Ŀǰ¥�㡿
    {
        HorizonCity.Instance.UpdatePos(StairList[newMiddleIndex].gameObject.transform.position.y);
        if (newMiddleIndex == 0)    //�����¥����ԭ��0����¥���Ϊ1,��ʱ�����0������2
        {
            GameObject temp = StairList[1];
            StairList[1] = null; Destroy(temp);
            temp = StairList[2];
            StairList[2] = null; Destroy(temp);

            StairList[1] = StairList[0];
            StairList[0] = TempStair;
            TempStair = null;
            AddFloor(1, true);  //��1���Ϸ�������������2
        }
        else if (newMiddleIndex == 2)    //�����¥����ԭ��2����¥���Ϊ1����ʱ�����2������0
        {
            GameObject temp = StairList[1];
            StairList[1] = null; Destroy(temp);
            temp = StairList[0];
            StairList[0] = null; Destroy(temp);


            StairList[1] = StairList[2];
            StairList[2] = TempStair;
            TempStair = null;
            AddFloor(1, false);  //��1���·�������������0
        }
        else if (newMiddleIndex == 1)
        {
            Debug.Log("��1Ϊ���м��SwitchFloor�Ĳ���������Ϸ��ʼʱ����");
            AddFloor(1, true);  //ֻ����¥�ϣ���Ϊ¥��Ϊ1¥
        }
        else Debug.LogError("�㳢�Ը���һ����0��1��2������¥��");
    }
    private void AddFloor(int index, bool up, bool tempStairFlag = false)  //��ĳ����Ϸ����·�����¥��
    {
        if (up)     //��¥������¥��
        {
            GameObject go;
            if (StairList[index].GetComponent<TheStair>().isNormal)    //ԭ¥������
            {
                if (StairList[index].GetComponent<TheStair>().floorNum + 1 == GOAL_FLOOR_NUM)    //��һ����ڽ��¥��ʱ�����ɽ��¥��
                {
                    go = InstantiateStair(StairList[index].GetComponent<TheStair>().floorNum + 1, true);
                }
                else
                {
                    go = InstantiateStair(StairList[index].GetComponent<TheStair>().floorNum + 1, false);    //��¥ Ϊ��ȷ�𰸣�����һ������/�쳣�ĸ�����¥��
                }
            }
            else
            {
                go = InstantiateStair(2, false); //�м�㲻����������¥��ص�2¥
            }

            if (index == 1) StairList[2] = go;   //��¥�ϲ����2
            else if (index == 2)
            {
                TempStair = go; //����һ���б�֮���¥�㣬�Ժ�����б�
            }
            else Debug.LogError("����¥�������쳣");
            go.GetComponent<TheStair>().SetPos(StairList[index].GetComponent<TheStair>().ReturnUpLocation());
        }
        else       //��¥������¥��
        {
            GameObject go;
            if (StairList[index].GetComponent<TheStair>().isNormal)    //�м������
            {
                go = InstantiateStair(2, false);  //��¥ ���󣬻ص�2¥
            }
            else
            {
                if (StairList[index].GetComponent<TheStair>().floorNum + 1 == GOAL_FLOOR_NUM)    //��һ����ڽ��¥��ʱ�����ɽ��¥��
                {
                    go = InstantiateStair(StairList[index].GetComponent<TheStair>().floorNum + 1, true);
                }
                else
                {
                    go = InstantiateStair(StairList[index].GetComponent<TheStair>().floorNum + 1, false); //�м�㲻����������¥ Ϊ��ȷ�𰸣�����һ������/�쳣�ĸ�����¥��
                }
            }
            if (index == 1) StairList[0] = go;   //��¥�²����0
            else if (index == 0)
            {
                TempStair = go; //����һ���б�֮���¥�㣬�Ժ�����б�
            }
            else Debug.LogError("����¥�������쳣");
            go.GetComponent<TheStair>().SetPos( StairList[index].transform.position - new Vector3(0,TheStair.HEIGHT,0) );   
        }
    }
    public int GetFloorNo()
    {
        floorNo++;
        return floorNo;
    }
    public void ReceiveLeaving(int leavingStairNo,bool upward)
    {
        if(leavingIndex == -1)   //˵��ԭ����δ�뿪ĳ��
        {
            for (int i = 0; i <= 2; i++)    //��ʵ������˵�뿪��ֻ�����м�㣬index=1
            {
                if (StairList[i] == null) continue;
                if (StairList[i].GetComponent<TheStair>()?.floorNo == leavingStairNo)
                {
                    leavingIndex = i;
                    leavingDirection = upward;
                    if (upward) //�����뿪index�㣬��index+1���Ϸ�����һ��,�ò�洢��tempStair
                    {
                        AddFloor(leavingIndex + 1, true, true);
                        StairList[i].GetComponent<TheStair>().ShowWall(false);  //���µĳ��ڶ�ס
                    }
                    else
                    {
                        AddFloor(leavingIndex - 1, false, true);
                        StairList[i].GetComponent<TheStair>().ShowWall(true);//���ϵĳ��ڶ�ס
                    }
                    break;
                }
            }
        }
    }
    public void ReceiveEntering(int enteringStairNo, int abnormalNo)
    {
        if (leavingIndex != -1)  //������뿪ĳ�㣬��������������һ��
        {
            EnterAbnormalFloor(abnormalNo); //���쳣�㱻������ˣ�������1
            //�������²�
            for (int i = 0; i <= 2; i++)    
            {
                if (StairList[i] == null) continue;
                if (StairList[i].GetComponent<TheStair>()?.floorNo == enteringStairNo)
                {
                    if (i != 1)  //�뿪��(ֻ������1)�ͽ����i��ͬ����������������Ч��
                    {
                        //����ò�󣬸��ĵ�ǰ����¥��
                        if (StairList[i].GetComponent<TheStair>().floorNum == 2)
                        {
                            currentFloorNum = 2;
                        }
                        else
                        {
                            currentFloorNum = StairList[i].GetComponent<TheStair>().floorNum;
                        }
                        //�ѽ������Ϊ�µ�һ�㣬��tempStair�����б�
                        SwitchFloor(i);
                        leavingIndex = -1;  //����
                    }
                    else    //�뿪ĳ��index=1���ֽ���ͬһ�㣬��������
                    {
                        if (leavingDirection)   //�����뿪�ĸò��ֽ���
                        {
                            StairList[i].GetComponent<TheStair>().ShowWall(false);  //���µĳ��ڶ�ס
                        }
                        else StairList[i].GetComponent<TheStair>().ShowWall(true);
                    }

                    if(TheFirstStair != null)Destroy(TheFirstStair);  //ɾ��һ¥
                    break;
                }
            }
        }
        else Debug.LogError("��2¥��¥ �� ����������ĳ��");
    }
    public GameObject InstantiateStair(int floorNum, bool isEnd)   //���ݸ��ʾ��������������쳣¥��
    {
        GameObject thisStair = null;
        int isNormal;
        int abnormalNo = -1;
        int r = -1;
        if (isEnd) isNormal = 0;     //����Ĳ�������Ϊ���¥�� 0
        else if (floorNum == 2) isNormal = 1;   //2¥һ������
        else if (Random.Range(0, 100) < TheStair.Err_Rate) //�쳣  -1
        {
            isNormal = -1;
            int min = AbnormalFrequency.Min();
            r = Random.Range(0, AbnormalList.Count);    //���ѡ��һ��
            while (AbnormalFrequency[r] != min)   //����ѡ��ֱ��ѡ�����ִ�����С���쳣¥��֮һ
            {
                if (r == AbnormalList.Count - 1) r = 0;
                else r++;
            }
            abnormalNo = r;
        }
        else isNormal = 1; //����  1

        if (isNormal == 1) thisStair = Instantiate(StairPrefab);
        else if (isNormal == 0) thisStair = Instantiate(EndingStair);
        else if (isNormal == -1)
        {
            if(r == -1) Debug.Log("StairManager��ʼ��¥��ʱ���쳣������Ч");
            else thisStair = Instantiate(AbnormalList[r]);
        }
        else Debug.Log("StairManager��ʼ��¥��������Ч");

        bool p;
        if (isNormal == 0 || isNormal == 1) p = true;
        else p = false;
        thisStair.GetComponent<TheStair>().InitStair(floorNum, p, abnormalNo);
        return thisStair;
    }
    public void EnterAbnormalFloor(int abnormalNo)
    {
        if(abnormalNo == -1) return;
        AbnormalFrequency[abnormalNo] += 1;
    }
    bool GiantFloorFlag = false;
    public void DeleteGiantFloor(){     //ĳһ�����쳣¥��ֻ�ܳ���һ��
        //�����¥�����б���ɾ�������͵�¥��
        //��ʱλ�ڸ��쳣¥�㣬����currentFloor����GiantFloor
        if(!GiantFloorFlag){
            GiantFloorFlag = true;
            int i = StairList[1].GetComponent<TheStair>().abnormalNo;
            AbnormalFrequency.RemoveAt(i);
            AbnormalList.RemoveAt(i);
        }
    }
}
