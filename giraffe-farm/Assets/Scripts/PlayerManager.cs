using UnityEngine;
using System.Collections;

public class PlayerManager : MonoBehaviour {

    public GameObject m_ball;
    private GameObject m_startMarker;

	void Start () {
       
        
    }
	
	void Update () {
	    if (m_startMarker == null)
        {
            m_startMarker = GameObject.Find("Start Marker");
            GameObject.Instantiate(m_ball, m_startMarker.transform.position, Quaternion.identity);
        }
	}
}
