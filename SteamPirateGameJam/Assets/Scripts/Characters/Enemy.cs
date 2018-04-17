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
            if (_CurrentSpawnTimer >= _RespawnDelay) {

                // Move agent to the availiable object pool
                foreach (var agent in GameManager._Instance._PendingEnemies) {

                    // We have found ourself
                    if (agent == this) {

                        // Move to next pool
                        GameManager._Instance._AvailiableEnemies.Add(agent);
                        GameManager._Instance._PendingEnemies.Remove(agent);
                        break;
                    }
                }
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

    public override void FireProjectile() {
        //Fire randomly downwards
        if(_FiringTarget==null)
        {
            if (_AreaBounds = null)
            {
                Vector3 firingTowards = _AreaBounds.transform.position;
                firingTowards.x += Random.Range(-_AreaBoundsExtents.x, _AreaBoundsExtents.x);
                SpawnBullet(transform.position, Quaternion.Euler(firingTowards - transform.position));
            }
        }
        else
        {
            SpawnBullet(transform.position, Quaternion.Euler(_FiringTarget.transform.position - transform.position));
        }
    }

}