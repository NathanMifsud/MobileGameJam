using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //----------------------------------------------------------------------------------
    // *** VARIABLES ***

    public static GameManager _Instance;

    [System.Serializable]
    public struct Enemies {

        public GameObject _Prefab;
        public int Count;
    }

    [Header("ENEMIES")]
    public int _OnScreenMaximum;
    public int _OnScreenMinimum;
    public List<Enemies> _EnemyTypes;

    // Object pools
    [HideInInspector]
    public List<Enemy> _AllEnemies;
    [HideInInspector]
    public List<Enemy> _ActiveEnemies;
    [HideInInspector]
    public List<Enemy> _PendingEnemies;
    [HideInInspector]
    public List<Enemy> _AvailiableEnemies;
    [HideInInspector]
    public Player _Player;

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    private void Awake() {
     
        // Destroy old singleton if it doesnt match THIS instance
        if (_Instance != null && _Instance != this) {

            Destroy(this.gameObject);
            return;
        }

        // Set new singleton
        _Instance = this;
    }

    private void Start () {

        // Instantiate each enemy types
        foreach (var enemyType in _EnemyTypes) {

            // Based on individual each enemy type count
            for (int i = 0; i < enemyType.Count; i++) {

                // Create game object
                Instantiate(enemyType._Prefab.gameObject);
            }
        }

        // Create lists
        _AllEnemies = new List<Enemy>();
        _ActiveEnemies = new List<Enemy>();
        _PendingEnemies = new List<Enemy>();
        _AvailiableEnemies = new List<Enemy>();

        // Get all enemies for object pooling
        foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy")) {

            _AllEnemies.Add(obj.GetComponent<Enemy>());
        }

        // All enemies are pending(dead) at startup
        _PendingEnemies = _AllEnemies;

        // Get reference to player
        _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
	
	private void Update () {

        // Update AI pending states
        foreach (var pendingEnemy in _PendingEnemies) {

            pendingEnemy._CurrentState = Enemy.State.Pending;
        }

        // Update AI availiable states
        foreach (var availiableEnemy in _AvailiableEnemies) {

            availiableEnemy._CurrentState = Enemy.State.Availiable;
        }

        // Update AI active states
        foreach (var activeEnemy in _ActiveEnemies) {

            activeEnemy._CurrentState = Enemy.State.Active;
        }

        // Active enemies are too low
        if (_ActiveEnemies.Count <= _OnScreenMinimum) {

            // Grab an availiable enemy and spawn it
        }

    }

}