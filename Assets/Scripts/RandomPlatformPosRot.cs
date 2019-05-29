using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPlatformPosRot : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(Random.Range(-100, -10), 90, 0);
        transform.position += new Vector3(0, Random.Range(-.2f, .2f), 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
