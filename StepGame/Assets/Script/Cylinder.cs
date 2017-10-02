using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cylinder : MonoBehaviour {
	private Rigidbody 	rb;
	private Renderer 	mRenderer;
	private Light 		mLight;
	private Color 		mColor;

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
		} 
		if (mState != cylinderState.Land) {
			mState = cylinderState.Land;
		}
	}
	// 衝突が終わったときに１度だけ呼ばれる関数
	void OnCollisionExit(Collision col){
	}

	// 光点灯
	IEnumerator SetupEmissionA(Color color, float delay) {
		yield return new WaitForSeconds (delay);
		mRenderer.material.EnableKeyword("_EMISSION"); //キーワードの有効化を忘れずに
		float alpha = 1.0f;
		if ( mState == cylinderState.Ready ) {
			alpha = 0.5f;
		}
		mRenderer.material.SetColor("_EmissionColor", mColor * alpha ); // 光らせる
		mLight.color = mColor;
		mLight.intensity = 1.0f;
		yield return new WaitForSeconds(0.2f); //1秒待って
		mRenderer.material.SetColor("_EmissionColor", mColor * 0.0f); // 光らせる
		mLight.intensity = 0.0f;
	}

	// beat に合わせて点灯
	public void SetupEmission(Color color, float delay) {
		StartCoroutine (SetupEmissionA(color, delay));  
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
}

