using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public GameObject Player;

    public Vector3 minPos;
    public Vector3 maxPos;

    public static CameraLevelLimit limit;

    public float rotationX = 5;

    public GameObject target;

    public float upDownLook = 3;

    public float lookTimeToActivate = 1f;
    private float lookTimer;


    private Vector3 oriPos;
    private Vector3 maxShake = new Vector3(1, 1, 0);
    public float shakeSpeed = .2f;
    public float shakeTime = 1;
    private float shakeTimer;

    // Start is called before the first frame update
    void Start()
    {
        resetTarget();
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxis("Vertical");
        if (PlayerController.instance.isMoving) v = 0;
        if (v == 0)
        {
            lookTimer -= Time.deltaTime;
            if (Input.GetButtonDown("Vertical")) lookTimer = lookTimeToActivate;
            if (lookTimer > 0) v = 0;
        }
        if (PlayerController.instance.climbing || !PlayerController.instance.controller.isGrounded) v = 0;
        transform.position = Vector3.Lerp(transform.position, target.transform.position + new Vector3(0, v * upDownLook, 0), .05f);

        Vector3 aux = transform.eulerAngles;
        aux.y = map(transform.position.x, limit.minPos.x, limit.maxPos.x, -rotationX, rotationX);
        transform.eulerAngles = aux;
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, limit.minPos.x, limit.maxPos.x), Mathf.Clamp(transform.position.y, limit.minPos.y, limit.maxPos.y), Mathf.Clamp(transform.position.z, limit.minPos.z, limit.maxPos.z));
    }

    public void resetTarget()
    {
        target = Player;
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    public void ScreenShake()
    {
        StartCoroutine("ScreenShakeCoroutine");
    }


    IEnumerator ScreenShakeCoroutine()
    {
        oriPos = transform.position;
        shakeTimer = shakeTime;
        Vector3 move;
        while (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            move = new Vector3(Random.Range(-1, 1), Random.Range(-1, 1), 0) * shakeSpeed;
            Vector3 aux = transform.position;
            aux.x = Mathf.Clamp(aux.x, oriPos.x - maxPos.x, oriPos.x + maxPos.x);
            aux.y = Mathf.Clamp(aux.y, oriPos.y - maxPos.y, oriPos.y + maxPos.y);
            transform.position = aux + move;

            yield return 0;
        }

        transform.position = oriPos;
    }
}
