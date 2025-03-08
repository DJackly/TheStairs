using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingMode : MonoBehaviour
{
    public float moveSpeed = 15f; // 移动速度
    public float verticalSpeed = 10f; // 垂直移动速度

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 

        Vector3 movement = new Vector3(horizontal, 0, vertical) * moveSpeed * Time.deltaTime;
        movement = transform.TransformDirection(movement) * moveSpeed * Time.deltaTime;
        transform.Translate(movement, Space.World);

        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0, verticalSpeed * Time.deltaTime, 0, Space.World);
        }
        else if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            transform.Translate(0, -verticalSpeed * Time.deltaTime, 0, Space.World);
        }
    }
}
