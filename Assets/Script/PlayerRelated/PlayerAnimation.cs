using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerAnimation : MonoBehaviour
{
    public GameObject BrightBackground;
    public bool StartGame = false;  //�����ʼ��Ϸ����true����AnimationSystem�޸�
    private void Update()
    {
        //if (Input.GetMouseButtonDown(1))StartEnding();
            
    }
    public void EndTitlePLayerShaking(){    //ҡ�ζ������������
        if(StartGame){  //˵���ѹرձ���
            DialogueSystem.Instance.PlayDialogue(0);    //�ȴ�ҡ�ζ����������ٿ�ʼ�Ի��Ϳ�������
            Player.Instance.GetComponent<Animator>().SetBool("EnterGame",true); //�ɱ������ƽ��뿪ʼ����
        }
    }
    public void StartEnteringGame()//�ɶ����¼�����
    {
        Player.Instance.ForbidMoving(false);
    }
    public void EndEnteringGame()   
    {
        UIManager.Instance.SetTitleState(false);
        TimeSystem.Instance.StartTimer();   //������ʱ
        UIManager.Instance.GraduallyShowBoard(0,true);  //������ʾ������ʾ
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
            newColor.a = i; // �����µ�alphaֵ
            spriteRenderer.color = newColor; // Ӧ���µ�͸����
            i += 0.02f;
            yield return new WaitForSeconds(0.04f);
        }
        yield return null;
    }
}
