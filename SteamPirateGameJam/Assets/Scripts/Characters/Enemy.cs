using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character {

    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    public enum State { Pending, Availiable, Active }

    [Header("Spawning")]
    public float _InitialSpawnDelay;
    public float _RespawnDelay;

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

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    protected override void Start () {

        base.Start();

        // Get component references
        _Pathfinding = GetComponent<FollowPath>();
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

        // Continuously stream fire
        if (_CanFire) {

            OnFire();
        }
    }

    private void OnSpawn() {

        // Reset health stats
        m_currentHealth = m_health;
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

    public override void OnFire() {
        
        // Reset firing delay
        base.OnFire();


    }

}