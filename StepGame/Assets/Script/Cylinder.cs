using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour {
	private Rigidbody 	rb;
	private Renderer 	mRenderer;
	private Light 		mLight;
	private Color 		mColor;

	private PlayerControl mPlayerControl;

	public enum cylinderState {
		Ready,
		Start,
		Fall,
		Land
	};
	private cylinderState mState;

	// Use this for initialization
	void Awake () {
		rb = this.GetComponent<Rigidbody>();
		mRenderer = GetComponent<Renderer>();
		mLight = GetComponent<Light>();
		mState = cylinderState.Ready;

		mPlayerControl = GameObject.Find ("Controller").GetComponent<PlayerControl>();
		StartCoroutine("SetupEmissionB");
	}

	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate() {
		if (this.transform.position.x >= 5.0f) {
			//	rb.position = transform.position - transform.right * 10.0f;
		} else {
			//	rb.MovePosition (transform.position + transform.right * Time.deltaTime);
		}
	}

	// 衝突が始まったときに１度だけ呼ばれる関数 
	void OnCollisionEnter(Collision col) {
		if (col.gameObject.name == ("Plane")) { 
			Debug.Log ("GAME OVER!");
			mPlayerControl.SetGameOver ();
		} 
		if (mState == cylinderState.Fall) {
			SetStateLand();
			mPlayerControl.NextInst ();
		}
	}
	// 衝突が終わったときに１度だけ呼ばれる関数
	void OnCollisionExit(Collision col){
	}

	// start & fall 状態 の 光点灯 beat に合わせて点灯
	IEnumerator SetupEmissionA(Color color, float delay) {
		yield return new WaitForSeconds (delay);
		mRenderer.material.EnableKeyword("_EMISSION"); //キーワードの有効化を忘れずに
		float alpha = 1.0f;
		if ( mState == cylinderState.Ready ) {
			alpha = 0.3f;
		}
		mRenderer.material.SetColor("_EmissionColor", mColor * alpha ); // 光らせる
		mLight.color = mColor;
		mLight.intensity = 1.0f;
		yield return new WaitForSeconds(0.2f); //1秒待って
		mRenderer.material.SetColor("_EmissionColor", mColor * 0.0f); // 消す
		mLight.intensity = 0.0f;
	}

	// ready 中 は 0.1 msec で点滅
	IEnumerator SetupEmissionB() {
		while ( mState == cylinderState.Ready ) {
			mRenderer.material.SetColor("_EmissionColor", mColor * 0.3f ); // 光らせる
			yield return new WaitForSeconds(0.1f);
		}
	}

	// beat に合わせて点灯
	public void SetupEmission(Color color, float delay) {
		if (mState != cylinderState.Ready) {
			StartCoroutine (SetupEmissionA (color, delay)); 
		}
	}

	// 方向転換
	public void SetParentTransform(GameObject parent, float x) {
		Vector3 a = new Vector3(rb.position.x - x, rb.position.y,rb.position.z);
		rb.MovePosition(a);
	}

	// instantiate 後に color を設定
	public void SetupColor(Color color) {
		mColor = color;
		// 必ず state は ready のはず。
		if (mState == cylinderState.Ready) { 
		}
	}

	public cylinderState GetCylinderState() {
		return mState;
	}

	public void SetStateStart() {
		if ( mState == cylinderState.Ready ) {
			mState = cylinderState.Start;
		}	
	}

	public void SetStateFall() {
		if ( mState == cylinderState.Start ) {
			mState = cylinderState.Fall;
		}	
	}
	public void SetStateLand(bool force = false) {
		if (  ( mState == cylinderState.Fall ) || force ) {
			mState = cylinderState.Land;
		}	
	}
}

