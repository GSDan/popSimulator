using UnityEngine;
using System.Collections;

public class optionsButton : MonoBehaviour {
	
	GameObject OptionsCont;
	UIPanel GUIOptionspanel;
	UIPanel GUIMainpanel;
	eventManager manager;
	GameObject chart;
	
	public void OnClick()
	{
		if(GUIOptionspanel == null)
		{
			OptionsCont = GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("popUp").transform.FindChild("options").gameObject;		
			GUIOptionspanel = GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("popUp").GetComponent<UIPanel>();
			GUIMainpanel = GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("inGame").gameObject.GetComponent<UIPanel>();
			chart = GUIMainpanel.transform.FindChild("Chart").gameObject;
		}
		
		GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("popUp").transform.FindChild("charViewer").gameObject.SetActive(false);
		GameObject.FindGameObjectWithTag("MainCamera").transform.FindChild("popUp").transform.FindChild("deathLog").gameObject.SetActive(false);
		OptionsCont.SetActive (true);
		
		GUIOptionspanel.enabled = true;
		GUIMainpanel.enabled = false;
		chart.SetActive (false);
	}
}
