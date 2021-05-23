using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x02000018 RID: 24
public class Girl : DisplayObject
{
	// Token: 0x1400000A RID: 10
	// (add) Token: 0x060000EE RID: 238 RVA: 0x0000325D File Offset: 0x0000145D
	// (remove) Token: 0x060000EF RID: 239 RVA: 0x00003276 File Offset: 0x00001476
	public event Girl.DialogLineDelegate DialogLineBeginEvent;

	// Token: 0x1400000B RID: 11
	// (add) Token: 0x060000F0 RID: 240 RVA: 0x0000328F File Offset: 0x0000148F
	// (remove) Token: 0x060000F1 RID: 241 RVA: 0x000032A8 File Offset: 0x000014A8
	public event Girl.DialogLineDelegate DialogLineReadEvent;

	// Token: 0x17000017 RID: 23
	// (get) Token: 0x060000F2 RID: 242 RVA: 0x0000EE84 File Offset: 0x0000D084
	public DisplayObject girlSpeechBubble
	{
		get
		{
			if (GameManager.System.Location.currentLocation == null || GameManager.System.Location.currentLocation.type == LocationType.NORMAL)
			{
				return this._speechBubbleNormal;
			}
			return this._speechBubbleDate;
		}
	}

	// Token: 0x17000018 RID: 24
	// (get) Token: 0x060000F3 RID: 243 RVA: 0x0000EED4 File Offset: 0x0000D0D4
	public SpriteObject dialogBackground
	{
		get
		{
			if (GameManager.System.Location.currentLocation == null || GameManager.System.Location.currentLocation.type == LocationType.NORMAL)
			{
				return this._dialogBackgroundNormal;
			}
			return this._dialogBackgroundDate;
		}
	}

	// Token: 0x17000019 RID: 25
	// (get) Token: 0x060000F4 RID: 244 RVA: 0x0000EF24 File Offset: 0x0000D124
	public LabelObject dialogLabel
	{
		get
		{
			if (GameManager.System.Location.currentLocation == null || GameManager.System.Location.currentLocation.type == LocationType.NORMAL)
			{
				return this._dialogLabelNormal;
			}
			return this._dialogLabelDate;
		}
	}

	// Token: 0x1700001A RID: 26
	// (get) Token: 0x060000F5 RID: 245 RVA: 0x0000EF74 File Offset: 0x0000D174
	public SpriteObject dialogPrompt
	{
		get
		{
			if (GameManager.System.Location.currentLocation == null || GameManager.System.Location.currentLocation.type == LocationType.NORMAL)
			{
				return this._dialogPromptNormal;
			}
			return this._dialogPromptDate;
		}
	}

	// Token: 0x060000F6 RID: 246 RVA: 0x0000EFC4 File Offset: 0x0000D1C4
	protected override void OnStart()
	{
		base.OnStart();
		this.girlContainer = base.GetChildByName("GirlContainer");
		this.girlPieceContainers = this.girlContainer.GetChildByName("GirlPieceContainers");
		this.backhair = this.girlPieceContainers.GetChildByName("GirlBackhair");
		this.body = this.girlPieceContainers.GetChildByName("GirlBody");
		this.bra = this.girlPieceContainers.GetChildByName("GirlBra");
		this.panties = this.girlPieceContainers.GetChildByName("GirlPanties");
		this.footwear = this.girlPieceContainers.GetChildByName("GirlFootwear");
		this.outfit = this.girlPieceContainers.GetChildByName("GirlOutfit");
		this.head = this.girlPieceContainers.GetChildByName("GirlHead");
		this.face = this.girlPieceContainers.GetChildByName("GirlFace");
		this.mouth = this.girlPieceContainers.GetChildByName("GirlMouth");
		this.eyes = this.girlPieceContainers.GetChildByName("GirlEyes");
		this.fronthair = this.girlPieceContainers.GetChildByName("GirlFronthair");
		this.eyebrows = this.girlPieceContainers.GetChildByName("GirlEyebrows");
		this.extraOne = this.girlPieceContainers.GetChildByName("GirlExtraOne");
		this.extraTwo = this.girlPieceContainers.GetChildByName("GirlExtraTwo");
		this._containers.Add(this.backhair);
		this._containers.Add(this.body);
		this._containers.Add(this.bra);
		this._containers.Add(this.panties);
		this._containers.Add(this.footwear);
		this._containers.Add(this.outfit);
		this._containers.Add(this.head);
		this._containers.Add(this.face);
		this._containers.Add(this.mouth);
		this._containers.Add(this.eyes);
		this._containers.Add(this.fronthair);
		this._containers.Add(this.eyebrows);
		this._containers.Add(this.extraOne);
		this._containers.Add(this.extraTwo);
		this._speechBubbleNormal = base.GetChildByName("GirlSpeechBubbleNormal");
		this._dialogBackgroundNormal = (this._speechBubbleNormal.GetChildByName("GirlSpeechBubbleBackground") as SpriteObject);
		this._dialogLabelNormal = (this._speechBubbleNormal.GetChildByName("GirlSpeechBubbleLabel") as LabelObject);
		this._dialogPromptNormal = (base.GetChildByName("GirlSpeechBubbleNormalPrompt") as SpriteObject);
		this._speechBubbleDate = base.GetChildByName("GirlSpeechBubbleDate");
		this._dialogBackgroundDate = (this._speechBubbleDate.GetChildByName("GirlSpeechBubbleBackground") as SpriteObject);
		this._dialogLabelDate = (this._speechBubbleDate.GetChildByName("GirlSpeechBubbleLabel") as LabelObject);
		this._dialogPromptDate = (base.GetChildByName("GirlSpeechBubbleDatePrompt") as SpriteObject);
		this._speechBubbleNormal.childrenAlpha = 0f;
		this._speechBubbleNormal.gameObj.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
		this._speechBubbleDate.childrenAlpha = 0f;
		this._speechBubbleDate.gameObj.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
		this._alphaNumericRegex = new Regex("^[a-zA-Z0-9]*$");
		this.ClearGirl();
		this.ClearDialog();
	}

	// Token: 0x060000F7 RID: 247 RVA: 0x0000F368 File Offset: 0x0000D568
	protected override void OnUpdate()
	{
		base.OnUpdate();
		if (this.definition == null || this.paused)
		{
			return;
		}
		if (this.definition.blinkHalfPiece.spriteName != string.Empty && this.definition.blinkFullPiece.spriteName != string.Empty && GameManager.System.Lifetime(true) - this._blinkTimestamp >= this._blinkDelay)
		{
			if (this._blinkStage < 3)
			{
				this._blinkStage = 3;
				this._blinkTimestamp = GameManager.System.Lifetime(true);
				this._blinkDelay = 0.1f;
				if (this._eyesExpression.expressionType != GirlExpressionType.EXCITED && !this._eyesClosed && !this._tempExpressionActive)
				{
					this.AddGirlPieceArtToContainer(this.definition.blinkFullPiece, this.GetContainerByLayer(GirlLayer.EYES));
				}
			}
			else if (this._blinkStage == 3)
			{
				this._blinkStage = 0;
				this._blinkTimestamp = GameManager.System.Lifetime(true);
				this._blinkDelay = UnityEngine.Random.Range(0.5f, 4f);
				if (!this._eyesClosed && !this._tempExpressionActive)
				{
					this.AddGirlPieceArtToContainer(this._eyesExpression.primaryArt, this.GetContainerByLayer(GirlLayer.EYES));
				}
			}
		}
		if (this._resetFace && (GameManager.System.Lifetime(true) - this._resetFaceTimestamp >= this._resetFaceDelay || this._resetFaceDelay == 0f))
		{
			this.ResetExpression();
		}
		if (this._currentDialogLine != null)
		{
			if (this._dialogListenForSkip)
			{
				this._dialogListenForSkip = false;
				GameManager.Stage.MouseUpEvent += this.OnDialogTextSkip;
			}
			int num = Mathf.Clamp((int)Mathf.Round((float)this._currentDialogLine.GetText().Length * this.dialogReadPercent), 0, this._currentDialogLine.GetText().Length - 1);
			string text = this._currentDialogLineSpeechText.Substring(0, num) + "^C00000000" + this._currentDialogLineSpeechText.Substring(num);
			this.dialogLabel.SetText(text);
			if (this._currentDialogLine.expressions.Count > this._dialogLineExpressionIndex + 1)
			{
				DialogLineExpression dialogLineExpression = this._currentDialogLine.expressions[this._dialogLineExpressionIndex + 1];
				if (num >= dialogLineExpression.startAtCharIndex)
				{
					this.ChangeExpression(dialogLineExpression.expression, true, dialogLineExpression.changeEyes, dialogLineExpression.changeMouth, 0f);
					if (dialogLineExpression.closeEyes)
					{
						this.AddGirlPieceArtToContainer(this.definition.blinkFullPiece, this.GetContainerByLayer(GirlLayer.EYES));
						this._eyesClosed = true;
					}
					this._dialogLineExpressionIndex++;
				}
			}
			if (GameManager.System.Lifetime(true) - this._dialogMouthTimestamp >= 0.064f || this._dialogMouthTimestamp == 0f)
			{
				this.MouthLetter(this._currentDialogLine.GetText().Substring(num, 1));
				this._dialogMouthTimestamp = GameManager.System.Lifetime(true);
				if (GameManager.System.settingsVoice == SettingsVoice.BOOPS && this._alphaNumericRegex.IsMatch(this._currentDialogLineSpeechText[num].ToString()))
				{
					GameManager.System.Audio.Play(AudioCategory.VOICE_ALT, GameManager.Stage.uiGirl.girlDialogBoopSounds, -1, false, 1f, true);
				}
			}
		}
		if (GameManager.System.Location.IsLocationSettled() && (GameManager.System.Lifetime(true) - this._panTimestamp >= 0.01f || this._panTimestamp == 0f) && ((!this.flip && GameManager.System.Dialog.IsMainGirlShowing()) || (this.flip && GameManager.System.Dialog.IsAltGirlShowing())))
		{
			float num2 = Mathf.Lerp(15f, -15f, GameManager.System.Cursor.GetMousePosition().x / 1200f);
			this.girlPieceContainers.localX += (num2 - this.girlPieceContainers.localX) / 40f;
			this._panTimestamp = GameManager.System.Lifetime(true);
		}
	}

	// Token: 0x060000F8 RID: 248 RVA: 0x0000F7D4 File Offset: 0x0000D9D4
	public void ShowGirl(GirlDefinition girlDefinition)
	{
		this.ClearGirl();
		this.definition = girlDefinition;
		this.spriteCollection = (Resources.Load("SpriteCollections/Girls/" + this.definition.spriteCollectionName, typeof(tk2dSpriteCollectionData)) as tk2dSpriteCollectionData);
		GirlPlayerData girlData = GameManager.System.Player.GetGirlData(this.definition);
		this._eyesExpression = this.definition.pieces[this.definition.defaultExpression];
		this._eyebrowsExpression = this.definition.pieces[this.definition.defaultExpression];
		this._mouthExpression = this.definition.pieces[this.definition.defaultExpression];
		this._faceExpression = this.definition.pieces[this.definition.defaultExpression];
		this._blinkStage = 0;
		this._blinkTimestamp = GameManager.System.Lifetime(true);
		this._blinkDelay = UnityEngine.Random.Range(0.5f, 4f);
		for (int i = 0; i < this.definition.pieces.Count; i++)
		{
			GirlPiece girlPiece = this.definition.pieces[i];
			if (girlPiece.type == GirlPieceType.PHONEME && girlPiece.name != null && !(girlPiece.name == string.Empty))
			{
				string[] array = girlPiece.name.Split(new char[]
				{
					','
				});
				for (int j = 0; j < array.Length; j++)
				{
					this._letterMouths.Add(array[j], girlPiece.primaryArt);
				}
			}
		}
		this.AddGirlPieceArtToContainer(this.definition.headPiece, this.GetContainerByLayer(GirlLayer.HEAD));
		this.AddGirlPieceArtToContainer(this.definition.bodyPiece, this.GetContainerByLayer(GirlLayer.BODY));
		if (GameManager.System.Location.currentLocation.type != LocationType.DATE || !GameManager.System.Location.currentLocation.bonusRoundLocation)
		{
			this.AddGirlPiece(this.definition.pieces[this.definition.defaultExpression]);
		}
		else
		{
			this.AddGirlPiece(this.definition.pieces[this.definition.bonusRoundExpression]);
		}
		if (GameManager.System.Location.currentLocation.type != LocationType.DATE || !GameManager.System.Location.currentLocation.bonusRoundLocation)
		{
			int artIndex;
			if (girlData.hairstyle < this.definition.hairstyles.Count)
			{
				artIndex = this.definition.hairstyles[girlData.hairstyle].artIndex;
			}
			else
			{
				artIndex = this.definition.hairstyles[girlData.GetRandomUnlockedHairstyle()].artIndex;
			}
			this.AddGirlPiece(this.definition.pieces[artIndex]);
		}
		else
		{
			this.AddGirlPiece(this.definition.pieces[this.definition.bonusRoundHairstyle]);
		}
		int index = 0;
		if (GameManager.System.Location.currentLocation.type != LocationType.DATE || !GameManager.System.Location.currentLocation.bonusRoundLocation)
		{
			if (girlData.outfit < this.definition.outfits.Count)
			{
				index = this.definition.outfits[girlData.outfit].artIndex;
			}
			else
			{
				index = this.definition.outfits[girlData.GetRandomUnlockedOutfit()].artIndex;
			}
			if (GameManager.System.Location.currentLocation.type == LocationType.NORMAL)
			{
				if (girlData.metStatus == GirlMetStatus.MET && GameManager.System.Location.currentLocation.outfitOverride > 0)
				{
					index = GameManager.System.Location.currentLocation.outfitOverride;
				}
			}
			else if (GameManager.System.Player.tutorialComplete)
			{
				for (int k = 0; k < this.definition.dateLocations.Count; k++)
				{
					if (this.definition.dateLocations[k].location == GameManager.System.Location.currentLocation)
					{
						index = this.definition.outfits[this.definition.dateLocations[k].outfit].artIndex;
						break;
					}
				}
			}
			this.AddGirlPiece(this.definition.pieces[index]);
			this.AddGirlPiece(this.definition.pieces[18]);
		}
		else
		{
			this.AddGirlPieceArtToContainer(this.definition.braPiece, this.GetContainerByLayer(GirlLayer.BRA));
			this.AddGirlPieceArtToContainer(this.definition.pantiesPiece, this.GetContainerByLayer(GirlLayer.PANTIES));
		}
		List<GirlPiece> piecesByType = this.definition.GetPiecesByType(GirlPieceType.EXTRA);
		int l = 0;
		while (l < piecesByType.Count)
		{
			if (l > 1)
			{
				break;
			}
			GirlPiece girlPiece2 = piecesByType[l];
			if (GameManager.System.Location.currentLocation.type == LocationType.DATE && GameManager.System.Location.currentLocation.bonusRoundLocation)
			{
				goto IL_5D6;
			}
			if (UnityEngine.Random.Range(0f, 1f) <= girlPiece2.showChance)
			{
				if (StringUtils.IsEmpty(girlPiece2.limitToOutfits))
				{
					goto IL_5D6;
				}
				List<string> list = new List<string>(girlPiece2.limitToOutfits.Split(new char[]
				{
					','
				}));
				if (list.Contains(index.ToString()))
				{
					goto IL_5D6;
				}
			}
			IL_692:
			l++;
			continue;
			IL_5D6:
			if (GameManager.System.Location.currentLocation.type == LocationType.DATE && !GameManager.System.Location.currentLocation.bonusRoundLocation && girlPiece2.hideOnDates)
			{
				goto IL_692;
			}
			if (GameManager.System.Location.currentLocation.type == LocationType.DATE && GameManager.System.Location.currentLocation.bonusRoundLocation && !girlPiece2.underwear)
			{
				goto IL_692;
			}
			DisplayObject containerByPiece = this.GetContainerByPiece(girlPiece2, l);
			this.AddGirlPieceArtToContainer(girlPiece2.primaryArt, containerByPiece);
			containerByPiece.SetOwnChildIndex(this.GetContainerByLayer(girlPiece2.layer).childIndex);
			goto IL_692;
		}
		if (!GameManager.System.Player.tutorialComplete && GameManager.System.Player.tutorialStep < 0 && this.definition == GameManager.Stage.uiGirl.fairyGirlDef)
		{
			this.backhair.RemoveAllChildren(true);
			this.fronthair.RemoveAllChildren(true);
			this.AddGirlPieceArtToContainer(this.definition.pieces[16].primaryArt, this.outfit);
			this.AddGirlPieceArtToContainer(this.definition.pieces[30].primaryArt, this.extraOne);
			this.AddGirlPieceArtToContainer(this.definition.pieces[29].primaryArt, this.extraTwo);
			this.extraOne.SetOwnChildIndex(this.GetContainerByLayer(this.definition.pieces[30].layer).childIndex);
			this.extraTwo.SetOwnChildIndex(this.GetContainerByLayer(this.definition.pieces[29].layer).childIndex);
		}
	}

	// Token: 0x060000F9 RID: 249 RVA: 0x0000FFAC File Offset: 0x0000E1AC
	public void ClearGirl()
	{
		TweenUtils.KillTweener(this._fadeBraTweener, true);
		this._fadeBraTweener = null;
		this.definition = null;
		this.spriteCollection = null;
		base.localX = 0f;
		this.ClearAllContainers();
		this.extraOne.ShiftSelfToTop();
		this.extraTwo.ShiftSelfToTop();
		this._letterMouths.Clear();
		this._eyesExpression = null;
		this._eyebrowsExpression = null;
		this._mouthExpression = null;
		this._faceExpression = null;
		this._tempExpressionActive = false;
		this._eyesClosed = false;
		this._resetFace = false;
		this._resetFaceTimestamp = 0f;
		this._resetFaceDelay = 0f;
		this._blinkStage = 0;
		this._blinkTimestamp = 0f;
		this._blinkDelay = 0f;
		this._panTimestamp = 0f;
		this.ClearDialog();
	}

	// Token: 0x060000FA RID: 250 RVA: 0x00010084 File Offset: 0x0000E284
	private void AddGirlPiece(GirlPiece girlPiece)
	{
		switch (girlPiece.type)
		{
		case GirlPieceType.EXPRESSION:
			if (this._blinkStage == 0)
			{
				this.AddGirlPieceArtToContainer(girlPiece.primaryArt, this.GetContainerByPiece(girlPiece, 0));
			}
			this.AddGirlPieceArtToContainer(girlPiece.secondaryArt, this.GetContainerByPiece(girlPiece, 1));
			this.AddGirlPieceArtToContainer(girlPiece.tertiaryArt, this.GetContainerByPiece(girlPiece, 2));
			this.AddGirlPieceArtToContainer(girlPiece.quaternaryArt, this.GetContainerByPiece(girlPiece, 3));
			break;
		case GirlPieceType.HAIRSTYLE:
			this.AddGirlPieceArtToContainer(girlPiece.primaryArt, this.GetContainerByPiece(girlPiece, 0));
			this.AddGirlPieceArtToContainer(girlPiece.secondaryArt, this.GetContainerByPiece(girlPiece, 1));
			break;
		case GirlPieceType.OUTFIT:
		case GirlPieceType.FOOTWEAR:
			this.AddGirlPieceArtToContainer(girlPiece.primaryArt, this.GetContainerByPiece(girlPiece, 0));
			break;
		case GirlPieceType.EXTRA:
			this.AddGirlPieceArtToContainer(girlPiece.primaryArt, this.GetContainerByPiece(girlPiece, 0));
			break;
		}
	}

	// Token: 0x060000FB RID: 251 RVA: 0x00010180 File Offset: 0x0000E380
	private void AddGirlPieceArtToContainer(GirlPieceArt girlPieceArt, DisplayObject container)
	{
		DisplayObject[] children = container.GetChildren(false);
		if (children.Length > 0 && girlPieceArt.spriteName == (container.GetChildren(false)[0] as SpriteObject).sprite.GetCurrentSpriteDef().name)
		{
			return;
		}
		container.RemoveAllChildren(true);
		if (girlPieceArt == null || girlPieceArt.spriteName == null || girlPieceArt.spriteName == string.Empty)
		{
			return;
		}
		SpriteObject spriteObject = DisplayUtils.CreateSpriteObject(this.spriteCollection, girlPieceArt.spriteName, "SpriteObject");
		container.AddChild(spriteObject);
		if (this.flip)
		{
			spriteObject.sprite.FlipX = true;
			spriteObject.SetLocalPosition((float)(1200 - girlPieceArt.x), (float)(-(float)girlPieceArt.y));
		}
		else
		{
			spriteObject.SetLocalPosition((float)girlPieceArt.x, (float)(-(float)girlPieceArt.y));
		}
	}

	// Token: 0x060000FC RID: 252 RVA: 0x00010264 File Offset: 0x0000E464
	private void ClearAllContainers()
	{
		for (int i = 0; i < this._containers.Count; i++)
		{
			this._containers[i].RemoveAllChildren(true);
		}
	}

	// Token: 0x060000FD RID: 253 RVA: 0x000102A0 File Offset: 0x0000E4A0
	private DisplayObject GetContainerByPiece(GirlPiece girlPiece, int artworkOffset = 0)
	{
		GirlLayer girlLayer = GirlLayer.BODY;
		switch (girlPiece.type)
		{
		case GirlPieceType.EXPRESSION:
			if (artworkOffset >= 3)
			{
				girlLayer = GirlLayer.FACE;
			}
			else if (artworkOffset == 2)
			{
				girlLayer = GirlLayer.MOUTH;
			}
			else if (artworkOffset == 1)
			{
				girlLayer = GirlLayer.EYEBROWS;
			}
			else
			{
				girlLayer = GirlLayer.EYES;
			}
			break;
		case GirlPieceType.HAIRSTYLE:
			if (artworkOffset == 1)
			{
				girlLayer = GirlLayer.BACKHAIR;
			}
			else
			{
				girlLayer = GirlLayer.FRONTHAIR;
			}
			break;
		case GirlPieceType.OUTFIT:
			girlLayer = GirlLayer.OUTFIT;
			break;
		case GirlPieceType.FOOTWEAR:
			girlLayer = GirlLayer.FOOTWEAR;
			break;
		case GirlPieceType.EXTRA:
			if (artworkOffset == 1)
			{
				girlLayer = GirlLayer.EXTRA_TWO;
			}
			else
			{
				girlLayer = GirlLayer.EXTRA_ONE;
			}
			break;
		}
		return this.GetContainerByLayer(girlLayer);
	}

	// Token: 0x060000FE RID: 254 RVA: 0x00010350 File Offset: 0x0000E550
	private DisplayObject GetContainerByLayer(GirlLayer girlLayer)
	{
		switch (girlLayer)
		{
		case GirlLayer.BACKHAIR:
			return this.backhair;
		case GirlLayer.BODY:
			return this.body;
		case GirlLayer.BRA:
			return this.bra;
		case GirlLayer.PANTIES:
			return this.panties;
		case GirlLayer.FOOTWEAR:
			return this.footwear;
		case GirlLayer.OUTFIT:
			return this.outfit;
		case GirlLayer.HEAD:
			return this.head;
		case GirlLayer.FACE:
			return this.face;
		case GirlLayer.MOUTH:
			return this.mouth;
		case GirlLayer.EYES:
			return this.eyes;
		case GirlLayer.FRONTHAIR:
			return this.fronthair;
		case GirlLayer.EYEBROWS:
			return this.eyebrows;
		case GirlLayer.EXTRA_ONE:
			return this.extraOne;
		case GirlLayer.EXTRA_TWO:
			return this.extraTwo;
		default:
			return null;
		}
	}

	// Token: 0x060000FF RID: 255 RVA: 0x000032C1 File Offset: 0x000014C1
	public void ChangeStyle(int pieceId, bool updateExtras = false)
	{
		if (pieceId < 0 || pieceId > this.definition.pieces.Count - 1)
		{
			return;
		}
		this.ChangeStyle(this.definition.pieces[pieceId], updateExtras);
	}

	// Token: 0x06000100 RID: 256 RVA: 0x00010408 File Offset: 0x0000E608
	public void ChangeStyle(GirlPiece piece, bool updateExtras = false)
	{
		if (!this.definition.pieces.Contains(piece) || piece.type == GirlPieceType.PHONEME)
		{
			return;
		}
		if (piece.type == GirlPieceType.EXPRESSION)
		{
			this.ChangeExpression(piece.expressionType, true, false, true, 0f);
		}
		else
		{
			this.AddGirlPiece(piece);
			if (updateExtras)
			{
				List<GirlPiece> piecesByType = this.definition.GetPiecesByType(GirlPieceType.EXTRA);
				for (int i = 0; i < piecesByType.Count; i++)
				{
					if (i > 1)
					{
						break;
					}
					if (!StringUtils.IsEmpty(piecesByType[i].limitToOutfits))
					{
						this.GetContainerByPiece(piecesByType[i], i).RemoveAllChildren(true);
					}
				}
			}
		}
	}

	// Token: 0x06000101 RID: 257 RVA: 0x000104C4 File Offset: 0x0000E6C4
	public void HideBra()
	{
		if (this.bra.GetChildren(false).Length == 0)
		{
			return;
		}
		TweenUtils.KillTweener(this._fadeBraTweener, true);
		SpriteObject p_target = this.bra.GetChildren(false)[0] as SpriteObject;
		this._fadeBraTweener = HOTween.To(p_target, 0.5f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.Linear));
	}

	// Token: 0x06000102 RID: 258 RVA: 0x00010534 File Offset: 0x0000E734
	public void ChangeExpression(GirlExpressionType expression, bool permanent = true, bool changeEyes = true, bool changeMouth = true, float tempDelay = 0f)
	{
		if (this.definition == null)
		{
			return;
		}
		this._tempExpressionActive = false;
		this._eyesClosed = false;
		this._resetFace = false;
		if (!permanent)
		{
			this._tempExpressionActive = true;
		}
		GirlPiece girlPieceByExpressionType = this.GetGirlPieceByExpressionType(expression);
		if (changeEyes)
		{
			if (this._blinkStage == 0)
			{
				this.AddGirlPieceArtToContainer(girlPieceByExpressionType.primaryArt, this.GetContainerByLayer(GirlLayer.EYES));
			}
			if (permanent)
			{
				this._eyesExpression = girlPieceByExpressionType;
			}
		}
		else if (this._blinkStage == 0)
		{
			this.AddGirlPieceArtToContainer(this._eyesExpression.primaryArt, this.GetContainerByLayer(GirlLayer.EYES));
		}
		this.AddGirlPieceArtToContainer(girlPieceByExpressionType.secondaryArt, this.GetContainerByLayer(GirlLayer.EYEBROWS));
		if (permanent)
		{
			this._eyebrowsExpression = girlPieceByExpressionType;
		}
		if (changeMouth)
		{
			if (this._currentDialogLine == null)
			{
				this.AddGirlPieceArtToContainer(girlPieceByExpressionType.tertiaryArt, this.GetContainerByLayer(GirlLayer.MOUTH));
			}
			if (permanent)
			{
				this._mouthExpression = girlPieceByExpressionType;
			}
		}
		if (tempDelay > 0f)
		{
			this.ResetFace(tempDelay);
		}
	}

	// Token: 0x06000103 RID: 259 RVA: 0x00010640 File Offset: 0x0000E840
	private GirlPiece GetGirlPieceByExpressionType(GirlExpressionType expression)
	{
		GirlPiece girlPiece = null;
		for (int i = 0; i < this.definition.pieces.Count; i++)
		{
			if (this.definition.pieces[i].type == GirlPieceType.EXPRESSION && this.definition.pieces[i].expressionType == expression)
			{
				girlPiece = this.definition.pieces[i];
				break;
			}
		}
		if (girlPiece == null)
		{
			girlPiece = this.definition.pieces[this.definition.defaultExpression];
		}
		return girlPiece;
	}

	// Token: 0x06000104 RID: 260 RVA: 0x000106E4 File Offset: 0x0000E8E4
	private void MouthLetter(string letter)
	{
		if (this.definition == null)
		{
			return;
		}
		this._resetFace = false;
		GirlPieceArt girlPieceArt = null;
		if (letter != null)
		{
			letter = letter.ToLower();
			if (this._letterMouths.ContainsKey(letter))
			{
				girlPieceArt = this._letterMouths[letter];
			}
		}
		else
		{
			girlPieceArt = this._mouthExpression.tertiaryArt;
		}
		if (girlPieceArt == null)
		{
			girlPieceArt = this._letterMouths["b"];
		}
		this.AddGirlPieceArtToContainer(girlPieceArt, this.GetContainerByLayer(GirlLayer.MOUTH));
	}

	// Token: 0x06000105 RID: 261 RVA: 0x000032FB File Offset: 0x000014FB
	private void ResetFace(float delay = 0f)
	{
		if (this.definition == null)
		{
			return;
		}
		this._resetFace = true;
		this._resetFaceTimestamp = GameManager.System.Lifetime(true);
		this._resetFaceDelay = delay;
	}

	// Token: 0x06000106 RID: 262 RVA: 0x00010770 File Offset: 0x0000E970
	private void ResetExpression()
	{
		if (this.definition == null)
		{
			return;
		}
		this._tempExpressionActive = false;
		this._eyesClosed = false;
		this._resetFace = false;
		if (this._blinkStage == 0)
		{
			this.AddGirlPieceArtToContainer(this._eyesExpression.primaryArt, this.GetContainerByLayer(GirlLayer.EYES));
		}
		this.AddGirlPieceArtToContainer(this._eyebrowsExpression.secondaryArt, this.GetContainerByLayer(GirlLayer.EYEBROWS));
		this.AddGirlPieceArtToContainer(this._mouthExpression.tertiaryArt, this.GetContainerByLayer(GirlLayer.MOUTH));
	}

	// Token: 0x06000107 RID: 263 RVA: 0x000107FC File Offset: 0x0000E9FC
	public void ClearDialog()
	{
		this._dialogListenForSkip = false;
		GameManager.Stage.MouseUpEvent -= this.OnDialogTextSkip;
		if (this._dialogProceedTimer != null)
		{
			this._dialogProceedTimer.Stop();
		}
		this._dialogProceedTimer = null;
		TweenUtils.KillTweener(this._dialogPromptTweener, false);
		this._dialogPromptTweener = null;
		this.dialogPrompt.SetAlpha(0f, 0f);
		if (this._currentDialogLine != null)
		{
			GameManager.System.Audio.Stop(this._currentDialogLine.GetAudio());
		}
		TweenUtils.KillSequence(this._dialogSequence, false);
		this.girlSpeechBubble.childrenAlpha = 0f;
		this.girlSpeechBubble.gameObj.transform.localScale = new Vector3(0.8f, 0.8f, 1f);
		this.MouthLetter(null);
		this._currentDialogLine = null;
		this._currentDialogLineSpeechText = null;
		this.dialogReadPercent = 0f;
		this._dialogLineExpressionIndex = -1;
		this._dialogMouthTimestamp = 0f;
		this._tempExpressionActive = false;
		this._dialogHideSpeechBubble = false;
		this._resetFace = false;
	}

	// Token: 0x06000108 RID: 264 RVA: 0x0001091C File Offset: 0x0000EB1C
	public void ReadDialogLine(DialogLine dialogLine, bool passive = true, bool useAltLine = false, bool hideSpeechBubble = false, bool useAltBaseVolume = false)
	{
		if (this.DialogLineBeginEvent != null)
		{
			this.DialogLineBeginEvent();
		}
		if (GameManager.System.Location.IsGirlLeaving())
		{
			passive = false;
		}
		this.ClearDialog();
		this.MouthLetter(null);
		if (dialogLine != null)
		{
			this.ChangeExpression(dialogLine.startExpression.expression, true, true, true, 0f);
			if (dialogLine.startExpression.closeEyes)
			{
				this.AddGirlPieceArtToContainer(this.definition.blinkFullPiece, this.GetContainerByLayer(GirlLayer.EYES));
				this._eyesClosed = true;
			}
			this._currentDialogLine = dialogLine;
			this._currentDialogLineSpeechText = this._currentDialogLine.GetText();
			while (this._currentDialogLineSpeechText.Contains("*"))
			{
				int num = this._currentDialogLineSpeechText.IndexOf("*");
				int num2 = this._currentDialogLineSpeechText.IndexOf("*", num + 1);
				string text = string.Empty;
				for (int i = 0; i < num2 + 1 - num; i++)
				{
					text += GameManager.System.Dialog.dialogSpacerChar;
				}
				this._currentDialogLineSpeechText = this._currentDialogLineSpeechText.Remove(num, num2 + 1 - num);
				this._currentDialogLineSpeechText = this._currentDialogLineSpeechText.Insert(num, text);
			}
			this.dialogLabel.SetText("^C00000000" + this._currentDialogLineSpeechText);
			float p_duration = (float)this._currentDialogLine.GetText().Length * 0.025f;
			if (this._currentDialogLine.GetAudio().clip != null)
			{
				p_duration = Mathf.Max(this._currentDialogLine.GetAudio().clip.length - 0.5f, 0.25f);
			}
			this._dialogHideSpeechBubble = hideSpeechBubble;
			this._dialogSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallbackWParms(this.DialogSequenceComplete), new object[]
			{
				!passive
			}));
			if (!this._dialogHideSpeechBubble)
			{
				this._dialogSequence.Insert(0f, HOTween.To(this.girlSpeechBubble, 0.25f, new TweenParms().Prop("childrenAlpha", 1).Ease(EaseType.EaseOutBack)));
				this._dialogSequence.Insert(0f, HOTween.To(this.girlSpeechBubble.gameObj.transform, 0.25f, new TweenParms().Prop("localScale", Vector3.one).Ease(EaseType.EaseOutBack)));
			}
			this._dialogSequence.Insert(0f, HOTween.To(this, p_duration, new TweenParms().Prop("dialogReadPercent", 1).Ease(EaseType.Linear)));
			this._dialogSequence.Play();
			float volume = 1f * this.definition.baseVoiceVolume;
			if (useAltBaseVolume)
			{
				volume = 1f * this.definition.baseSexVolume;
			}
			GameManager.System.Audio.Play(AudioCategory.VOICE, this._currentDialogLine.GetAudio(), false, volume, true);
			if (!passive)
			{
				this._dialogListenForSkip = true;
			}
		}
	}

	// Token: 0x06000109 RID: 265 RVA: 0x0000332E File Offset: 0x0000152E
	private void DialogSequenceComplete(TweenEvent data)
	{
		this.DialogTextRead((bool)data.parms[0], true);
	}

	// Token: 0x0600010A RID: 266 RVA: 0x00010C44 File Offset: 0x0000EE44
	private void OnDialogTextSkip(DisplayObject displayObject)
	{
		if (this.paused)
		{
			return;
		}
		if (this._currentDialogLine != null)
		{
			GameManager.System.Audio.Stop(this._currentDialogLine.GetAudio());
			this.DialogTextRead(false, false);
		}
		else
		{
			this.DialogTextProceed();
		}
	}

	// Token: 0x0600010B RID: 267 RVA: 0x00010C98 File Offset: 0x0000EE98
	private void DialogTextRead(bool proceedTimer, bool autoProceed)
	{
		TweenUtils.KillSequence(this._dialogSequence, true);
		DialogLineExpression dialogLineExpression = null;
		if (this._currentDialogLine.hasEndExpression)
		{
			dialogLineExpression = this._currentDialogLine.endExpression;
		}
		else if (this._currentDialogLine.expressions.Count > 0)
		{
			dialogLineExpression = this._currentDialogLine.expressions[this._currentDialogLine.expressions.Count - 1];
		}
		if (dialogLineExpression != null)
		{
			GirlPiece girlPieceByExpressionType = this.GetGirlPieceByExpressionType(dialogLineExpression.expression);
			if (dialogLineExpression.changeEyes)
			{
				this._eyesExpression = girlPieceByExpressionType;
			}
			this._eyebrowsExpression = girlPieceByExpressionType;
			if (dialogLineExpression.changeMouth)
			{
				this._mouthExpression = girlPieceByExpressionType;
			}
		}
		this.ResetFace(0f);
		if (!this._dialogHideSpeechBubble)
		{
			this.girlSpeechBubble.childrenAlpha = 1f;
			this.girlSpeechBubble.gameObj.transform.localScale = Vector3.one;
		}
		this._dialogHideSpeechBubble = false;
		this.dialogLabel.SetText(this._currentDialogLineSpeechText);
		this._currentDialogLine = null;
		this._currentDialogLineSpeechText = null;
		if (proceedTimer)
		{
			this._dialogProceedTimer = GameManager.System.Timers.New(0.5f, new Action(this.DialogTextProceed));
		}
		else if (autoProceed)
		{
			this.DialogTextProceed();
		}
		else
		{
			this._dialogPromptTweener = HOTween.To(this.dialogPrompt, 1f, new TweenParms().Prop("spriteAlpha", 1).Ease(EaseType.EaseInOutCubic).Loops(-1, LoopType.Yoyo));
		}
	}

	// Token: 0x0600010C RID: 268 RVA: 0x00010E2C File Offset: 0x0000F02C
	private void DialogTextProceed()
	{
		this._dialogListenForSkip = false;
		GameManager.Stage.MouseUpEvent -= this.OnDialogTextSkip;
		TweenUtils.KillTweener(this._dialogPromptTweener, false);
		this._dialogPromptTweener = null;
		this.dialogPrompt.SetAlpha(0f, 0f);
		if (this._dialogProceedTimer != null)
		{
			this._dialogProceedTimer.Stop();
		}
		this._dialogProceedTimer = null;
		if (this.DialogLineReadEvent != null)
		{
			this.DialogLineReadEvent();
		}
	}

	// Token: 0x0600010D RID: 269 RVA: 0x00003344 File Offset: 0x00001544
	public bool IsReadingDialog()
	{
		return this._currentDialogLine != null || this._dialogProceedTimer != null;
	}

	// Token: 0x0600010E RID: 270 RVA: 0x00010EB4 File Offset: 0x0000F0B4
	public override void Pause()
	{
		base.Pause();
		if (!this.paused)
		{
			return;
		}
		if (this._dialogSequence != null && !this._dialogSequence.isPaused)
		{
			this._dialogSequence.Pause();
		}
		if (this._dialogPromptTweener != null && !this._dialogPromptTweener.isPaused)
		{
			this._dialogPromptTweener.Pause();
		}
		if (this._fadeBraTweener != null && !this._fadeBraTweener.isPaused)
		{
			this._fadeBraTweener.Pause();
		}
	}

	// Token: 0x0600010F RID: 271 RVA: 0x00010F48 File Offset: 0x0000F148
	public override void Unpause()
	{
		base.Unpause();
		if (this.paused)
		{
			return;
		}
		if (this._dialogSequence != null && this._dialogSequence.isPaused)
		{
			this._dialogSequence.Play();
		}
		if (this._dialogPromptTweener != null && this._dialogPromptTweener.isPaused)
		{
			this._dialogPromptTweener.Play();
		}
		if (this._fadeBraTweener != null && this._fadeBraTweener.isPaused)
		{
			this._fadeBraTweener.Play();
		}
	}

	// Token: 0x04000099 RID: 153
	public const string SPRITE_ICON_FULL = "_full";

	// Token: 0x0400009A RID: 154
	public const string SPRITE_ICON_TRANS = "_trans";

	// Token: 0x0400009B RID: 155
	public const string SPRITE_ICON_TRANS_UNKNOWN = "_trans_unknown";

	// Token: 0x0400009C RID: 156
	public const string SPRITE_ICON_MINI = "_mini";

	// Token: 0x0400009D RID: 157
	public const string SPRITE_ICON_MICRO = "_micro";

	// Token: 0x0400009E RID: 158
	private const float BLINK_DURATION = 0.1f;

	// Token: 0x0400009F RID: 159
	private const float BLINK_DELAY_MIN = 0.5f;

	// Token: 0x040000A0 RID: 160
	private const float BLINK_DELAY_MAX = 4f;

	// Token: 0x040000A1 RID: 161
	private const string DIALOG_INLINE_STYLER = "^C00000000";

	// Token: 0x040000A2 RID: 162
	private const float DIALOG_MOUTH_DELAY = 0.064f;

	// Token: 0x040000A3 RID: 163
	private const int DEFAULT_FOOTWEAR_INDEX = 18;

	// Token: 0x040000A4 RID: 164
	public bool flip;

	// Token: 0x040000A5 RID: 165
	public GirlDefinition definition;

	// Token: 0x040000A6 RID: 166
	public tk2dSpriteCollectionData spriteCollection;

	// Token: 0x040000A7 RID: 167
	public DisplayObject girlContainer;

	// Token: 0x040000A8 RID: 168
	public DisplayObject girlPieceContainers;

	// Token: 0x040000A9 RID: 169
	public DisplayObject backhair;

	// Token: 0x040000AA RID: 170
	public DisplayObject body;

	// Token: 0x040000AB RID: 171
	public DisplayObject bra;

	// Token: 0x040000AC RID: 172
	public DisplayObject panties;

	// Token: 0x040000AD RID: 173
	public DisplayObject footwear;

	// Token: 0x040000AE RID: 174
	public DisplayObject outfit;

	// Token: 0x040000AF RID: 175
	public DisplayObject head;

	// Token: 0x040000B0 RID: 176
	public DisplayObject face;

	// Token: 0x040000B1 RID: 177
	public DisplayObject mouth;

	// Token: 0x040000B2 RID: 178
	public DisplayObject eyes;

	// Token: 0x040000B3 RID: 179
	public DisplayObject fronthair;

	// Token: 0x040000B4 RID: 180
	public DisplayObject eyebrows;

	// Token: 0x040000B5 RID: 181
	public DisplayObject extraOne;

	// Token: 0x040000B6 RID: 182
	public DisplayObject extraTwo;

	// Token: 0x040000B7 RID: 183
	private List<DisplayObject> _containers = new List<DisplayObject>();

	// Token: 0x040000B8 RID: 184
	private Dictionary<string, GirlPieceArt> _letterMouths = new Dictionary<string, GirlPieceArt>();

	// Token: 0x040000B9 RID: 185
	private GirlPiece _eyesExpression;

	// Token: 0x040000BA RID: 186
	private GirlPiece _eyebrowsExpression;

	// Token: 0x040000BB RID: 187
	private GirlPiece _mouthExpression;

	// Token: 0x040000BC RID: 188
	private GirlPiece _faceExpression;

	// Token: 0x040000BD RID: 189
	private bool _tempExpressionActive;

	// Token: 0x040000BE RID: 190
	private bool _eyesClosed;

	// Token: 0x040000BF RID: 191
	private bool _resetFace;

	// Token: 0x040000C0 RID: 192
	private float _resetFaceTimestamp;

	// Token: 0x040000C1 RID: 193
	private float _resetFaceDelay;

	// Token: 0x040000C2 RID: 194
	private int _blinkStage;

	// Token: 0x040000C3 RID: 195
	private float _blinkTimestamp;

	// Token: 0x040000C4 RID: 196
	private float _blinkDelay;

	// Token: 0x040000C5 RID: 197
	private float _panTimestamp;

	// Token: 0x040000C6 RID: 198
	private Regex _alphaNumericRegex;

	// Token: 0x040000C7 RID: 199
	private DisplayObject _speechBubbleNormal;

	// Token: 0x040000C8 RID: 200
	private SpriteObject _dialogBackgroundNormal;

	// Token: 0x040000C9 RID: 201
	private LabelObject _dialogLabelNormal;

	// Token: 0x040000CA RID: 202
	private SpriteObject _dialogPromptNormal;

	// Token: 0x040000CB RID: 203
	private DisplayObject _speechBubbleDate;

	// Token: 0x040000CC RID: 204
	private SpriteObject _dialogBackgroundDate;

	// Token: 0x040000CD RID: 205
	private LabelObject _dialogLabelDate;

	// Token: 0x040000CE RID: 206
	private SpriteObject _dialogPromptDate;

	// Token: 0x040000CF RID: 207
	private DialogLine _currentDialogLine;

	// Token: 0x040000D0 RID: 208
	private string _currentDialogLineSpeechText;

	// Token: 0x040000D1 RID: 209
	private Sequence _dialogSequence;

	// Token: 0x040000D2 RID: 210
	public float dialogReadPercent;

	// Token: 0x040000D3 RID: 211
	private int _dialogLineExpressionIndex;

	// Token: 0x040000D4 RID: 212
	private float _dialogMouthTimestamp;

	// Token: 0x040000D5 RID: 213
	private Timer _dialogProceedTimer;

	// Token: 0x040000D6 RID: 214
	private bool _dialogListenForSkip;

	// Token: 0x040000D7 RID: 215
	private bool _dialogHideSpeechBubble;

	// Token: 0x040000D8 RID: 216
	private Tweener _dialogPromptTweener;

	// Token: 0x040000D9 RID: 217
	private Tweener _fadeBraTweener;

	// Token: 0x02000019 RID: 25
	// (Invoke) Token: 0x06000111 RID: 273
	public delegate void DialogLineDelegate();
}
