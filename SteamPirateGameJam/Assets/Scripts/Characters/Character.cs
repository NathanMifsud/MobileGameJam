using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    
    public enum TEAM { PLAYER,ENEMY};

    [HideInInspector]
    public TEAM _team;

    public float _health = 1;
    [SerializeField]
    protected float _currentHealth = 1;    

    [Header("Firing Delay")]
    public float _baseFireDelay = 1;
    public float _currentFireDelay = 1;
    private float _CurrentFiringDelay = 0f;

    [Header("OnDeath")]
    public GameObject _deathEffect = null;

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
        _CurrentFiringDelay -= Time.deltaTime;

        if(_CurrentFiringDelay <= 0f)
        {
            FireProjectile();
            _CurrentFiringDelay = _currentFireDelay;
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
            Destroy(Instantiate(_deathEffect, transform.position, Quaternion.identity), 5.0f);
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
    }

    public virtual void FireProjectile()
    {

    }
    protected void SpawnBullet(Vector3 position, Quaternion facingDir)
    {
        SpawnBullet(position, facingDir, 0);
    }

    protected void SpawnBullet(Vector3 position, Quaternion facingDir, float rotationOffset)
    {
        Projectile projectile = GameManager._Instance.GetProjectile(_team);
        if (projectile != null)
        {
            projectile.transform.position = position;
            projectile.transform.rotation = facingDir * Quaternion.Euler(0, rotationOffset, 0);
        }
    }
}