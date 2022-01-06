using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Base class for enemies.
/// </summary>
public class Enemy : MonoBehaviour
{
    //Public
    public float health;
    public GameObject healthBarPrefab;
    public float value;

    //Private
    private float currentHealth;
    private GameObject healthBar;
    private Transform destination;
    private NavMeshAgent agent;


    private void Awake()
    {
        currentHealth = health;
        healthBar = Instantiate(healthBarPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.Euler(90f, 0, 0), transform);
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        destination = GameObject.FindGameObjectWithTag(Tags.DESTINATION).transform;
        agent.SetDestination(destination.position);
    }

    /// <summary>
    /// Handles damage scenarios. 
    /// </summary>
    /// <param name="damage"></param>
    public void DamageHandler(float damage)
    {
        ReduceHealth(damage);
        ReduceHealthBar();

        if (currentHealth <= 0)
        {
            Death();
        }
    }

    /// <summary>
    /// Reduces the enemies health.
    /// </summary>
    /// <param name="damage"></param>
    private void ReduceHealth(float damage)
    {
        currentHealth -= damage;
    }

    /// <summary>
    /// Reduces the visuals of the health bar. 
    /// </summary>
    private void ReduceHealthBar()
    {
        Transform pivot = healthBar.transform.Find(Tags.HEALTHY_PIVOT);
        Vector3 scale = pivot.localScale;
        scale.x = Mathf.Clamp(currentHealth / health, 0, 1);
        pivot.localScale = scale;
    }
    
    /// <summary>
    /// Applies "money" to players wallet and destroys the enemy object.
    /// </summary>
    private void Death()
    {
        Wallet.Amount += value;
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        CheckDestinationReached(other);
    }

    /// <summary>
    /// Checks destination has been reached.
    /// </summary>
    /// <param name="other"></param>
    private void CheckDestinationReached(Collider other)
    {
        if (other.tag == Tags.DESTINATION) Destroy(gameObject);        
    }
}