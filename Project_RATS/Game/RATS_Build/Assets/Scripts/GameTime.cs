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
    TextMeshProUGUI clockText;
    public static float currentHour = 0;
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


        if (currentHour >= 11)
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
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                break;
            case 10:
                break;
            case 11:
                break;
        }

        clockText.text = currentHour.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        HourManagement();
    }

    IEnumerator ResetDay() //types out each character
    {
        tempReset.SetActive(true);
        yield return new WaitForSeconds(3f);
        
        PlayerPrefs.SetFloat("currentHour", 0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator ToNight() //types out each character
    {
        tempSwitch.SetActive(true);
        yield return new WaitForSeconds(3f);
        tempSwitch.SetActive(false);
        XHoursLater(1);
    }
}
