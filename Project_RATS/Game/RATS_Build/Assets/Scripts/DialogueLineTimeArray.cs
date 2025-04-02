using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

[SerializableAttribute]
public class DialogueLineTimeArray : MonoBehaviour //ScriptableObject 
{
    [SerializeField] bool repeatLines;
    public string NPCName;
    public List<string> DayLines;
    public List<string> SunsetLines;
    public List<string> NightLines;

    private void Start()
    {
        if (repeatLines)
        {
            SunsetLines = DayLines;
            NightLines = DayLines;
        }
    }

}