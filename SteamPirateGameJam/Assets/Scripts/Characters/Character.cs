﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script written by Deeon

public class Character : MonoBehaviour
{
    public enum TEAM { PLAYER,ENEMY};

    [HideInInspector]
    public TEAM _team;

    [Header("Health")]
    public float _health = 1;
    [SerializeField]
    protected float _currentHealth = 1;
    public GameObject _deathEffect = null;

    [Header("Firing")]
    public GameObject _FiringEffect = null;
    public Transform _MuzzleLaunchPoint;
    public float _baseFireDelay = 1;
    public float _currentFireDelay = 1;
    private float _FiringTimer = 0f;

    // Use this for initialization
    protected virtual void Start ()
    {
        _currentHealth = _health;
        _currentFireDelay = _baseFireDelay;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (IsDead())
        {
            OnDeath();
        }

        // Update firing delay
        _FiringTimer -= Time.deltaTime;

        if(_FiringTimer <= 0f)
        {
            FireProjectile();
            _FiringTimer = _currentFireDelay;
        }
 
    }

    private bool IsDead()
    {
        return _currentHealth <= 0;
    }

    public virtual void OnDeath()
    {
        if(_deathEffect != null) // Create death effect
        {
            Destroy(Instantiate(_deathEffect, Vector3.zero, Quaternion.identity, transform), 5.0f);
        }
        transform.position = GameManager._Instance.gameObject.transform.position;
        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
    }

    public virtual void FireProjectile()
    {
        // Sam wanted it this way HURR DURR I AM SAM NO SPACES PLEASE EXCEPT HERE ROKAY
    }

    /// -------------------------------------------
    /// 
    ///     Default Firing
    ///     Using default sound no offset in rotation
    ///     
    /// -------------------------------------------
    protected void SpawnBullet(Vector3 position, Quaternion facingDir)
    {
        SpawnBullet(position, facingDir, 0);
    }

    /// -------------------------------------------
    /// 
    ///     Default Firing
    ///     Using default sound, offset in rotation
    ///     
    /// -------------------------------------------
    protected void SpawnBullet(Vector3 position, Quaternion facingDir, float rotationOffset)
    {
        Projectile projectile = GameManager._Instance.GetProjectile(_team);
        if (projectile != null)
        {
            projectile.transform.position = position;
            projectile.transform.rotation = facingDir * Quaternion.Euler(0, rotationOffset, 0);
            projectile.transform.rotation = projectile.transform.rotation * Quaternion.Euler(0, 180, 0);
            // Move to active pool
            GameManager._Instance.OnProjectileFired(projectile);

            SoundManager._Instance.PlayFireProjectileDefault(0.9f, 1.1f);
        }
    }

    /// -------------------------------------------
    /// 
    ///     Defualt Firing
    ///     Using character passed in determine what sound to play
    ///     
    /// -------------------------------------------
    protected void SpawnBullet(Vector3 position, Quaternion facingDir, float rotationOffset, Character character)
    {
        Projectile projectile = GameManager._Instance.GetProjectile(_team);
        if (projectile != null)
        {
            projectile.transform.position = position;
            projectile.transform.rotation = facingDir * Quaternion.Euler(0, rotationOffset, 0);

            // Move to active pool
            GameManager._Instance.OnProjectileFired(projectile);

            //Determine sound to play
            if (character._team == TEAM.ENEMY)
                SoundManager._Instance.PlayFireProjectileDefault(0.9f, 1.1f);
            else
            {
                if (character.GetComponent<Player>()._HasPickupRapidFire)
                    SoundManager._Instance.PlayPickupRapidFire(0.9f, 1.1f);
                else
                    SoundManager._Instance.PlayFireProjectileDefault(0.9f, 1.1f);

                if (_deathEffect != null) // Create death effect
                {
                    Destroy(Instantiate(_FiringEffect, _MuzzleLaunchPoint.position, transform.rotation), 5.0f);
                }
            }
        }
    }
}