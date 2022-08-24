using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesManager : MonoBehaviour {

    [SerializeField] Collider2D despawnCollider;

    [SerializeField] GameObject lowEnemyPrefab;
    [SerializeField] Transform spawnPivot;
    [SerializeField] private float timeBetweenLowEnemySpawn = 2.0f;

    private float elapsedTimeSinceLastLowEnemySpawn = 0;

    private GameObjectPool pool;
    public Action<GameObject> MoveEnemy = delegate{};
    private List<GameObject> enemies = new List<GameObject>();


    private void Awake() {
        pool = new GameObjectPool(lowEnemyPrefab, 5, CreateNewLowEnemyInstance);
    }

    private GameObject CreateNewLowEnemyInstance() {
        var instance = GameObject.Instantiate(lowEnemyPrefab);
        var enemy = instance.GetComponent<Enemy>();
        enemy.SetDespawnCollider(despawnCollider);
        enemy.OnDespawnColliderExited += DespawnLowEnemy;
        return instance;
    }

    private void SpawnLowEnemy() {
        var enemy = pool.Pop();
        enemy.transform.position = spawnPivot.position;

        elapsedTimeSinceLastLowEnemySpawn = 0;
        enemies.Add(enemy);
    }

    public void DespawnLowEnemy(GameObject gameObject) {
        enemies.Remove(gameObject);
        pool.Push(gameObject);
    }

    private void FixedUpdate() {
        if(ShouldSpawnLowEnemy()){
            SpawnLowEnemy();
        }

        MoveEnemies();
        elapsedTimeSinceLastLowEnemySpawn += Time.fixedDeltaTime;

    }

    private bool ShouldSpawnLowEnemy() {
        return elapsedTimeSinceLastLowEnemySpawn > timeBetweenLowEnemySpawn;
    }

    private void MoveEnemies() {
        foreach(var enemy in enemies) {
            MoveEnemy(enemy);
        }
    }

}
