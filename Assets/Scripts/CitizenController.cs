using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CitizenController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;

    private Animator animator;

    // public Role role;
    public List<RoutePoint> routePoints;

    public bool isEnchanted = false;

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

        if (!isEnchanted && (!navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude == 0f))
        {
            Debug.Log("new path");
            navMeshAgent.destination = routePoints[Random.Range(0, routePoints.Count)].transform.position;
        }
    }

    public void EnchantTo(Vector3 transformPosition)
    {
        Debug.Log("EnchantTo called");
        if (isEnchanted)
        {
            return;
        }
        Debug.Log("Enchanted");
        isEnchanted = true;
        navMeshAgent.SetDestination(transformPosition);
    }

    public void StopEnchanting()
    {
        isEnchanted = false;
    }
}

[Serializable]
public class RoutePoint
{
    public Transform transform;
}