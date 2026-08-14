using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using Unity.Cinemachine;
using TMPro;
public class NPCEscapeController : MonoBehaviour
{
    public static NPCEscapeController instance;

    public Transform police;

    [Header("Distance check")]
    public float escape_Distance=15f;
    public float detection_Radius;
    public float safe_Distance = 20f;
    public float random_Escape_Angle = 45f;
    public float escape_Repath_Time = 2.5f;
    public float run_Speed = 5f;

    [Header("NPC moving speed")]
    public float walk_Speed = 2f;
    float nextEscapeTime = 0f;

    [Header("Arrow")]
    public GameObject arrow_Pointer;
    public float arrow_Rotate_Speed = 3f;

    public bool isEscaping=false;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Rigidbody rb;

    //Animatior names 
    private readonly string run_Param = "isRunning";
    private readonly string hit_Param = "isHit";

    [Header("Health System")]
    HealthSystem health_System = new HealthSystem(100);
    public HealthBar healthBar;

    [Header("Explosion")]
    public GameObject explosion_Prefab;
    private CinemachineImpulseSource impulse_Source;

    [Header("Audio")]
    public AudioSource car_Hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        if (police == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                police = player.transform;
            }
        }

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        animator.SetBool(run_Param, false);

        healthBar.Setup(health_System);

        //Getting access to the CinemachineImpulseSource 
        impulse_Source = GetComponent<CinemachineImpulseSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Rotating the arrow pointer on above of enemy
        arrow_Pointer.transform.Rotate(2 * arrow_Rotate_Speed, 0, 0);

        if (police == null || navMeshAgent == null)
            return;
        float distance = Vector3.Distance(transform.position, police.position);
        //Debug.Log("Distance: " + distance);

        

        if (distance < detection_Radius&&!isEscaping)
        {
            isEscaping = true;
            nextEscapeTime = Time.time;
            SetEscapeDestination();
            animator.SetBool(run_Param, true);
           
        }
        if(distance > safe_Distance && isEscaping)
        {
            isEscaping=false;
            navMeshAgent.speed = walk_Speed;
            navMeshAgent.ResetPath();
            animator.SetBool(run_Param,false);
        }
        if (isEscaping && Time.time >= nextEscapeTime)
        {
            nextEscapeTime = Time.time + escape_Repath_Time;
            SetEscapeDestination();
        }

        //Damaging
        Debug.Log("Health" + health_System.GetHealthPercentage());

    }
    void SetEscapeDestination()
    {
        Vector3 escape_Direction = (transform.position - police.position).normalized;
        float randomAngle = Random.Range(-random_Escape_Angle, random_Escape_Angle);
        escape_Direction = Quaternion.Euler(0, randomAngle, 0) * escape_Direction;
        escape_Direction = escape_Direction.normalized;

        Vector3 escape_Target = transform.position + escape_Direction * escape_Distance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(escape_Target, out hit, 5f, NavMesh.AllAreas))
        {
            navMeshAgent.SetDestination(hit.position);
            navMeshAgent.speed = run_Speed;
        }

        Debug.Log("Police Car detected");
    }

    private void OnHitPoliceCar()
    {
        if (navMeshAgent.isStopped == true) return;
        animator.SetTrigger(hit_Param);
        car_Hit.Play();
        StartCoroutine(StopMovement());
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            OnHitPoliceCar();

            health_System.Damage(20);
            if (health_System.GetHealth() <= 0)
            {
                GameManager.instance.AddKill();

                Destroy(gameObject);
            }
        }
    }
    private IEnumerator StopMovement()
    {
        // Stop NavMesh completely
        navMeshAgent.isStopped = true;
        navMeshAgent.velocity = Vector3.zero;

        //Waiting until the standing up animation completing time
        yield return new WaitForSeconds(8.67f);

        //Re-enabling the navmesh Agent 
        navMeshAgent.isStopped=false;

        if (isEscaping)
        {
            animator.SetBool(run_Param, true);
            SetEscapeDestination();
        }


    }
   
    private void Explosion()
    {
        
        
        if(impulse_Source!=null)
        {
            impulse_Source.GenerateImpulse();
        }

        if(explosion_Prefab != null)
        {
            GameObject explsion_Instance=Instantiate(explosion_Prefab,transform.position, Quaternion.identity);
            Destroy(explsion_Instance,5);
        }

        Debug.Log("Explosion Happened");
    }

   

    public void TriggerExplosionForce()
    {
        
        Explosion(); 

        // Destroy the NPC visuals/object after explosion
        Destroy(gameObject, 0.5f);
    }

   
}
