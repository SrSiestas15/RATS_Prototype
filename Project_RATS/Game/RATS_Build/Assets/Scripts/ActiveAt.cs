using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAt : MonoBehaviour
{
    public GameObject activateGameObject;
    public float[] timesActive;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (float time in timesActive)
        {
            if (time == GameTime.currentHour)
            {
                activateGameObject.SetActive(true);
                return;
            }
            else activateGameObject.SetActive(false);
        }
    }
}
