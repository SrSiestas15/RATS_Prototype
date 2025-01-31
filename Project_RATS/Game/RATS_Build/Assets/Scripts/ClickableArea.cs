using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableArea : MonoBehaviour
{
    [SerializeField]
    private bool isFloor; //used to decide whether to use mouse pos or a preassigned transform

    private bool hasAction = false;

    [SerializeField] 
    private Transform destinationTransform; //assign the transform that the player should go to when clicking on this item

    private Vector3 destinationPos;

    private PlayerController playerScript;

    void Start()
    {
        playerScript = GameObject.Find("Player").GetComponent<PlayerController>();
        if(GetComponent<InteractableItem>() != null)
        {
            hasAction = true;
        }
    }

    private void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("on mouse down being called");
        Debug.Log(destinationPos);
        if (isFloor)
        {
            destinationPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        }
        else
        {
            destinationPos = destinationTransform.position;
        }

        destinationPos.z = 0;
        if(hasAction)
        {
            playerScript.MoveToAndAct(destinationPos, gameObject);
        } else playerScript.MoveTo(destinationPos);
    }
}
