using UnityEngine;
using UnityEngine.AI;
 
public class WanderingAI : MonoBehaviour 
{
 
    [SerializeField] float wanderRadius;
    [SerializeField] float wanderTimer;
    private NavMeshAgent agent;
    private float timer;
    
    Animator animator;
    // Use this for initialization
    void OnEnable ()
    {
        if (wanderTimer < 3.1f)
            wanderTimer = 3.1f;
        
        wanderTimer = Random.Range(3, wanderTimer);
        agent = GetComponent<NavMeshAgent> ();
        timer = wanderTimer;

        animator = GetComponent<Animator>();
        
        
    }
 
    // Update is called once per frame
    void Update () 
    {
        timer += Time.deltaTime;
 
        if (timer >= wanderTimer) 
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            animator.SetTrigger("Run");
            timer = 0;
        }
        
        if (agent.remainingDistance != Mathf.Infinity && agent.pathStatus == NavMeshPathStatus.PathComplete &&
            agent.remainingDistance == 0)
        {
            animator.SetTrigger("Shoot");
        }
    }
 
    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) 
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;
        randDirection += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition (randDirection, out navHit, dist, layermask);
        return navHit.position;
    }
    
}