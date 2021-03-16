using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    // points to create a section/segment
    public Transform pointA;
    public Transform pointB;
    
    // times
    private float startTime;
    private float endTime;

    // section points
    public List<TrackPoint> points;
    private int currentSectionIndex = 0;

    void Awake()
    {
        // reset all data before starting
        Initialize();
    }

    void FixedUpdate()
    {
        // update every section every update and move ghost
        UpdateSection();
        MoveGhost();
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


    // If there is a path recorded earlier, it gets all points from that path and prepares it for the ghost to follow all points
    // If there is no path, it means it is the first time player plays
    public void PrepareData(CarTracking carPath)
    {
        if (carPath != null)
        {
            points = carPath.GetCarPoints();
            
            if (points.Count >= 2) 
            {
                GetFirstSection(); 
            }
            else
            {
                DisableGhost();
            }
        }
        else
        {
            DisableGhost();
        }
    }

    // quick method to disable ghost
    public void DisableGhost()
    {
        gameObject.SetActive(false);
    }

    // get the very first section to prepare it for the data
    private void GetFirstSection()
    {
        // point A
        pointA.position = points[currentSectionIndex].GetPosition();
        pointA.rotation = points[currentSectionIndex].GetRotation();
        
        // start time and section
        startTime = points[currentSectionIndex].GetTime();
        currentSectionIndex++;
        
        // point B
        pointB.position = points[currentSectionIndex].GetPosition();
        pointB.rotation = points[currentSectionIndex].GetRotation();
        
        // end time and section
        endTime = points[currentSectionIndex].GetTime();
        currentSectionIndex++;
    }

    // update every section with the next points
    private void UpdateSection()
    {
        if (Time.timeSinceLevelLoad >= endTime)
        {
            // PointA is now pointB and endTime is now endTime
            pointA.position = pointB.position;
            pointA.rotation = pointB.rotation;
            startTime = endTime;

            // Go to next section
            if (points.Count > currentSectionIndex)
            {
                // now pointB get the next section of the list
                pointB.position = points[currentSectionIndex].GetPosition();
                pointB.rotation = points[currentSectionIndex].GetRotation();

                // same with time
                endTime = points[currentSectionIndex].GetTime();

                // go to next index
                currentSectionIndex++;
            }
        }
    }

    // basic movement for the ghost car
    private void MoveGhost()
    {
        float currentTime = Time.timeSinceLevelLoad - startTime;
        float fraction = currentTime / (endTime - startTime);

        // lerp the position from A to B to make it look like it is actually moving
        gameObject.transform.position = Vector3.Lerp(pointA.position, pointB.position, fraction);
        gameObject.transform.rotation = Quaternion.Lerp(pointA.rotation, pointB.rotation, fraction);
    }
}
