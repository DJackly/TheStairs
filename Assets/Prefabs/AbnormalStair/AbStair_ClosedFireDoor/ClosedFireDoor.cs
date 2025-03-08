using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClosedFireDoor : BeingTriggered
{
    public GameObject OpenedDoor;
    public GameObject ClosedDoor;
    public GameObject Window;
    public GameObject WallAboveWindow;
    public GameObject OldSafeDoor;  //好像是因为合页组件的原因，有绑定动画的门，即使动画组件取消了，生成时还是会错位
    public GameObject NewSafeDoor;
    public GameObject OutsidePortal;
    public GameObject InsidePortal;
    public GameObject ColliderWall; //在传送门出现后，关闭该墙的collider以免阻碍进入传送门

    private void Awake()
    {
        OpenedDoor.SetActive(true);
        ClosedDoor.SetActive(false);
        Window.SetActive(true);
        WallAboveWindow.SetActive(false);
        NewSafeDoor.SetActive(false);
        ColliderWall.GetComponent<MeshCollider>().enabled = true;
        OldSafeDoor.SetActive(false);
    }
    void Start(){
        OutsidePortal.SetActive(false);
        InsidePortal.SetActive(false);
        OldSafeDoor.SetActive(true);
        OldSafeDoor.GetComponent<Animator>().enabled = false;
    }
    public override void TriggerIt()
    {
        OpenedDoor.SetActive(false);
        ClosedDoor.SetActive(true);
    }
    public void SetPortalActive()
    {
        OutsidePortal.SetActive(true);
        InsidePortal.SetActive(true);
        Window.SetActive(false);
        WallAboveWindow.SetActive(true);
        NewSafeDoor.SetActive(true);
        ColliderWall.GetComponent<MeshCollider>().enabled = false;
    }
}
