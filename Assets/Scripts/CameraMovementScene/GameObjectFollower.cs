using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectFollower : MonoBehaviour {

    [SerializeField] GameObject target;

    [SerializeField] bool lockXAxis;
    [SerializeField] bool lockYAxis;

    [SerializeField] bool ignoreY;
    [SerializeField] bool ignoreZ;

    private Vector3 initialPosition;

    void Start() {
        initialPosition = target.transform.position;

        if(ignoreY) {
            initialPosition.y = this.gameObject.transform.position.y;
        }

        if(ignoreZ) {
            initialPosition.z = this.gameObject.transform.position.z;
        }
    }

    void FixedUpdate() {
        var position = target.transform.position;

        if(ignoreZ) {
            position.z = this.initialPosition.z;
        }

        if(!lockXAxis && !lockYAxis) {
            this.gameObject.transform.position = position;
            return;
        }

        if(lockXAxis) {
            position.x = initialPosition.x;
        }

        if(lockYAxis) {
            position.y = initialPosition.y;
        }

        this.gameObject.transform.position = position;
    }

}
