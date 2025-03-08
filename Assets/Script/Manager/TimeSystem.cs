using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem Instance;
    public int minCount = 0;
    int minute = 30;
    int hour = 23;
    float timer = 0f;
    string h;   string m;
    readonly int RATE = 10; //现实中几秒等于这里的一分钟
    bool start = false;

    private void Awake() {
        Instance = this;
    }
    private void Update()
    {
        if(start){
            timer += Time.deltaTime;
            if (timer > RATE)
            {
                timer = 0f;
                minute++;
                minCount++;
                if(minute >= 60)
                {
                    minute = 0;
                    hour++;
                    if(hour >= 24)
                    {
                        hour = 0;
                    }
                }
                UpdateTime();
            }
        }
    }
    public void StartTimer(){
        start = true;
    }
    private void UpdateTime()
    {
        m = ""; h = "";
        if(minute < 10) m += "0";
        m += minute.ToString();
        if (hour < 10) h += "0";
        h += hour.ToString();
        GetComponent<TextMeshProUGUI>().text = h + " : " + m;
    }
    public string GetTime(){
        return h + ":" + m;
    }
}
