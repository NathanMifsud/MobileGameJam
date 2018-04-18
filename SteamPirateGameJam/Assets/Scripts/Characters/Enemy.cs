using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    public enum State { Pending, Availiable, Active }
    public enum EnemyType { Creatures, SmallBoat, BigBoat, Size }

    [Header("Type")]
    public EnemyType _EnemyType;

    [Header("Spawning")]

    public float _RespawnDelay;
    public Transform _SpawnLocation = null;

    [HideInInspector]
    public State _CurrentState;
    private float _CurrentSpawnTimer = 0f;

    [Header("Movement")]
    public float _MovementSpeed;
    public float _RotationSpeed;

    private FollowPath _Pathfinding;

    [Header("Firing")]
    public GameObject _FiringTarget;
    public bool _FindRandomTargetWithinArea;
    public GameObject _AreaBounds;
    private Vector3 _AreaBoundsExtents;

    private bool _inGameArea = false;

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    protected override void Start () {

        base.Start();

        // Get component references
        _Pathfinding = GetComponent<FollowPath>();

        // Set team
        _team = TEAM.ENEMY;

        if(_AreaBounds!=null)
        {
            _AreaBoundsExtents = _AreaBounds.GetComponent<Collider>().bounds.extents;
        }
	}

    protected override void Update () {

        base.Update();

        // Perform pathfinding if agent in an active state
        _Pathfinding.enabled = _CurrentState == State.Active;

        // Update respawn timer if agent is currently pending(dead)
        if (_CurrentState == State.Pending) {

            _CurrentSpawnTimer += Time.deltaTime;

            // Respawn timer has reached threshold
            if (_CurrentSpawnTimer >= _RespawnDelay)
            {

                // Move to next pool
                GameManager._Instance._AvailiableEnemies.Add(this);
                GameManager._Instance._PendingEnemies.Remove(this);
            }
        }
    }

    private void OnSpawn() {

        // Reset health stats
        _currentHealth = _health;
    }

    public override void OnDeath() {

        base.OnDeath();

        // Reset respawn timer
        _CurrentSpawnTimer = 0f;

        // Move to pending object pool
        foreach (var agent in GameManager._Instance._ActiveEnemies) {

            // We have found ourself
            if (agent == this) {

                // Move to next pool
                GameManager._Instance._PendingEnemies.Add(agent);
                GameManager._Instance._ActiveEnemies.Remove(agent);
                break;
            }
        }
    }

    public override void FireProjectile()
    {
        if (_FiringTarget == null)
        {
            if (_AreaBounds = null) // Check if shooting towards general area
            {
                Vector3 firingTowards = _AreaBounds.transform.position;
                firingTowards.x += Random.Range(-_AreaBoundsExtents.x, _AreaBoundsExtents.x);
                SpawnBullet(transform.position, Quaternion.Euler(firingTowards - transform.position));
            }
            else // default action shoot stright down
            {
                SpawnBullet(transform.position, Quaternion.Euler(Vector3.down));
            }
        }
        else // Shoot towards target
        {
            SpawnBullet(transform.position, Quaternion.Euler(_FiringTarget.transform.position - transform.position));
        }
    }

    //Moved onto the screen, so player can see
    private void OnBecameVisible()
    {
        _inGameArea = true;
    }

    //Moved off screen
    void OnBecameInvisible()
    {
        if (_inGameArea)
        {
            _CurrentState = State.Pending;
            _inGameArea = false;

            GameManager._Instance._PendingEnemies.Add(this);
            GameManager._Instance._ActiveEnemies.Remove(this);
        }
    }
}