using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchImg : MonoBehaviour
{
    public List<Sprite> ImgList;    //0为默认的图
    private int imgIndex = 0;   //当前图索引
    public void Switch()
    {
        if (GetComponent<Image>())
        {
            if (++imgIndex >= ImgList.Count) imgIndex = 0;
            GetComponent<Image>().sprite = ImgList[imgIndex];
        }
        else Debug.LogWarning("SpriteRenderer还没做");
    }
}
