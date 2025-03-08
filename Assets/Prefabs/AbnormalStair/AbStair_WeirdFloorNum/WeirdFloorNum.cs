using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeirdFloorNum : MonoBehaviour
{
    List<string> list = new List<string> {"-1F","-18F","99F","?","0","1F" };
    public int i = 0;
    private void Awake()
    {
        i = Random.Range(0, list.Count);
    }
    private void Start()
    {
        GetComponent<TextMeshPro>().text = list[i];
    }
}
