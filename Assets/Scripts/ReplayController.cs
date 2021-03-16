using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplayController : MonoBehaviour
{
    // points to create a section/segment
    public Transform pointA;
    public Transform pointB;
    
    // time
    public float currentTime;
    private float startTime;
    private float endTime;



    // section points
    public List<TrackPoint> points;
    public int currentSectionIndex = 0;

    // Car tracked object
    private CarTracking carTrack;

    private void MakeUpdates()
    {
        UpdateSection();
        Move();
    }

    // method to start the replay
    public void StartReplay(CarTracking ct)
    {
        // assign the car tracked and get its points
        carTrack = ct;
        Initialize();
        points = carTrack.GetCarPoints();

        // id there are more than 2 points tracked, then get the very first section
        if (points.Count >= 2) 
            GetFirstSection();

        // repeat the updates every 0.1 seconds
        InvokeRepeating("MakeUpdates", 0f, 0.1f);
    }

    // initialize points method
    private void Initialize()
    {
        pointA.position = Vector3.zero;
        pointA.rotation = Quaternion.identity;

        pointB.position = Vector3.zero;
        pointB.rotation = Quaternion.identity;

        startTime = 0f;
        endTime = 0f;

        points = new List<TrackPoint>();
    }

    // Get the very first section
    private void GetFirstSection()
    {
        // point A
        pointA.position = points[currentSectionIndex].GetPosition();
        pointA.rotation = points[currentSectionIndex].GetRotation();
        startTime = points[currentSectionIndex].GetTime();
        // go to next index
        currentSectionIndex++;

        // pointB
        pointB.position = points[currentSectionIndex].GetPosition();
        pointB.rotation = points[currentSectionIndex].GetRotation();
        endTime = points[currentSectionIndex].GetTime();
        // go to next index
        currentSectionIndex++;
    }

    // Update the section every time
    private void UpdateSection()
    {
        // PointA is now pointB
        pointA.position = pointB.position;
        pointA.rotation = pointB.rotation;

        // Go to next section
        if (points.Count > currentSectionIndex)
        {
            // now pointB get the next section of the list
            pointB.position = points[currentSectionIndex].GetPosition();
            pointB.rotation = points[currentSectionIndex].GetRotation();

            // go to next index
            currentSectionIndex++;
        }
    }

    // basic movement
    private void Move()
    {
        float currentTime = Time.timeSinceLevelLoad - startTime;
        float fraction = currentTime / (endTime - startTime);

        // lerp the position from A to B to make it look like it is actually moving
        gameObject.transform.position = Vector3.Lerp(pointA.position, pointB.position, fraction);
        gameObject.transform.rotation = Quaternion.Lerp(pointA.rotation, pointB.rotation, fraction);
    }
}
