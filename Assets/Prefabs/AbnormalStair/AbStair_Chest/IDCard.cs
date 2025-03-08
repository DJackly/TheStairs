using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IDCard : Interactable
{
    private void Start() {
        InteractType = "Pick up";
    }
    public override void Interact()
    {
        Player.Instance.InspectThis(gameObject);
        Player.Instance.EnablecheatMode();
        DialogueSystem.Instance.PlayDialogue(5);
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }
}
