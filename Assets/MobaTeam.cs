using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MobaTeam{
	public enum MobaTeams {
		TEAM_CAPSULES = 0,
		TEAM_CUBE = 1
	};

	public MobaTeams TeamNum;

	public bool IsFriendly(MobaTeam other) {
		return TeamNum == other.TeamNum;
	}

	public void CloneFromOther(MobaTeam other) {
		this.TeamNum = other.TeamNum;
	}
}
