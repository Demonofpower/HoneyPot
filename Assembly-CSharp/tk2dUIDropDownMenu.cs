using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020001AC RID: 428
[AddComponentMenu("2D Toolkit/UI/tk2dUIDropDownMenu")]
public class tk2dUIDropDownMenu : MonoBehaviour
{
	// Token: 0x14000050 RID: 80
	// (add) Token: 0x06000A89 RID: 2697 RVA: 0x0000A388 File Offset: 0x00008588
	// (remove) Token: 0x06000A8A RID: 2698 RVA: 0x0000A3A1 File Offset: 0x000085A1
	public event Action OnSelectedItemChange;

	// Token: 0x1700010D RID: 269
	// (get) Token: 0x06000A8B RID: 2699 RVA: 0x0000A3BA File Offset: 0x000085BA
	// (set) Token: 0x06000A8C RID: 2700 RVA: 0x0000A3C2 File Offset: 0x000085C2
	public List<string> ItemList
	{
		get
		{
			return this.itemList;
		}
		set
		{
			this.itemList = value;
		}
	}

	// Token: 0x1700010E RID: 270
	// (get) Token: 0x06000A8D RID: 2701 RVA: 0x0000A3CB File Offset: 0x000085CB
	// (set) Token: 0x06000A8E RID: 2702 RVA: 0x0000A3D3 File Offset: 0x000085D3
	public int Index
	{
		get
		{
			return this.index;
		}
		set
		{
			this.index = Mathf.Clamp(value, 0, this.ItemList.Count - 1);
			this.SetSelectedItem();
		}
	}

	// Token: 0x1700010F RID: 271
	// (get) Token: 0x06000A8F RID: 2703 RVA: 0x0000A3F5 File Offset: 0x000085F5
	public string SelectedItem
	{
		get
		{
			if (this.index >= 0 && this.index < this.itemList.Count)
			{
				return this.itemList[this.index];
			}
			return string.Empty;
		}
	}

	// Token: 0x06000A90 RID: 2704 RVA: 0x00048E90 File Offset: 0x00047090
	private void Awake()
	{
		foreach (string item in this.startingItemList)
		{
			this.itemList.Add(item);
		}
		this.index = this.startingIndex;
		this.UpdateList();
	}

	// Token: 0x06000A91 RID: 2705 RVA: 0x0000A430 File Offset: 0x00008630
	private void OnEnable()
	{
		this.dropDownButton.OnDown += this.ExpandButtonPressed;
	}

	// Token: 0x06000A92 RID: 2706 RVA: 0x0000A449 File Offset: 0x00008649
	private void OnDisable()
	{
		this.dropDownButton.OnDown -= this.ExpandButtonPressed;
	}

	// Token: 0x06000A93 RID: 2707 RVA: 0x00048EDC File Offset: 0x000470DC
	public void UpdateList()
	{
		if (this.dropDownItems.Count > this.ItemList.Count)
		{
			for (int i = this.ItemList.Count; i < this.dropDownItems.Count; i++)
			{
				this.dropDownItems[i].gameObject.SetActive(false);
			}
		}
		while (this.dropDownItems.Count < this.ItemList.Count)
		{
			this.dropDownItems.Add(this.CreateAnotherDropDownItem());
		}
		for (int j = 0; j < this.ItemList.Count; j++)
		{
			tk2dUIDropDownItem tk2dUIDropDownItem = this.dropDownItems[j];
			Vector3 localPosition = tk2dUIDropDownItem.transform.localPosition;
			localPosition.y = -this.height - (float)j * tk2dUIDropDownItem.height;
			tk2dUIDropDownItem.transform.localPosition = localPosition;
			if (tk2dUIDropDownItem.label != null)
			{
				tk2dUIDropDownItem.LabelText = this.itemList[j];
			}
			tk2dUIDropDownItem.Index = j;
		}
		this.SetSelectedItem();
	}

	// Token: 0x06000A94 RID: 2708 RVA: 0x00048FFC File Offset: 0x000471FC
	public void SetSelectedItem()
	{
		if (this.index < 0 || this.index >= this.ItemList.Count)
		{
			this.index = 0;
		}
		if (this.index >= 0 && this.index < this.ItemList.Count)
		{
			this.selectedTextMesh.text = this.ItemList[this.index];
			this.selectedTextMesh.Commit();
		}
		else
		{
			this.selectedTextMesh.text = string.Empty;
			this.selectedTextMesh.Commit();
		}
		if (this.OnSelectedItemChange != null)
		{
			this.OnSelectedItemChange();
		}
	}

	// Token: 0x06000A95 RID: 2709 RVA: 0x000490B4 File Offset: 0x000472B4
	private tk2dUIDropDownItem CreateAnotherDropDownItem()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(this.dropDownItemTemplate.gameObject) as GameObject;
		gameObject.name = "DropDownItem";
		gameObject.transform.parent = base.transform;
		gameObject.transform.localPosition = this.dropDownItemTemplate.transform.localPosition;
		gameObject.transform.localRotation = this.dropDownItemTemplate.transform.localRotation;
		gameObject.transform.localScale = this.dropDownItemTemplate.transform.localScale;
		tk2dUIDropDownItem component = gameObject.GetComponent<tk2dUIDropDownItem>();
		component.OnItemSelected += this.ItemSelected;
		tk2dUIUpDownHoverButton component2 = gameObject.GetComponent<tk2dUIUpDownHoverButton>();
		component.upDownHoverBtn = component2;
		component2.OnToggleOver += this.DropDownItemHoverBtnToggle;
		return component;
	}

	// Token: 0x06000A96 RID: 2710 RVA: 0x0000A462 File Offset: 0x00008662
	private void ItemSelected(tk2dUIDropDownItem item)
	{
		if (this.isExpanded)
		{
			this.CollapseList();
		}
		this.Index = item.Index;
	}

	// Token: 0x06000A97 RID: 2711 RVA: 0x0000A481 File Offset: 0x00008681
	private void ExpandButtonPressed()
	{
		if (this.isExpanded)
		{
			this.CollapseList();
		}
		else
		{
			this.ExpandList();
		}
	}

	// Token: 0x06000A98 RID: 2712 RVA: 0x00049180 File Offset: 0x00047380
	private void ExpandList()
	{
		this.isExpanded = true;
		foreach (tk2dUIDropDownItem tk2dUIDropDownItem in this.dropDownItems)
		{
			tk2dUIDropDownItem.gameObject.SetActive(true);
		}
		tk2dUIDropDownItem tk2dUIDropDownItem2 = this.dropDownItems[this.index];
		if (tk2dUIDropDownItem2.upDownHoverBtn != null)
		{
			tk2dUIDropDownItem2.upDownHoverBtn.IsOver = true;
		}
	}

	// Token: 0x06000A99 RID: 2713 RVA: 0x00049218 File Offset: 0x00047418
	private void CollapseList()
	{
		this.isExpanded = false;
		foreach (tk2dUIDropDownItem tk2dUIDropDownItem in this.dropDownItems)
		{
			tk2dUIDropDownItem.gameObject.SetActive(false);
		}
	}

	// Token: 0x06000A9A RID: 2714 RVA: 0x00049280 File Offset: 0x00047480
	private void DropDownItemHoverBtnToggle(tk2dUIUpDownHoverButton upDownHoverButton)
	{
		if (upDownHoverButton.IsOver)
		{
			foreach (tk2dUIDropDownItem tk2dUIDropDownItem in this.dropDownItems)
			{
				if (tk2dUIDropDownItem.upDownHoverBtn != upDownHoverButton && tk2dUIDropDownItem.upDownHoverBtn != null)
				{
					tk2dUIDropDownItem.upDownHoverBtn.IsOver = false;
				}
			}
		}
	}

	// Token: 0x06000A9B RID: 2715 RVA: 0x0004930C File Offset: 0x0004750C
	private void OnDestroy()
	{
		foreach (tk2dUIDropDownItem tk2dUIDropDownItem in this.dropDownItems)
		{
			tk2dUIDropDownItem.OnItemSelected -= this.ItemSelected;
			if (tk2dUIDropDownItem.upDownHoverBtn != null)
			{
				tk2dUIDropDownItem.upDownHoverBtn.OnToggleOver -= this.DropDownItemHoverBtnToggle;
			}
		}
	}

	// Token: 0x04000BD3 RID: 3027
	public tk2dUIItem dropDownButton;

	// Token: 0x04000BD4 RID: 3028
	public tk2dTextMesh selectedTextMesh;

	// Token: 0x04000BD5 RID: 3029
	public float height;

	// Token: 0x04000BD6 RID: 3030
	public tk2dUIDropDownItem dropDownItemTemplate;

	// Token: 0x04000BD7 RID: 3031
	[SerializeField]
	private string[] startingItemList;

	// Token: 0x04000BD8 RID: 3032
	[SerializeField]
	private int startingIndex;

	// Token: 0x04000BD9 RID: 3033
	private List<string> itemList = new List<string>();

	// Token: 0x04000BDA RID: 3034
	private int index;

	// Token: 0x04000BDB RID: 3035
	private List<tk2dUIDropDownItem> dropDownItems = new List<tk2dUIDropDownItem>();

	// Token: 0x04000BDC RID: 3036
	private bool isExpanded;
}
