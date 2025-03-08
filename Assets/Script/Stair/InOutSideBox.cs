using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InOutSideBox : MonoBehaviour
{
    public bool isInside;   //ÊÇÄÚ²àµÄ¼ì²âÏäÂð£¿
    private float timer = 0f;
    private bool ready = true;

    private void Update()
    {
        if (!ready)
        {
            timer += Time.deltaTime;
            if(timer > 0.3f)
            {
                ready = true;
                timer = 0f;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && ready)
        {
            transform.parent.GetComponent<LeavingBox>().ReceiveDetecting(isInside);
            ready = false;
        }
    }
}
