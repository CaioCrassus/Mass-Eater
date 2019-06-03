using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutLlineOnSelected : MonoBehaviour
{
    public bool selected;
    // Start is called before the first frame update
    void Start()
    {
        Color aux = GetComponent<Outline>().effectColor;
        if (selected)
            aux.a = 1;
        else
            aux.a = 0;
        GetComponent<Outline>().effectColor = aux;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnSelect()
    {
        Color aux = GetComponent<Outline>().effectColor;
        aux.a = 1;
        GetComponent<Outline>().effectColor = aux;
    }

    void OnDeselect()
    {
        Color aux = GetComponent<Outline>().effectColor;
        aux.a = 0;
        GetComponent<Outline>().effectColor = aux;
    }
}
