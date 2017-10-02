using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cylinderMove : MonoBehaviour {

//	private Vector3 m_pos;
	// Use this for initialization
//	void Start () {
//		m_pos = transform.localPosition;  // 形状位置を保持
//	}

	// Update is called once per frame
//	void Update () {
//		transform.localPosition = m_pos;  // 形状位置を更新
//		m_pos.x += 0.05f;

//		Rigidbody rb = GetComponent<Rigidbody>();
//		// rb.velocity = new Vector3(0.5f, 0.0f, 0.0f);
//		rb.MovePosition(this.transform.position + transform.right * 0.1f);
//	}

	private Rigidbody rb;
	private bool mDir; 
	private Cylinder mCylinderScriptA;
	private Cylinder mCylinderScriptB;

	void Start () {
		rb = this.GetComponent<Rigidbody>();
		mDir = true;

//		mCylinderScriptA = GameObject.Find ("Cylinder").GetComponent<cylinderScript>();
//		mCylinderScriptB = GameObject.Find ("Cylinder (1)").GetComponent<cylinderScript>();
//		GameObject cylinderRsc = (GameObject)Resources.Load("prefab/Cylinder");
//		GameObject cylinder = Instantiate (cylinderRsc, new Vector3(0, 5, 0), Quaternion.identity);
	//	cylinder.transform.SetParent(this.transform, false);
	}
	void FixedUpdate(){
/*		
		if (this.transform.position.x >= 5.0f) {
			mCylinderScriptA.SetParentTransform (this.gameObject, (transform.right * 10.0f).x);
			mCylinderScriptB.SetParentTransform (this.gameObject, (transform.right * 10.0f).x);
			rb.position = transform.position - transform.right * 10.0f;
		} else {
			rb.MovePosition (transform.position + transform.right * Time.deltaTime);
		}
*/
		if (mDir) {
			if (this.transform.position.x >= 4.0f) {
				mDir = false;
			} else {
				rb.MovePosition (transform.position + transform.right * Time.deltaTime);
			}
		} else {
			if (this.transform.position.x <= -4.0f) {
				mDir = true;
			} else {
				rb.MovePosition (transform.position - transform.right * Time.deltaTime);
			}
		}
	}
}
