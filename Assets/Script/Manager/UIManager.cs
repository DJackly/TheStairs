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
    public TextMeshProUGUI PressETip;   //控制按E交互和查看的UI
    public List<GameObject> PressTipList;
    public GameObject CheatModeBoard;  //包含L C V三个按键在内
    public GameObject PressButtonTipBoard;  //其他按键的面板
    public GameObject TitleBoard;   //标题面板
    public GameObject TimerAndPoint;   //计时器和中心点
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
        ShowPressTip(6, false);  //滚轮
        ShowPressTip(7, false);  //wasd
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && titleState==false)
        {
            if(isInspecting)    //检视状态下按ESC退出检视
            {
                InspectingMode(false);
                Cursor.visible = false; // 隐藏鼠标
                Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标在屏幕中心
            }
            else    //弹出菜单栏
            {
                ShowESCBoard(!ESCState);
            }
        }
    }
    public void InspectingMode(bool b)
    {
        if (b)  //进入查看模式
        {
            DarkBackground.SetActive(true);
            CentralPoint.SetActive(false);
            CheatModeBoard.SetActive(false);  //关闭作弊面板
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
            CheatModeBoard.SetActive(true);   //开启作弊面板，但是作弊按钮得根据玩家是否开启作弊出现
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
    public void SetTitleBoard(bool b){  //进入时由Scene Manager打开，点击开始游戏则关闭
        if(!b){     //关闭标题
            //SetTitleState(false); 在动画结束后再关闭此状态
            GraduallyShowBoard(2,false);    //缓慢隐藏标题
            //按键提示在入场动画后打开
            SEManager.Instance.StartCoroutine(SEManager.Instance.SwitchingToGameBGM()); //切换音乐

            AnimationSystem.Instance.EnteringGame();
            Cursor.visible = false; // 隐藏鼠标
            Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标在屏幕中心
        } 
        else{       //开启标题
            SetTitleState(true);
            TitleBoard.GetComponent<CanvasGroup>().alpha = 1;   //立即显示标题
            PressButtonTipBoard.GetComponent<CanvasGroup>().alpha = 0;  //立即隐藏按键提示
            TimerAndPoint.GetComponent<CanvasGroup>().alpha = 0;  

            Player.Instance.ForbidMoving(false);
            Cursor.visible = true; // 显示鼠标
            Cursor.lockState = CursorLockMode.None; // 解除鼠标锁定
        }
    }
    public void GraduallyShowBoard(int boardNo,bool onOrOff){
        //0号为计时器与中心点，1号为按键提示板，2号为标题板
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
        Cursor.visible = true; // 显示鼠标
        Cursor.lockState = CursorLockMode.None; // 解除鼠标锁定
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

        if(b){  //打开ESC板
            CentralPoint.SetActive(false);
            CheatModeBoard.SetActive(false);  //关闭作弊面板
            ShowPressTip(4, false);
            ShowPressTip(5, false);
            ShowPressTip(6, true);
            ShowPressTip(7, true);
            Player.Instance.ForbidMoving(false);
            Cursor.visible = true; // 显示鼠标
            Cursor.lockState = CursorLockMode.None; // 解除鼠标锁定
        }
        else{
            CentralPoint.SetActive(true);
            CheatModeBoard.SetActive(true);   //开启作弊面板，但是作弊按钮得根据玩家是否开启作弊出现
            ShowPressTip(4, true);
            ShowPressTip(5, true);
            ShowPressTip(6, false);
            ShowPressTip(7, false);
            Player.Instance.ForbidMoving(true);
            Cursor.visible = false; // 隐藏鼠标
            Cursor.lockState = CursorLockMode.Locked; // 锁定鼠标在屏幕中心
        }
    }

    
    public Slider MouseSenstivitySlider;    //鼠标灵敏度滑动条
    public void OnMouseSenstivitySliderChange(){
        Player.Instance.ChangeMouseSenstive(MouseSenstivitySlider.value);
    }
    public Slider MainVolumeSlider;    //音量滑动条
    public void OnMainVolumeSliderChange(){
        SEManager.Instance.ChangeMainVolume(MainVolumeSlider.value);
    }
}