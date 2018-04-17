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
        public int FlockSize;
    }
    
    [Header("Enemies")]
    public List<Enemies> _EnemyTypes = new List<Enemies>();
    [HideInInspector]
    public List<Enemy> _AllEnemies = new List<Enemy>();
    [HideInInspector]
    public List<Enemy> _ActiveEnemies = new List<Enemy>();
    [HideInInspector]
    public List<Enemy> _PendingEnemies = new List<Enemy>();
    [HideInInspector]
    public List<Enemy> _AvailiableEnemies = new List<Enemy>();
    [HideInInspector]
    public Player _Player;
    
    [Header("Projectiles")]
    public GameObject _Projectile;
    [HideInInspector]
    public List<Projectile> _PlayerProjectiles = new List<Projectile>();
    [HideInInspector]
    public List<Projectile> _ActivePlayerProjectiles = new List<Projectile>();
    [HideInInspector]
    public List<Projectile> _PendingPlayerProjectiles = new List<Projectile>();
    [HideInInspector]
    public List<Projectile> _EnemyProjectiles = new List<Projectile>();
    [HideInInspector]
    public List<Projectile> _ActiveEnemyProjectiles = new List<Projectile>();
    [HideInInspector]
    public List<Projectile> _PendingEnemyProjectiles = new List<Projectile>();
    private int _POOL_SIZE_PLAYER_PROJECTILES = 30;
    private int _POOL_SIZE_ENEMY_PROJECTILES = 100;

    // Pickups
    [HideInInspector]
    public List<Pickup> _AllPickups;
    [HideInInspector]
    public List<Pickup> _ActivePickups;
    [HideInInspector]
    public List<Pickup> _PendingPickups;

    //----------------------------------------------------------------------------------
    // *** FUNCTIONS ***

    /// -------------------------------------------
    /// 
    ///     Startup
    /// 
    /// -------------------------------------------
     
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
            for (int i = 0; i < enemyType.Count; i++){

                // Create game object
                _AllEnemies.Add(Instantiate(enemyType._Prefab.gameObject, transform.position, Quaternion.identity).GetComponent<Enemy>());
            }
        }

        // All enemies are pending(dead) at startup
        _PendingEnemies = _AllEnemies;

        // Get reference to player
        _Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        // Instantiate each projectile type
        for (int i = 0; i < _POOL_SIZE_PLAYER_PROJECTILES; i++) {

            // Create game object
            GameObject proj =  Instantiate(_Projectile, transform.position, Quaternion.identity);
            Projectile projectileScript = proj.GetComponent<Projectile>();
            projectileScript._team = Character.TEAM.PLAYER;
            _PlayerProjectiles.Add(projectileScript);
        }

        for (int i = 0; i < _POOL_SIZE_ENEMY_PROJECTILES; i++) {

            // Create game object
            GameObject proj = Instantiate(_Projectile, transform.position, Quaternion.identity);
            Projectile projectileScript = proj.GetComponent<Projectile>();
            projectileScript._team = Character.TEAM.ENEMY;
            _EnemyProjectiles.Add(projectileScript);
        }


        _PendingPlayerProjectiles = _PlayerProjectiles;
        _PendingEnemyProjectiles = _EnemyProjectiles;

        // Create pickup object lists
        _AllPickups = new List<Pickup>();
        _ActivePickups = new List<Pickup>();
        _PendingPickups = new List<Pickup>();
    }

    /// -------------------------------------------
    /// 
    ///     Update
    /// 
    /// -------------------------------------------

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
    }
    
    /// -------------------------------------------
    /// 
    ///     Projectiles
    /// 
    /// -------------------------------------------
    
    public Projectile GetProjectile(Character.TEAM team) {

        // Get pending projectile from player pool
        if (team == Character.TEAM.PLAYER) {

            if (_PendingPlayerProjectiles.Count > 0) {

                int size = _PendingPlayerProjectiles.Count;
                Projectile proj = _PendingPlayerProjectiles[size - 1];
                return proj;
            }
            else { return null; }
        }

        // Get pending projectile from enemy pool
        else  {

            if (_PendingEnemyProjectiles.Count > 0) {

                int size = _PendingEnemyProjectiles.Count;
                Projectile proj = _PendingEnemyProjectiles[size - 1];
                return proj;
            }
            else { return null; }
        }
    }

    public void OnProjectileFired(Projectile proj) {

        // Player team projectiles
        if (proj._team == Character.TEAM.PLAYER) {

            // Move to active array
            _ActivePlayerProjectiles.Add(proj);
            _PendingPlayerProjectiles.Remove(proj);
        }

        // Enemy team projectiles
        else {

            // Move to pending array
            _ActiveEnemyProjectiles.Add(proj);
            _PendingEnemyProjectiles.Remove(proj);
        }
    }

    public void OnProjectileDestroyed(Projectile proj) {

        // Player team projectiles
        if (proj._team == Character.TEAM.PLAYER)
        {

            // Move to pending array
            _PendingPlayerProjectiles.Add(proj);
            _ActivePlayerProjectiles.Remove(proj);
            // Play sound
            SoundManager._Instance.PlayPlayerProjectileImpact(0.9f, 1.1f);
        }

        // Enemy team projectiles
        else {

            // Move to pending array
            _PendingEnemyProjectiles.Add(proj);
            _ActiveEnemyProjectiles.Remove(proj);

            // Play sound
            SoundManager._Instance.PlayEnemyProjectileImpact(0.9f, 1.1f);
        }
    }

    /// -------------------------------------------
    /// 
    ///     Enemies
    /// 
    /// -------------------------------------------

    public List<Enemy> GetEnemies(Enemy.EnemyType type, int amount) {

        // Make empty list
        List<Enemy> list = new List<Enemy>();

        // There are enough pending enemies to return
        if (_AvailiableEnemies.Count >= amount) {

            for (int i = 0; i < amount; i++) {

                // If it is a matching enemy type
                if (_AvailiableEnemies[i]._EnemyType == type) {

                    list.Add(_AvailiableEnemies[i]);
                }
            }
        }

        // There are NOT enough pending enemies to return
        else {

            // Get as many enemies as possible of the same type
            int difference = amount - _AvailiableEnemies.Count;
            for (int i = 0; i < amount - difference; i++) { 

                // If it is a matching enemy type
                if (_AvailiableEnemies[i]._EnemyType == type) {

                    list.Add(_AvailiableEnemies[i]);
                }
            }
        }

        foreach (Enemy enemy in list)
        {
            enemy._CurrentState = Enemy.State.Active;

            _AvailiableEnemies.Remove(enemy);
            _ActiveEnemies.Add(enemy);
        }

        // Return list of enemies to use
        return list;
    }

    /// -------------------------------------------
    /// 
    ///     Pickups
    /// 
    /// -------------------------------------------

    public Pickup GetPickup() {

        if (_PendingPickups.Count > 0) {

            int size = _PendingPickups.Count;
            Pickup pickup = _PendingPickups[size - 1];
            return pickup;
        }
        else { return null; }
    }

    public void OnPickupSpawn(Pickup pickup) {

        // Move to active object pool
        foreach (var pendingPickup in _PendingPickups) {

            // We have found the pickup
            if (pickup == pendingPickup) {

                // Move to active array
                _ActivePickups.Add(pendingPickup);
                _PendingPickups.Remove(pendingPickup);
            }
        }
    }

    public void OnPickup(Pickup pickup) {

        // Move to pending object pool
        foreach (var activePickup in _ActivePickups) {

            // We have found the pickup
            if (pickup == activePickup) {

                // Move to pending array
                _PendingPickups.Add(activePickup);
                _ActivePickups.Remove(activePickup);
            }
        }
    }

    /// -------------------------------------------
    /// 
    ///     Pause / Unpause
    /// 
    /// -------------------------------------------

    public void SetPause(bool value) {

        // Pause time based on value
        if      (value == true)     { Time.timeScale = 0f; }
        else /* (value == false) */ { Time.timeScale = 1f; }
    }

}