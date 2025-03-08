using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject DarkBackground;
    public GameObject CentralPoint;
    public GameObject Timer;
    public TextMeshProUGUI PressETip;   //���ư�E�����Ͳ鿴��UI
    public List<GameObject> PressTipList;
    public GameObject CheatModeBoard;  //����L C V������������
    public GameObject PressButtonTipBoard;  //�������������
    public GameObject TitleBoard;   //�������
    public GameObject TimerAndPoint;   //��ʱ�������ĵ�
    public GameObject TheEndingBoard;
    public GameObject ESCBoard;
    bool isInspecting = false;
    bool titleState = false;
    bool ESCState = false;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //SEManager.Instance.ChangeMainVolume(MainVolumeSlider.value);
        SEManager.Instance.ChangeMainVolume(PlayerPrefs.GetFloat("Volume",MainVolumeSlider.value));
        Player.Instance.ChangeMouseSenstive(PlayerPrefs.GetFloat("MouseSensivity",MouseSenstivitySlider.value));
        PressETip.transform.parent.gameObject.SetActive(false);
        ESCBoard.SetActive(false);
        CheatModeBoard.SetActive(false);
        ShowPressTip(0, true);  //ESD
        ShowPressTip(1, false);  //C
        ShowPressTip(2, false);  //L
        ShowPressTip(3, false);  //V
        ShowPressTip(4, true);  //Shift
        ShowPressTip(5, true);  //F
        ShowPressTip(6, false);  //����
        ShowPressTip(7, false);  //wasd
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && titleState==false)
        {
            if(isInspecting)    //����״̬�°�ESC�˳�����
            {
                InspectingMode(false);
                Cursor.visible = false; // �������
                Cursor.lockState = CursorLockMode.Locked; // �����������Ļ����
            }
            else    //�����˵���
            {
                ShowESCBoard(!ESCState);
            }
        }
    }
    public void InspectingMode(bool b)
    {
        if (b)  //����鿴ģʽ
        {
            DarkBackground.SetActive(true);
            CentralPoint.SetActive(false);
            CheatModeBoard.SetActive(false);  //�ر��������
            ShowPressTip(4, false);
            ShowPressTip(5, false);
            ShowPressTip(6, true);
            ShowPressTip(7, true);
            isInspecting = true;
        }
        else
        {
            DarkBackground.SetActive(false);
            CentralPoint.SetActive(true);
            CheatModeBoard.SetActive(true);   //����������壬�������װ�ť�ø�������Ƿ������׳���
            ShowPressTip(4, true);
            ShowPressTip(5, true);
            ShowPressTip(6, false);
            ShowPressTip(7, false);
            isInspecting = false;
        }
    }
    public void ShowPressTip(int no,bool b)
    {
        if (no >= PressTipList.Count) return;
        PressTipList[no].SetActive(b);
    }
    public void SwitchUITip(int no)
    {
        if (no >= PressTipList.Count) return;
        if (PressTipList[no].GetComponent<SwitchImg>() != null)
        {
            PressTipList[no].GetComponent<SwitchImg>().Switch();
        }
    }
    public void DeathMode()
    {
        for(int i=0;i<PressTipList.Count;i++){
            PressTipList[i].SetActive(false);
        }
    }
    public void ShowPressETip(bool b, string s = null){
        if(s!=null){
            PressETip.text = s;
        }
        PressETip.transform.parent.gameObject.SetActive(b);
    }
    public void ShowCheatModeButton(bool b){
        ShowPressTip(2, b);
        ShowPressTip(3, b);
        ShowPressTip(1, b);
        if(!isInspecting)CheatModeBoard.SetActive(true);
    }
    public void SetTitleBoard(bool b){  //����ʱ��Scene Manager�򿪣������ʼ��Ϸ��ر�
        if(!b){     //�رձ���
            //SetTitleState(false); �ڶ����������ٹرմ�״̬
            GraduallyShowBoard(2,false);    //�������ر���
            //������ʾ���볡�������
            SEManager.Instance.StartCoroutine(SEManager.Instance.SwitchingToGameBGM()); //�л�����

            AnimationSystem.Instance.EnteringGame();
            Cursor.visible = false; // �������
            Cursor.lockState = CursorLockMode.Locked; // �����������Ļ����
        } 
        else{       //��������
            SetTitleState(true);
            TitleBoard.GetComponent<CanvasGroup>().alpha = 1;   //������ʾ����
            PressButtonTipBoard.GetComponent<CanvasGroup>().alpha = 0;  //�������ذ�����ʾ
            TimerAndPoint.GetComponent<CanvasGroup>().alpha = 0;  

            Player.Instance.ForbidMoving(false);
            Cursor.visible = true; // ��ʾ���
            Cursor.lockState = CursorLockMode.None; // ����������
        }
    }
    public void GraduallyShowBoard(int boardNo,bool onOrOff){
        //0��Ϊ��ʱ�������ĵ㣬1��Ϊ������ʾ�壬2��Ϊ�����
        CanvasGroup cg = null;
        if(boardNo==0)cg = TimerAndPoint.GetComponent<CanvasGroup>();
        else if(boardNo==1)cg = PressButtonTipBoard.GetComponent<CanvasGroup>();
        else if(boardNo==2)cg = TitleBoard.GetComponent<CanvasGroup>();

        float finalAlpha = 0;
        if(onOrOff)finalAlpha = 1;

        StartCoroutine(FadeCanvasGroupRoutine(cg, finalAlpha, 2f));
    }
    private IEnumerator FadeCanvasGroupRoutine(CanvasGroup cg, float end, float duration)
    {
        float start = 0f;
        if(end == 0)start = 1f;

        float counter = 0f;
        while (counter < duration)
        {
            counter += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, counter / duration);
            if (Mathf.Approximately(cg.alpha, end)) break;
            yield return null;
        }
        cg.alpha = end;
    }
    public void ShowEndingBoard(){
        TimerAndPoint.GetComponent<CanvasGroup>().alpha = 0;  
        Cursor.visible = true; // ��ʾ���
        Cursor.lockState = CursorLockMode.None; // ����������
        TheEndingBoard.GetComponent<EndingBoard>().Show();
        CanvasGroup cg = TheEndingBoard.GetComponent<CanvasGroup>();
        StartCoroutine(FadeCanvasGroupRoutine(cg,1,2f));
    }
    public void SetTitleState(bool b){
        titleState = b;
    }
    public void ShowESCBoard(bool b){
        SwitchUITip(3);
        VHSEffect vhs1 = Player.Instance.PlayerCamera.GetComponent<VHSEffect>();
        VHSEffect vhs2 = Player.Instance.UICamera.GetComponent<VHSEffect>();
        vhs1.enabled = !vhs1.enabled;
        vhs2.enabled = !vhs2.enabled;
        ESCBoard.SetActive(b);
        ESCState = b;

        if(b){  //��ESC��
            CentralPoint.SetActive(false);
            CheatModeBoard.SetActive(false);  //�ر��������
            ShowPressTip(4, false);
            ShowPressTip(5, false);
            ShowPressTip(6, true);
            ShowPressTip(7, true);
            Player.Instance.ForbidMoving(false);
            Cursor.visible = true; // ��ʾ���
            Cursor.lockState = CursorLockMode.None; // ����������
        }
        else{
            CentralPoint.SetActive(true);
            CheatModeBoard.SetActive(true);   //����������壬�������װ�ť�ø�������Ƿ������׳���
            ShowPressTip(4, true);
            ShowPressTip(5, true);
            ShowPressTip(6, false);
            ShowPressTip(7, false);
            Player.Instance.ForbidMoving(true);
            Cursor.visible = false; // �������
            Cursor.lockState = CursorLockMode.Locked; // �����������Ļ����
        }
    }

    
    public Slider MouseSenstivitySlider;    //��������Ȼ�����
    public void OnMouseSenstivitySliderChange(){
        Player.Instance.ChangeMouseSenstive(MouseSenstivitySlider.value);
    }
    public Slider MainVolumeSlider;    //����������
    public void OnMainVolumeSliderChange(){
        SEManager.Instance.ChangeMainVolume(MainVolumeSlider.value);
    }
}