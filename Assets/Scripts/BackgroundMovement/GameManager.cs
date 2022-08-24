using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [SerializeField] private GameObject player;
    [SerializeField] private RendererScreenScroller backgroundScroller;
    [SerializeField] private RendererScreenScroller tilemapScroller;

    [SerializeField] private float initialScrollSpeed = 2.0f;
    [SerializeField] private float maxScrollSpeed = 10.0f;

    [SerializeField] private float speedUpInterval = 10.0f;
    [SerializeField] private float scrollSpeedDelta = 0.15f;

    [SerializeField] private float backgroundSpeedMultiplier = 1.25f;
    [SerializeField] private float floorSpeedMultiplier = 1f;


    private float scrollSpeed = 0;
    private float elapsedTimeSinceSpeedUp = 0;


    private List<GameObject> spawnedEnemies = new List<GameObject>();

    private Rigidbody2D playerRigidbody;
    private EnemiesManager enemiesManager;

    private void Awake() {
        playerRigidbody = player.GetComponent<Rigidbody2D>();
        enemiesManager = GetComponent<EnemiesManager>();

        scrollSpeed = initialScrollSpeed;
        SetSpeed();

        enemiesManager.MoveEnemy += MoveGameObject;
    }

    private void FixedUpdate() {
        if(ShouldSpeedUpScrollSpeed()) {
            SpeedUp();
        }

        if(ShouldMovePlayer()) {
            MoveGameObject(player);
        }

        elapsedTimeSinceSpeedUp += Time.fixedDeltaTime;
    }

    private bool ShouldSpeedUpScrollSpeed() {
        return elapsedTimeSinceSpeedUp > speedUpInterval && scrollSpeed < maxScrollSpeed;
    }

    private void SpeedUp() {
        scrollSpeed += scrollSpeedDelta;

        if(scrollSpeed > maxScrollSpeed) { 
            scrollSpeed = maxScrollSpeed;
        }

        SetSpeed();
        elapsedTimeSinceSpeedUp = 0;
    }

    private void SetSpeed() {
        backgroundScroller.SetScrollSpeed(scrollSpeed * backgroundSpeedMultiplier);
        tilemapScroller.SetScrollSpeed(scrollSpeed * floorSpeedMultiplier);
    }

    private bool ShouldMovePlayer() {
        return scrollSpeed > 0;
    }

    private void MoveGameObject(GameObject gameObject) {
        var position = gameObject.transform.position;
        var delta =  scrollSpeed * floorSpeedMultiplier * Time.fixedDeltaTime;
        // Moving with trasform as we need to preserve velocity
        gameObject.transform.position = new Vector3(position.x - delta, position.y, position.z);
    }

}
