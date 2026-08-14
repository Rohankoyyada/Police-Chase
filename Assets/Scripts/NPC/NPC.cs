using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public Transform[] waypoints;
    public float waitTime = 0f;

    private bool movingForward = true;
    [HideInInspector]
    public int currentwaypoint = 0;
    private float waitTimer = 0f;
    private NavMeshAgent agent;
    private Animator animator;

    public AudioSource car_HitSound;

    //Animatior names 
    private readonly string walking_Param = "isWalking";
    private readonly string hit_Param = "isHit";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        if(waypoints.Length>0)
        {
            agent.SetDestination(waypoints[currentwaypoint].position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(!agent.pathPending && agent.remainingDistance<0.5)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer>waitTime)
            {
                waitTimer = 0;
               
                GoToNextWaypoint();
            }
        }
       
        animator.SetBool(walking_Param,true);
       

    }

    void GoToNextWaypoint()
    {
        if(movingForward)
        {
            currentwaypoint++;
            if(currentwaypoint >= waypoints.Length)
            {
                currentwaypoint = waypoints.Length - 2;
                agent.speed = 2f;
                movingForward = false;
            }
        }
        else
        {
            currentwaypoint--;
            if(currentwaypoint<0)
            {
                currentwaypoint = 1;
                agent.speed = 2f;
                movingForward =true;
            }
        }
        

        agent.SetDestination(waypoints[currentwaypoint].position);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnHitPoliceCar();
        }
    }

    private void OnHitPoliceCar()
    {
        animator.SetBool(walking_Param,false);
        animator.SetTrigger(hit_Param);
        car_HitSound.Play();
        agent.speed = 0f;
    }
}
