using UnityEngine;
using System.Collections;

public class charTracker : MonoBehaviour {

	static UIPanel GUIProfilepanel;
	static UIPanel GUIMainpanel;
	static UILabel GUIname;
	static UILabel GUIFlavour;
	static GameObject chart;

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


	public void OnClick()
	{
		if(GUIProfilepanel == null)
		{
			GUIProfilepanel = GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("charViewer").gameObject.GetComponent<UIPanel>();
			GUIMainpanel = GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("inGame").gameObject.GetComponent<UIPanel>();
			GUIname = GUIProfilepanel.transform.FindChild("name").GetComponent<UILabel> ();
			GUIFlavour = GUIProfilepanel.transform.FindChild("flavourText").GetComponent<UILabel> ();
			chart = GUIMainpanel.transform.FindChild("Chart").gameObject;
		}

		GUIProfilepanel.enabled = true;
		GUIMainpanel.enabled = false;
		GUIname.text = trackedSim.getName ();
		GUIFlavour.text = trackedSim.getThoughts ();
		chart.SetActive (false);
	}
}
