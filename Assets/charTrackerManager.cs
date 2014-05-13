using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class charTrackerManager : MonoBehaviour {

	List<charTracker> chars = new List<charTracker>();
	int numDead = 0;

	int lastBorn = 0;

	public int getNumChars()
	{
		return chars.Count;
	}

	public void updateTrackers(GameObject[] bars, int currentYear)
	{
		bool finished = false;

		for(int i = 0; i < chars.Count; i++)
		{
			if(chars[i] != null && chars[i].trackedSim.isAlive )
			{
				chars[i].updatePos(bars, currentYear);
			}
			else if(chars[i] != null)
			{
				DestroyObject(chars[i].gameObject);
				chars.RemoveAt(i);
				i = 0;
			}
		}
		

	}

	public void addTracker(personSim sim)
	{
		charTracker tracker = (Instantiate (Resources.Load ("charIcon", typeof(GameObject))) as GameObject).GetComponent<charTracker> ();

		bool needsOffset = false;

		for(int i = chars.Count -1; i >= 0; i--)
		{
			if(sim.yearBorn - chars[i].trackedSim.yearBorn < 10)
			{
				needsOffset = true;
				break;
			}
			if(sim.yearBorn - chars[i].trackedSim.yearBorn > 10 )
			{
				break;
			}
		}

		if(needsOffset)
		{
			List<int> banned = new List<int>();

			tracker.yOffset = chars[chars.Count-1].yOffset + 33;

			for(int i = chars.Count -1; i >= 0; i--)
			{
				if(sim.yearBorn - chars[i].trackedSim.yearBorn >= 10  && !banned.Contains(chars[i].yOffset) && chars[i].yOffset < chars[chars.Count-1].yOffset + 33)
				{
					tracker.yOffset = chars[i].yOffset;
					//Debug.Log(sim.firstName + " found a suitable offset from " + chars[i].trackedSim.firstName);
				}
				else if(!banned.Contains(chars[i].yOffset))
				{
					banned.Add(chars[i].yOffset);
				}
			}

		}

		tracker.transform.parent = transform;
		tracker.transform.localScale = Vector3.one;
		tracker.trackedSim = sim;
		tracker.transform.GetComponentInChildren<UILabel> ().text = sim.firstName;
		chars.Add(tracker);

		lastBorn = sim.yearBorn;
	}


}
