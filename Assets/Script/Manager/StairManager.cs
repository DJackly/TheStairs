using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class StairManager : MonoBehaviour
{
    public static StairManager Instance;
    public List<GameObject> StairList = new List<GameObject>();     //仅0，1，2三个位置，1为中间层，0为1的下层，2为1的上层
    public List<GameObject> AbnormalList = new List<GameObject>();  //异常楼层表
    public List<int> AbnormalFrequency = new List<int>();   //对应索引的异常楼层出现的次数，出现过一次后再出现几率小
    public GameObject BaseStair;    //起始的示范层2楼
    public GameObject StairPrefab;
    public GameObject TheFirstStair;    //在步入正轨后删除
    public GameObject EndingStair;
    private GameObject TempStair;   //用于临时存储一个楼层（稍后加入列表中）

    public int currentFloorNum = 2;     //从2楼开始，玩家目前的进度楼层
    public int leavingIndex = -1;
    private int tempStairIndex = -1;    //临时楼层的目标位置索引
    private bool leavingDirection;  //up为真
    private int floorNo = 2;    //Stair的唯一编号，每个楼层都不会重复，1号为初始的2楼

    public int GOAL_FLOOR_NUM = 6;     //目的地楼层号

    private void Awake()
    {
        Instance = this;
        AbnormalFrequency = Enumerable.Repeat(0, AbnormalList.Count).ToList();
    }
    private void Start()    //最终要实现：游戏开始在1楼，且2楼正常（示范层），3楼开始随机
    {
        GameObject empty = null;
        StairList.Add(empty);
        StairList.Add(BaseStair);   //1号位 中间层
        StairList.Add(empty);
        SwitchFloor(1);
    }
    public void SwitchFloor(int newMiddleIndex) //将Stair作为中间位置，即把Stair放入1处，然后按规则更新表，并更新【目前楼层】
    {
        HorizonCity.Instance.UpdatePos(StairList[newMiddleIndex].gameObject.transform.position.y);
        if (newMiddleIndex == 0)    //玩家下楼，则原在0处的楼层变为1,临时层放入0，更新2
        {
            GameObject temp = StairList[1];
            StairList[1] = null; Destroy(temp);
            temp = StairList[2];
            StairList[2] = null; Destroy(temp);

            StairList[1] = StairList[0];
            StairList[0] = TempStair;
            TempStair = null;
            AddFloor(1, true);  //在1的上方新增，即生成2
        }
        else if (newMiddleIndex == 2)    //玩家上楼，则原在2处的楼层变为1，临时层放入2，更新0
        {
            GameObject temp = StairList[1];
            StairList[1] = null; Destroy(temp);
            temp = StairList[0];
            StairList[0] = null; Destroy(temp);


            StairList[1] = StairList[2];
            StairList[2] = TempStair;
            TempStair = null;
            AddFloor(1, false);  //在1的下方新增，即生成0
        }
        else if (newMiddleIndex == 1)
        {
            Debug.Log("以1为新中间层SwitchFloor的操作仅在游戏开始时允许");
            AddFloor(1, true);  //只生成楼上，因为楼下为1楼
        }
        else Debug.LogError("你尝试更换一个非0，1，2索引的楼层");
    }
    private void AddFloor(int index, bool up, bool tempStairFlag = false)  //在某层的上方或下方新增楼层
    {
        if (up)     //在楼上新增楼层
        {
            GameObject go;
            if (StairList[index].GetComponent<TheStair>().isNormal)    //原楼层正常
            {
                if (StairList[index].GetComponent<TheStair>().floorNum + 1 == GOAL_FLOOR_NUM)    //下一层等于结局楼层时，生成结局楼层
                {
                    go = InstantiateStair(StairList[index].GetComponent<TheStair>().floorNum + 1, true);
                }
                else
                {
                    go = InstantiateStair(StairList[index].GetComponent<TheStair>().floorNum + 1, false);    //上楼 为正确答案，生成一个正常/异常的高数字楼层
                }
            }
            else
            {
                go = InstantiateStair(2, false); //中间层不正常，则上楼会回到2楼
            }

            if (index == 1) StairList[2] = go;   //把楼上层加入2
            else if (index == 2)
            {
                TempStair = go; //生成一个列表之外的楼层，稍后加入列表
            }
            else Debug.LogError("新增楼层索引异常");
            go.GetComponent<TheStair>().SetPos(StairList[index].GetComponent<TheStair>().ReturnUpLocation());
        }
        else       //在楼下新增楼层
        {
            GameObject go;
            if (StairList[index].GetComponent<TheStair>().isNormal)    //中间层正常
            {
                go = InstantiateStair(2, false);  //下楼 错误，回到2楼
            }
            else
            {
                if (StairList[index].GetComponent<TheStair>().floorNum + 1 == GOAL_FLOOR_NUM)    //下一层等于结局楼层时，生成结局楼层
                {
                    go = InstantiateStair(StairList[index].GetComponent<TheStair>().floorNum + 1, true);
                }
                else
                {
                    go = InstantiateStair(StairList[index].GetComponent<TheStair>().floorNum + 1, false); //中间层不正常，则下楼 为正确答案，生成一个正常/异常的高数字楼层
                }
            }
            if (index == 1) StairList[0] = go;   //把楼下层加入0
            else if (index == 0)
            {
                TempStair = go; //生成一个列表之外的楼层，稍后加入列表
            }
            else Debug.LogError("新增楼层索引异常");
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
        if(leavingIndex == -1)   //说明原本尚未离开某层
        {
            for (int i = 0; i <= 2; i++)    //其实按理来说离开层只会是中间层，index=1
            {
                if (StairList[i] == null) continue;
                if (StairList[i].GetComponent<TheStair>()?.floorNo == leavingStairNo)
                {
                    leavingIndex = i;
                    leavingDirection = upward;
                    if (upward) //向上离开index层，在index+1的上方增加一层,该层存储在tempStair
                    {
                        AddFloor(leavingIndex + 1, true, true);
                        StairList[i].GetComponent<TheStair>().ShowWall(false);  //向下的出口堵住
                    }
                    else
                    {
                        AddFloor(leavingIndex - 1, false, true);
                        StairList[i].GetComponent<TheStair>().ShowWall(true);//向上的出口堵住
                    }
                    break;
                }
            }
        }
    }
    public void ReceiveEntering(int enteringStairNo, int abnormalNo)
    {
        if (leavingIndex != -1)  //玩家有离开某层，才能正常进入另一层
        {
            EnterAbnormalFloor(abnormalNo); //该异常层被体验过了，计数＋1
            //生成上下层
            for (int i = 0; i <= 2; i++)    
            {
                if (StairList[i] == null) continue;
                if (StairList[i].GetComponent<TheStair>()?.floorNo == enteringStairNo)
                {
                    if (i != 1)  //离开层(只可能是1)和进入层i不同，则正常触发进入效果
                    {
                        //进入该层后，更改当前进度楼层
                        if (StairList[i].GetComponent<TheStair>().floorNum == 2)
                        {
                            currentFloorNum = 2;
                        }
                        else
                        {
                            currentFloorNum = StairList[i].GetComponent<TheStair>().floorNum;
                        }
                        //把进入层作为新的一层，且tempStair加入列表
                        SwitchFloor(i);
                        leavingIndex = -1;  //重置
                    }
                    else    //离开某层index=1后又进入同一层，视作反悔
                    {
                        if (leavingDirection)   //向上离开的该层又进入
                        {
                            StairList[i].GetComponent<TheStair>().ShowWall(false);  //向下的出口堵住
                        }
                        else StairList[i].GetComponent<TheStair>().ShowWall(true);
                    }

                    if(TheFirstStair != null)Destroy(TheFirstStair);  //删除一楼
                    break;
                }
            }
        }
        else Debug.LogError("在2楼下楼 或 非正常进入某层");
    }
    public GameObject InstantiateStair(int floorNum, bool isEnd)   //根据概率决定生成正常或异常楼层
    {
        GameObject thisStair = null;
        int isNormal;
        int abnormalNo = -1;
        int r = -1;
        if (isEnd) isNormal = 0;     //传入的参数表明为结局楼层 0
        else if (floorNum == 2) isNormal = 1;   //2楼一定正常
        else if (Random.Range(0, 100) < TheStair.Err_Rate) //异常  -1
        {
            isNormal = -1;
            int min = AbnormalFrequency.Min();
            r = Random.Range(0, AbnormalList.Count);    //随机选择一个
            while (AbnormalFrequency[r] != min)   //往后选择，直到选到出现次数最小的异常楼层之一
            {
                if (r == AbnormalList.Count - 1) r = 0;
                else r++;
            }
            abnormalNo = r;
        }
        else isNormal = 1; //正常  1

        if (isNormal == 1) thisStair = Instantiate(StairPrefab);
        else if (isNormal == 0) thisStair = Instantiate(EndingStair);
        else if (isNormal == -1)
        {
            if(r == -1) Debug.Log("StairManager初始化楼层时，异常索引无效");
            else thisStair = Instantiate(AbnormalList[r]);
        }
        else Debug.Log("StairManager初始化楼层类型无效");

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
    public void DeleteGiantFloor(){     //某一特殊异常楼层只能出现一次
        //进入该楼层后从列表中删除该类型的楼层
        //此时位于该异常楼层，所以currentFloor就是GiantFloor
        if(!GiantFloorFlag){
            GiantFloorFlag = true;
            int i = StairList[1].GetComponent<TheStair>().abnormalNo;
            AbnormalFrequency.RemoveAt(i);
            AbnormalList.RemoveAt(i);
        }
    }
}
