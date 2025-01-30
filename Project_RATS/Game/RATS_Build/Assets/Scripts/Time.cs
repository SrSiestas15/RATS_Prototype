using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Time : MonoBehaviour
{
    public GameObject playerHUD;
    public Canvas canvas;
    TextMeshProUGUI clockText;
    CurrentTime hour;
    public bool day;
    public bool dusk;
    public bool night;
    public GameObject tempDay; //switches to dusk at 5 pm
    public GameObject tempDusk; //switches to night at 8 pm
    public GameObject tempNight;

    

    enum CurrentTime {EightAM, NineAM, TenAM, ElevenAM, Noon, OnePM, TwoPM, ThreePM, FourPM, FivePM, SixPM, SevenPM, EightPM, NinePM, TenPM, ElevenPM, Midnight};
    // Start is called before the first frame update
    void Start()
    {
        hour = CurrentTime.EightAM;
        clockText = playerHUD.GetComponent<TextMeshProUGUI>();
    }

    public void XHoursLater (int hours)
    {
        switch (hours)
        {
            case 1:
                if (hour == CurrentTime.FivePM)
                {
                    tempDay.SetActive(false);
                    tempDusk.SetActive(true);
                }
                else if (hour == CurrentTime.EightPM)
                {
                    tempDusk.SetActive(false);
                    tempNight.SetActive(true);
                }
                else if (hour == CurrentTime.ElevenPM)
                {
                    tempNight.SetActive(false);
                    tempDay.SetActive(true);
                }
                hour = hour + 1;
                break;
            case 4:
                if ((hour >= CurrentTime.EightPM))
                {
                    tempNight.SetActive(false);
                    tempDay.SetActive(true);
                    hour = CurrentTime.EightAM;
                }
                else if ((hour >= CurrentTime.OnePM) && (hour <= CurrentTime.ThreePM))
                {
                    tempDay.SetActive(false);
                    tempDusk.SetActive(true);
                    hour = hour + 4;
                }
                else if (hour >= CurrentTime.FourPM)
                {
                    tempDusk.SetActive(false);
                    tempNight.SetActive(true);
                    hour = hour + 4;
                }
                else
                {
                    hour = hour + 4;
                }
                break;
        }
    }

    void HourManagement ()
    {
        switch (hour) //this will handle active changes between hours
        {
            case CurrentTime.EightAM:
                day = true;
                night = false;
                dusk = false;
                break;
            case CurrentTime.NineAM:
                day = true;
                night = false;
                dusk = false;
                break;
            case CurrentTime.TenAM:
                day = true;
                night = false;
                dusk = false;
                break;
            case CurrentTime.ElevenAM:
                day = true;
                night = false;
                dusk = false;
                break;
            case CurrentTime.Noon:
                day = true;
                night = false;
                dusk = false;
                break;
            case CurrentTime.OnePM:
                day = true;
                night = false;
                dusk = false;
                break;
            case CurrentTime.TwoPM:
                day = true;
                night = false;
                dusk = false;
                break;
            case CurrentTime.ThreePM:
                day = true;
                night = false;
                dusk = false;
                break;
            case CurrentTime.FourPM:
                day = true;
                night = false;
                dusk = false;
                break;
            case CurrentTime.FivePM:
                day = false;
                dusk = true;
                night = false;
                break;
            case CurrentTime.SixPM:
                day = false;
                dusk = true;
                night = false;
                break;
            case CurrentTime.SevenPM:
                day = false;
                dusk = true;
                night = false;
                break;
            case CurrentTime.EightPM:
                day = false;
                dusk = false;
                night = true;
                break;
            case CurrentTime.NinePM:
                day = false;
                dusk = false;
                night = true;
                break;
            case CurrentTime.TenPM:
                day = false;
                dusk = false;
                night = true;
                break;
            case CurrentTime.ElevenPM:
                day = false;
                dusk = false;
                night = true;
                break;
            case CurrentTime.Midnight: //this is specifically if we want to have some sort of animation to indicate a clock reset
                day = false;
                dusk = false;
                night = true;
                break;
        }
        clockText.text = hour.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        HourManagement();
    }
}
