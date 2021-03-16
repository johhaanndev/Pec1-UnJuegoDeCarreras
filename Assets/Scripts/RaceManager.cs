using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceManager : MonoBehaviour
{
    [Header("Player car parameters")]
    public GameObject car, carReplay;
    private bool isSaved = false;
    private bool hasFinished = false;
    private bool isReplay = false;

    [Header("Checkpoints parameters")]
    public GameObject[] checkpoints;
    private GameObject currentCheckpoint;

    [Header("Ghost and Replay parameters")]
    public GhostController ghostController;
    public ReplayManager replayController;
    public CarTracking carPath;
    public CarPathLoader carPathLoader;
    public TimeCount timeCount;
    private LapCount lapCount;

    [Header("Others")]
    public float countDownToStart = 3f;
    private WaitForSeconds startWait;
    public UIController uiController;
    public AudioSource checkpointSound;

    void Awake()
    {
        // Before starting to play, load the best time so the ghost can start (if there were any play before)
        LoadBestPreviousTime();
    }

    // Use this for initialization
    void Start()
    {
        // Initial countdown
        startWait = new WaitForSeconds(countDownToStart);
        
        // creating the lap counter
        lapCount = new LapCount();
        
        // start the race gameplay routine
        StartCoroutine(RaceStarting());

        // when starting, chekpoints are deactivated
        DeactivateAllCheckpoints();

        // Only first chekpoint is activated
        ActivateCheckpointByIndex(0);

        // prepare the data for ghost car
        ghostController.PrepareData(carPath);

        // as it is gameplay, replay is off when starting the scene
        carReplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // if current lap number is equal (or higher) than maximum laps
        if (lapCount.GetCurrentLap() >= lapCount.GetMaxLaps())
        {
            if (!hasFinished)
            {
                hasFinished = true;

                // call the method to show the total timing
                ShowTimeResult();
            }

            if (!isSaved)
            {
                // if there are no previous car tracking
                if (carPath == null)
                {
                    // show the New Record Message and save the path
                    uiController.ShowNewRecordMessage();
                    car.GetComponent<CarPathRecorder>().SaveTrack();

                    // Refresh unity database to ensure the car was tracked
                    UnityEditor.AssetDatabase.Refresh();

                    // set saved to true
                    isSaved = true;
                }
                // if race time is better than earlier best time
                else if (Time.timeSinceLevelLoad < carPath.GetTotalTime())
                {
                    // show the New Record message and save the path
                    uiController.ShowNewRecordMessage();
                    car.GetComponent<CarPathRecorder>().SaveTrack();

                    // Refresh unity database to ensure the car was tracked
                    UnityEditor.AssetDatabase.Refresh();

                    // set saved to true
                    isSaved = true;
                }
                // if time was not improved, show Keep Trying Message
                else
                {
                    if (!isReplay) uiController.ShowTryAgainMessage();
                }
            }
            // stop the car when trespassing the finish line
            StopCar();

            // Call the ending game routine
            StartCoroutine(RaceEnding());
        }

        // if it is still not finished, update the current chrono
        UpdateCurrentLapTime();
    }

    // routine for initial countdown
    // Enables car control after countdown and starts chrono
    private IEnumerator RaceStarting()
    {
        DisableCarControl();
        yield return startWait;
        EnableCarControl();
        timeCount.StartChrono();
    }

    // routine for when trespassing finish line on lap 3
    // Disables all controls and clears the UI
    private IEnumerator RaceEnding()
    {
        yield return new WaitForSeconds(4f);
        DisableCarControl();
        DisableCar();
        DisableGhost();
        ClearUI();

        isReplay = true;
        carReplay.SetActive(true);
        replayController.PlayReplay(car.GetComponent<CarPathRecorder>().GetCarPath());
    }


    private void DisableCarControl()
    {
        car.GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl>().enabled = false;
    }


    private void EnableCarControl()
    {
        car.GetComponent<UnityStandardAssets.Vehicles.Car.CarUserControl>().enabled = true;
    }

    // ********* CARS METHODS *********
    private void DisableCar()
    {
        car.SetActive(false);
    }


    private void DisableGhost()
    {
        ghostController.DisableGhost();
    }


    private void ClearUI()
    {
        uiController.DisplayReplay();
    }


    private void StopCar()
    {
        car.GetComponent<Rigidbody>().drag = 10;
        car.GetComponent<Rigidbody>().angularDrag = 10;
    }
    // ************* END **************

    // ********* CHECKPOINTS **********
    private void DeactivateAllCheckpoints()
    {
        foreach (GameObject checkpoint in checkpoints)
        {
            checkpoint.SetActive(false);
        }
    }


    private void ActivateCheckpointByIndex(int index)
    {
        currentCheckpoint = checkpoints[index];
        currentCheckpoint.SetActive(true);
    }

    // when reaching checkpoint make it sound, deactivacte it and activate the next one
    // if it is the last checkpoint, add lap and activate again the first
    public void CheckpointReached(GameObject checkpoint)
    {
        checkpointSound.Play();
        checkpoint.SetActive(false);
        for (int i = 0; i < checkpoints.Length; ++i)
        {
            if (checkpoints[i] == checkpoint)
            {
                if (i < checkpoints.Length - 1)
                {
                    ActivateCheckpointByIndex(i + 1);
                }
                else if (i == checkpoints.Length - 1)
                { // End of lap
                    ActivateCheckpointByIndex(0);
                    UpdateLap();
                }
            }
        }
    }
    // ************* END **************

    // ******** LAP AND TIMES *********
    // Save lap time, show the lap time in the UI and restart the chrono for the new lap
    private void UpdateLap()
    {
        lapCount.SaveLap(timeCount.ChronoToString());
        uiController.DisplayLapValue(lapCount.GetCurrentLap(), timeCount.ChronoToString());
        uiController.UpdateLapText(lapCount.CurrentLapToString());
        timeCount.ResetChrono();
    }

    // get the lap time and shows it as a string in the UI
    private void UpdateCurrentLapTime()
    {
        uiController.UpdateLapTimeText(timeCount.ChronoToString());
    }

    // Loads the previous best time
    private void LoadBestPreviousTime()
    {
        carPathLoader = new CarPathLoader();
        carPath = carPathLoader.getCarTracked();
    }

    // gets the total time and converts it as string to show it in the UI
    private void ShowTimeResult()
    {
        int minutes = (int)Time.timeSinceLevelLoad / 60;
        float seconds = Time.timeSinceLevelLoad % 60;

        string result = "TOTAL TIME: ";

        if (minutes < 10) 
            result += "0";

        result += minutes + ":";

        result += seconds.ToString("00.0");

        uiController.ShowTimeResult(result);
    }
    // ************* END **************
}