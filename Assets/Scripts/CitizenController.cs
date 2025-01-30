using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utility;

public class CitizenController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    private Animator animator;
    public GameObject canvasik;
    public GameObject enchantVfx;
    public GameObject eyesVfx;

    public List<Transform> waypoints = new();

    public bool isEnchanting = false;
    private float enchantedTime = 0f;
    public float timeToEnchanted = 5f;
    int count = 0;

    public bool ConvertedSoul { get; set; } = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updatePosition = false;

        animator = GetComponent<Animator>();
        animator.applyRootMotion = true;
    }

    private void OnAnimatorMove()
    {
        Vector3 animatorRootPos = animator.rootPosition;
        animatorRootPos.y = navMeshAgent.nextPosition.y;
        navMeshAgent.nextPosition = animatorRootPos;
        transform.position = animatorRootPos;
    }

    private void FixedUpdate()
    {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

        if (isEnchanting && !ConvertedSoul)
        {
            enchantVfx.SetActive(true);
            enchantedTime += Time.deltaTime;
            if (enchantedTime >= timeToEnchanted)
            {
                Debug.Log("Enchanted soul");
                ConvertedSoul = true;
                animator.SetBool("isConverted", true);
                canvasik.SetActive(true);
                eyesVfx.SetActive(true);
                EventManager.GameProgressEvent.OnEnchant?.Invoke(this);
                StopEnchanting();
            }
        }

        if (waypoints.Count == 0)
        {
            return;
        }
        
        if (!isEnchanting && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f))
        {
            if (Vector3.Distance(transform.position, navMeshAgent.destination) <= navMeshAgent.stoppingDistance)
            {
                Debug.Log("new path");
                count++;
                if (count >= waypoints.Count)
                {
                    count = 0;
                }

                navMeshAgent.destination = waypoints[count].position;
            }
        }
    }

    public void EnchantTo(Vector3 transformPosition)
    {
        Debug.Log("EnchantTo called");
        if (isEnchanting)
        {
            return;
        }

        Debug.Log("Enchanted");
        isEnchanting = true;
        transform.rotation = Quaternion.LookRotation(transformPosition - transform.position);
        navMeshAgent.isStopped = true;
    }

    public void StopEnchanting()
    {
        navMeshAgent.isStopped = false;
        isEnchanting = false;
        enchantedTime = 0f;
        if (!ConvertedSoul)
        {
            enchantVfx.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        foreach (Transform waypoint in waypoints)
        {
            Gizmos.DrawSphere(waypoint.position, 0.5f);
        }
    }
}