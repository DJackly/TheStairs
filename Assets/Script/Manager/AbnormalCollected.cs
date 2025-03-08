using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AbnormalCollected : MonoBehaviour
{
    public static AbnormalCollected Instance;
    public static readonly int TotalAbnormal = 18;
    public GameObject Board;    //整个子物体板
    public List<GameObject> AbCollectionList;    //展示收集到的异常的小方格
    public bool[] abnormalCollectedArray = new bool[TotalAbnormal];
    public TextMeshProUGUI CollectCount;
    [SerializeField]private string abnormalCollected;
    private void Awake() {
        Instance = this;
    }
    void Start()
    {
        Board.GetComponent<CanvasGroup>().alpha = 0;
        Board.GetComponent<CanvasGroup>().interactable = false;
        Board.GetComponent<CanvasGroup>().blocksRaycasts = false;
        RefreshAbnormalCollected();
    }
    public void RefreshAbnormalCollected() //更新状态
    {
        int count = 0;
        // 读取PlayerPrefs中的异常收集状态
        abnormalCollected = PlayerPrefs.GetString("AbnormalCollected", "");
        if (abnormalCollected == "")    // 如果没有保存的数据，初始化为全部未收集
        {
            abnormalCollected = new string('0', 18);
        }
        // 将字符串转换为布尔数组，并且，将对应的1的异常方格显示，0的隐藏
        for(int i=0;i<abnormalCollected.Length;i++){
            if(abnormalCollected[i]=='1') {
                abnormalCollectedArray[i] = true;
                AbCollectionList[i].GetComponent<AbStairCollection>().EnableAbCollection();
                count++;
            }
            else {
                abnormalCollectedArray[i] = false;
            }
        }
        CollectCount.text = count.ToString();
    }
    public void CollectAbnormal(int abnormalIndex){
        if (abnormalIndex >= 0 && abnormalIndex < 18){
            if(abnormalCollectedArray[abnormalIndex] == false){     //之前未收集该异常
                // 更新布尔数组
                abnormalCollectedArray[abnormalIndex] = true;
                // 将布尔数组转换为字符串
                abnormalCollected = new string(abnormalCollectedArray.Select(b => b ? '1' : '0').ToArray());

                PlayerPrefs.SetString("AbnormalCollected", abnormalCollected);
                PlayerPrefs.Save();
            }
        }
        RefreshAbnormalCollected();
    }
    
    
    
    
    bool startCount = false;
    float timer = 0f;
    bool isIndex1Confirm = false;
    private int itemIndex1; //十位
    private int itemIndex2; //个位
    private void Update() {
        // if(Input.GetKey(KeyCode.M)){
        //     startCount = true;
        // }
        if(Input.GetKey(KeyCode.R)){
            if(Input.GetKey(KeyCode.E)){
                if(Input.GetKeyDown(KeyCode.S)){
                    PlayerPrefs.SetString("AbnormalCollected","");
                }
            }
        }
        if (startCount)
        {
            timer += Time.deltaTime;
            if (timer > 5f)
            {
                ResetThis();
                return;
            }
            //输入两位数
            if(! isIndex1Confirm)   //十位数未输入
            {
                for(int i = 0; i <= 9; i++)
                {
                    if(Input.GetKeyDown(KeyCode.Alpha0 + i))
                    {
                        itemIndex1 = i;
                        isIndex1Confirm = true;
                    }
                }
            }
            else     //十位数输入了
            {
                for (int i = 0; i <= 9; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                    {
                        itemIndex2 = i;
                        CollectAbnormal(10*itemIndex1 + itemIndex2);
                        int n = 10*itemIndex1 + itemIndex2;
                        Debug.Log("@@@测试，获得异常："+n);
                        ResetThis();
                    }
                }
            }
        }
    }
    private void ResetThis()
    {
        timer = 0;
        startCount = false;
        isIndex1Confirm = false;
    }
}
