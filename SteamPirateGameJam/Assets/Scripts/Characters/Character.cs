using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    
    public enum TEAM { PLAYER,ENEMY};

    [HideInInspector]
    public TEAM _team;

    public float m_health = 1;
    [SerializeField]
    protected float m_currentHealth = 1;    

    [Header("Firing Delay")]
    public float m_baseFireDelay = 1;
    private float _CurrentFiringDelay = 0f;
    protected bool _CanFire;

    [Header("OnDeath")]
    public GameObject m_deathEffect = null;

    // Use this for initialization
    protected virtual void Start ()
    {
        m_currentHealth = m_health;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (IsDead())
        {
            OnDeath();
        }

        // Update firing delay
        _CurrentFiringDelay -= Time.deltaTime;

        if(_CurrentFiringDelay <= 0f)
        {
            FireProjectile();
            _CurrentFiringDelay = m_baseFireDelay;
        }
 
    }

    private bool IsDead()
    {
        return m_currentHealth <= 0;
    }

    public virtual void OnDeath()
    {
        if(m_deathEffect!=null) // Create death effect
        {
            Destroy(Instantiate(m_deathEffect, transform.position, Quaternion.identity), 5.0f);
        }
    }

    public void TakeDamage(float damage)
    {
        m_currentHealth -= damage;
    }

    public virtual void FireProjectile()
    {

    }

}