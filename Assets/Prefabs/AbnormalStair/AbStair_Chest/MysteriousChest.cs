using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteriousChest : Interactable
{
    public Vector3 OpenRotation, CloseRotation;
    public GameObject ChestDoor;
    public GameObject theIDCard;
	public float rotSpeed = 1f;
	public bool doorBool;   //箱子�?
    private void Awake() {
        InteractType = "Open";
    }
    private void Start() {
        theIDCard.SetActive(false);
    }
    public override void Interact()
    {
        if(Player.Instance.haveMysteriousChestKey){
            if (!doorBool)
				doorBool = true;
			else
				doorBool = false;
            GetComponent<Collider>().enabled = false;
            theIDCard.SetActive(true);
        }
        else{
            DialogueSystem.Instance.PlayDialogue(2);
        }
    }
    void Update()
	{
		if (doorBool)
			ChestDoor.transform.rotation = Quaternion.Lerp (ChestDoor.transform.rotation, Quaternion.Euler (OpenRotation), rotSpeed * Time.deltaTime);
		else
			ChestDoor.transform.rotation = Quaternion.Lerp (ChestDoor.transform.rotation, Quaternion.Euler (CloseRotation), rotSpeed * Time.deltaTime);	
	}
}
