
public class PlayerData
{
	public class Player 
	{
		public class Rank
		{
			public class NextRank
			{
				int nr;
				string img;
				string name;
				int needed;
				int curr;
				int relNeeded;
				int relCurr;
				float relProg;
			}
			
			int nr;
			string imgLarge;
			string img;
			string name;
			int needed;
			NextRank next;
		}
	
		string id;
		string game;
		string plat;
		string name;
		string tag;
		string dateCheck;
		string dateUpdate;
		string dateCreate;
		string lastDay;
		string country;
		string countryName;
		Rank rank;
		int score;
		int timePlayed;
		string uId;
		string uName;
		string uGava;
		string udCreate;
		string blPlayer;
		string blUser; 
		bool editable;
		bool viewable;
		bool adminable;
		bool linked;
	}
	
	
	
	public class Stats
	{
		public class Reset
		{
			int lastReset;
			int score;
			int timePlayed;
			int timePlayedSinceLastReset;
			int kills;
			int deaths;
			int shotsFired;
			int shotsHit;
			int numLosses;
			int numWins;
		}
		
		public class Scores
		{
			int score;
			int award;
			int bonus;
			int unlock;
			int vehicle;
			int team;
			int objective;
			int squad;
			int general;
			int totalScore;
			int rankScore;
			int combatScore;
		}
		
		public class Mode
		{
			string id;
			int score;
			string name;
		}
		
		public class Kits
		{
			Kit assault;
			Kit engineer;
			Kit support;
			Kit recon;
			Kit commander;
		}
		
		public class Kit
		{
			string id;
			int score;
			int time;
			int stars;
			float spm;
			string name;
		}
		
		public class Extra
		{
			float kdr;
			float wlr;
			float spm;
			float gspm;
			float kpm;
			float sfpm;
			float hkp;
			float khp;
			float accuracy;
			int roundsFinished;
			int vehicleTime;
			int vehicleKills;
			int weaponTime;
			int weaponKills;
			float weaTimeP;
			float weaKillsP;
			float weaKpm;
			float vehTimeP;
			float vehKillsP;
			float vehKpm;
			int ribbons;
			int ribbonsTotal;
			int ribbonsUnique;
			int medals;
			int medalsTotal;
			int medalsUnique;
			int assignments;
			int assignmentsTotal;
			float ribpr;
		}
	
		Reset reset;
		Scores scores;
		int skill;
		int elo;
		int rank;
		int timePlayed;
		int kills;
		int deaths;
		int headshots;
		int shotsFired;
		int shotsHit;
		int suppressionAssists;
		int avengerKills;
		int saviorKills;
		int nemesisKills;
		int numRounds;
		int numLosses;
		int numWins;
		int killStreakBonus;
		int nemesisStreak;
		int mcomDefendKills;
		int resupplies;
		int repairs;
		int heals;
		int revives;
		float longestHeadshot;
		int longestWinStreak;
		int flagDefend;
		int flagCaptures;
		int killAssists;
		int vehiclesDestroyed;
		int vehicleDamage;
		int dogtagsTaken;
		Mode[] modes;
		Kits kits;
		Extra extra;
	}
	
	
	
	public class Dogtags
	{
		public class Dogtag
		{
			string id;
			string image;
			string name;
			string desc;
			string license;
			string category;
			string img;
		}
	
		Dogtag advanced;
		Dogtag basic;
	}
	
	
	
	public class Weapon
	{
		public class Stat
		{
			string id;
			int shots;
			int hits;
			int hs;
			int kills;
			int time;
		}
		
		public class Detail
		{
			public class WeaponData
			{
				float statHandling;
				bool fireModeBurst;
				float statDamage;
				float statMobility;
				bool fireModeSingle;
				bool fireModeAuto;
				float statRange;
				string range;
				string rateOfFire;
				//altAmmoName: null,
				string ammoType;
				float statAccuracy;
				string ammo;
			}
	
			public class Unlock
			{
				string code;
				string name;
				int needed;
			}
	
			string id;
			string category;
			string name;
			string type;
			string desc;
			string image;
			WeaponData weaponData;
			string code;
			string[] kits;
			//unlockLicense: null
			Unlock unlock;
		}
	
		public class Extra
		{
			float kpm;
			float sfpm;
			float hkp;
			float accuracy;
		}
	
		public class Stars
		{
			int count;
			int curr;
			int needed;
			int relNeeded;
			int relCurr;
			int relProg;
		}
	
		public class Unlock
		{
			string id;
			string category;
			string name;
			string desc;
			string rareness;
			string image;
			string type;
			int needed;
			//license: null
		}
	
		Stat stat;
		string name;
		Detail detail;
		Extra extra;
		Stars stars;
		string imgFancy;
		string imgLineart;
		Unlock[] unlocks;
		int unlocksTotal;
		int unlocksCurr;
	}

	public class WeaponCategory
	{
		public class Stat
		{
			int score;
			int time;
			int kills;
			int hs;
			int hits;
			int shots;
		}

		public class Extra
		{
			float kpm;
			float spm;
			float sfpm;
			float hkp;
			float accuracy;
		}

		string name;
		Stat stat;
		Extra extra;
	}

	public class Kititem
	{
		public class Stat
		{
			string id;
			int prog;
		}

		public class Detail
		{
			public class Unlocks
			{
				string code;
				string name;
				int needed;
			}

			public class Stat
			{
				string kills;
				string repairs;
				string revives;
				string heals;
				string shots;
				string spawns;
				string spotAssists;
				string damageAssists;
				string resupplies;
			}

			string id;
			string name;
			string category;
			string desc;
			string image;
			string kit;
			Unlocks unlock;
			Stat stat;
			string ranked; 
		}

		Stat stat;
		string name;
		Detail detail;
		string imgFancy;
		string imgLineart;
	}

	public class Vehicles
	{
		public class Stat
		{
			string id;
			int destroys;
			int kills;
			int time;
		}

		public class Detail
		{
			string id;
			string name;
			string category;
			string image;
		}

		public class Extra
		{
			float kpm;
		}

		Stat stat;
		string name;
		Detail detail;
		Extra extra;
		string imgFancy;
		string imgLineart;
	}

	public class VehicleCategory
	{
		public class Stat 
		{
			int destroys;
			int time;
			int kills;
			int score;
		}

		public class Unlock
		{
			string id;
			string category;
			string name;
			string desc;
			string image;
			string type;
			int needed;
			//license: null },
		}

		public class Extra
		{
			float kpm;
			float spm;
		}

		string id;
		string name;
		Stat stat;
		Unlock[] unlocks;
		int unlocksTotal;
		int unlocksCurr;
		Extra extra;
	}

	public class Award
	{
		public class Medal
		{
			string id;
			string name;
			string desc;
			string image;
			string category;
			string ribbon;
			int needed;
		}

		public class Ribbon
		{
			string id;
			string name;
			string desc;
			string image;
		}

		string id;
		int mCount;
		int rCount;
		string mName;
		string rName;
		Medal medal;
		Ribbon ribbon;
		int curr;
		int needed;
		int relNeeded;
		int relCurr;
		int relProg;
		string imgMedal;
		string imgRibbon;
	}

	public class Assignment
	{
		public class Dependency
		{
			string group;
			string realm;
			string code;
			string name;
		}

		public class Criteria
		{
			string desc;
			int needed;
			int curr;
			float prog;
		}

		string id;
		string image;
		string name;
		string category;
		string desc;
		Dependency[] dependencies;
		Criteria[] criterias;
		float curr;
		int needed;
		float prog;
		string img;
	}

	public class UpcomingUnlock
	{
		string type;
		string name;
		string subname;
		string image;
		string subimage;
		int needed;
		string format;
		string nname;
		int curr;
		int prog;
		int left;
	}

	Player player;
	Stats stats;
	Dogtags dogtags;
	Weapon[] weapons;
	WeaponCategory[] weaponCategory;
	Kititem[] kititems;
	Vehicles[] vehicles;
	VehicleCategory[] vehicleCategory;
	Award[] awards;
	Assignment[] assignments;
	UpcomingUnlock[] upcomingUnlocks;
}
