using UnityEngine;
using System.Collections;

public class CourseBlock : MonoBehaviour {

	[SerializeField] private int m_parScore;

	public int ParScore { get { return this.m_parScore; } }
}
