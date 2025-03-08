using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class MonaLisa : MonoBehaviour
{
    public GameObject Light;
    public SpriteRenderer Drawing;
    public Sprite NormalDrawing;
    public Sprite WeirdDrawing;
    bool lightOn = true;
    float timer = 0f;
    float gap = 2f;
    private void Start() {
        Light.SetActive(lightOn);
        Drawing.sprite = NormalDrawing;
    }
    private void Update() {
        timer += Time.deltaTime;
        if(timer>gap){
            lightOn = !lightOn;
            Light.SetActive(lightOn);
            Drawing.sprite =  lightOn ? NormalDrawing : WeirdDrawing;
            timer = 0f;
        }
    }
}
