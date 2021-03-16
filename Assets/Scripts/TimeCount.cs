using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCount : MonoBehaviour
{
    private float minutes;
    private float seconds;
    private float milli;

    private bool isChrono = false;

    public void StartChrono()
    {
        isChrono = true;
        ResetChrono();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateChrono();
    }

    // To get the chrono time, first count milli, then convert to seconds and then to minutes
    private void UpdateChrono()
    {
        if (isChrono)
        {
            milli += Time.deltaTime * 10;
            if (milli > 10)
            {
                milli = 0f;
                seconds++;
            }
            if (seconds >= 60)
            {
                seconds = 0f;
                minutes++;
            }
        }
    }

    // Method to make the chrono a string, so it can be used in the HUD
    public string ChronoToString()
    {
        string result = "";
        if (minutes < 10) 
            result += "0";

        result += minutes + ":";

        if (seconds < 10)
            result += "0";

        result += seconds + "." + (int)milli;

        return result;
    }

    public void ResetChrono()
    {
        minutes = 0f;
        seconds = 0f;
        milli = 0f;
    }
}
