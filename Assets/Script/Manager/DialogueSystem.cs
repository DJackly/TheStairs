using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    //挂载于UI下的对话框物体
    public static DialogueSystem Instance;
    public GameObject DialogueObject;
    public List<List<string>> DialogueList = new List<List<string>>();
    public List<string> DialogueStart = new List<string>();
    public List<string> DialogueEnd = new List<string>();
    public List<string> Dialogue_MysteriousChest = new List<string>();
    public List<string> Dialogue_FlyMode = new List<string>();
    public List<string> Dialogue_GetKey = new List<string>();
    public List<string> Dialogue_GetIDCard = new List<string>();

    readonly float GAP_PER_WORD = 0.06f;    //每个字留给玩家阅读的时间
    readonly float LEAST_GAP = 1f;    //每句话最低停留时间
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        DialogueObject.SetActive(false);
        DialogueList.Add(DialogueStart);    //0
        DialogueList.Add(DialogueEnd);
        DialogueList.Add(Dialogue_MysteriousChest);
        DialogueList.Add(Dialogue_FlyMode);
        DialogueList.Add(Dialogue_GetKey);  //4
        DialogueList.Add(Dialogue_GetIDCard);  
        //第一个对话在AnimationSystem处启用播放
    }
    IEnumerator DialoguePlaying(List<string> dialogue)
    {
        DialogueObject.SetActive(true);
        for (int i=0; i<dialogue.Count; i++)
        {
            string s = dialogue[i];
            DialogueObject.GetComponent<TextMeshProUGUI>().text = s;
            yield return new WaitForSeconds(LEAST_GAP + GAP_PER_WORD * s.Length);
        }
        DialogueObject.SetActive(false);
        yield return null;
    }
    public void PlayDialogue(int index)
    {
        StartCoroutine(DialoguePlaying(DialogueList[index]));
    }
}
