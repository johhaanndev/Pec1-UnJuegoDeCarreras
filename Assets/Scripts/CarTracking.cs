using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CarTracking
{
    public List<TrackPoint> points = new List<TrackPoint>();
    public float totalTime;

    public void SavePoint(Transform tr, float time)
    {
        TrackPoint newPoint = new TrackPoint(tr.position, tr.rotation, time);
        points.Add(newPoint);
    }


    // ****** GETTERS AND SETTERS ******
    public List<TrackPoint> GetCarPoints()
    {
        return points;
    }

    public float GetTotalTime()
    {
        return totalTime;
    }

    public void SetTotalTime(float t)
    {
        totalTime = t;
    }
}

// Create a class to use it as object for the list of points
[System.Serializable]
public class TrackPoint
{
    public Vector3 position;
    public Quaternion rotation;
    
    public float time;

    public TrackPoint(Vector3 pos, Quaternion rot, float t)
    {
        position = pos;
        rotation = rot;
        time = t;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public Quaternion GetRotation()
    {
        return rotation;
    }

    public float GetTime()
    {
        return time;
    }

    public override string ToString()
    {
        return "| Position : " + position + " | Rotation: " + rotation + " | Time: " + time;
    }
}
