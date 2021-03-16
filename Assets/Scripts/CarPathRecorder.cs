using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CarPathRecorder : MonoBehaviour
{
    public CarTracking carTrack;
    private bool isSaved = false;
    private string filePath;

    // Use this for initialization
    void Start()
    {
        // create the carPath object
        carTrack = new CarTracking();

        // save transforms every 0.1 seconds (this way we do not save for EVERY frame)
        InvokeRepeating("SaveCarTransform", 3f, 0.1f);
        
        // set the file path where the position and rotation for every point will be saved
        filePath = Application.dataPath + "/Resources/BestRace.json";
    }

    // save totaltime since level has loaded and writes the data to the file stored in the path declared
    public void SaveTrack()
    {
        carTrack.SetTotalTime(Time.timeSinceLevelLoad);
        string journeyData = JsonUtility.ToJson(carTrack);
        File.WriteAllText(filePath, journeyData);
        isSaved = true;
    }

    void SaveCarTransform()
    {
        carTrack.SavePoint(gameObject.transform, Time.timeSinceLevelLoad);
    }

    public CarTracking GetCarPath()
    {
        return carTrack;
    }
}
