using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityStandardAssets.Characters.FirstPerson;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public GameObject FlashLight;
    public GameObject InspectLocation;
    public GameObject CopyObject;
    public GameObject PlayerCamera;
    public GameObject DirectedLight;    //全局光照
    public GameObject UICamera;

    public bool haveMysteriousChestKey = false;
    public bool cheatModeEnable = false;    //允许作弊
    public bool cheatMode = false;  //作弊开启
    public float scaleSpeed = 1.1f;   // 缩放速度
    private float scale; // 原始缩放值
    public bool lightState = false;
    bool tipLock = false;
    Ray ray;
    RaycastHit hit;
    readonly float MAX_INTERACT_DIS = 4.0f;
    private void Awake()
    {
        if(Instance  != null && Instance != this)
        {
            Destroy(Instance);
            return;
        }
        Instance = this;
    }
    void Update()
    {
        //手电
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (lightState)FlashLight.SetActive(false);
            else FlashLight.SetActive(true);
            lightState = !lightState;
            UIManager.Instance.SwitchUITip(5);
            SEManager.Instance.SwitchFlash();
            MyEventSystem.Instance.TriggerPlayerSwitchFlashLight();
        }
        FlashLight.transform.rotation = PlayerCamera.transform.rotation;

        //检测射线 & 点击E查看
        ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Inspectable") && Vector3.Distance(transform.position, hit.point) < MAX_INTERACT_DIS)
            {       //落在可交互物体上 && 距离够近才可查看
                if (!tipLock)UIManager.Instance.ShowPressETip(true, "Check");
                tipLock = true;
                if (Input.GetKeyDown(KeyCode.E) && CopyObject == null) //按E查看
                {
                    InspectThis(hit.transform.gameObject);
                }
            }
            else if(hit.collider.CompareTag("Interactable") && Vector3.Distance(transform.position, hit.point) < MAX_INTERACT_DIS)
            {
                if (!tipLock) UIManager.Instance.ShowPressETip(true, hit.transform.GetComponent<Interactable>().InteractType);
                tipLock = true;
                if (Input.GetKeyDown(KeyCode.E)) //按E互动
                {
                    hit.transform.GetComponent<Interactable>().Interact();
                }
            }
            else    //移开视线
            {
                tipLock = false; 
                UIManager.Instance.ShowPressETip(false);
                UIManager.Instance.ShowPressETip(false);
            }
        }

        //按ESC退出检查模式
        if (Input.GetKeyDown(KeyCode.Escape) && CopyObject != null)
        {
            Destroy(CopyObject);
            ForbidMoving(true);
        }

        //缩放功能
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetAxis("Mouse ScrollWheel") != 0 && CopyObject != null ) //滚轮缩放
        {
            if(scale > 0.05f && scrollDelta < 0)
            {
                scale += Input.GetAxis("Mouse ScrollWheel") * scaleSpeed; 
                CopyObject.transform.localScale = new Vector3(1 * scale, 1 * scale, 1 * scale);
            }
            else if(scale < 1.3f && scrollDelta  > 0)
            {
                scale += Input.GetAxis("Mouse ScrollWheel") * scaleSpeed; 
                CopyObject.transform.localScale = new Vector3(1 * scale, 1 * scale, 1 * scale);
            }
        }

        //按键反馈
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftShift)) UIManager.Instance.SwitchUITip(4);

        //查看模式下WASD移动物体
        if(CopyObject != null )
        {
            float movingSpeed = 0.02f;
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                if (Input.GetKey(KeyCode.W)) CopyObject.transform.Translate(0, -1 * movingSpeed, 0);
                else if (Input.GetKey(KeyCode.S)) CopyObject.transform.Translate(0,movingSpeed, 0);
            }
            if(Input.GetKey(KeyCode.D)|| Input.GetKey(KeyCode.A))
            {
                if (Input.GetKey(KeyCode.D)) CopyObject.transform.Translate(-1 * movingSpeed, 0,  0);
                else CopyObject.transform.Translate(movingSpeed, 0, 0);
            }
        }
    
        //作弊模式
        if(cheatModeEnable){    //已获得作弊权限
            if(Input.GetKeyDown(KeyCode.C)){
                if(cheatMode){  //此时关闭作弊模式
                    cheatMode = false;
                    GetComponent<FlyingMode>().enabled = false;
                    GetComponent<CharacterController>().enabled = true;
                    UIManager.Instance.SwitchUITip(1);
                }
                else{   //此时打开作弊模式
                    cheatMode = true;
                    GetComponent<FlyingMode>().enabled = true;
                    GetComponent<CharacterController>().enabled = false;
                    UIManager.Instance.SwitchUITip(1);
                    DialogueSystem.Instance.PlayDialogue(3);
                }
            }
            if(Input.GetKeyDown(KeyCode.L)){
                DirectedLight.SetActive(!DirectedLight.activeSelf);
                UIManager.Instance.SwitchUITip(2);
            }
            if(Input.GetKeyDown(KeyCode.V)){
                UIManager.Instance.SwitchUITip(3);
                VHSEffect vhs1 = PlayerCamera.GetComponent<VHSEffect>();
                VHSEffect vhs2 = UICamera.GetComponent<VHSEffect>();
                vhs1.enabled = !vhs1.enabled;
                vhs2.enabled = !vhs2.enabled;
            }
        }
        //开启作弊
        if(Input.GetKey(KeyCode.C)){
            if(Input.GetKeyDown(KeyCode.H)){
                EnablecheatMode();
            }
        }
    }
    public void ForbidMoving(bool b)
    {
        GetComponent<CharacterController>().enabled = b;
        GetComponent<FPSController>().enabled = b;
    }
    public void InspectThis(GameObject go)
    {
        SEManager.Instance.PaperFlip();
        UIManager.Instance.InspectingMode(true);
        CopyObject = Instantiate(go);
        CopyObject.layer = LayerMask.NameToLayer("FrontLayer");
        foreach(Transform child in CopyObject.transform){
            child.gameObject.layer = LayerMask.NameToLayer("FrontLayer");
        }
        CopyObject.transform.position = InspectLocation.transform.position;
        CopyObject.transform.rotation = Quaternion.Euler(new Vector3(CopyObject.transform.rotation.x, 0, CopyObject.transform.rotation.z));
        scale = CopyObject.transform.localScale.x;
        ForbidMoving(false);

        CopyObject.transform.localScale = new Vector3(1 * scale, 1 * scale, 1 * scale);
    }
    public void Blink()
    {
        GameObject.Find("Canvas/Blink").GetComponent<Animator>().Play("Blink");
    }
    public bool SeeIt(GameObject targetObj)
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(PlayerCamera.transform.position, (targetObj.transform.position - PlayerCamera.transform.position).normalized, out hit, Mathf.Infinity);
        bool isInView = false;
        if (isHit && hit.collider.gameObject == targetObj)
        {
            // 计算摄像机前方方向和目标物体方向之间的夹角
            Vector3 toTarget = (targetObj.transform.position - PlayerCamera.transform.position).normalized;
            float angle = Vector3.Angle(PlayerCamera.transform.forward, toTarget);
            // 检查角度是否在摄像机的视野范围内
            isInView = angle <= (Camera.main.fieldOfView * 0.8);
        }
        isInView = isInView && isHit && hit.collider.gameObject == targetObj;
        return isInView;
    }
    public void SwitchClearFlag(int i){
        if(i==0){
            PlayerCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Skybox;
        }
        else PlayerCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.SolidColor;
        MyEventSystem.Instance.TriggerPlayerSwitchClearFlag();
    }
    public void EnablecheatMode(){
        cheatModeEnable = true;
        UIManager.Instance.ShowCheatModeButton(true);
    }
    public void ChangeMouseSenstive(float value){
        GetComponent<FPSController>().mouseSensitivity = value;
    }
}
