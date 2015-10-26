using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CourseCreator : MonoBehaviour {

    public List<GameObject> m_blockTypesForward = new List<GameObject>();
    public List<GameObject> m_blockTypesLeft = new List<GameObject>();
    public List<GameObject> m_course = new List<GameObject>();
    private Quaternion m_currRotation = Quaternion.identity;

    private enum Direction
    {
        Forward,
        Backward,
        Left,
        Right
    }

    private Direction m_currentDirection = Direction.Forward;
    
	void Start () {
        m_course.Add(GameObject.Instantiate(m_blockTypesForward[0], Vector3.zero, Quaternion.identity) as GameObject);
        for (int i = 0; i < 100; i++)
        {
            GameObject lastBlock = m_course[m_course.Count - 1];
            if (lastBlock.name.Contains("left"))
            {
                m_currentDirection = Direction.Left;
            } else if (lastBlock.name.Contains("forward"))
            {
                m_currentDirection = Direction.Forward;
            }

            if (m_currentDirection == Direction.Forward)
            {
                Renderer renderer = lastBlock.GetComponentInChildren<Renderer>();
                Vector3 nextPos = Vector3.zero;
                if (lastBlock.name.Contains("corner"))
                {
                    nextPos = new Vector3(renderer.transform.position.x, renderer.transform.position.y, renderer.transform.position.z + (4 * renderer.bounds.extents.z));
                }
                else
                {
                    nextPos = new Vector3(renderer.transform.position.x, renderer.transform.position.y, renderer.transform.position.z + (2 * renderer.bounds.extents.z));
                }
                m_course.Add(GameObject.Instantiate(m_blockTypesForward[Random.Range(1, m_blockTypesForward.Count)], nextPos, Quaternion.identity) as GameObject);
            } else if (m_currentDirection == Direction.Left) {
                Renderer renderer = lastBlock.GetComponentInChildren<Renderer>();
                Vector3 nextPos = new Vector3(renderer.transform.position.x - (2 * renderer.bounds.extents.x), renderer.transform.position.y, renderer.transform.position.z);
                m_course.Add(GameObject.Instantiate(m_blockTypesLeft[Random.Range(0, m_blockTypesLeft.Count)], nextPos, Quaternion.identity) as GameObject);
                GameObject placed = m_course[m_course.Count - 1];
                if (placed.name.Contains("corner"))
                {
                    placed.transform.position = new Vector3(placed.transform.position.x + (2 * renderer.bounds.extents.x), placed.transform.position.y, placed.transform.position.z - (2 * renderer.bounds.extents.x));
                }
            }

          
            
            /*if (m_course[m_course.Count - 1].name.Contains("Round"))
            {
                m_course[m_course.Count - 1].transform.rotation = (Quaternion.Euler(0.0f, 270.0f, 0.0f));
                m_course[m_course.Count - 1].transform.position = nextPos;
            }*/
        }

    }
	
	void Update () {
	    
	}
}
