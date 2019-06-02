﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnTrigger : MonoBehaviour
{
    public GameObject[] deactivateList;

    public PressPlate pp;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Boss"))
        {
            col.GetComponent<Boss>().LoseMass();
            foreach (GameObject obj in deactivateList)
            {
                obj.SetActive(false);
            }
        }
        pp.objPressing.GetComponent<LimitX>().DestroyBox();
    }
}