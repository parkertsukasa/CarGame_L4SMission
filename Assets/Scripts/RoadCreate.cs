using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCreate : MonoBehaviour
{

	public GameObject road;

	public GameObject coin;

	// Use this for initialization
	void Start()
	{

		for (int i = 0; i < 100; i++)
		{
			Instantiate(road, new Vector3(0, 0, i * 60), Quaternion.Euler(-90, 0, 0));
			Instantiate(coin, new Vector3(Random.Range(-9.0f, 9.0f), 12, i * Random.Range(14, 16)), Quaternion.identity);
		}

	}
	
	// Update is called once per frame
	void Update()
	{
		
	}
}
