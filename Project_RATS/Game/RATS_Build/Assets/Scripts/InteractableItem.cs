using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;

public class InteractableItem : MonoBehaviour
{
    private bool isInteractable;
    private Rigidbody2D itemRB;
    private Vector3 destinationPos;

    public enum typeOfAction { push, door }
    public typeOfAction chosenAction;

    [SerializeField] float XSpawnPos;

    public string sceneName;

    public void SpecialAction()
    {
        destinationPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));

        if (chosenAction == typeOfAction.push)
        {
            itemRB = GetComponent<Rigidbody2D>();
            itemRB.AddForce(Vector2.left * 100);

            TelemetryLogger.Log(this, "Pushes", destinationPos);
        } 
        else if (chosenAction == typeOfAction.door)
        {
            PlayerPrefs.SetFloat("currentHour", GameTime.currentHour);
            PlayerPrefs.SetFloat("spawnX", XSpawnPos);

            SceneManager.LoadScene(sceneName);

            TelemetryLogger.Log(this, "Entering Room", destinationPos);
        }

    }
}
