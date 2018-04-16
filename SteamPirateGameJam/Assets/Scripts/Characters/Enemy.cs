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

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    private void Start () {
		
	}

    private void Update () {

        base.Update();
		
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

                // Reset timer
                _CurrentSpawnTimer = 0f;
            }
        }
        
	}

    private void OnSpawn() {
    
        // Reset health stats
        
    }

    protected override void OnDeath() {

        base.OnDeath();

    }

}