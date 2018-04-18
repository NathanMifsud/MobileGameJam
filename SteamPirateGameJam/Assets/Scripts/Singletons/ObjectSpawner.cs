using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//script written by Deeon

public class ObjectSpawner : MonoBehaviour
{
    //Setup as singleton
    [HideInInspector]
    public static ObjectSpawner _Instance;
    private void Awake()
    {

        // Destroy old singleton if it doesnt match THIS instance
        if (_Instance != null && _Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        // Set new singleton
        _Instance = this;
    }


    [Header("Spawning Varibles")]
    public float _minEnemySpawnTime;
    public float _maxEnemySpawnTime;

    public float _minPickupSpawnTime;
    public float _maxPickupSpawnTime;

    private Vector3 _playAreaMin;
    private Vector3 _playAreaMax;

    public float _SpawnAtZ = 1;
    public float _DeathAtZ = 1;

    // Use this for initialization
    void Start ()
    {
        Camera mainCamera = Camera.main;
        _playAreaMax = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, transform.position.y - mainCamera.transform.position.y));
        _playAreaMin = Camera.main.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, transform.position.y - mainCamera.transform.position.y));

        //Invoke("SpawnPickup", Random.Range(_minPickupSpawnTime, _maxPickupSpawnTime));
        Invoke("SpawnEnemies", Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime));
    }

    public void SpawnPickup()
    {
        Pickup pickup = GameManager._Instance.GetPickup();
        //TODO, maybe need to activate pickup.gameObject.SetActive(true);
        Vector3 position = pickup.transform.position;

        //Random x pos in range of screen;
        position.x = Random.Range(_playAreaMin.x, _playAreaMax.x);

        //Starting Z just above play area;
        position.z = _playAreaMax.z + 1;

        pickup.transform.position = position;

        GameManager._Instance.OnPickupSpawn(pickup);

        Invoke("SpawnPickup", Random.Range(_minPickupSpawnTime, _maxPickupSpawnTime));
    }

    public void SpawnEnemies()
    {
        int rand = Random.Range(0, 3);

        List<Enemy> enemies = new List<Enemy>();

        switch (rand)
        {
            case 0:
                enemies = GameManager._Instance.GetEnemies(Enemy.EnemyType.Creatures, GameManager._Instance._EnemyTypes[0].FlockSize);
                break;
            case 1:
                enemies = GameManager._Instance.GetEnemies(Enemy.EnemyType.BigBoat, GameManager._Instance._EnemyTypes[1].FlockSize);
                break;
            case 2:
                enemies = GameManager._Instance.GetEnemies(Enemy.EnemyType.SmallBoat, GameManager._Instance._EnemyTypes[2].FlockSize);
                break;
            default:
                break;
        }

        //Set spawn locaiton of enemy
        foreach (Enemy enemy in enemies)
        {
            //Spawn at 
            if(enemy._SpawnLocation !=null)
            {
                enemy.transform.position = enemy._SpawnLocation.position;
            }
            else
            {
                Vector3 position = Vector3.zero;

                //Random x pos in range of screen;
                position.x = Random.Range(_playAreaMin.x, _playAreaMax.x);

                //Starting Y just above play area;
                position.z = _SpawnAtZ;

                enemy.transform.position = position;
                enemy.transform.rotation = Quaternion.Euler(0,180,0);

                enemy.gameObject.SetActive(true);
                enemy._deathZone = _DeathAtZ;
            }
        }
        Invoke("SpawnEnemies", Random.Range(_minEnemySpawnTime, _maxEnemySpawnTime));
    }
}
