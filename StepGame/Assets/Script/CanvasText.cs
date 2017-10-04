using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CanvasText : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<Text>().enabled = false;
	}

	public void Show (bool on) {
		this.gameObject.GetComponent<Text> ().enabled = on;
	}
	public void SetText (string text) {
		this.gameObject.GetComponent<Text> ().text = text;
	}
}
