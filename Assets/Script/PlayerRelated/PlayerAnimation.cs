using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerAnimation : MonoBehaviour
{
    public GameObject BrightBackground;
    public bool StartGame = false;  //点击开始游戏后变成true，由AnimationSystem修改
    private void Update()
    {
        //if (Input.GetMouseButtonDown(1))StartEnding();
            
    }
    public void EndTitlePLayerShaking(){    //摇晃动画播放完调用
        if(StartGame){  //说明已关闭标题
            DialogueSystem.Instance.PlayDialogue(0);    //等待摇晃动画播放完再开始对话和开场动画
            Player.Instance.GetComponent<Animator>().SetBool("EnterGame",true); //由变量控制进入开始动画
        }
    }
    public void StartEnteringGame()//由动画事件调用
    {
        Player.Instance.ForbidMoving(false);
    }
    public void EndEnteringGame()   
    {
        UIManager.Instance.SetTitleState(false);
        TimeSystem.Instance.StartTimer();   //开启计时
        UIManager.Instance.GraduallyShowBoard(0,true);  //缓慢显示按键提示
        UIManager.Instance.GraduallyShowBoard(1,true);
        Player.Instance.ForbidMoving(true);
        GetComponent<Animator>().enabled = false ;
    }
    public void StartEnding()
    {
        DialogueSystem.Instance.PlayDialogue(1);
        GetComponent<FPSController>().enabled = false;
    }
    public void EndEnding()  
    {
        StartCoroutine(Ending());
        UIManager.Instance.ShowEndingBoard();
    }
    IEnumerator Ending()
    {
        BrightBackground.SetActive(true);
        SpriteRenderer spriteRenderer = BrightBackground.GetComponent<SpriteRenderer>();
        for(float i = 0f; i <= 1f; )
        {
            Color newColor = spriteRenderer.color;
            newColor.a = i; // 设置新的alpha值
            spriteRenderer.color = newColor; // 应用新的透明度
            i += 0.02f;
            yield return new WaitForSeconds(0.04f);
        }
        yield return null;
    }
}
