using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAt : MonoBehaviour
{
    public GameObject activateGameObject;
    public float[] timesActive;

    private bool checkedYet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activateGameObject.activeInHierarchy == false)
        {
            foreach (float time in timesActive)
            {
                if (time == GameTime.currentHour)
                {
                    Debug.Log("activate");
                    activateGameObject.SetActive(true);
                    return;
                }
                else activateGameObject.SetActive(false);
            }
        }
    }
}
