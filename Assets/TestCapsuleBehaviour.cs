using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestCapsuleBehaviour : NetworkBehaviour {

	private Animator m_Animation;

	void Start () {
		m_Animation = GetComponent<Animator>();
		if(isServer) {
			m_Animation.StartPlayback();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
