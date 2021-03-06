﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern : MonoBehaviour {

	private int mTotalStep;		// 16 step sequencer
	private int mCounter;		// current step

	private List<bool> mStepState;	
	private GameObject mCylinder;

	private PlayerControl mPlayerControl;

	// step 毎にコールされる
	// <delay> sec ずらして、action すること
	public void OnStep(float delay) {
		Cylinder cylinder = GetComponent<Cylinder> ();

		// PlayerControl が管理する global の current step が 0 に戻った時点で、
		//  ready -> start 状態に遷移。それまでは、step increment しない。
		if (cylinder.GetCylinderState () == Cylinder.cylinderState.Ready) {
			return;
		}

		if (mStepState [mCounter]) {
			AudioSource audioSource = GetComponent<AudioSource> ();
			audioSource.PlayDelayed (delay);
			if(cylinder) {
				cylinder.SetupEmission (Color.red, delay);
			}
		}

		if (++mCounter >= mTotalStep) {
			mCounter = 0;
		}
	}

	// step sequencer の state を view に 渡す際に利用
	public List<bool> GetStepState() {
		return mStepState;
	}

	// step sequencer の state を view が変更する際に利用する
	public void SetStepState(int idx, bool val) {
		mStepState[ idx ] = val;    
	}
		
	public void SetupClip(AudioClip audioClip) {   
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		audioSource.clip = audioClip;
	}

	// Use this for initialization
	void Awake () {
		// pattern sequencer用 data
		mTotalStep = 16;
		mCounter = 0;
		mStepState = new List<bool> (mTotalStep);
		for (int i = 0; i < mTotalStep; i++) {
			mStepState.Add (false);
		}
		mPlayerControl = GameObject.Find ("Controller").GetComponent<PlayerControl>();
//[
// instantiate は PlayerCotrol で
//		GameObject CylinderRsc = (GameObject)Resources.Load("prefab/BDCylinder");
//		mCylinder = Instantiate (CylinderRsc, new Vector3(0.0f, 0.2f, 0.0f), Quaternion.identity) as GameObject;	// 0.2 : y of Pedestral	
//]
	}
	
	// Update is called once per frame
	void Update () {
	}
}
