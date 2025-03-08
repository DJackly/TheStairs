using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShutDownDoor : BeingTriggered
{
    public GameObject DownDoor;
    public GameObject PortalB1;
    public GameObject PortalB2;
    private void Start() {
        
        DownDoor.SetActive(false);
        PortalB1.SetActive(false);
        PortalB2.SetActive(false);
    }
    public override void TriggerIt()
    {
        StairManager.Instance.DeleteGiantFloor();
        DownDoor.SetActive(true);
        PortalB1.SetActive(true);
        PortalB2.SetActive(true);
    }
}
