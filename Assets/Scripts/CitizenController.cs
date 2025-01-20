using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class CitizenController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    private Animator animator;

    // public Role role;
    public List<RoutePoint> routePoints;

    public bool isEnchanting = false;
    private float enchantedTime = 0f;
    public float timeToEnchanted = 5f;

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

    private void Update()
    {
        animator.SetFloat("Speed", navMeshAgent.velocity.magnitude);

        if (isEnchanting && !ConvertedSoul)
        {
            enchantedTime += Time.deltaTime;
            if (enchantedTime >= timeToEnchanted)
            {
                Debug.Log("Enchanted soul");
                ConvertedSoul = true;
                animator.SetBool("isConverted", true);
                StopEnchanting();
            }
        }

        if (!isEnchanting && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f))
        {
            Debug.Log("new path");
            navMeshAgent.destination = routePoints[Random.Range(0, routePoints.Count)].transform.position;
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
    }
}

[Serializable]
public class RoutePoint
{
    public Transform transform;
}