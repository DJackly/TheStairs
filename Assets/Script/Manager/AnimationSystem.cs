using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class AnimationSystem : MonoBehaviour
{
    //�����ڿ�������
    public static AnimationSystem Instance;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        //EnteringGame();
    }
    public void EnteringGame()  //������Ϸʱ���볡����
    {
        Player.Instance.GetComponent<PlayerAnimation>().StartGame = true;   //ͨ������������ӳٿ��������Ի�
    }
    public void Ending()
    {
        Player.Instance.GetComponent<Animator>().Play("Ending");
        Player.Instance.GetComponent<Animator>().enabled = true;    //��ΪĬ�϶������谭���ƽ�ɫ������ƽʱ����
    }
}
