using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public int direction;
    public float limitYUp;
    public float limitYDown;

    public bool active;
    public bool stopAtEnd = true;
    private bool atEnd;

    public float speed;

    private Vector3 origin;

    private GameObject objPressing;
    // Start is called before the first frame update
    void Start()
    {
        origin = transform.position;
    }

    void Update()
    {
        if (active)
        {
            Vector3 aux = transform.position;
            aux.y += speed * direction * Time.deltaTime;
            if (aux.y >= limitYUp && direction == 1) atEnd = true;
            else if (aux.y >= limitYUp && direction == -1) atEnd = true;
            else atEnd = false;
            aux.y = Mathf.Clamp(aux.y, limitYDown, limitYUp);
            transform.position = aux;
        }
        else if (Mathf.Abs(transform.position.y - origin.y) > 0.02)
        {
            int dir;
            if (transform.position.y < origin.y)
            {
                dir = 1;
            }
            else dir = -1;

            Vector3 aux = transform.position;
            aux.y += speed * dir * Time.deltaTime;
            //aux.y = Mathf.Clamp(aux.y, limitYDown, limitYUp);
            transform.position = aux;
        }

        if (active && objPressing == null)
        {
            active = false;
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.CompareTag("Moveble"))
        {
            Debug.Log("UP");
            active = true;
            objPressing = col.gameObject;
        }
    }
    void OnCollisionExit(Collision col)
    {
        if (col.gameObject.CompareTag("Moveble"))
        {
            active = false;
        }
    }
}
