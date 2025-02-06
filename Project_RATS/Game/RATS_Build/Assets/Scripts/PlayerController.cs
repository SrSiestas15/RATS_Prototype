using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum States { moving, talking, nothing }; 
    public static States currentState; //keeps track of the player's current state

    
    [SerializeField]
    private float moveSpeed;
    
    private Vector3 clickDestination; //end position determined by what the player clicked on
    private Vector3 toDestination; //vector between destination and current position

    //variables to hold extra scripts if an object is interactable or has dialogue
    InteractableItem currentAction; 
    DialogueController currentDialogue;

    private void Start()
    {        
        clickDestination = transform.position; //makes the first destination the current position
    }

    void Update()
    {
        toDestination = clickDestination - transform.position; //gets direction towarfs end destination

        if(currentState == States.moving) //if the player is currently moving
        {
            transform.Translate((toDestination).normalized * Time.deltaTime * moveSpeed); //move logic
            if(toDestination.magnitude < .2f) //if reached the goal
            {
                if(currentAction != null)
                {
                    currentAction.SpecialAction(); //do a special action if it has one
                }
                if(currentDialogue != null)
                {
                    currentDialogue.StartDialogue(); //begin a conversation if it has one
                    currentState = States.talking; //switch player-state to 'talking'
                }
                else currentState = States.nothing; //if the goal is reached and there's nothing for the player to do, just set state back to regular
            }
        }

    }

    //the following functions are called through 'clickableArea.cs'
    public void MoveTo(Vector3 destinationPos)
    {
        clickDestination = destinationPos;
        currentState = States.moving;
        currentAction = null;
        currentDialogue = null;
    }
    
    public void MoveToAndAct(Vector3 destinationPos, GameObject clickedObject)
    {
        MoveTo(destinationPos);
        if (clickedObject.GetComponent<InteractableItem>() != null)
        {
            currentAction = clickedObject.GetComponent<InteractableItem>();
        }
        else if (clickedObject.GetComponent<DialogueController>() != null)
        {
            currentDialogue = clickedObject.GetComponent<DialogueController>();
        }
    }
}
