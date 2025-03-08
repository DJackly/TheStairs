using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbStairCollection : MonoBehaviour
{
    public TextMeshProUGUI DescriText;
    public GameObject AbstairImg;
    public string desci;    //该异常的名称
    private void Start() {
        DescriText.text = "???";
        AbstairImg.SetActive(false);
    }
    public void EnableAbCollection(){
        DescriText.text = desci;
        AbstairImg.SetActive(true);
    }
}
