using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndLevel : MonoBehaviour
{
    public GameObject endMenu;
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
        if (col.CompareTag("Player"))
        {
            endMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }

    public void ReloadScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
