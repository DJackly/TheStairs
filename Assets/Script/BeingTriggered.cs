using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BeingTriggered : MonoBehaviour
{
    //提供一个统一的接口受另一函数调用I
    public abstract void TriggerIt();
}
