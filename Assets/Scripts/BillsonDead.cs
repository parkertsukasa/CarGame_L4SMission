using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillsonDead : MonoBehaviour
{

	public bool dead = false;

	// Use this for initialization
	void Start()
	{

	}
	
	// Update is called once per frame
	void Update()
	{
		if(dead)
		{
			transform.position += new Vector3(0.0f, 2.0f, 0.0f);
			transform.Rotate(Vector3.forward * Random.Range(-70, 70));
		}
	}
}
