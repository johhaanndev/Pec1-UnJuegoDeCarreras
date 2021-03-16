using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPathLoader {
	
	private TextAsset bestRaceData;
	private CarTracking carTracked = null;

	// loads the file data as a text and then transfers to the car path 
	// for the ghost to follow it in the next race
	public CarPathLoader()
	{
		bestRaceData = Resources.Load("BestRace") as TextAsset;

		if(bestRaceData != null)
		{
			carTracked = JsonUtility.FromJson<CarTracking>(bestRaceData.ToString());
		}
	}

	public CarTracking getCarTracked(){
		return carTracked;
	}
}
