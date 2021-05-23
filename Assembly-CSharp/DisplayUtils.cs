using System;
using UnityEngine;

// Token: 0x0200013D RID: 317
public class DisplayUtils
{
	// Token: 0x0600076F RID: 1903 RVA: 0x00037798 File Offset: 0x00035998
	public static void SetLightnessOfChildren(DisplayObject parent, float lightness)
	{
		SpriteObject[] componentsInChildren = parent.transform.GetComponentsInChildren<SpriteObject>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetLightness(lightness, 0f);
		}
		SlicedSpriteObject[] componentsInChildren2 = parent.transform.GetComponentsInChildren<SlicedSpriteObject>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].SetLightness(lightness, 0f);
		}
		LabelObject[] componentsInChildren3 = parent.transform.GetComponentsInChildren<LabelObject>();
		for (int i = 0; i < componentsInChildren3.Length; i++)
		{
			componentsInChildren3[i].SetLightness(lightness);
		}
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x0003782C File Offset: 0x00035A2C
	public static void SetColorOfChildren(DisplayObject parent, Color color)
	{
		SpriteObject[] componentsInChildren = parent.transform.GetComponentsInChildren<SpriteObject>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetColor(color, 0f);
		}
		SlicedSpriteObject[] componentsInChildren2 = parent.transform.GetComponentsInChildren<SlicedSpriteObject>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].SetColor(color, 0f);
		}
		LabelObject[] componentsInChildren3 = parent.transform.GetComponentsInChildren<LabelObject>();
		for (int i = 0; i < componentsInChildren3.Length; i++)
		{
			componentsInChildren3[i].SetColor(color);
		}
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x000378C0 File Offset: 0x00035AC0
	public static void SetAlphaOfChildren(DisplayObject parent, float alpha)
	{
		SpriteObject[] componentsInChildren = parent.transform.GetComponentsInChildren<SpriteObject>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].SetAlpha(alpha, 0f);
		}
		SlicedSpriteObject[] componentsInChildren2 = parent.transform.GetComponentsInChildren<SlicedSpriteObject>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].SetAlpha(alpha, 0f);
		}
		LabelObject[] componentsInChildren3 = parent.transform.GetComponentsInChildren<LabelObject>();
		for (int i = 0; i < componentsInChildren3.Length; i++)
		{
			componentsInChildren3[i].SetAlpha(alpha);
		}
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x00037954 File Offset: 0x00035B54
	public static void ToggleInteractivity(DisplayObject parent, bool interactive)
	{
		Collider[] componentsInChildren = parent.transform.GetComponentsInChildren<Collider>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = interactive;
		}
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x0003798C File Offset: 0x00035B8C
	public static void SetPausable(DisplayObject parent, bool pausable)
	{
		parent.SetPausable(pausable);
		DisplayObject[] children = parent.GetChildren(false);
		for (int i = 0; i < children.Length; i++)
		{
			DisplayUtils.SetPausable(children[i], pausable);
		}
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x000379C8 File Offset: 0x00035BC8
	public static void ChangeLayer(DisplayObject parent, int layer)
	{
		parent.gameObj.layer = layer;
		DisplayObject[] children = parent.GetChildren(false);
		for (int i = 0; i < children.Length; i++)
		{
			DisplayUtils.ChangeLayer(children[i], layer);
		}
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x00037A08 File Offset: 0x00035C08
	public static SpriteObject CreateSpriteObject(tk2dSpriteCollectionData spriteCollection, string spriteName, string gameObjectName = "SpriteObject")
	{
		SpriteObject component = new GameObject(gameObjectName, new Type[]
		{
			typeof(SpriteObject)
		}).GetComponent<SpriteObject>();
		component.sprite.SetSprite(spriteCollection, spriteName);
		return component;
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x00037A44 File Offset: 0x00035C44
	public static ClippedSpriteObject CreateClippedSpriteObject(tk2dSpriteCollectionData spriteCollection, string spriteName, string gameObjectName = "ClippedSpriteObject")
	{
		ClippedSpriteObject component = new GameObject(gameObjectName, new Type[]
		{
			typeof(ClippedSpriteObject)
		}).GetComponent<ClippedSpriteObject>();
		component.sprite.SetSprite(spriteCollection, spriteName);
		return component;
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00037A80 File Offset: 0x00035C80
	public static SlicedSpriteObject CreateSlicedSpriteObject(tk2dSpriteCollectionData spriteCollection, string spriteName, string gameObjectName = "SlicedSpriteObject")
	{
		SlicedSpriteObject component = new GameObject(gameObjectName, new Type[]
		{
			typeof(SlicedSpriteObject)
		}).GetComponent<SlicedSpriteObject>();
		component.sprite.SetSprite(spriteCollection, spriteName);
		return component;
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x00037ABC File Offset: 0x00035CBC
	public static PopLabelObject CreatePopLabelObject(tk2dFontData fontData, string text, string gameObjectName = "PopLabelObject")
	{
		PopLabelObject component = new GameObject(gameObjectName, new Type[]
		{
			typeof(PopLabelObject)
		}).GetComponent<PopLabelObject>();
		component.label.anchor = TextAnchor.MiddleCenter;
		component.label.maxChars = 32;
		component.SetFont(fontData);
		component.SetText(text);
		return component;
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x00037B10 File Offset: 0x00035D10
	public static BurstLabelObject CreateBurstLabelObject(tk2dFontData fontData, string text, string gameObjectName = "BurstLabelObject")
	{
		BurstLabelObject component = new GameObject(gameObjectName, new Type[]
		{
			typeof(BurstLabelObject)
		}).GetComponent<BurstLabelObject>();
		component.label.anchor = TextAnchor.MiddleCenter;
		component.label.maxChars = 32;
		component.SetFont(fontData);
		component.SetText(text);
		return component;
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x00037B64 File Offset: 0x00035D64
	public static JackpotLabelObject CreateJackpotLabelObject(tk2dFontData fontData, EnergyTrailDefinition energyTrailDef, string gameObjectName = "JackpotLabelObject")
	{
		JackpotLabelObject component = new GameObject(gameObjectName, new Type[]
		{
			typeof(JackpotLabelObject)
		}).GetComponent<JackpotLabelObject>();
		component.label.anchor = TextAnchor.MiddleCenter;
		component.label.maxChars = 32;
		component.SetFont(fontData);
		component.SetText(string.Empty);
		component.energyTrailDef = energyTrailDef;
		return component;
	}
}
