using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetEnemys : MonoBehaviour
{
    public static GameObject scorpion;
    public static GameObject bird;

    public GameObject enemys;
    public GameObject enemysPos;

    public void resetEnemys()
    {
        for (int i = 0; i < enemysPos.transform.childCount; i++)
        {
            Transform e = enemysPos.transform.GetChild(i);
        }
    }
}
