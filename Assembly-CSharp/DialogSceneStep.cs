using System;
using System.Collections.Generic;

// Token: 0x020000C1 RID: 193
[Serializable]
public class DialogSceneStep : SubDefinition
{
	// Token: 0x040004B9 RID: 1209
	public int editorResponseOptionIndex;

	// Token: 0x040004BA RID: 1210
	public DialogSceneStepType type;

	// Token: 0x040004BB RID: 1211
	public LocationDefinition locationDefinition;

	// Token: 0x040004BC RID: 1212
	public GirlDefinition girlDefinition;

	// Token: 0x040004BD RID: 1213
	public AudioDefinition soundEffect;

	// Token: 0x040004BE RID: 1214
	public DialogSceneLine sceneLine = new DialogSceneLine();

	// Token: 0x040004BF RID: 1215
	public bool preventOptionShuffle;

	// Token: 0x040004C0 RID: 1216
	public bool hasBestOption;

	// Token: 0x040004C1 RID: 1217
	public List<DialogSceneResponseOption> responseOptions = new List<DialogSceneResponseOption>();

	// Token: 0x040004C2 RID: 1218
	public bool hasBestBranch;

	// Token: 0x040004C3 RID: 1219
	public List<DialogSceneConditionalBranch> conditionalBranchs = new List<DialogSceneConditionalBranch>();

	// Token: 0x040004C4 RID: 1220
	public string showGirlStyles;

	// Token: 0x040004C5 RID: 1221
	public bool hideOppositeSpeechBubble;

	// Token: 0x040004C6 RID: 1222
	public GirlDefinition altGirl;

	// Token: 0x040004C7 RID: 1223
	public DialogSceneDefinition insertScene;

	// Token: 0x040004C8 RID: 1224
	public int waitTime;

	// Token: 0x040004C9 RID: 1225
	public GirlMetStatus metStatus;

	// Token: 0x040004CA RID: 1226
	public DialogTriggerDefinition dialogTrigger;

	// Token: 0x040004CB RID: 1227
	public int dialogTriggerIndex;

	// Token: 0x040004CC RID: 1228
	public GirlDetailType girlDetailType;

	// Token: 0x040004CD RID: 1229
	public int stepBackSteps;

	// Token: 0x040004CE RID: 1230
	public ItemDefinition itemDefinition;

	// Token: 0x040004CF RID: 1231
	public bool toEquipment;

	// Token: 0x040004D0 RID: 1232
	public bool wrapped;

	// Token: 0x040004D1 RID: 1233
	public PuzzleTokenDefinition tokenDefinition;

	// Token: 0x040004D2 RID: 1234
	public string gridKey;

	// Token: 0x040004D3 RID: 1235
	public int tokenCount;

	// Token: 0x040004D4 RID: 1236
	public ParticleEmitter2DDefinition particleEmitterDefinition;

	// Token: 0x040004D5 RID: 1237
	public SpriteGroupDefinition spriteGroupDefinition;

	// Token: 0x040004D6 RID: 1238
	public float xPos;

	// Token: 0x040004D7 RID: 1239
	public float yPos;

	// Token: 0x040004D8 RID: 1240
	public MessageDefinition messageDef;
}
