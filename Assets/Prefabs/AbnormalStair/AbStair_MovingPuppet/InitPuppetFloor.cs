using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class InitPuppetFloor : MonoBehaviour
{
    // 获取或创建NavMeshSurface组件
    public NavMeshSurface navMeshSurface;
    public GameObject puppet;

    private void Awake()
    {
        puppet.SetActive(false) ;
        navMeshSurface.BuildNavMesh();
    }
    private void Start()
    {
        puppet.SetActive(true);
    }

}
