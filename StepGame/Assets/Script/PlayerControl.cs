using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PlayerControl : MonoBehaviour {

	private const int mStepCount = 16;		// 16 step sequencer
	private const int mInstCount = 4;   	// 4 inst [BD,SN,HH,CP]
	private int mCurrentInst;				// 操作中の inst  	                                       
	private int mCurrentStep;				// 現在の step 

	private List< List<bool> > mStepState;	// 16 step x 4 inst 分の step data を保存
	private List<GameObject>   mInst;		// inst
	private CanvasText 	   	   mCanvasText;	// canvas 上に表示する文字列

	private readonly bool[][] mPattern = new bool[][]
	{
		new bool[]{ true,false,false,false, true,false,false,false, true,false,false,false, true,false,false,false},
		new bool[]{ true,false, true,false, true,false, true,false, true,false, true,false, true,false, true,false},
		new bool[]{false,false,false,false, true,false,false,false,false,false,false,false, true,false, true,false},
		new bool[]{false,false,false,false,false,false, true,false,false,false,false,false,false,false, true,false}
	};

	private readonly Color[] mColor = new Color[] {
		Color.red,
		Color.yellow,
		Color.green,
		Color.blue
	};
	private List<AudioClip> mAudioClip;

	// clock generator が step timing を通知 
	public void OnStep(float delay) {
		for (int i = 0; i <= mCurrentInst; i++) {
			mInst[i].GetComponent<Pattern>().OnStep(delay);
		}

		// 
		if (++mCurrentStep >= mStepCount) {
			mCurrentStep = 0;

			if (mInst [mCurrentInst].GetComponent<Cylinder> ().GetCylinderState() 
				== Cylinder.cylinderState.Ready) {
				SetGameStart ();
			}
		}
	}
		
	// Use this for initialization
	void Start () {
		mCurrentInst = 0;
		mCurrentStep = 0;

		mStepState =  new List< List<bool> >( mInstCount );
		for (int ii = 0; ii < mInstCount; ii++) {
			List<bool> listA = new List<bool> ( mStepCount );
			for (int si = 0; si < mStepCount; si++) {
				listA.Add ( mPattern[ii][si] );
			}
			mStepState.Add(listA); 
		}
			
		mAudioClip = new List<AudioClip> (mInstCount);
		mAudioClip.Add( (AudioClip) Resources.Load("Audio/BD",typeof(AudioClip) ) );
		mAudioClip.Add( (AudioClip) Resources.Load("Audio/CH",typeof(AudioClip) ) );
		mAudioClip.Add( (AudioClip) Resources.Load("Audio/SD",typeof(AudioClip) ) );
		mAudioClip.Add( (AudioClip) Resources.Load("Audio/CYM",typeof(AudioClip) ) );

		mInst = new List<GameObject>(mInstCount);

		mCanvasText = GameObject.Find ("Text").GetComponent<CanvasText>();

		// BD は土台に設置済みとする
		GameObject cylinderRsc = (GameObject)Resources.Load("prefab/BDCylinder");
		GameObject cylinder = Instantiate(cylinderRsc,new Vector3(0.0f, 0.2f, 0.0f), Quaternion.identity);
		cylinder.GetComponent<Cylinder>().SetupColor (Color.red);
		Pattern pattern = cylinder.GetComponent<Pattern> ();
		pattern.SetupClip (mAudioClip [0]);
		for (int i = 0; i < mStepCount; i++) {
			pattern.SetStepState(i, mPattern[mCurrentInst][i]);
		}
		mInst.Add(cylinder);
		mInst [mCurrentInst].GetComponent<Cylinder> ().GetComponent<Rigidbody> ().useGravity = true;
		mInst [mCurrentInst].GetComponent<Cylinder> ().SetStateLand (true);
		mCurrentInst++;

		// 初回は SN を落下
		cylinder = Instantiate(cylinderRsc,new Vector3(0.0f, 4.0f, 0.0f), Quaternion.identity);
		cylinder.GetComponent<Cylinder>().SetupColor (Color.yellow);
		pattern = cylinder.GetComponent<Pattern> ();
		pattern.SetupClip (mAudioClip [1]);
		for (int i = 0; i < mStepCount; i++) {
			pattern.SetStepState(i, mPattern[mCurrentInst][i]);
		}
		mInst.Add(cylinder);	
		SetGameReady ();
	}

	// Update is called once per frame
	void Update () {
		// space ボタンで落下開始
		if (Input.GetKeyDown (KeyCode.Space)) {
			mInst [mCurrentInst].GetComponent<Cylinder> ().GetComponent<Rigidbody> ().useGravity = true;
			SetGameFall ();
		} 
	}

	// cylinder の OnCollisionEnter で呼ばれる。
	// 次の inst へ進む。
	public void NextInst() {
		if (mCurrentInst < mInstCount - 1) {
			GameObject cylinderRsc = (GameObject)Resources.Load ("prefab/BDCylinder");
			GameObject cylinder = Instantiate (cylinderRsc, new Vector3 (0.0f, 4.0f, 0.0f), Quaternion.identity);
			cylinder.GetComponent<Cylinder> ().SetupColor (mColor [mCurrentInst + 1]);
			Pattern pattern = cylinder.GetComponent<Pattern> ();
			pattern.SetupClip (mAudioClip [mCurrentInst + 1]);
			for (int i = 0; i < mStepCount; i++) {
				pattern.SetStepState (i, mPattern [mCurrentInst + 1] [i]);
			}
			mInst.Add (cylinder);
			mCurrentInst++;
			SetGameReady ();
		}
	}

	public void SetGameReady() {
		mCanvasText.SetText("Ready");
		mCanvasText.Show (true);
	}
	public void SetGameStart() {
		mInst [mCurrentInst].GetComponent<Cylinder> ().SetStateStart ();
		mCanvasText.SetText("Push Space!");
		mCanvasText.Show (true);
	}
	public void SetGameFall() {
		mInst [mCurrentInst].GetComponent<Cylinder> ().SetStateFall ();
		mCanvasText.SetText("");
		mCanvasText.Show (false);
	}
	public void SetGameOver() {
		mCanvasText.SetText("GAME OVER!");
		mCanvasText.Show (true);
	}
}
