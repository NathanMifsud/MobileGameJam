using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private float m_health = 1;
    private float m_currentHealth = 1;

    [SerializeField]
    private GameObject m_deathEffect = null;

    [SerializeField]
    private float m_baseFireRate = 1;

    private GameManager m_gameManager = null;

    // Use this for initialization
    void Start ()
    {
        m_currentHealth = m_health;
        //TODO grab gamemanger instance
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(IsDead())
        {
            OnDeath();
        }
	}

    private bool IsDead()
    {
        return m_currentHealth < 0;
    }

    private void OnDeath()
    {
        if(m_deathEffect!=null) // Create death effect
        {
            Destroy(Instantiate(m_deathEffect, transform.position, Quaternion.identity), 5.0f);
        }

        //TODO add to objectpool

    }
}
