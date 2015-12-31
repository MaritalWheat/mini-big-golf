using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class CameraManager : MonoBehaviour {

	private static CameraManager Instance;
    private Transform m_ball;
	private Transform m_cameraMarker;
	private Vector3 m_courseCenter;
	private float m_radius = 5.0f;
	private float m_radiusMax = 20.0f;
	private float m_radiusMin = 10.0f;
	private float m_radiusSpeed = 0.5f;
	private Vector3 m_desiredCamPos = Vector3.zero;

	void Start () {
		if (Instance == null) {
			Instance = this;
		}
	}
	
	void Update () {
		if (GameManager.CurrentGameState == GameManager.GameState.Unstarted || GameManager.CurrentGameState == 
		    GameManager.GameState.Paused) {
			Camera.main.transform.RotateAround (m_courseCenter, Vector3.up, 4.0f * Time.deltaTime);
			Vector3 desiredPosition = (Camera.main.transform.position - m_courseCenter).normalized * m_radius + m_courseCenter;

			if (Mathf.Approximately(Camera.main.transform.position.x - desiredPosition.x, 0.0f)) {
				if (m_radius == m_radiusMin) {
					m_radius = m_radiusMax;
				} else {
					m_radius = m_radiusMin;
				}
			}

			Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, desiredPosition, Time.deltaTime * m_radiusSpeed);   
			Camera.main.transform.LookAt (m_courseCenter);
		} else {
			if (m_ball == null) {
				m_ball = GameObject.FindGameObjectWithTag ("Player").GetComponent<Transform> ();
				m_cameraMarker = GameObject.FindGameObjectWithTag ("CameraMarker").GetComponent<Transform> ();
			} else {
				Camera.main.transform.position = new Vector3 (m_ball.position.x, m_ball.position.y + 2.0f, m_ball.position.z - 5.0f);
				/*
				Vector3 velocity = m_ball.GetComponent<Rigidbody>().velocity;

				if (velocity.magnitude > 1.0f) {
					velocity = velocity.normalized;
					m_cameraMarker.position = new Vector3 (m_ball.position.x - (3.0f * velocity.x), m_ball.position.y + 2.0f, m_ball.position.z - (3.0f * velocity.z));
					m_desiredCamPos = new Vector3 (m_cameraMarker.position.x, m_cameraMarker.position.y, m_cameraMarker.position.z);
				}
				Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, m_desiredCamPos, Time.deltaTime * 3.0f);   
				*/
				Camera.main.transform.LookAt (m_ball);
			}
		}
    }

	public static void SetCameraPreGamePosition() {
		Vector3 courseCenter = CourseCreator.Course [CourseCreator.Course.Count / 2].transform.position;
		Camera.main.transform.position = new Vector3 (courseCenter.x - 8.0f, courseCenter.y + 6.0f, courseCenter.z);
		Camera.main.transform.LookAt (courseCenter);

		Instance.m_courseCenter = courseCenter;
	}

	public static void FadeCameraOnLaunch() {
		Instance.StartCoroutine ("CameraFadeInCoroutine");
	}

	public static void FadeCamera() {
		Instance.StartCoroutine ("CameraFadeOutCoroutine");
	}

	IEnumerator CameraFadeOutCoroutine () {
		VignetteAndChromaticAberration vignette = Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration> ();
		float t = 0.0f;
		while(t < 1.01f) {
			vignette.intensity = Mathf.Lerp(0.0f, 1.0f, t / 1.0f);
			t += 0.5f * Time.deltaTime;
			yield return null;
		}	

		yield return new WaitForSeconds (0.75f);

		CourseCreator.GenerateNewCourse ();

		StartCoroutine ("CameraFadeInCoroutine");
	}

	IEnumerator CameraFadeInCoroutine () {
		VignetteAndChromaticAberration vignette = Camera.main.gameObject.GetComponent<VignetteAndChromaticAberration> ();
		float t = 0.0f;
		while(t < 1.01f) {
			vignette.intensity = Mathf.Lerp(1.0f, 0.0f, t / 1.0f);
			t += 0.5f * Time.deltaTime;
			yield return null;
		}	
	}


}
