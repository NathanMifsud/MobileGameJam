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
    public List<Enemies> _EnemyTypes;

    // Object pools
    [HideInInspector]
    public List<Enemy> _AllEnemies;
    [HideInInspector]
    private List<Enemy> _AliveEnemies;
    [HideInInspector]
    private List<Enemy> _PendingEnemies;
    [HideInInspector]
    private List<Enemy> _AvailiableEnemies;
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
        _AliveEnemies = new List<Enemy>();
        _PendingEnemies = new List<Enemy>();
        _AvailiableEnemies = new List<Enemy>();

        // Get all enemies for object pooling
        foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy")) {

            _AllEnemies.Add(obj.GetComponent<Enemy>());
        }

        // All enemies are availiable at startup
        _AvailiableEnemies = _AllEnemies;

        // Get reference to player
        _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
	
	private void Update () {
		        
	}

}