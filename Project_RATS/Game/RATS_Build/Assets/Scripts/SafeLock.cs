using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this script handles the input of the code and checking if the inputed code is correct
//to run a simple check if it's locked, look at the locked variable

public class SafeLock : MonoBehaviour
{
    public GameObject ones;
    SafeDigit onesCode;
    public GameObject tens;
    SafeDigit tensCode;
    public GameObject hundreds;
    SafeDigit hundredsCode;
    public GameObject thousands;
    SafeDigit thousandsCode;

    bool locked = true;
    public GameObject lightObj;
    Image lightSprite;
    
    int digitOne;
    int digitTwo;
    int digitThree;
    int digitFour;

    public int combination; //if the combination given here does not have 4 digits, the code will supply a 0 at the begining for each missing digit
    int comboOne;
    int comboTwo;
    int comboThree;
    int comboFour;

    public float antiBrute;
    float currentTime;
    // Start is called before the first frame update
    void Start()
    {
        thousandsCode = thousands.GetComponent<SafeDigit>();
        hundredsCode = hundreds.GetComponent<SafeDigit>();
        tensCode = tens.GetComponent<SafeDigit>();
        onesCode = ones.GetComponent<SafeDigit>();
        lightSprite = lightObj.GetComponent<Image>();
        lightSprite.color = Color.red;
        currentTime = Time.unscaledTime;
        
        float holder = combination;
        float digOne;
        if (combination < 1000)
        {
            digOne = 0;
        }
        else
        {
            digOne = holder - (holder % 1000);
            digOne = digOne / 1000;
        }
        float digTwo;
        if (combination < 100)
        {
            digTwo = 0;
        }
        else
        {
            digTwo = holder - (digOne * 1000);
            digTwo = digTwo - (digTwo % 100);
            digTwo = digTwo / 100;
        }
        float digThree;
        if (combination < 10)
        {
            digThree = 0;
        }
        else
        {
            digThree = holder - (digOne * 1000) - (digTwo * 100);
            digThree = digThree - (digThree % 10);
            digThree = digThree / 10;
        }
        float digFour;
        digFour = holder - (digOne * 1000) - (digTwo * 100) - (digThree * 10);
        comboOne = (int)digOne;
        comboTwo = (int)digTwo;
        comboThree = (int)digThree;
        comboFour = (int)digFour;
    }

    // Update is called once per frame
    void Update() //all update does is check if the tickpass returns true each frame
    {
        if (TickPass(digitOne, digitTwo, digitThree, digitFour))
        {
            locked = false;
            lightSprite.color = Color.green;
        }
    }

    public void ComboChange()
    {
        currentTime = Time.unscaledTime;
        digitOne = thousandsCode.currentDigit;
        digitTwo = hundredsCode.currentDigit;
        digitThree = tensCode.currentDigit;
        digitFour = onesCode.currentDigit;
    }

    //specifically checks the time since the digit change made on the safe
    public bool TickPass(int first, int second, int third, int fourth) //this will only return true when all the digits input are correct and the anti brute time has passed
    {
        if (first == comboOne)
        {
            if (second == comboTwo)
            {
                if (third == comboThree)
                {
                    if (fourth == comboFour)
                    {
                        if (Time.unscaledTime - currentTime >= antiBrute) //prevents spamming through the numbers and brute forcing the safe
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }
}
