using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000172 RID: 370
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Sprite/tk2dSpriteAttachPoint")]
public class tk2dSpriteAttachPoint : MonoBehaviour
{
	// Token: 0x06000958 RID: 2392 RVA: 0x00009387 File Offset: 0x00007587
	private void Awake()
	{
		if (this.sprite == null)
		{
			this.sprite = base.GetComponent<tk2dBaseSprite>();
			if (this.sprite != null)
			{
				this.HandleSpriteChanged(this.sprite);
			}
		}
	}

	// Token: 0x06000959 RID: 2393 RVA: 0x000093C3 File Offset: 0x000075C3
	private void OnEnable()
	{
		if (this.sprite != null)
		{
			this.sprite.SpriteChanged += this.HandleSpriteChanged;
		}
	}

	// Token: 0x0600095A RID: 2394 RVA: 0x000093ED File Offset: 0x000075ED
	private void OnDisable()
	{
		if (this.sprite != null)
		{
			this.sprite.SpriteChanged -= this.HandleSpriteChanged;
		}
	}

	// Token: 0x0600095B RID: 2395 RVA: 0x00040760 File Offset: 0x0003E960
	private void UpdateAttachPointTransform(tk2dSpriteDefinition.AttachPoint attachPoint, Transform t)
	{
		t.localPosition = Vector3.Scale(attachPoint.position, this.sprite.scale);
		t.localScale = this.sprite.scale;
		float num = Mathf.Sign(this.sprite.scale.x) * Mathf.Sign(this.sprite.scale.y);
		t.localEulerAngles = new Vector3(0f, 0f, attachPoint.angle * num);
	}

	// Token: 0x0600095C RID: 2396 RVA: 0x000407EC File Offset: 0x0003E9EC
	private void HandleSpriteChanged(tk2dBaseSprite spr)
	{
		tk2dSpriteDefinition currentSprite = spr.CurrentSprite;
		int num = Mathf.Max(currentSprite.attachPoints.Length, this.attachPoints.Count);
		if (num > tk2dSpriteAttachPoint.attachPointUpdated.Length)
		{
			tk2dSpriteAttachPoint.attachPointUpdated = new bool[num];
		}
		foreach (tk2dSpriteDefinition.AttachPoint attachPoint in currentSprite.attachPoints)
		{
			bool flag = false;
			int num2 = 0;
			foreach (Transform transform in this.attachPoints)
			{
				if (transform != null && transform.name == attachPoint.name)
				{
					tk2dSpriteAttachPoint.attachPointUpdated[num2] = true;
					this.UpdateAttachPointTransform(attachPoint, transform);
					flag = true;
				}
				num2++;
			}
			if (!flag)
			{
				GameObject gameObject = new GameObject(attachPoint.name);
				Transform transform2 = gameObject.transform;
				transform2.parent = base.transform;
				this.UpdateAttachPointTransform(attachPoint, transform2);
				tk2dSpriteAttachPoint.attachPointUpdated[this.attachPoints.Count] = true;
				this.attachPoints.Add(transform2);
			}
		}
		if (this.deactivateUnusedAttachPoints)
		{
			for (int j = 0; j < this.attachPoints.Count; j++)
			{
				if (this.attachPoints[j] != null)
				{
					GameObject gameObject2 = this.attachPoints[j].gameObject;
					if (tk2dSpriteAttachPoint.attachPointUpdated[j] && !gameObject2.activeSelf)
					{
						gameObject2.SetActive(true);
					}
					else if (!tk2dSpriteAttachPoint.attachPointUpdated[j] && gameObject2.activeSelf)
					{
						gameObject2.SetActive(false);
					}
				}
				tk2dSpriteAttachPoint.attachPointUpdated[j] = false;
			}
		}
	}

	// Token: 0x04000A25 RID: 2597
	private tk2dBaseSprite sprite;

	// Token: 0x04000A26 RID: 2598
	public List<Transform> attachPoints = new List<Transform>();

	// Token: 0x04000A27 RID: 2599
	private static bool[] attachPointUpdated = new bool[32];

	// Token: 0x04000A28 RID: 2600
	public bool deactivateUnusedAttachPoints;
}
