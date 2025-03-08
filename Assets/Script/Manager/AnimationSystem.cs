using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class AnimationSystem : MonoBehaviour
{
    //挂载于控制中心
    public static AnimationSystem Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //EnteringGame();
    }
    public void EnteringGame()  //进入游戏时的入场动画
    {
        Player.Instance.GetComponent<PlayerAnimation>().StartGame = true;   //通过这个变量来延迟开启开场对话
    }
    public void Ending()
    {
        Player.Instance.GetComponent<Animator>().Play("Ending");
        Player.Instance.GetComponent<Animator>().enabled = true;    //因为默认动画会阻碍控制角色，所以平时不打开
    }
}
