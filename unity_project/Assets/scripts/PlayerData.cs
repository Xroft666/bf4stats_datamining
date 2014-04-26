
public class PlayerData
{
	public class Player 
	{
		public class Rank
		{
			public class NextRank
			{
				public int nr;
				public string img;
				public string name;
				public int needed;
				public int curr;
				public int relNeeded;
				public int relCurr;
				public float relProg;
			}
			
			public int nr;
			public string imgLarge;
			public string img;
			public string name;
			public int needed;
			public NextRank next;
		}
	
		public string id;
		public string game;
		public string plat;
		public string name;
		public string tag;
		public string dateCheck;
		public string dateUpdate;
		public string dateCreate;
		public string lastDay;
		public string country;
		public string countryName;
		public Rank rank;
		public int score;
		public int timePlayed;
		public string uId;
		public string uName;
		public string uGava;
		public string udCreate;
		public string blPlayer;
		public string blUser; 
		public bool editable;
		public bool viewable;
		public bool adminable;
		public bool linked;
	}
	
	
	
	public class Stats
	{
		public class Reset
		{
			public int lastReset;
			public int score;
			public int timePlayed;
			public int timePlayedSinceLastReset;
			public int kills;
			public int deaths;
			public int shotsFired;
			public int shotsHit;
			public int numLosses;
			public int numWins;
		}
		
		public class Scores
		{
			public int score;
			public int award;
			public int bonus;
			public int unlock;
			public int vehicle;
			public int team;
			public int objective;
			public int squad;
			public int general;
			public int totalScore;
			public int rankScore;
			public int combatScore;
		}
		
		public class Mode
		{
			public string id;
			public int score;
			public string name;
		}
		
		public class Kits
		{
			public Kit assault;
			public Kit engineer;
			public Kit support;
			public Kit recon;
			public Kit commander;
		}
		
		public class Kit
		{
			public string id;
			public int score;
			public int time;
			public int stars;
			public float spm;
			public string name;
		}
		
		public class Extra
		{
			public float kdr;
			public float wlr;
			public float spm;
			public float gspm;
			public float kpm;
			public float sfpm;
			public float hkp;
			public float khp;
			public float accuracy;
			public int roundsFinished;
			public int vehicleTime;
			public int vehicleKills;
			public int weaponTime;
			public int weaponKills;
			public float weaTimeP;
			public float weaKillsP;
			public float weaKpm;
			public float vehTimeP;
			public float vehKillsP;
			public float vehKpm;
			public int ribbons;
			public int ribbonsTotal;
			public int ribbonsUnique;
			public int medals;
			public int medalsTotal;
			public int medalsUnique;
			public int assignments;
			public int assignmentsTotal;
			public float ribpr;
		}
	
		public Reset reset;
		public Scores scores;
		public int skill;
		public int elo;
		public int rank;
		public int timePlayed;
		public int kills;
		public int deaths;
		public int headshots;
		public int shotsFired;
		public int shotsHit;
		public int suppressionAssists;
		public int avengerKills;
		public int saviorKills;
		public int nemesisKills;
		public int numRounds;
		public int numLosses;
		public int numWins;
		public int killStreakBonus;
		public int nemesisStreak;
		public int mcomDefendKills;
		public int resupplies;
		public int repairs;
		public int heals;
		public int revives;
		public float longestHeadshot;
		public int longestWinStreak;
		public int flagDefend;
		public int flagCaptures;
		public int killAssists;
		public int vehiclesDestroyed;
		public int vehicleDamage;
		public int dogtagsTaken;
		public Mode[] modes;
		public Kits kits;
		public Extra extra;
	}
	
	
	
	public class Dogtags
	{
		public class Dogtag
		{
			public string id;
			public string image;
			public string name;
			public string desc;
			public string license;
			public string category;
			public string img;
		}
	
		public Dogtag advanced;
		public Dogtag basic;
	}
	
	
	
	public class Weapon
	{
		public class Stat
		{
			public string id;
			public int shots;
			public int hits;
			public int hs;
			public int kills;
			public int time;
		}
		
		public class Detail
		{
			public class WeaponData
			{
				public float statHandling;
				public bool fireModeBurst;
				public float statDamage;
				public float statMobility;
				public bool fireModeSingle;
				public bool fireModeAuto;
				public float statRange;
				public string range;
				public string rateOfFire;
				public string altAmmoName;
				public string ammoType;
				public float statAccuracy;
				public string ammo;
			}
	
			public class Unlock
			{
				public string code;
				public string name;
				public int needed;
			}
	
			public string id;
			public string category;
			public string name;
			public string type;
			public string desc;
			public string image;
			public WeaponData weaponData;
			public string code;
			public string[] kits;
			public string unlockLicense;
			public Unlock unlock;
		}
	
		public class Extra
		{
			public float kpm;
			public float sfpm;
			public float hkp;
			public float accuracy;
		}
	
		public class Stars
		{
			public int count;
			public int curr;
			public int needed;
			public int relNeeded;
			public int relCurr;
			public float relProg;
		}
	
		public class Unlock
		{
			public string id;
			public string category;
			public string name;
			public string desc;
			public string rareness;
			public string image;
			public string type;
			public int needed;
			public string license;
		}
	
		public Stat stat;
		public string name;
		public Detail detail;
		public Extra extra;
		public Stars stars;
		public string imgFancy;
		public string imgLineart;
		public Unlock[] unlocks;
		public int unlocksTotal;
		public int unlocksCurr;
	}

	public class WeaponCategory
	{
		public class Stat
		{
			public int score;
			public int time;
			public int kills;
			public int hs;
			public int hits;
			public int shots;
		}

		public class Extra
		{
			public float kpm;
			public float spm;
			public float sfpm;
			public float hkp;
			public float accuracy;
		}

		public string name;
		public Stat stat;
		public Extra extra;
	}

	public class Kititem
	{
		public class Stat
		{
			public string id;
			public int prog;
		}

		public class Detail
		{
			public class Unlocks
			{
				public string code;
				public string name;
				public int needed;
			}

			public class Stat
			{
				public string kills;
				public string repairs;
				public string revives;
				public string heals;
				public string shots;
				public string spawns;
				public string spotAssists;
				public string damageAssists;
				public string resupplies;
			}

			public string id;
			public string name;
			public string category;
			public string desc;
			public string image;
			public string kit;
			public Unlocks unlock;
			public Stat stat;
			public string ranked; 
		}

		public Stat stat;
		public string name;
		public Detail detail;
		public string imgFancy;
		public string imgLineart;
	}

	public class Vehicles
	{
		public class Stat
		{
			public string id;
			public int destroys;
			public int kills;
			public int time;
		}

		public class Detail
		{
			public string id;
			public string name;
			public string category;
			public string image;
		}

		public class Extra
		{
			public float kpm;
		}

		public Stat stat;
		public string name;
		public Detail detail;
		public Extra extra;
		public string imgFancy;
		public string imgLineart;
	}

	public class VehicleCategory
	{
		public class Stat 
		{
			public int destroys;
			public int time;
			public int kills;
			public int score;
		}

		public class Unlock
		{
			public string id;
			public string category;
			public string name;
			public string desc;
			public string image;
			public string type;
			public int needed;
			public string license;
		}

		public class Extra
		{
			public float kpm;
			public float spm;
		}

		public string id;
		public string name;
		public Stat stat;
		public Unlock[] unlocks;
		public int unlocksTotal;
		public int unlocksCurr;
		public Extra extra;
	}

	public class Award
	{
		public class Medal
		{
			public string id;
			public string name;
			public string desc;
			public string image;
			public string category;
			public string ribbon;
			public int needed;
		}

		public class Ribbon
		{
			public string id;
			public string name;
			public string desc;
			public string image;
		}

		public string id;
		public int mCount;
		public int rCount;
		public string mName;
		public string rName;
		public Medal medal;
		public Ribbon ribbon;
		public int curr;
		public int needed;
		public int relNeeded;
		public int relCurr;
		public float relProg;
		public string imgMedal;
		public string imgRibbon;
	}

	public class Assignment
	{
		public class Dependency
		{
			public string group;
			public string realm;
			public string code;
			public string name;
		}

		public class Criteria
		{
			public string desc;
			public int needed;
			public int curr;
			public float prog;
		}

		public string id;
		public string image;
		public string name;
		public string category;
		public string desc;
		public Dependency[] dependencies;
		public Criteria[] criterias;
		public float curr;
		public int needed;
		public float prog;
		public string img;
	}

	public class UpcomingUnlock
	{
		public string type;
		public string name;
		public string subname;
		public string image;
		public string subimage;
		public int needed;
		public string format;
		public string nname;
		public int curr;
		public float prog;
		public int left;
	}

	public Player player;
	public Stats stats;
	public Dogtags dogtags;
	public Weapon[] weapons;
	public WeaponCategory[] weaponCategory;
	public Kititem[] kititems;
	public Vehicles[] vehicles;
	public VehicleCategory[] vehicleCategory;
	public Award[] awards;
	public Assignment[] assignments;
	public UpcomingUnlock[] upcomingUnlocks;
}
