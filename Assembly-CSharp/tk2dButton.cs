using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000159 RID: 345
[AddComponentMenu("2D Toolkit/Deprecated/GUI/tk2dButton")]
public class tk2dButton : MonoBehaviour
{
	// Token: 0x1400004A RID: 74
	// (add) Token: 0x0600082A RID: 2090 RVA: 0x000083C3 File Offset: 0x000065C3
	// (remove) Token: 0x0600082B RID: 2091 RVA: 0x000083DC File Offset: 0x000065DC
	public event tk2dButton.ButtonHandlerDelegate ButtonPressedEvent;

	// Token: 0x1400004B RID: 75
	// (add) Token: 0x0600082C RID: 2092 RVA: 0x000083F5 File Offset: 0x000065F5
	// (remove) Token: 0x0600082D RID: 2093 RVA: 0x0000840E File Offset: 0x0000660E
	public event tk2dButton.ButtonHandlerDelegate ButtonAutoFireEvent;

	// Token: 0x1400004C RID: 76
	// (add) Token: 0x0600082E RID: 2094 RVA: 0x00008427 File Offset: 0x00006627
	// (remove) Token: 0x0600082F RID: 2095 RVA: 0x00008440 File Offset: 0x00006640
	public event tk2dButton.ButtonHandlerDelegate ButtonDownEvent;

	// Token: 0x1400004D RID: 77
	// (add) Token: 0x06000830 RID: 2096 RVA: 0x00008459 File Offset: 0x00006659
	// (remove) Token: 0x06000831 RID: 2097 RVA: 0x00008472 File Offset: 0x00006672
	public event tk2dButton.ButtonHandlerDelegate ButtonUpEvent;

	// Token: 0x06000832 RID: 2098 RVA: 0x0000848B File Offset: 0x0000668B
	private void OnEnable()
	{
		this.buttonDown = false;
	}

	// Token: 0x06000833 RID: 2099 RVA: 0x0003C2C0 File Offset: 0x0003A4C0
	private void Start()
	{
		if (this.viewCamera == null)
		{
			Transform transform = base.transform;
			while (transform && transform.camera == null)
			{
				transform = transform.parent;
			}
			if (transform && transform.camera != null)
			{
				this.viewCamera = transform.camera;
			}
			if (this.viewCamera == null && tk2dCamera.Instance)
			{
				this.viewCamera = tk2dCamera.Instance.camera;
			}
			if (this.viewCamera == null)
			{
				this.viewCamera = Camera.main;
			}
		}
		this.sprite = base.GetComponent<tk2dBaseSprite>();
		if (this.sprite)
		{
			this.UpdateSpriteIds();
		}
		if (base.collider == null)
		{
			BoxCollider boxCollider = base.gameObject.AddComponent<BoxCollider>();
			Vector3 size = boxCollider.size;
			size.z = 0.2f;
			boxCollider.size = size;
		}
		if ((this.buttonDownSound != null || this.buttonPressedSound != null || this.buttonUpSound != null) && base.audio == null)
		{
			AudioSource audioSource = base.gameObject.AddComponent<AudioSource>();
			audioSource.playOnAwake = false;
		}
	}

	// Token: 0x06000834 RID: 2100 RVA: 0x0003C434 File Offset: 0x0003A634
	public void UpdateSpriteIds()
	{
		this.buttonDownSpriteId = ((this.buttonDownSprite.Length <= 0) ? -1 : this.sprite.GetSpriteIdByName(this.buttonDownSprite));
		this.buttonUpSpriteId = ((this.buttonUpSprite.Length <= 0) ? -1 : this.sprite.GetSpriteIdByName(this.buttonUpSprite));
		this.buttonPressedSpriteId = ((this.buttonPressedSprite.Length <= 0) ? -1 : this.sprite.GetSpriteIdByName(this.buttonPressedSprite));
	}

	// Token: 0x06000835 RID: 2101 RVA: 0x00008494 File Offset: 0x00006694
	private void PlaySound(AudioClip source)
	{
		if (base.audio && source)
		{
			base.audio.PlayOneShot(source);
		}
	}

	// Token: 0x06000836 RID: 2102 RVA: 0x0003C4CC File Offset: 0x0003A6CC
	private IEnumerator coScale(Vector3 defaultScale, float startScale, float endScale)
	{
		float t0 = Time.realtimeSinceStartup;
		Vector3 scale = defaultScale;
		for (float s = 0f; s < this.scaleTime; s = Time.realtimeSinceStartup - t0)
		{
			float t = Mathf.Clamp01(s / this.scaleTime);
			float scl = Mathf.Lerp(startScale, endScale, t);
			scale = defaultScale * scl;
			base.transform.localScale = scale;
			yield return 0;
		}
		base.transform.localScale = defaultScale * endScale;
		yield break;
	}

	// Token: 0x06000837 RID: 2103 RVA: 0x0003C514 File Offset: 0x0003A714
	private IEnumerator LocalWaitForSeconds(float seconds)
	{
		float t0 = Time.realtimeSinceStartup;
		for (float s = 0f; s < seconds; s = Time.realtimeSinceStartup - t0)
		{
			yield return 0;
		}
		yield break;
	}

	// Token: 0x06000838 RID: 2104 RVA: 0x0003C538 File Offset: 0x0003A738
	private IEnumerator coHandleButtonPress(int fingerId)
	{
		this.buttonDown = true;
		bool buttonPressed = true;
		Vector3 defaultScale = base.transform.localScale;
		if (this.targetScale != 1f)
		{
			yield return base.StartCoroutine(this.coScale(defaultScale, 1f, this.targetScale));
		}
		this.PlaySound(this.buttonDownSound);
		if (this.buttonDownSpriteId != -1)
		{
			this.sprite.spriteId = this.buttonDownSpriteId;
		}
		if (this.ButtonDownEvent != null)
		{
			this.ButtonDownEvent(this);
		}
		for (;;)
		{
			Vector3 cursorPosition = Vector3.zero;
			bool cursorActive = true;
			if (fingerId != -1)
			{
				bool found = false;
				for (int i = 0; i < Input.touchCount; i++)
				{
					Touch touch = Input.GetTouch(i);
					if (touch.fingerId == fingerId)
					{
						if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
						{
							break;
						}
						cursorPosition = touch.position;
						found = true;
					}
				}
				if (!found)
				{
					cursorActive = false;
				}
			}
			else
			{
				if (!Input.GetMouseButton(0))
				{
					cursorActive = false;
				}
				cursorPosition = Input.mousePosition;
			}
			if (!cursorActive)
			{
				break;
			}
			Ray ray = this.viewCamera.ScreenPointToRay(cursorPosition);
			RaycastHit hitInfo;
			bool colliderHit = base.collider.Raycast(ray, out hitInfo, float.PositiveInfinity);
			if (buttonPressed && !colliderHit)
			{
				if (this.targetScale != 1f)
				{
					yield return base.StartCoroutine(this.coScale(defaultScale, this.targetScale, 1f));
				}
				this.PlaySound(this.buttonUpSound);
				if (this.buttonUpSpriteId != -1)
				{
					this.sprite.spriteId = this.buttonUpSpriteId;
				}
				if (this.ButtonUpEvent != null)
				{
					this.ButtonUpEvent(this);
				}
				buttonPressed = false;
			}
			else if (!buttonPressed && colliderHit)
			{
				if (this.targetScale != 1f)
				{
					yield return base.StartCoroutine(this.coScale(defaultScale, 1f, this.targetScale));
				}
				this.PlaySound(this.buttonDownSound);
				if (this.buttonDownSpriteId != -1)
				{
					this.sprite.spriteId = this.buttonDownSpriteId;
				}
				if (this.ButtonDownEvent != null)
				{
					this.ButtonDownEvent(this);
				}
				buttonPressed = true;
			}
			if (buttonPressed && this.ButtonAutoFireEvent != null)
			{
				this.ButtonAutoFireEvent(this);
			}
			yield return 0;
		}
		if (buttonPressed)
		{
			if (this.targetScale != 1f)
			{
				yield return base.StartCoroutine(this.coScale(defaultScale, this.targetScale, 1f));
			}
			this.PlaySound(this.buttonPressedSound);
			if (this.buttonPressedSpriteId != -1)
			{
				this.sprite.spriteId = this.buttonPressedSpriteId;
			}
			if (this.targetObject)
			{
				this.targetObject.SendMessage(this.messageName);
			}
			if (this.ButtonUpEvent != null)
			{
				this.ButtonUpEvent(this);
			}
			if (this.ButtonPressedEvent != null)
			{
				this.ButtonPressedEvent(this);
			}
			if (base.gameObject.activeInHierarchy)
			{
				yield return base.StartCoroutine(this.LocalWaitForSeconds(this.pressedWaitTime));
			}
			if (this.buttonUpSpriteId != -1)
			{
				this.sprite.spriteId = this.buttonUpSpriteId;
			}
		}
		this.buttonDown = false;
		yield break;
	}

	// Token: 0x06000839 RID: 2105 RVA: 0x0003C564 File Offset: 0x0003A764
	private void Update()
	{
		if (this.buttonDown)
		{
			return;
		}
		bool flag = false;
		if (Input.multiTouchEnabled)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch(i);
				if (touch.phase == TouchPhase.Began)
				{
					Ray ray = this.viewCamera.ScreenPointToRay(touch.position);
					RaycastHit raycastHit;
					if (base.collider.Raycast(ray, out raycastHit, 100000000f) && !Physics.Raycast(ray, raycastHit.distance - 0.01f))
					{
						base.StartCoroutine(this.coHandleButtonPress(touch.fingerId));
						flag = true;
						break;
					}
				}
			}
		}
		if (!flag && Input.GetMouseButtonDown(0))
		{
			Ray ray2 = this.viewCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit raycastHit2;
			if (base.collider.Raycast(ray2, out raycastHit2, 100000000f) && !Physics.Raycast(ray2, raycastHit2.distance - 0.01f))
			{
				base.StartCoroutine(this.coHandleButtonPress(-1));
			}
		}
	}

	// Token: 0x0400097A RID: 2426
	public Camera viewCamera;

	// Token: 0x0400097B RID: 2427
	public string buttonDownSprite = "button_down";

	// Token: 0x0400097C RID: 2428
	public string buttonUpSprite = "button_up";

	// Token: 0x0400097D RID: 2429
	public string buttonPressedSprite = "button_up";

	// Token: 0x0400097E RID: 2430
	private int buttonDownSpriteId = -1;

	// Token: 0x0400097F RID: 2431
	private int buttonUpSpriteId = -1;

	// Token: 0x04000980 RID: 2432
	private int buttonPressedSpriteId = -1;

	// Token: 0x04000981 RID: 2433
	public AudioClip buttonDownSound;

	// Token: 0x04000982 RID: 2434
	public AudioClip buttonUpSound;

	// Token: 0x04000983 RID: 2435
	public AudioClip buttonPressedSound;

	// Token: 0x04000984 RID: 2436
	public GameObject targetObject;

	// Token: 0x04000985 RID: 2437
	public string messageName = string.Empty;

	// Token: 0x04000986 RID: 2438
	private tk2dBaseSprite sprite;

	// Token: 0x04000987 RID: 2439
	private bool buttonDown;

	// Token: 0x04000988 RID: 2440
	public float targetScale = 1.1f;

	// Token: 0x04000989 RID: 2441
	public float scaleTime = 0.05f;

	// Token: 0x0400098A RID: 2442
	public float pressedWaitTime = 0.3f;

	// Token: 0x0200015A RID: 346
	// (Invoke) Token: 0x0600083B RID: 2107
	public delegate void ButtonHandlerDelegate(tk2dButton source);
}
