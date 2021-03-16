using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LapCount
{
    public string[] lapTimes;
    public int maxLaps;
    public int currentLap;

    public LapCount()
    {
        lapTimes = new string[3];
        maxLaps = 3;
        currentLap = 0;
    }

    public void UpdateCurrentLap()
    {
        currentLap++;
    }


    // To string for the HUD
    public string CurrentLapToString()
    {
        return (currentLap + 1) + "/3";
    }

    public void SaveLap(string value)
    {
        lapTimes[currentLap++] = value;
    }

    public int GetCurrentLap()
    {
        return currentLap;
    }
    public int GetMaxLaps()
    {
        return maxLaps;
    }
}
