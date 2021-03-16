using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour 
{	
	public RaceManager raceManager;

	private void OnTriggerEnter(Collider other)
	{
		raceManager.CheckpointReached(gameObject);
	}
}
