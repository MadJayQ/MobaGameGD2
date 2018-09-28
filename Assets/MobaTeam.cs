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

	public MobaTeam(MobaTeams team) {
		this.TeamNum = team;		
	}


	public bool IsFriendly(MobaTeam other) {
		return TeamNum == other.TeamNum;
	}

	public void CloneFromOther(MobaTeam other) {
		this.TeamNum = other.TeamNum;
	}

	public static MobaTeam FromEnum(MobaTeams team) {
		switch(team) {
			case MobaTeams.TEAM_CAPSULES: {
				return new MobaTeam(MobaTeams.TEAM_CAPSULES);
			}
			case MobaTeams.TEAM_CUBE: {
				return new MobaTeam(MobaTeams.TEAM_CUBE);
			}
		}
		return null;
	}
}
