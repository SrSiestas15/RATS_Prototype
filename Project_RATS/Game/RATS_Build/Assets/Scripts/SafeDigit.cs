using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//this exclusively handles the individual digit displays on the safe, to modify combinations or images used, look at the buttons, or the safe itself

public class SafeDigit : MonoBehaviour
{
    public GameObject textObject;
    TextMeshProUGUI digitText;
    public int currentDigit;

    public GameObject safe;
    SafeLock safeInterface;
    void Start()
    {
        digitText = textObject.GetComponent<TextMeshProUGUI>();
        digitText.text = "0";
        currentDigit = 0;
        safeInterface = safe.GetComponent<SafeLock>();
    }

    public void DigitChange (bool increase)
    {
         if (increase)
        {
            if (currentDigit == 9)
            {
                currentDigit = 0;
            }
            else
            {
                currentDigit++;
            }
        }
        else
        {
            if (currentDigit == 0)
            {
                currentDigit = 9;
            }
            else
            {
                currentDigit--;
            }
        }
        digitText.text = currentDigit.ToString();
        safeInterface.ComboChange();
    }
    // Update is called once per frame

    void Update()
    {
        switch (currentDigit)
        {
            case 0:
                digitText.text = "0";
                break;
            case 1:
                digitText.text = "1";
                break;
            case 2:
                digitText.text = "2";
                break;
            case 3:
                digitText.text = "3";
                break;
            case 4:
                digitText.text = "4";
                break;
            case 5:
                digitText.text = "5";
                break;
            case 6:
                digitText.text = "6";
                break;
            case 7:
                digitText.text = "7";
                break;
            case 8:
                digitText.text = "8";
                break;
            case 9:
                digitText.text = "9";
                break;
        }
    }
}
