using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CarControl : MonoBehaviour
{ 
	enum hundle{forward, right, left}

	private Rigidbody rig;

	private GameObject model;

	public float max_speed;
	private Image coingauge;
	private int coin = 0;



	// Use this for initialization
	void Start()
	{
		Input.gyro.enabled = true;//入力のジャイロを有効にする。

		coingauge = GameObject.Find("CoinGauge").GetComponent<Image>();
		rig = GetComponent<Rigidbody>();
		model = GameObject.Find("CarModel");
	}
	
	// Update is called once per frame
	void Update()
	{
		CoinManage();

		Accelerate();
		Hundle();

		if (Input.GetMouseButton(0))
			Brake();
	}

	/* ---------- Accelerate ----------
	 * 加速
	 */
	void Accelerate ()
	{
		transform.rotation = Quaternion.identity;//回転を固定

		if(rig.velocity.z < max_speed)
		{
			rig.velocity += Vector3.forward * 1.0f;
		}
	}

	/* ---------- Brake ----------
	 * ブレーキ
	 */
	void Brake ()
	{
		if(rig.velocity.z > 1.0f)
		{
			rig.velocity -= Vector3.forward * 1.5f;
		}
	}

	/* ---------- Hundle ----------
	 * ハンドル ジャイロセンサーで操作する
	 */
	void Hundle ()
	{
		Vector3 gyro = Input.gyro.rotationRateUnbiased;//ジャイロの値をVector3で取得する

		hundle state;

		/* どうやらiPhoneの場合は横持ちした場合は、Z軸回転がちょうど良さそう！
		 * Androidの場合はちがうかも！＞＜
		 */

		if (gyro.z > 0.1f)
			state = hundle.left;
		else if (gyro.z < -0.1f)
			state = hundle.right;
		else
			state = hundle.forward;

		const float turn = -1.0f;//横移動の移動量

		switch(state)
		{
			case hundle.forward:


				break;

			case hundle.right:

				//----- はじまで行かないように右移動 -----
				if(transform.position.x < 9.0f)
					transform.Translate(Vector3.right * gyro.z * turn);

				if (transform.position.x > 9.0f)
					transform.position = new Vector3(9.0f, transform.position.y, transform.position.z);

				
				break;

			case hundle.left:

				//----- はじまで行かないように左移動 -----
				if (transform.position.x > -9.0f)
					transform.Translate(Vector3.right * gyro.z * turn);

				if (transform.position.x < -9.0f)
					transform.position = new Vector3(-9.0f, transform.position.y, transform.position.z);

				break;
		}
	}

	void CoinManage()
	{
		coingauge.fillAmount = (float)coin / 10.0f;
	}

	void OnTriggerEnter(Collider col)
	{
		//------ コインに当たった時の処理 -----
		if(col.gameObject.tag == "Coin")
		{
			if(coin < 10)
				coin += 1;

			Destroy(col.gameObject);

		}
	}

}
