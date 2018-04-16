﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float _damage = 1;
    public float _projectileSpeed = 1;

    [HideInInspector]
    public Character.TEAM _team;

    [Header("OnCharacterHit")]
    public GameObject m_hitEffect = null;
    // Use this for initialization

    private void Update()
    {
        //Move forwards
        Vector3 position = transform.position;
        position += _projectileSpeed * transform.forward;
        transform.position = position;
    }

    private void OnTriggerEnter(Collider other)
    {
        Character character = other.GetComponent<Character>();

        if(character!=null)
        {
            //Collided with character from other team
            if(_team != character._team)
            {
                character.TakeDamage(_damage);
                OnHit();
            }
        }
    }

    //Object on longer on screen
    void OnBecameInvisible()
    {
        GameManager._Instance.OnProjectileDestroyed(this);
    }

    public void OnHit()
    {
        if (m_hitEffect != null) // Create death effect
        {
            Destroy(Instantiate(m_hitEffect, transform.position, Quaternion.identity), 5.0f);
        }

        //add to bullet pool
        GameManager._Instance.OnProjectileDestroyed(this);
    }
}
