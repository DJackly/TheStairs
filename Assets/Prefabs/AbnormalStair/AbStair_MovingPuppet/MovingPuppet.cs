using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MovingPuppet : MonoBehaviour
{
    Vector3 playerPos;
    float speed = 2.5f;
    public bool canMove = false;
    public bool activated = false;
    public GameObject HeadLocation;
    private NavMeshAgent navMeshAgent;

    bool attack = false;
    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        navMeshAgent.enabled = true;    //在生成NavMesh后再打开！！
    }
    private void Update()
    {
        if (Player.Instance!=null && Player.Instance.SeeIt(gameObject))
        {
            activated = true;
            canMove = false;
            navMeshAgent.isStopped = true;
        }
        else canMove = true;
        
        if(activated && canMove)
        {
            playerPos = Player.Instance.transform.position;
            Vector3 newPlayerPos = playerPos;
            newPlayerPos.y = transform.position.y;
            transform.LookAt(newPlayerPos); //看向的视角不要抬头低头

            navMeshAgent.isStopped = false;
            navMeshAgent.destination = newPlayerPos;
        }
        else navMeshAgent.isStopped = true;
    }
    public void PuppetAttack()
    {
        Death.Instance.Die(HeadLocation);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!attack)
            {
                PuppetAttack();
                navMeshAgent.enabled = false;
                attack = true;
            }
        }
    }
}
