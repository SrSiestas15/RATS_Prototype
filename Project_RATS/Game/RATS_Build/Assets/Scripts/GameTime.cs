using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class GameTime : MonoBehaviour
{
    public GameObject clockHUDObject;
    public Canvas canvas;
    public GameObject timeDisplay; 
    TextMeshProUGUI clockText;
    public static float currentHour = 0;
    private string currentHourText;
    public GameObject tempDay; //switches to night at 5 pm
    public GameObject tempSunset;//switches to day at 10 pm
    public GameObject tempNight;//switches to day at 10 pm
    public GameObject tempReset;//switches to day at 10 pm
    public GameObject tempSwitch;
    public float whenToSunset;
    public float whenToNight;

    enum CurrentTime {Noon, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, ResetHour};

    // Start is called before the first frame update
    void Start()
    {
        currentHour = PlayerPrefs.GetFloat("currentHour");
        clockText = clockHUDObject.GetComponent<TextMeshProUGUI>();

        if(0 <= currentHour && currentHour < whenToSunset)
        {
            tempDay.SetActive(true);
            tempNight.SetActive(false);
        } 
        else if(whenToSunset <= currentHour && currentHour < whenToNight)
        {
            tempDay.SetActive(false);
            tempSunset.SetActive(true);
        }
        else if (whenToNight <= currentHour && currentHour < 11)
        {
            tempSunset.SetActive(false);
            tempNight.SetActive(true);
        }
    }

    public void XHoursLater(int hours)
    {

        switch (hours)
        {
            case 1:
                currentHour = currentHour + 1;
                break;
            case 4:
                int passageHolder = 4;
                currentHour += passageHolder;
                break;
        }

        if (0 <= currentHour && currentHour < whenToSunset)
        {
            tempDay.SetActive(true);
            tempNight.SetActive(false);
        }
        else if (whenToSunset <= currentHour && currentHour < whenToNight)
        {
            tempDay.SetActive(false);
            tempSunset.SetActive(true);
        }
        else if (whenToNight <= currentHour && currentHour < 11)
        {
            tempSunset.SetActive(false);
            tempNight.SetActive(true);
        }


        if (currentHour >= 12)
        {
            StartCoroutine(ResetDay());
        }

        if (currentHour == whenToNight)
        {
            StartCoroutine(ToNight());
        }

    }

    public DaySlot.hour WhatTimeIsIt()
    {
        if ((currentHour >= 0) && (currentHour <= 4))
        {
            return DaySlot.hour.Day;
        }
        else if (currentHour == 5)
        {
            return DaySlot.hour.Sunset;
        }
        else
        {
            return DaySlot.hour.Night;
        }
    }

    void HourManagement()
    {
        switch (currentHour) //this will handle active changes between hours
        {
            case 0:
                currentHourText = "12 PM";
                break;
            case 1:
                currentHourText = "1 PM";
                break;
            case 2:
                currentHourText = "2 PM";
                break;
            case 3:
                currentHourText = "3 PM";
                break;
            case 4:
                currentHourText = "4 PM";
                break;
            case 5:
                currentHourText = "5 PM";
                break;
            case 6:
                currentHourText = "6 PM";
                break;
            case 7:
                currentHourText = "7 PM";
                break;
            case 8:
                currentHourText = "8 PM";
                break;
            case 9:
                currentHourText = "9 PM";
                break;
            case 10:
                currentHourText = "10 PM";
                break;
            case 11:
                currentHourText = "11 PM";
                break;
        }

        clockText.text = currentHourText;
    }

    // Update is called once per frame
    void Update()
    {
        HourManagement();
    }

    IEnumerator ResetDay() //types out each character
    {
        timeDisplay.SetActive(false);
        tempReset.SetActive(true);
        yield return new WaitForSeconds(5f);
        
        PlayerPrefs.SetFloat("currentHour", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator ToNight() //types out each character
    {
        tempSwitch.SetActive(true);
        yield return new WaitForSeconds(5f);
        tempSwitch.SetActive(false);
        XHoursLater(1);
    }
}
