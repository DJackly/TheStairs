using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class AbnormalCollected : MonoBehaviour
{
    public static AbnormalCollected Instance;
    public static readonly int TotalAbnormal = 18;
    public GameObject Board;    //�����������
    public List<GameObject> AbCollectionList;    //չʾ�ռ������쳣��С����
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
    public void RefreshAbnormalCollected() //����״̬
    {
        int count = 0;
        // ��ȡPlayerPrefs�е��쳣�ռ�״̬
        abnormalCollected = PlayerPrefs.GetString("AbnormalCollected", "");
        if (abnormalCollected == "")    // ���û�б�������ݣ���ʼ��Ϊȫ��δ�ռ�
        {
            abnormalCollected = new string('0', 18);
        }
        // ���ַ���ת��Ϊ�������飬���ң�����Ӧ��1���쳣������ʾ��0������
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
            if(abnormalCollectedArray[abnormalIndex] == false){     //֮ǰδ�ռ����쳣
                // ���²�������
                abnormalCollectedArray[abnormalIndex] = true;
                // ����������ת��Ϊ�ַ���
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
    private int itemIndex1; //ʮλ
    private int itemIndex2; //��λ
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
            //������λ��
            if(! isIndex1Confirm)   //ʮλ��δ����
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
            else     //ʮλ��������
            {
                for (int i = 0; i <= 9; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                    {
                        itemIndex2 = i;
                        CollectAbnormal(10*itemIndex1 + itemIndex2);
                        int n = 10*itemIndex1 + itemIndex2;
                        Debug.Log("@@@���ԣ�����쳣��"+n);
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
