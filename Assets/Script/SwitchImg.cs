using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchImg : MonoBehaviour
{
    public List<Sprite> ImgList;    //0ΪĬ�ϵ�ͼ
    private int imgIndex = 0;   //��ǰͼ����
    public void Switch()
    {
        if (GetComponent<Image>())
        {
            if (++imgIndex >= ImgList.Count) imgIndex = 0;
            GetComponent<Image>().sprite = ImgList[imgIndex];
        }
        else Debug.LogWarning("SpriteRenderer��û��");
    }
}
