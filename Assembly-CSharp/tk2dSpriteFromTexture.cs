using System;
using tk2dRuntime;
using UnityEngine;

// Token: 0x0200018D RID: 397
[ExecuteInEditMode]
[AddComponentMenu("2D Toolkit/Sprite/tk2dSpriteFromTexture")]
public class tk2dSpriteFromTexture : MonoBehaviour
{
	// Token: 0x170000E9 RID: 233
	// (get) Token: 0x060009A1 RID: 2465 RVA: 0x00041F60 File Offset: 0x00040160
	private tk2dBaseSprite Sprite
	{
		get
		{
			if (this._sprite == null)
			{
				this._sprite = base.GetComponent<tk2dBaseSprite>();
				if (this._sprite == null)
				{
					Debug.Log("tk2dSpriteFromTexture - Missing sprite object. Creating.");
					this._sprite = base.gameObject.AddComponent<tk2dSprite>();
				}
			}
			return this._sprite;
		}
	}

	// Token: 0x060009A2 RID: 2466 RVA: 0x00009836 File Offset: 0x00007A36
	private void Awake()
	{
		this.Create(this.spriteCollectionSize, this.texture, this.anchor);
	}

	// Token: 0x170000EA RID: 234
	// (get) Token: 0x060009A3 RID: 2467 RVA: 0x00009850 File Offset: 0x00007A50
	public bool HasSpriteCollection
	{
		get
		{
			return this.spriteCollection != null;
		}
	}

	// Token: 0x060009A4 RID: 2468 RVA: 0x0000985E File Offset: 0x00007A5E
	private void OnDestroy()
	{
		this.DestroyInternal();
		if (base.renderer != null)
		{
			base.renderer.material = null;
		}
	}

	// Token: 0x060009A5 RID: 2469 RVA: 0x00041FBC File Offset: 0x000401BC
	public void Create(tk2dSpriteCollectionSize spriteCollectionSize, Texture texture, tk2dBaseSprite.Anchor anchor)
	{
		this.DestroyInternal();
		if (texture != null)
		{
			this.spriteCollectionSize.CopyFrom(spriteCollectionSize);
			this.texture = texture;
			this.anchor = anchor;
			GameObject gameObject = new GameObject("tk2dSpriteFromTexture - " + texture.name);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
			gameObject.hideFlags = HideFlags.DontSave;
			Vector2 anchorOffset = tk2dSpriteGeomGen.GetAnchorOffset(anchor, (float)texture.width, (float)texture.height);
			this.spriteCollection = SpriteCollectionGenerator.CreateFromTexture(gameObject, texture, spriteCollectionSize, new Vector2((float)texture.width, (float)texture.height), new string[]
			{
				"unnamed"
			}, new Rect[]
			{
				new Rect(0f, 0f, (float)texture.width, (float)texture.height)
			}, null, new Vector2[]
			{
				anchorOffset
			}, new bool[1]);
			string text = "SpriteFromTexture " + texture.name;
			this.spriteCollection.spriteCollectionName = text;
			this.spriteCollection.spriteDefinitions[0].material.name = text;
			this.spriteCollection.spriteDefinitions[0].material.hideFlags = (HideFlags.HideInInspector | HideFlags.DontSave);
			this.Sprite.SetSprite(this.spriteCollection, 0);
		}
	}

	// Token: 0x060009A6 RID: 2470 RVA: 0x00009883 File Offset: 0x00007A83
	public void Clear()
	{
		this.DestroyInternal();
	}

	// Token: 0x060009A7 RID: 2471 RVA: 0x0000988B File Offset: 0x00007A8B
	public void ForceBuild()
	{
		this.DestroyInternal();
		this.Create(this.spriteCollectionSize, this.texture, this.anchor);
	}

	// Token: 0x060009A8 RID: 2472 RVA: 0x00042130 File Offset: 0x00040330
	private void DestroyInternal()
	{
		if (this.spriteCollection != null)
		{
			if (this.spriteCollection.spriteDefinitions[0].material != null)
			{
				UnityEngine.Object.DestroyImmediate(this.spriteCollection.spriteDefinitions[0].material);
			}
			UnityEngine.Object.DestroyImmediate(this.spriteCollection.gameObject);
			this.spriteCollection = null;
		}
	}

	// Token: 0x04000B2E RID: 2862
	public Texture texture;

	// Token: 0x04000B2F RID: 2863
	public tk2dSpriteCollectionSize spriteCollectionSize = new tk2dSpriteCollectionSize();

	// Token: 0x04000B30 RID: 2864
	public tk2dBaseSprite.Anchor anchor = tk2dBaseSprite.Anchor.MiddleCenter;

	// Token: 0x04000B31 RID: 2865
	private tk2dSpriteCollectionData spriteCollection;

	// Token: 0x04000B32 RID: 2866
	private tk2dBaseSprite _sprite;
}
