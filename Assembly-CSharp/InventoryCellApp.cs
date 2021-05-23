using System;
using System.Collections.Generic;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using UnityEngine;

// Token: 0x0200003A RID: 58
public class InventoryCellApp : UICellApp
{
	// Token: 0x060001F0 RID: 496 RVA: 0x00014F40 File Offset: 0x00013140
	public override void Init()
	{
		this.equipmentHeader = (base.GetChildByName("InventoryEquipmentHeader") as SpriteObject);
		this.inventorySlotsContainer = base.GetChildByName("InventorySlotsContainer");
		this.equipmentSlotsContainer = base.GetChildByName("EquipmentSlotsContainer");
		this.tossZone = (base.GetChildByName("InventoryTossZone") as InventoryTossZone);
		this._currentEquipmentTab = 0;
		if (GameManager.Stage.cellPhone.cellMemory.ContainsKey("cell_memory_equipment_tab"))
		{
			this._currentEquipmentTab = GameManager.Stage.cellPhone.cellMemory["cell_memory_equipment_tab"];
		}
		else
		{
			GameManager.Stage.cellPhone.cellMemory.Add("cell_memory_equipment_tab", this._currentEquipmentTab);
		}
		this._allItemSlots = new List<InventorySlot>();
		this._inventorySlots = new List<InventorySlot>();
		for (int i = 0; i < 30; i++)
		{
			InventorySlot inventorySlot = this.inventorySlotsContainer.GetChildByName("InventorySlot" + i.ToString()) as InventorySlot;
			inventorySlot.Init(i);
			inventorySlot.InventorySlotDownEvent += this.OnInventorySlotDown;
			inventorySlot.InventorySlotPressedEvent += this.OnInventorySlotPressed;
			this._inventorySlots.Add(inventorySlot);
			this._allItemSlots.Add(inventorySlot);
		}
		this._equipmentSlots = new List<InventorySlot>();
		for (int j = 0; j < 6; j++)
		{
			InventorySlot inventorySlot2 = this.equipmentSlotsContainer.GetChildByName("EquipmentSlot" + j.ToString()) as InventorySlot;
			inventorySlot2.Init(j);
			inventorySlot2.InventorySlotDownEvent += this.OnInventorySlotDown;
			this._equipmentSlots.Add(inventorySlot2);
			this._allItemSlots.Add(inventorySlot2);
		}
		this.tossZone.tooltip.Disable();
		this.tossZone.SetAlpha(0f, 0f);
		this._arrowLeft = (base.GetChildByName("InventoryArrowLeft") as SpriteObject);
		this._arrowLeft.button.ButtonPressedEvent += this.OnArrowPressed;
		this._arrowRight = (base.GetChildByName("InventoryArrowRight") as SpriteObject);
		this._arrowRight.button.ButtonPressedEvent += this.OnArrowPressed;
		this._swapItemCursor = DisplayUtils.CreateSpriteObject(this.itemsSpriteCollection, "item_blank", "InventoryItemCursor");
		this._swapItemCursor.gameObj.SetActive(false);
		this._swapMode = false;
		this._swapSlotFrom = null;
		this.Refresh();
		if (GameManager.System.GameState != GameState.SIM)
		{
			for (int k = 0; k < this._allItemSlots.Count; k++)
			{
				this._allItemSlots[k].button.Disable();
				this._allItemSlots[k].tooltip.Disable();
			}
			this._arrowLeft.button.Disable();
			this._arrowRight.button.Disable();
			GameManager.Stage.cellPhone.ShowCellAppError("Not while on a date!", true, 0f);
		}
		GameManager.Stage.cellNotifications.ClearNotificationsOfType(CellNotificationType.INVENTORY);
		base.Init();
	}

	// Token: 0x060001F1 RID: 497 RVA: 0x00015278 File Offset: 0x00013478
	public override void Refresh()
	{
		this.equipmentHeader.sprite.SetSprite(InventoryCellApp.EQUIPMENT_TABS[this._currentEquipmentTab]);
		if (this._swapMode)
		{
			for (int i = 0; i < this._allItemSlots.Count; i++)
			{
				InventorySlot inventorySlot = this._allItemSlots[i];
				inventorySlot.PopulateSlotItem();
				inventorySlot.tooltip.Disable();
				if (!this._allItemSlots[i].CanAcceptItem(this._swapSlotFrom))
				{
					inventorySlot.button.Disable();
					inventorySlot.childrenAlpha = 0.5f;
				}
				else
				{
					inventorySlot.button.Enable();
				}
			}
		}
		else
		{
			for (int j = 0; j < this._allItemSlots.Count; j++)
			{
				InventorySlot inventorySlot2 = this._allItemSlots[j];
				inventorySlot2.childrenAlpha = 1f;
				inventorySlot2.PopulateSlotItem();
				inventorySlot2.tooltip.Enable();
				if (inventorySlot2.itemDefinition == null)
				{
					inventorySlot2.button.Disable();
				}
				else
				{
					inventorySlot2.button.Enable();
				}
			}
		}
	}

	// Token: 0x060001F2 RID: 498 RVA: 0x000153A0 File Offset: 0x000135A0
	private void OnInventorySlotDown(InventorySlot inventorySlot)
	{
		if (this._swapMode || inventorySlot.itemDefinition == null || inventorySlot.itemDefinition.type == ItemType.PRESENT)
		{
			return;
		}
		this._swapSlotFrom = inventorySlot;
		this._swapSlotFrom.itemIcon.SetAlpha(0.25f, 0f);
		this._swapSlotFrom.typeIcon.SetAlpha(0.25f, 0f);
		this._swapItemCursor.gameObj.SetActive(true);
		this._swapItemCursor.sprite.SetSprite(inventorySlot.itemDefinition.iconName);
		GameManager.Stage.effects.cursorContainer.AddChild(this._swapItemCursor);
		GameManager.System.Cursor.AttachObject(this._swapItemCursor, 0f, 0f);
		this._swapMode = true;
		this._arrowLeft.button.Disable();
		this._arrowRight.button.Disable();
		GameManager.Stage.cellPhone.Lock(false);
		this.Refresh();
		if (inventorySlot.itemDefinition.type != ItemType.MISC && inventorySlot.itemDefinition.type != ItemType.PANTIES)
		{
			this.tossZone.tooltip.Enable();
			TweenUtils.KillTweener(this._tossZoneTweener, false);
			this._tossZoneTweener = HOTween.To(this.tossZone, 0.2f, new TweenParms().Prop("spriteAlpha", 0.8f).Ease(EaseType.Linear));
		}
		GameManager.Stage.MouseUpEvent += this.OnStageUp;
		GameManager.System.Audio.Play(AudioCategory.SOUND, this.itemPickUpSound, false, 1f, true);
	}

	// Token: 0x060001F3 RID: 499 RVA: 0x00015560 File Offset: 0x00013760
	private void OnStageUp(DisplayObject displayObject)
	{
		GameManager.Stage.MouseUpEvent -= this.OnStageUp;
		TweenUtils.KillSequence(this._swapItemSequence, true);
		if (this.tossZone.tooltip.IsEnabled())
		{
			this.tossZone.tooltip.Disable();
			TweenUtils.KillTweener(this._tossZoneTweener, false);
			this._tossZoneTweener = HOTween.To(this.tossZone, 0.2f, new TweenParms().Prop("spriteAlpha", 0).Ease(EaseType.Linear));
		}
		GameManager.System.Cursor.DetachObject(this._swapItemCursor);
		DisplayObject mouseTarget = GameManager.System.Cursor.GetMouseTarget();
		bool flag = false;
		if (mouseTarget != null)
		{
			InventorySlot component = mouseTarget.GetComponent<InventorySlot>();
			if (component != null && component != this._swapSlotFrom && component.CanAcceptItem(this._swapSlotFrom))
			{
				ItemDefinition itemDefinition = component.itemDefinition;
				component.ReplaceSlotItem(this._swapSlotFrom.itemDefinition);
				this._swapSlotFrom.ReplaceSlotItem(itemDefinition);
				flag = true;
				GameManager.System.Audio.Play(AudioCategory.SOUND, this.itemPutDownSound, false, 1f, true);
			}
			else if (mouseTarget == this.tossZone && this._swapSlotFrom.itemDefinition.type != ItemType.MISC && this._swapSlotFrom.itemDefinition.type != ItemType.PANTIES)
			{
				GameManager.System.Player.LogTossedItem(this._swapSlotFrom.itemDefinition);
				this._swapSlotFrom.ReplaceSlotItem(null);
				flag = true;
				GameManager.System.Audio.Play(AudioCategory.SOUND, this.itemTossSound, false, 1f, true);
				ParticleEmitter2D component2 = new GameObject("ItemDropParticleEmitter", new Type[]
				{
					typeof(ParticleEmitter2D)
				}).GetComponent<ParticleEmitter2D>();
				GameManager.Stage.effects.AddParticleEffect(component2, GameManager.Stage.cellPhone.cellAppContainer);
				component2.Init(this.dropEffectEmitter, this.dropEffectSpriteGroup, false);
				component2.SetGlobalPosition(GameManager.System.Cursor.GetMousePosition().x, GameManager.System.Cursor.GetMousePosition().y);
			}
			GameManager.Stage.uiGirl.RefreshItemSlots();
		}
		if (flag)
		{
			this.SwapModeComplete();
		}
		else
		{
			this._swapItemSequence = new Sequence(new SequenceParms().OnComplete(new TweenDelegate.TweenCallback(this.SwapModeComplete)));
			this._swapItemSequence.Insert(0f, HOTween.To(this._swapItemCursor.gameObj.transform, 0.2f, new TweenParms().Prop("position", new Vector3(this._swapSlotFrom.gameObj.transform.position.x, this._swapSlotFrom.gameObj.transform.position.y, this._swapItemCursor.gameObj.transform.position.z)).Ease(EaseType.EaseOutCubic)));
			this._swapItemSequence.Play();
		}
	}

	// Token: 0x060001F4 RID: 500 RVA: 0x000158A4 File Offset: 0x00013AA4
	private void SwapModeComplete()
	{
		GameManager.Stage.effects.cursorContainer.RemoveChild(this._swapItemCursor, false);
		this._swapItemCursor.gameObj.SetActive(false);
		this._swapSlotFrom.itemIcon.SetAlpha(1f, 0f);
		this._swapSlotFrom.typeIcon.SetAlpha(1f, 0f);
		this._swapSlotFrom = null;
		this._swapMode = false;
		this._arrowLeft.button.Enable();
		this._arrowRight.button.Enable();
		GameManager.Stage.cellPhone.Unlock();
		this.Refresh();
	}

	// Token: 0x060001F5 RID: 501 RVA: 0x00015954 File Offset: 0x00013B54
	private void OnArrowPressed(ButtonObject buttonObject)
	{
		if (this._swapMode)
		{
			return;
		}
		SpriteObject x = buttonObject.GetDisplayObject() as SpriteObject;
		if (x == this._arrowLeft)
		{
			if (this._currentEquipmentTab - 1 < 0)
			{
				this._currentEquipmentTab = InventoryCellApp.EQUIPMENT_TABS.Length - 1;
			}
			else
			{
				this._currentEquipmentTab--;
			}
		}
		else if (InventoryCellApp.EQUIPMENT_TABS.Length > this._currentEquipmentTab + 1)
		{
			this._currentEquipmentTab++;
		}
		else
		{
			this._currentEquipmentTab = 0;
		}
		GameManager.Stage.cellPhone.cellMemory["cell_memory_equipment_tab"] = this._currentEquipmentTab;
		this.Refresh();
	}

	// Token: 0x060001F6 RID: 502 RVA: 0x00015A14 File Offset: 0x00013C14
	private void OnInventorySlotPressed(InventorySlot inventorySlot)
	{
		if (this._swapMode || inventorySlot.itemDefinition == null || inventorySlot.itemDefinition.type != ItemType.PRESENT)
		{
			return;
		}
		inventorySlot.UnwrapItem();
		this.Refresh();
		inventorySlot.tooltip.Disable();
		inventorySlot.tooltip.Enable();
		ParticleEmitter2D component = new GameObject("ItemUnwrapParticleEmitter", new Type[]
		{
			typeof(ParticleEmitter2D)
		}).GetComponent<ParticleEmitter2D>();
		GameManager.Stage.effects.AddParticleEffect(component, GameManager.Stage.cellPhone.cellAppContainer);
		component.Init(this.dropEffectEmitter, this.dropEffectSpriteGroup, false);
		component.SetGlobalPosition(inventorySlot.globalX, inventorySlot.globalY);
		GameManager.System.Audio.Play(AudioCategory.SOUND, this.openPresentSound, false, 1f, true);
	}

	// Token: 0x060001F7 RID: 503 RVA: 0x00015AF8 File Offset: 0x00013CF8
	protected override void Destructor()
	{
		base.Destructor();
		TweenUtils.KillSequence(this._swapItemSequence, true);
		this._swapItemSequence = null;
		TweenUtils.KillTweener(this._tossZoneTweener, true);
		this._tossZoneTweener = null;
		GameManager.Stage.MouseUpEvent -= this.OnStageUp;
		this._arrowLeft.button.ButtonPressedEvent -= this.OnArrowPressed;
		this._arrowRight.button.ButtonPressedEvent -= this.OnArrowPressed;
		if (this._swapItemCursor != null)
		{
			GameManager.System.Cursor.DetachObject(this._swapItemCursor);
			GameManager.Stage.effects.cursorContainer.RemoveChild(this._swapItemCursor, false);
			UnityEngine.Object.Destroy(this._swapItemCursor.gameObj);
		}
		this._swapItemCursor = null;
		for (int i = 0; i < this._allItemSlots.Count; i++)
		{
			this._allItemSlots[i].InventorySlotDownEvent -= this.OnInventorySlotDown;
		}
		this._allItemSlots.Clear();
		this._allItemSlots = null;
		for (int j = 0; j < this._inventorySlots.Count; j++)
		{
			this._inventorySlots[j].InventorySlotPressedEvent -= this.OnInventorySlotPressed;
		}
		this._inventorySlots.Clear();
		this._inventorySlots = null;
		this._equipmentSlots.Clear();
		this._equipmentSlots = null;
	}

	// Token: 0x04000181 RID: 385
	public const string CELL_MEMORY_EQUIPMENT_TAB = "cell_memory_equipment_tab";

	// Token: 0x04000182 RID: 386
	public const string EQUIPMENT_TAB_GIFTS = "cell_app_inventory_header_gifts";

	// Token: 0x04000183 RID: 387
	public const string EQUIPMENT_TAB_DATEGIFTS = "cell_app_inventory_header_dategifts";

	// Token: 0x04000184 RID: 388
	public const string ITEM_BLANK_SPRITE = "item_blank";

	// Token: 0x04000185 RID: 389
	public static readonly string[] EQUIPMENT_TABS = new string[]
	{
		"cell_app_inventory_header_gifts",
		"cell_app_inventory_header_dategifts"
	};

	// Token: 0x04000186 RID: 390
	public tk2dSpriteCollectionData itemsSpriteCollection;

	// Token: 0x04000187 RID: 391
	public ParticleEmitter2DDefinition dropEffectEmitter;

	// Token: 0x04000188 RID: 392
	public SpriteGroupDefinition dropEffectSpriteGroup;

	// Token: 0x04000189 RID: 393
	public AudioDefinition itemPickUpSound;

	// Token: 0x0400018A RID: 394
	public AudioDefinition itemPutDownSound;

	// Token: 0x0400018B RID: 395
	public AudioDefinition itemTossSound;

	// Token: 0x0400018C RID: 396
	public AudioDefinition openPresentSound;

	// Token: 0x0400018D RID: 397
	public SpriteObject equipmentHeader;

	// Token: 0x0400018E RID: 398
	public DisplayObject inventorySlotsContainer;

	// Token: 0x0400018F RID: 399
	public DisplayObject equipmentSlotsContainer;

	// Token: 0x04000190 RID: 400
	public InventoryTossZone tossZone;

	// Token: 0x04000191 RID: 401
	private int _currentEquipmentTab;

	// Token: 0x04000192 RID: 402
	private List<InventorySlot> _inventorySlots;

	// Token: 0x04000193 RID: 403
	private List<InventorySlot> _equipmentSlots;

	// Token: 0x04000194 RID: 404
	private List<InventorySlot> _allItemSlots;

	// Token: 0x04000195 RID: 405
	private SpriteObject _arrowLeft;

	// Token: 0x04000196 RID: 406
	private SpriteObject _arrowRight;

	// Token: 0x04000197 RID: 407
	private bool _swapMode;

	// Token: 0x04000198 RID: 408
	private SpriteObject _swapItemCursor;

	// Token: 0x04000199 RID: 409
	private InventorySlot _swapSlotFrom;

	// Token: 0x0400019A RID: 410
	private Sequence _swapItemSequence;

	// Token: 0x0400019B RID: 411
	private Tweener _tossZoneTweener;
}
