﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour {

	private static PlayerManager Instance;
    public GameObject m_ballPrefab;
    private GameObject m_startMarker;
	private GameObject m_ball;

	[SerializeField]
	private List<AudioClip> m_ballHitSounds;

	private int m_hits;
	private Vector3 m_savedVelocity;
	private Vector3 m_savedAngularVelocity;
	private bool m_isRolling;
	private bool m_hasBeenHit;
	private bool m_hasStopped;
	private bool m_mayHaveStopped;
	private float m_stopTimer = 0.0f;


	public static GameObject Ball {
		get { return Instance.m_ball; }
	}

	public static int Hits {
		get { return Instance.m_hits; }
	}

	public static bool HasBeenHit {
		get { return Instance.m_hasBeenHit; }
	}

	public static bool IsRolling {
		get { return Instance.m_isRolling; }
	}

	void Start () {
       if (Instance == null) {
			Instance = this;
		}  
    }

	void Update () {
		if (GameManager.CurrentGameState == GameManager.GameState.Unstarted || GameManager.CurrentGameState == 
		    GameManager.GameState.Paused) return;

		if (m_ball != null) {
			Rigidbody rigidbody = m_ball.GetComponent<Rigidbody> ();

			if (rigidbody.velocity.magnitude < 0.1f && (!m_hasStopped && !m_mayHaveStopped)) {
				m_mayHaveStopped = true;
			} else if (rigidbody.velocity.magnitude < 0.1f && m_mayHaveStopped) {
				m_stopTimer += Time.deltaTime;
				if (m_stopTimer > 0.3f) {
					m_mayHaveStopped = false;
					m_stopTimer = 0.0f;
					m_hasStopped = true;
				}
			} else if (rigidbody.velocity.magnitude < 0.1f && m_hasStopped) {
				rigidbody.Sleep();
				rigidbody.velocity = Vector3.zero;
				
				if (m_isRolling) {
					PlayerManager.SetRollState(false);
				}

				m_hasStopped = false;
			}
		}
	}

	public static void OnStart() {
		if (Instance.m_startMarker == null)
		{
			Instance.m_startMarker = GameObject.Find("Start Marker");
		}

		if (Instance.m_startMarker != null) {
			Instance.m_ball = GameObject.Instantiate(Instance.m_ballPrefab, Instance.m_startMarker.transform.position, Quaternion.identity) as GameObject;
		} 

		Instance.ResetHits ();
	}

	public static void OnPause() {
		Rigidbody rigidbody = Instance.m_ball.GetComponent<Rigidbody> ();
		Instance.m_savedVelocity = rigidbody.velocity;
		Instance.m_savedAngularVelocity = rigidbody.angularVelocity;
		rigidbody.isKinematic = true;
	}

	public static void OnUnpause() {
		Rigidbody rigidbody = Instance.m_ball.GetComponent<Rigidbody> ();
		rigidbody.velocity = Instance.m_savedVelocity;
		rigidbody.angularVelocity = Instance.m_savedAngularVelocity;
		rigidbody.isKinematic = false;


		Instance.ResetHasBeenHit ();

	}

	public static void OnUnpausePostReset() {
		Rigidbody rigidbody = Instance.m_ball.GetComponent<Rigidbody> ();
		rigidbody.isKinematic = false;
		Instance.ResetHasBeenHit ();
	}

	public void OnResetClick() {
		PlayerManager.Reset();
		GameManager.ResetGame ();
	}

	public void OnMainMenu() {
		ResetHits ();
		GameObject.Destroy (m_ball);
		GameManager.ResetGameToMenu ();
	}

	public static void Reset () {
		Instance.m_ball.transform.position = Instance.m_startMarker.transform.position;
		Instance.m_ball.transform.rotation = Quaternion.identity;
		Rigidbody rigidbody = Instance.m_ball.GetComponent<Rigidbody> ();
		rigidbody.Sleep ();
		rigidbody.velocity = Vector3.zero;

		Instance.ResetHits ();
		PlayerManager.SetRollState (false);
	}

	private void ResetHits() {
		m_hits = 0;
		ResetHasBeenHit ();
		UIManager.UpdateHits (m_hits);
	}

	private void ResetHasBeenHit() {
		Instance.m_hasBeenHit = false;
		CameraManager.Reset ();
	}

	public static void IncrementHits() {
		Instance.m_hits++;
		UIManager.UpdateHits (Instance.m_hits);
	}

	public static void SetRollState(bool isRolling) {
		if (isRolling && !Instance.m_hasBeenHit) {
			Instance.m_hasBeenHit = true;
		}

		//play ball hit sound
		if (isRolling) {
			AudioManager.PlaySoundAtObject (PlayerManager.Ball, Instance.m_ballHitSounds [Random.Range (0, Instance.m_ballHitSounds.Count)]);
		}

		Instance.m_isRolling = isRolling;
		UIManager.DisplayBallControls (!isRolling);
	}
}
