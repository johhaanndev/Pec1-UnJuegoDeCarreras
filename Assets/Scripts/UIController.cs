using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour 
{
	[Header("Panels")]
	public GameObject statsPanel;
	public GameObject resultPanel;
	public GameObject lapsPanel;
	
	[Header("Lap Texts")]
	public Text lap, lapTime, lap1, lap2, lap3;

	[Header("Other displays")]
	public Text timeResult, countDownToStart, replayMessage;
	public Image endMessage;

	[Header("Sprites")]
	public Sprite newRecord;
	public Sprite keepTrying;

	void Awake()
	{
		// deactivate and disable final messages
		resultPanel.SetActive(false);
		endMessage.enabled = false;
		replayMessage.enabled = false;
	}

	void Update()
	{
		// create the countdown effect using times
		if(Time.timeSinceLevelLoad > 4) 
			countDownToStart.enabled = false;
		else if(Time.timeSinceLevelLoad > 3)
			countDownToStart.text = "GO";
		else if(Time.timeSinceLevelLoad > 2)
			countDownToStart.text = "1";
		else if(Time.timeSinceLevelLoad > 1)
			countDownToStart.text = "2";
	}

	// Updates the lap time text
	public void UpdateLapTimeText(string value)
	{
		lapTime.text = value;
	}

	// update the lap text
	public void UpdateLapText(string value)
	{
		lap.text = value;
	}

	// displays the current lap
	public void DisplayLapValue(int lap, string value)
	{
		switch(lap){
			case 1:
				lap1.text = value;
				break;
			case 2:
				lap2.text = value;
				break;
			case 3:
				lap3.text = value;
				break;
		}
	}

	// displays total time in screen
	public void ShowTimeResult(string value)
	{
		timeResult.text = value;
		resultPanel.SetActive(true);
		statsPanel.SetActive(false);
	}

	// displays the "NEW RECORD" message
	public void ShowNewRecordMessage()
	{
		endMessage.enabled = true;
		endMessage.sprite = newRecord;
	}

	// displays the "TRY AGAIN" message
	public void ShowTryAgainMessage()
	{
		endMessage.enabled = true;
		endMessage.sprite = keepTrying;
	}

	// Disables all messages and displays the replay message
	public void DisplayReplay()
	{
		statsPanel.SetActive(false);
		resultPanel.SetActive(false);
		lapsPanel.SetActive(false);
		endMessage.enabled = false;
		countDownToStart.enabled = false;

		replayMessage.enabled = true;
	}
}
