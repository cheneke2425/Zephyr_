﻿using UnityEngine;
using System.Collections;

public class PortalScript : MonoBehaviour {

	public GameObject otherPortal;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		other.gameObject.transform.position = otherPortal.transform.position;
	}
}
