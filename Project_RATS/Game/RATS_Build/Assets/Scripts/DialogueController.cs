using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DialogueController : MonoBehaviour
{
    public TextMeshProUGUI textComponent; //text in box referenced in the inspector
    DialogueLineTimeArray dialogueArray;
    List<string> DayEarly;
    List<string> DayLate;
    List<string> NightEarly;
    List<string> NightLate;
    public GameObject timeController;
    GameTime timeScript;
    List<string> lines;
    
    //public string[] lines; //these are the actual lines of dialogue displayed on-screen (assign through inspector)
    public float textSpeed; //intervals in seconds between each character displayed

    private int index = 0; //what line of dialogue is currently displayed
    //public GameObject dialogueBox; //references the UI to enable and disable it
    public GameObject dialogueBox;

    void Start()
    {
        //changing this to an explicit call in the editor - allie
        //dialogueBox = GameObject.Find("DialogueBox"); //gets reference to UI by name

        dialogueArray = GetComponent<DialogueLineTimeArray>();
        dialogueBox.SetActive(false); //turn off dialogue box to start
        textComponent.text = string.Empty; //start text empty
        timeScript = timeController.GetComponent<GameTime>();
        lines = new List<string>();
        DayEarly = new List<string>();
        DayLate = new List<string>();
        NightEarly = new List<string>();
        NightLate = new List<string>();
        //Debug.Log(dialogueArray.DayEarlyLines.Count);
        for (int i = 0; i < dialogueArray.DayEarlyLines.Count; i++)
        {
            DayEarly.Add(dialogueArray.DayEarlyLines[i]);
            //DayEarly[i] = dialogueArray.DayEarlyLines[i];
        }
        for (int i = 0; i < dialogueArray.DayLateLines.Count; i++)
        {
            DayLate.Add(dialogueArray.DayLateLines[i]);
        }
        for (int i = 0; i < dialogueArray.NightEarlyLines.Count; i++)
        {
            NightEarly.Add(dialogueArray.NightEarlyLines[i]);
        }
        for (int i = 0; i < dialogueArray.NightLateLines.Count; i++)
        {
            NightLate.Add(dialogueArray.NightLateLines[i]);
        }
        if (gameObject.GetComponent<DialogueLineTimeArray>() == null)
        {
            Debug.LogError("Dialogue Component not found.");
        }
        else
        {
            dialogueArray = gameObject.GetComponent<DialogueLineTimeArray>();
        }
    }

    void Update()
    {
        Debug.Log(timeScript.WhatTimeIsIt());
        if (Input.GetMouseButtonDown(0) && PlayerController.currentState != PlayerController.States.moving) //text only runs if the player is NOT MOVING!
        {
            if (lines.Count == 0)
            {
                if (timeScript.WhatTimeIsIt() == DaySlot.hour.EarlyDay)
                {
                    lines = DayEarly;
                }
                else if (timeScript.WhatTimeIsIt() == DaySlot.hour.LateDay)
                {
                    lines = DayLate;
                }
                else if (timeScript.WhatTimeIsIt() == DaySlot.hour.EarlyNight)
                {
                    lines = NightEarly;
                }
                else
                {
                    lines = NightLate;
                }
            }
            else
            {
                if (textComponent.text == lines[index])
                {
                    dialogueBox.SetActive(true);
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    textComponent.text = lines[index];
                }
            }
        }
    }

    public void StartDialogue() //this gets called through the player controller, after moving to a NPC
    {
        Debug.Log("running");
        lines.Clear();
        dialogueBox.SetActive(true);
        if (timeScript.WhatTimeIsIt() == DaySlot.hour.EarlyDay)
        {
            lines = DayEarly;
        }
        else if (timeScript.WhatTimeIsIt() == DaySlot.hour.LateDay)
        {
            lines = DayLate;
        }
        else if (timeScript.WhatTimeIsIt() == DaySlot.hour.EarlyNight)
        {
            lines = NightEarly;
        }
        else
        {
            lines = NightLate;
        }
        //resets the text
        index = 0;
        textComponent.text = string.Empty;

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
        if (index == 0)
        {
            if (timeScript.WhatTimeIsIt() == DaySlot.hour.EarlyDay)
            {
                lines = DayEarly;
            }
            else if (timeScript.WhatTimeIsIt() == DaySlot.hour.LateDay)
            {
                lines = DayLate;
            }
            else if (timeScript.WhatTimeIsIt() == DaySlot.hour.EarlyNight)
            {
                lines = NightEarly;
            }
            else
            {
                lines = NightLate;
            }
        }
        if (index < lines.Count - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
            dialogueBox.SetActive(true);
        }
        else
        {
            lines = new List<string>();
            PlayerController.currentState = PlayerController.States.nothing;
            dialogueBox.SetActive(false);
        }
    }
}
