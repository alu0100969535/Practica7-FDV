using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool {

    private GameObject gameObject;
    private Queue<GameObject> queue = new Queue<GameObject>();

    Func<GameObject> CreateInstance;

    public GameObjectPool(GameObject gameObject, int initialInstances = 5, Func<GameObject> createInstance = null) {
        this.gameObject = gameObject;

        if(createInstance == null) {
            this.CreateInstance = CreateNewInstace_Internal;
        } else {
            this.CreateInstance = createInstance;
        }

        for(var i = 0; i < initialInstances; i++) {
            CreateNewInstance();
        }
    }

    public GameObject Pop() {
        if(queue.Count == 0){
            CreateNewInstance();
        }

        var instance = queue.Dequeue();
        instance.SetActive(true);
        return instance;
    }

    public void Push(GameObject instance) {
        instance.SetActive(false);
        queue.Enqueue(instance);
    }

    private void CreateNewInstance() {
        var instance = CreateInstance();
        Push(instance);
    }

    private GameObject CreateNewInstace_Internal() {
        return GameObject.Instantiate(gameObject);
    }   
}