using UnityEngine;
using System.Collections;

public class closeButton : MonoBehaviour {

	public GameObject infoPanel;
	public GameObject mainPanel;
	public GameObject chart;

	public void OnClick()
	{
		infoPanel.GetComponent<UIPanel> ().enabled = false;
		mainPanel.GetComponent<UIPanel> ().enabled = true;
		chart.SetActive(true);
	}
}
