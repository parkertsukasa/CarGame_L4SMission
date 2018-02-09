using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CarControl : MonoBehaviour
{ 
	enum hundle{forward, right, left}

	private Rigidbody rig;

	private GameObject effect;
	private GameObject blackbord;

	private int coin = 0;
	private float cointimer = 0.0f;

	private float dushtimer = 5.0f;

	private bool dush;

	public GameObject explosion;

	//---------- ステータス ---------
	public float max_speed;
	public float accelaration;

	// Use this for initialization
	void Start()
	{
		Input.gyro.enabled = true;//入力のジャイロを有効にする。
	
		rig = GetComponent<Rigidbody>();
		effect = GameObject.Find("Effect");
		effect.SetActive(false);
		blackbord = GameObject.Find("BlackOut");
		blackbord.SetActive(false);
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
			rig.velocity += Vector3.forward * accelaration;
		}

		if(rig.velocity.z > max_speed)
		{
			rig.velocity -= Vector3.forward * accelaration * 2;
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
				if (rig.velocity.z > 3.0f)
				{
					//----- はじまで行かないように右移動 -----
					if (transform.position.x < 9.0f)
						transform.Translate(Vector3.right * gyro.z * turn);

					if (transform.position.x > 9.0f)
						transform.position = new Vector3(9.0f, transform.position.y, transform.position.z);
				}
				
				break;

			case hundle.left:
				if (rig.velocity.z > 3.0f)
				{
					//----- はじまで行かないように左移動 -----
					if (transform.position.x > -9.0f)
						transform.Translate(Vector3.right * gyro.z * turn);

					if (transform.position.x < -9.0f)
						transform.position = new Vector3(-9.0f, transform.position.y, transform.position.z);
				}

				break;
		}
	}

	void CoinManage()
	{
		Image coingauge = GameObject.Find("CoinGauge").GetComponent<Image>();
		if (!dush)
		{
			coingauge.fillAmount = (float)coin / 10.0f;
			dushtimer = 5.0f;
		}	
		else
		{
			dushtimer -= Time.deltaTime;
			coingauge.fillAmount = dushtimer / 5.0f;
		}

		Image button = GameObject.Find("DushButton").GetComponent<Image>();
		Text text = GameObject.Find("DushText").GetComponent<Text>();

		//----- コインが溜まった時の処理 -----
		if (coin == 10)
		{
			float sin = (Mathf.Sin(cointimer) + 1.0f) / 2;
			cointimer += Time.deltaTime * 2;
			text.text = "超絶\n加速";
			button.color = new Color(1.0f, sin, sin);
			text.color = new Color(1.0f, sin, sin);
		}
		else
		{
			button.color = new Color(1.0f, 1.0f, 1.0f);
			text.color = new Color(1.0f, 1.0f, 1.0f);
			text.text = "";
		}
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

		//----- 牛に当たった時の処理 -----
		if (col.gameObject.tag == "Walker")
		{
			if (!dush)
			{
				Instantiate(explosion, transform.position, transform.rotation);
				blackbord.SetActive(true);
				Invoke("GameOver", 1.0f);		
			}
			else
			{
				col.GetComponent<BillsonDead>().dead = true;
			}
		}

		//----- ゴールに当たった時の処理 -----
		if(col.gameObject.tag == "Goal")
		{
			StartCoroutine("Goal");
		}
	}

	IEnumerator Goal ()
	{
		blackbord.SetActive(true);

		yield return new WaitForSeconds(1.0f);

		SceneManager.LoadScene("Goal");
	}

	void GameOver ()
	{
		SceneManager.LoadScene("Stage");
	}

	//----- 超絶加速ボタンが押された時の処理 -----
	public void Dush()
	{
		if (!dush && coin == 10)
		{
			StartCoroutine("FiveSecDush");
			coin = 0;
		}
	}

	IEnumerator FiveSecDush ()
	{
		max_speed *= 2;
		accelaration *= 2;
		dush = true;
		effect.SetActive(true);

		yield return new WaitForSeconds(5.0f);

		max_speed /= 2;
		accelaration /= 2;
		coin = 0;
		dush = false;
		effect.SetActive(false);
	}


}
