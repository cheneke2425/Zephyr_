﻿using UnityEngine;
using System.Collections;

public class BallScript : MonoBehaviour
{
	public float xSpeed = 0.1f;
	public float ySpeed = 0.1f;

	public float xPos = -7f;
	public float yPos = -3.02f;

	public GameObject YouWin;

	public Animator winAnimator;
	private Animator animator;

	private float xValue;
	private float yValue;

	public int i;

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		GameObject objs = GameObject.Find("AllObjs");
		whenlevelloaded whenloaded = objs.GetComponent<whenlevelloaded>();

		if (whenloaded.loaded)
		{
			//transform.position = new Vector3(xPos, yPos, 0);

			Vector3 SelectedPos;

			if (Input.GetMouseButtonDown(0))
			{
				SelectedPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
										   Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
				RaycastHit2D hit = Physics2D.Raycast(SelectedPos, Vector2.zero, 0f);

				if (hit && hit.transform.gameObject.CompareTag("Replay"))
				{
					animator = hit.transform.gameObject.GetComponent<Animator>();
					animator.SetTrigger("OnClick");
					StartCoroutine(WaitForMovement());
				}
			}

			xValue = GetComponent<Rigidbody2D>().velocity.x;
			yValue = GetComponent<Rigidbody2D>().velocity.y;

			if (GetComponent<Rigidbody2D>().velocity != new Vector2(0, 0))
			{
				float angle = Mathf.Atan2(yValue, xValue) * Mathf.Rad2Deg;
				transform.rotation = Quaternion.AngleAxis(angle + 90, Vector3.forward);
			}
		}

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		GameObject objs = GameObject.Find("AllObjs");
		whenlevelloaded whenloaded = objs.GetComponent<whenlevelloaded>();

		if (whenloaded.loaded)
		{
			if (other.gameObject.CompareTag("Paddle"))
			{
				GameObject paddle = other.gameObject;

				PaddleAudio paddleaudio = paddle.GetComponent<PaddleAudio>();
				paddleaudio.play();

			}

			if (other.gameObject.CompareTag("Goal"))
			{
				Win();
			}
			else if (other.gameObject.CompareTag("Obstacle"))
			{
				Lose();
			}
		}
	}


	void Win()
	{
		animator = winAnimator;
		animator.SetTrigger("Win");
		GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
		Debug.Log("YOU WIN!");
		StartCoroutine(WaitForNextLevel());
	}

	void Lose()
	{
		animator = gameObject.GetComponent<Animator>();
		animator.SetTrigger("Lose");
		GetComponent<Rigidbody2D>().velocity = new Vector3(0, 0, 0);
		Debug.Log("YOU LOSE!");
		StartCoroutine(WaitForRestart());
	}

	void Movement()
	{
		GetComponent<Rigidbody2D>().velocity = new Vector3(xSpeed, ySpeed, 0);
	}

	void NextLevel()
	{
		YouWin.transform.position = new Vector3(0, 0, 0);
	}

	IEnumerator WaitForMovement()
	{
		yield return new WaitForSeconds(0.833f);
		Movement();
	}

	IEnumerator WaitForRestart()
	{
		yield return new WaitForSeconds(2);
		GameObject restart = GameObject.Find("SwitchLevel");
		SwitchLevel switchLevel = restart.GetComponent<SwitchLevel>();
		switchLevel.WhenLoseGame();
	}

	IEnumerator WaitForNextLevel()
	{
		yield return new WaitForSeconds(2);
		NextLevel();
	}

}
