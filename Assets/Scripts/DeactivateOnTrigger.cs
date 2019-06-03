using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateOnTrigger : MonoBehaviour
{
    public GameObject[] List;
    public bool[] active;

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
            //col.GetComponent<Boss>().LoseMass();
            for (int i = 0; i < List.Length; i++)
            {
                List[i].SetActive(active[i]);
            }
        }
        pp.objPressing.GetComponent<LimitX>().DestroyBox();
    }
}
