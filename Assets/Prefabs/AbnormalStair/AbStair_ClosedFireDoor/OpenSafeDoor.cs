using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSafeDoor : Interactable
{
    public ClosedFireDoor closedFireDoorComponent;
    private void Awake() {
        
        InteractType = "Open";
    }
    public override void Interact()
    {
        GetComponent<Animator>().enabled = true;
        GetComponent<Animator>().Play("OpenSafeDoor");
        SEManager.Instance.OpenDoor();
        closedFireDoorComponent.SetPortalActive();
    }
}
