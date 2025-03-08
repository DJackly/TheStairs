using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDoor : BeingTriggered
{
    public GameObject TriggerBox;
    public GameObject LinearGuy;
    public GameObject Door;
    bool flag = false;
    private void Awake()
    {
        Door.SetActive(false);
    }
    private void Start()
    {
        LinearGuy.SetActive(false);
        TriggerBox.SetActive(true);
        Door.SetActive(true);
        Door.GetComponent<Animator>().enabled = false;
    }
    private void Update()
    {
        if(Player.Instance.SeeIt(LinearGuy) && flag)
        {
            LinearGuy.GetComponent<Animator>().Play("LinearGuyMoving");
            Door.GetComponent<Animator>().Play("CloseDoor");
            SEManager.Instance.CloseDoor();
            flag = false;
        }
    }
    public override void TriggerIt()
    {
        TriggerBox.SetActive(false);
        LinearGuy.SetActive(true);
        Door.GetComponent<Animator>().enabled = true;
        Door.GetComponent<Animator>().Play("OpenDoor");
        SEManager.Instance.OpenDoor();
        flag = true;
    }

}
