using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizonCity : MonoBehaviour
{
    public static HorizonCity Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void UpdatePos(float y)
    {
        float x = transform.position.x;
        float z = transform.position.z;
        transform.position = new Vector3(x, y-50, y);
    }
}
