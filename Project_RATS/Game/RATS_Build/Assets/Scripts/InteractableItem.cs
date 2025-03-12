using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableItem : MonoBehaviour
{
    private bool isInteractable;
    private Rigidbody2D itemRB;

    public enum typeOfAction { push, door }
    public typeOfAction chosenAction;

    public string sceneName;

    public void SpecialAction()
    {
        if(chosenAction == typeOfAction.push)
        {
            itemRB = GetComponent<Rigidbody2D>();
            itemRB.AddForce(Vector2.left * 100);

            TelemetryLogger.Log(this, "Pushes");
        } 
        else if (chosenAction == typeOfAction.door)
        {
            SceneManager.LoadScene(sceneName);

            TelemetryLogger.Log(this, "Enterings");
        }

    }
}
