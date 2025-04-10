using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI textComponent; //text in box referenced in the inspector
    DialogueLineTimeArray dialogueArray;
    List<string> Day;
    List<string> Sunset;
    List<string> Night;
    public GameObject timeController;
    GameTime timeScript;
    List<string> lines;

    [SerializeField] bool includeName;
    private string withName;
    
    private float textSpeed = .005f; //intervals in seconds between each character displayed

    private int index = 0; //what line of dialogue is currently displayed
    //public GameObject dialogueBox; //references the UI to enable and disable it
    public GameObject dialogueBox;

    void Start()
    {
        //changing this to an explicit call in the editor - allie
        //dialogueBox = GameObject.Find("DialogueBox"); //gets reference to UI by NPCName

        dialogueArray = GetComponent<DialogueLineTimeArray>();
        dialogueBox.SetActive(false); //turn off dialogue box to start
        textComponent.text = string.Empty; //start text empty
        timeScript = timeController.GetComponent<GameTime>();
        lines = new List<string>();
        Day = new List<string>();
        Sunset = new List<string>();
        Night = new List<string>();
        //Debug.Log(dialogueArray.DayEarlyLines.Count);
        for (int i = 0; i < dialogueArray.DayLines.Count; i++)
        {
            Day.Add(dialogueArray.DayLines[i]);
        }
        for (int i = 0; i < dialogueArray.SunsetLines.Count; i++)
        {
            Sunset.Add(dialogueArray.SunsetLines[i]);
        }
        for (int i = 0; i < dialogueArray.NightLines.Count; i++)
        {
            Night.Add(dialogueArray.NightLines[i]);
        }
        if (gameObject.GetComponent<DialogueLineTimeArray>() == null)
        {
            Debug.LogError("Dialogue Component not found.");
        }
        else
        {
            dialogueArray = gameObject.GetComponent<DialogueLineTimeArray>();
            if(includeName == true)
            {
                withName = dialogueArray.NPCName + ": ";
            } else withName = string.Empty;
        }
    }

    void Update()
    {
        //Debug.Log(timeScript.WhatTimeIsIt());
        if (lines.Count > 0)
        {
            if (Input.GetMouseButtonDown(0) && PlayerController.currentState == PlayerController.States.talking && !EventSystem.current.IsPointerOverGameObject()) //text only runs if the player is NOT MOVING!
            {
                    if (textComponent.text == withName + lines[index])
                    {
                        dialogueBox.SetActive(true);
                        NextLine();
                    }
                    else if(textComponent.text != string.Empty)
                    {
                        StopAllCoroutines();
                        textComponent.text = withName + lines[index];
                    }
            }
        }
    }

    public void StartDialogue() //this gets called through the player controller, after moving to a NPC
    {
        lines.Clear();
        if (timeScript.WhatTimeIsIt() == DaySlot.hour.Day)
        {
            lines = Day;
        }
        else if (timeScript.WhatTimeIsIt() == DaySlot.hour.Sunset)
        {
            lines = Sunset;
        }
        else if (timeScript.WhatTimeIsIt() == DaySlot.hour.Night)
        {
            lines = Night;
        }

        //resets the text
        index = 0;
        textComponent.text = withName;

        //turns on UI
        dialogueBox.SetActive(true);

        //sets player-state to 'talking' so they can't move during conversation
        PlayerController.currentState = PlayerController.States.talking;

        //types the first line
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine() //types out each character
    {
        foreach(char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine() //moves to next line or ends convo if no more lines are available
    {
        if (index < lines.Count - 1)
        {
            index++;
            textComponent.text = withName;
            StartCoroutine(TypeLine());
            dialogueBox.SetActive(true);
        }
        else
        {
            textComponent.text = withName;
            lines = new List<string>();
            PlayerController.currentState = PlayerController.States.nothing;
            dialogueBox.SetActive(false);
            timeScript.XHoursLater(1);
        }
    }
}
