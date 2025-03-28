using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class BackgroundController : MonoBehaviour
{
    [SerializeField] GameTime timeController;
    public Sprite dayBG;
    public Sprite sunsetBG;
    public Sprite nightBG;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ChangeBackground(string BGtype)
    {
        if (BGtype == "day")
        {
            sr.sprite = dayBG;
        }
        if (BGtype == "sunset")
        {
            sr.sprite = sunsetBG;
        }
        if (BGtype == "night")
        {
            sr.sprite = nightBG;
        }
    }
}
