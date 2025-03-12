using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SessionDisplay : MonoBehaviour
{
    public void onConnectionSucces(int sessionID)
    {
        var displayfield = GetComponent<TextMeshProUGUI>();
        if (sessionID < 0)
        {
            displayfield.text = $"Logging locally (Session {sessionID})";
        }
        else
        {
            displayfield.text = $"Connected to Server (Session {sessionID})";
        }
    }

    public void OnConnectionFail(string errorMessage)
    {
        var displayField = GetComponent<TextMeshProUGUI>();
        displayField.text = $"Error: {errorMessage}";
    }
}
