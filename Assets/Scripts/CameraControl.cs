using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject Player;

    public Vector3 minPos;
    public Vector3 maxPos;

    public float rotationX = 5;

    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        resetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, .1f);


        Vector3 aux = transform.eulerAngles;
        aux.y = map(transform.position.x, minPos.x, maxPos.x, -rotationX, rotationX);
        transform.eulerAngles = aux;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minPos.x, maxPos.x), Mathf.Clamp(transform.position.y, minPos.y, maxPos.y), -6);
    }

    public void resetTarget(){
        target = Player;
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }
}
