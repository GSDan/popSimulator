using UnityEngine;
using System.Collections;

public class charTracker : MonoBehaviour {

	public int yOffset = 0;

	public personSim trackedSim { get; set;}

	public void updatePos(GameObject[] bars, int currentYear)
	{
		if(trackedSim.getAge(currentYear) < bars.Length && trackedSim.isAlive)
		{
			GameObject thisBar = bars[trackedSim.getAge(currentYear)];
			transform.localPosition = thisBar.transform.localPosition + new Vector3(25,thisBar.transform.localScale.y + yOffset,0);
		}
	}
	
}
