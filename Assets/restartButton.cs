using UnityEngine;
using System.Collections;

public class restartButton : MonoBehaviour {

	public GameObject simulator;

	public void OnClick()
	{
		simulator.GetComponent<citySim> ().StopAllCoroutines ();
		Application.LoadLevel (0);
	}
}
