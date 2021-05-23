using System;
using UnityEngine;

// Token: 0x020000F1 RID: 241
public class GameData
{
	// Token: 0x060004E6 RID: 1254 RVA: 0x00025924 File Offset: 0x00023B24
	public GameData()
	{
		Resources.LoadAll("GameData");
		this._locationData = new LocationData();
		this._girlData = new GirlData();
		this._itemData = new ItemData();
		this._puzzleTokenData = new PuzzleTokenData();
		this._particleEmitter2DData = new ParticleEmitter2DData();
		this._spriteGroupData = new SpriteGroupData();
		this._energyTrailData = new EnergyTrailData();
		this._abilityData = new AbilityData();
		this._traitData = new TraitData();
		this._actionMenuItemData = new ActionMenuItemData();
		this._cellAppData = new CellAppData();
		this._dialogTriggers = new DialogTriggerData();
		this._dialogSceneData = new DialogSceneData();
		this._messageData = new MessageData();
		this._debugProfileData = new DebugProfileData();
	}

	// Token: 0x17000028 RID: 40
	// (get) Token: 0x060004E7 RID: 1255 RVA: 0x00005D67 File Offset: 0x00003F67
	public LocationData Locations
	{
		get
		{
			return this._locationData;
		}
	}

	// Token: 0x17000029 RID: 41
	// (get) Token: 0x060004E8 RID: 1256 RVA: 0x00005D6F File Offset: 0x00003F6F
	public GirlData Girls
	{
		get
		{
			return this._girlData;
		}
	}

	// Token: 0x1700002A RID: 42
	// (get) Token: 0x060004E9 RID: 1257 RVA: 0x00005D77 File Offset: 0x00003F77
	public ItemData Items
	{
		get
		{
			return this._itemData;
		}
	}

	// Token: 0x1700002B RID: 43
	// (get) Token: 0x060004EA RID: 1258 RVA: 0x00005D7F File Offset: 0x00003F7F
	public PuzzleTokenData PuzzleTokens
	{
		get
		{
			return this._puzzleTokenData;
		}
	}

	// Token: 0x1700002C RID: 44
	// (get) Token: 0x060004EB RID: 1259 RVA: 0x00005D87 File Offset: 0x00003F87
	public ParticleEmitter2DData Particles
	{
		get
		{
			return this._particleEmitter2DData;
		}
	}

	// Token: 0x1700002D RID: 45
	// (get) Token: 0x060004EC RID: 1260 RVA: 0x00005D8F File Offset: 0x00003F8F
	public SpriteGroupData SpriteGroups
	{
		get
		{
			return this._spriteGroupData;
		}
	}

	// Token: 0x1700002E RID: 46
	// (get) Token: 0x060004ED RID: 1261 RVA: 0x00005D97 File Offset: 0x00003F97
	public EnergyTrailData EnergyTrails
	{
		get
		{
			return this._energyTrailData;
		}
	}

	// Token: 0x1700002F RID: 47
	// (get) Token: 0x060004EE RID: 1262 RVA: 0x00005D9F File Offset: 0x00003F9F
	public AbilityData Abilities
	{
		get
		{
			return this._abilityData;
		}
	}

	// Token: 0x17000030 RID: 48
	// (get) Token: 0x060004EF RID: 1263 RVA: 0x00005DA7 File Offset: 0x00003FA7
	public TraitData Traits
	{
		get
		{
			return this._traitData;
		}
	}

	// Token: 0x17000031 RID: 49
	// (get) Token: 0x060004F0 RID: 1264 RVA: 0x00005DAF File Offset: 0x00003FAF
	public ActionMenuItemData ActionMenuItems
	{
		get
		{
			return this._actionMenuItemData;
		}
	}

	// Token: 0x17000032 RID: 50
	// (get) Token: 0x060004F1 RID: 1265 RVA: 0x00005DB7 File Offset: 0x00003FB7
	public CellAppData CellApps
	{
		get
		{
			return this._cellAppData;
		}
	}

	// Token: 0x17000033 RID: 51
	// (get) Token: 0x060004F2 RID: 1266 RVA: 0x00005DBF File Offset: 0x00003FBF
	public DialogTriggerData DialogTriggers
	{
		get
		{
			return this._dialogTriggers;
		}
	}

	// Token: 0x17000034 RID: 52
	// (get) Token: 0x060004F3 RID: 1267 RVA: 0x00005DC7 File Offset: 0x00003FC7
	public DialogSceneData DialogScenes
	{
		get
		{
			return this._dialogSceneData;
		}
	}

	// Token: 0x17000035 RID: 53
	// (get) Token: 0x060004F4 RID: 1268 RVA: 0x00005DCF File Offset: 0x00003FCF
	public MessageData Messages
	{
		get
		{
			return this._messageData;
		}
	}

	// Token: 0x17000036 RID: 54
	// (get) Token: 0x060004F5 RID: 1269 RVA: 0x00005DD7 File Offset: 0x00003FD7
	public DebugProfileData DebugProfiles
	{
		get
		{
			return this._debugProfileData;
		}
	}

	// Token: 0x04000675 RID: 1653
	private LocationData _locationData;

	// Token: 0x04000676 RID: 1654
	private GirlData _girlData;

	// Token: 0x04000677 RID: 1655
	private ItemData _itemData;

	// Token: 0x04000678 RID: 1656
	private PuzzleTokenData _puzzleTokenData;

	// Token: 0x04000679 RID: 1657
	private ParticleEmitter2DData _particleEmitter2DData;

	// Token: 0x0400067A RID: 1658
	private SpriteGroupData _spriteGroupData;

	// Token: 0x0400067B RID: 1659
	private EnergyTrailData _energyTrailData;

	// Token: 0x0400067C RID: 1660
	private AbilityData _abilityData;

	// Token: 0x0400067D RID: 1661
	private TraitData _traitData;

	// Token: 0x0400067E RID: 1662
	private ActionMenuItemData _actionMenuItemData;

	// Token: 0x0400067F RID: 1663
	private CellAppData _cellAppData;

	// Token: 0x04000680 RID: 1664
	private DialogTriggerData _dialogTriggers;

	// Token: 0x04000681 RID: 1665
	private DialogSceneData _dialogSceneData;

	// Token: 0x04000682 RID: 1666
	private MessageData _messageData;

	// Token: 0x04000683 RID: 1667
	private DebugProfileData _debugProfileData;
}
