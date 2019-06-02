﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    public bool singleSpawn;
    private GameObject spawn;

    public PressPlate pp;


    void Update()
    {
    }


    public void SpawnObject()
    {
        if (spawn != null && singleSpawn)
        {
            Destroy(spawn);
        }
        spawn = Instantiate(prefab, transform.position, Quaternion.identity);
        Debug.Log("Spawn");
    }
}
