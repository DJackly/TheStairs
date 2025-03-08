using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairLight : MonoBehaviour
{
    public GameObject SpotLight;
    public GameObject PointLight;
    public int modeNo = 0;
    public float timer = 0f;
    readonly float FLASH_GAP = 1.7f;    //闪烁间隔
    public void SwitchLightMode(int no) //0为正常灯，1为频闪，2为熄灭，3为红灯？
    {
        if (no == 0) SwitchLight(true);
        else if (no == 1) modeNo = 1;
        else if (no == 2)
        {
            SwitchLight(false);
            modeNo = 2;
        }
        else
        {

        }
    }
    private void Update()
    {
        if(modeNo == 1)
        {
            timer += Time.deltaTime;
            if(timer > FLASH_GAP)
            {
                SwitchLight(false);
                if (timer > FLASH_GAP + 0.3f)
                {
                    timer = 0f;
                    SwitchLight(true);  
                }
            }
        }
    }
    private void SwitchLight(bool b)
    {
        if (b)
        {
            SpotLight.SetActive(true);
            PointLight.SetActive(true);
        }
        else
        {
            SpotLight.SetActive(false);
            PointLight.SetActive(false);
        }
    }
}
