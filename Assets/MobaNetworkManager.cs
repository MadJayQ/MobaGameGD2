using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Networking;
public class MobaNetworkManager :  NetworkManager {

	[SerializeField] private NetworkStartPosition m_CapsulesStart;
	[SerializeField] private NetworkStartPosition m_CubesStart;

	[SerializeField] private Material m_CubeMaterial;
	[SerializeField] private Material m_CapsuleMaterial;

	public GameObject SpectatorPrefab;



	private bool m_CapsulesSpawned = false;
	private bool m_CubesSpawned = false;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId) {
		GameObject player = null;
		if(!m_CapsulesSpawned) {
			var start = m_CapsulesStart;
			player = Instantiate(playerPrefab, start.transform.position, start.transform.rotation);
			player.GetComponent<MobaCharacterController>().Team = MobaTeam.FromEnum(MobaTeam.MobaTeams.TEAM_CAPSULES);
			player.GetComponent<Renderer>().material = m_CapsuleMaterial;
			m_CapsulesSpawned = true;
			NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		}
		else if(!m_CubesSpawned) {
			var start = m_CubesStart;
			player = Instantiate(playerPrefab, start.transform.position, start.transform.rotation);
			player.GetComponent<MobaCharacterController>().Team = MobaTeam.FromEnum(MobaTeam.MobaTeams.TEAM_CUBE);
			player.GetComponent<Renderer>().material = m_CubeMaterial;
			m_CubesSpawned = true;
			NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		}
		else {
			conn.Disconnect(); //Yeet
		}
	}
}
