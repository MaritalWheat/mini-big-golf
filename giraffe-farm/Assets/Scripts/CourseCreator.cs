﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CourseCreator : MonoBehaviour {

	public static CourseCreator Instance;

    public List<GameObject> m_blockTypesForward = new List<GameObject>();
    public List<GameObject> m_blockTypesLeft = new List<GameObject>();
    public List<GameObject> m_blockTypesRight = new List<GameObject>();
	public List<GameObject> m_blockTypesForwardEnd = new List<GameObject> ();
	public List<GameObject> m_blockTypesLeftEnd = new List<GameObject> ();
	public List<GameObject> m_blockTypesRightEnd = new List<GameObject> ();
    private List<GameObject> m_course = new List<GameObject>();

	private int m_seed = 12345;
	private int m_coursePar = 0;

	[SerializeField] private int m_courseSize = 0; //exposing for easier testing

	public static List<GameObject> Course { get { return Instance.m_course; } }
	public static int CoursePar { get { return Instance.m_coursePar; } }
	public static int CourseID { get { return Instance.m_seed; } }

    private enum Direction
    {
        Forward,
        Backward,
        Left,
        Right
    }

    private Direction m_currentDirection = Direction.Forward;
    
	void Start () {
		if (Instance == null) {
			Instance = this;
		}

		m_seed = DataManager.LoadLastCourse ();
    }
	
	void Update () {
		if (Course.Count == 0) {
			GenerateCourse ();
		}
	}

	//called from UI next button
	public void GenerateNextCourse() {
		//this is unsafe and timing not guaranteed, should fix but quick test
		m_seed++;
		DataManager.SaveLastCourse (m_seed);
		CameraManager.FadeCamera();
	}

	//called from UI previous button
	public void GeneratePreviousCourse() {
		//this is unsafe and timing not guaranteed, should fix but quick test
		m_seed--;
		DataManager.SaveLastCourse (m_seed);
		CameraManager.FadeCamera();
	}

	public static void GenerateNewCourse() {
		foreach (GameObject block in Instance.m_course) {
			Destroy(block);
		}

		Instance.m_coursePar = 0;
		Instance.m_currentDirection = Direction.Forward;
		Instance.m_course = new List<GameObject> ();
		Instance.GenerateCourse ();
	}

	private void GenerateCourse() {
		//MASSIVELY important - "map a day" concept will be fueled by server sending all clients the same seed, or
		//friend can exchange a seed value (opaquely), or leaderboards can be seed based
		Random.seed = m_seed;

		int determiner = Random.Range (0, 10);

		if (determiner <= 2) {
			m_courseSize = Random.Range (3, 6);
		} else if (determiner <= 5) {
			m_courseSize = Random.Range (6, 10);
		} else if (determiner <= 8) {
			m_courseSize = Random.Range (10, 12);
		} else if (determiner == 10) {
			m_courseSize = Random.Range (12, 20);
		}
		
		m_course.Add(GameObject.Instantiate(m_blockTypesForward[0], Vector3.zero, Quaternion.identity) as GameObject);
		for (int i = 0; i < m_courseSize; i++)
		{
			bool courseEnd = false;
			if (i == (m_courseSize - 1)) {
				courseEnd = true;
			}
			GameObject lastBlock = m_course[m_course.Count - 1];
			int lastBlockParScore = lastBlock.GetComponent<CourseBlock>().ParScore;

			if (lastBlock.name.Contains("left"))
			{
				m_currentDirection = Direction.Left;
			} else if (lastBlock.name.Contains("forward"))
			{
				m_currentDirection = Direction.Forward;
			} else if (lastBlock.name.Contains("right"))
			{
				m_currentDirection = Direction.Right;
			}
			
			if (m_currentDirection == Direction.Forward)
			{
				Renderer renderer = lastBlock.GetComponentInChildren<Renderer>();
				Vector3 nextPos = Vector3.zero;
				if (lastBlock.name.Contains("corner") && !lastBlock.name.Contains("right") && !lastBlock.name.Contains("r_forward"))
				{
					nextPos = new Vector3(renderer.transform.position.x, renderer.transform.position.y, renderer.transform.position.z + (4 * renderer.bounds.extents.z));
				}
				else
				{
					nextPos = new Vector3(renderer.transform.position.x, renderer.transform.position.y, renderer.transform.position.z + (2 * renderer.bounds.extents.z));
				}
				
				if (courseEnd) {
					m_course.Add(GameObject.Instantiate(m_blockTypesForwardEnd[0], nextPos, Quaternion.identity) as GameObject);
				} else {
					m_course.Add(GameObject.Instantiate(GenerateNextCourseBlock(lastBlockParScore, 1, m_blockTypesForward), nextPos, Quaternion.identity) as GameObject);
				}
			} else if (m_currentDirection == Direction.Left) {
				Renderer renderer = lastBlock.GetComponentInChildren<Renderer>();
				Vector3 nextPos = new Vector3(renderer.transform.position.x - (2 * renderer.bounds.extents.x), renderer.transform.position.y, renderer.transform.position.z);
				if (courseEnd) {
					m_course.Add(GameObject.Instantiate(m_blockTypesLeftEnd[0], nextPos, Quaternion.identity) as GameObject);
				} else {	
					m_course.Add(GameObject.Instantiate(GenerateNextCourseBlock(lastBlockParScore, 0, m_blockTypesLeft), nextPos, Quaternion.identity) as GameObject);
				}
				GameObject placed = m_course[m_course.Count - 1];
				if (placed.name.Contains("corner"))
				{
					placed.transform.position = new Vector3(placed.transform.position.x + (2 * renderer.bounds.extents.x), placed.transform.position.y, placed.transform.position.z - (2 * renderer.bounds.extents.x));
				}
			} else if (m_currentDirection == Direction.Right)
			{
				Renderer renderer = lastBlock.GetComponentInChildren<Renderer>();
				Vector3 nextPos = Vector3.zero;
				if (lastBlock.name.Contains("corner"))
				{
					nextPos = new Vector3(renderer.transform.position.x + (4 * renderer.bounds.extents.x), renderer.transform.position.y, renderer.transform.position.z);
				}
				else
				{
					nextPos = new Vector3(renderer.transform.position.x + (2 * renderer.bounds.extents.x), renderer.transform.position.y, renderer.transform.position.z);
				}
				if (courseEnd) {
					m_course.Add(GameObject.Instantiate(m_blockTypesRightEnd[0], nextPos, Quaternion.identity) as GameObject);
				} else {
					m_course.Add(GameObject.Instantiate(GenerateNextCourseBlock(lastBlockParScore, 0, m_blockTypesRight), nextPos, Quaternion.identity) as GameObject);
				}
			}

			//IMPORTANT - send the "pre game" camera position signal once the course is completed
			AddCoursePar(m_course[m_course.Count - 1]);
		}
		CameraManager.SetCameraPreGamePosition();
		UIManager.SetCoursePar (m_coursePar);
		UIManager.SetCourseBestScore (m_seed);
		Debug.Log("Course par: " + m_coursePar);
	}

	private void AddCoursePar(GameObject placedCourseBlock) {
		if (placedCourseBlock.GetComponent<CourseBlock> () != null) {
			CourseBlock block = placedCourseBlock.GetComponent<CourseBlock>();
			m_coursePar += block.ParScore;
		}
	}

	private GameObject GenerateNextCourseBlock(int previousPar, int rangeStart, List<GameObject> blockList) {
		bool foundMatch = false;
		GameObject toInstantiate = null;
		while (!foundMatch) {
			toInstantiate = blockList [Random.Range (rangeStart, blockList.Count)];
			int parScore = toInstantiate.GetComponent<CourseBlock>().ParScore;
			if ((previousPar > 1 && (parScore < previousPar || parScore <= 1)) || previousPar <= 1) {
				foundMatch = true;
			}
		}

		return toInstantiate; 
	}
}
