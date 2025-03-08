using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheKey : Interactable
{
    GameObject KeyObject;
    bool keyExist = true;
    private void Start() {
        KeyObject = transform.GetChild(0).gameObject;
        InteractType = "Check flowerpot";
    }
    public override void Interact()
    {
        Player.Instance.InspectThis(gameObject);
        DialogueSystem.Instance.PlayDialogue(4);
        GetComponent<BoxCollider>().enabled = false;
        Invoke("PlayerGetKey",6f);
    }
    public void PlayerGetKey(){
        Player.Instance.haveMysteriousChestKey = true;
    }
    private void Update() {
        if(keyExist){
            if(Player.Instance.haveMysteriousChestKey){
                gameObject.tag = "Untagged";
                KeyObject.SetActive(false);
                keyExist = false;
            }
        }
    }
}
