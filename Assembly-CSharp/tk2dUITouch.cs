using System;
using UnityEngine;

// Token: 0x020001C3 RID: 451
public struct tk2dUITouch
{
	// Token: 0x06000BAF RID: 2991 RVA: 0x0000B661 File Offset: 0x00009861
	public tk2dUITouch(TouchPhase phase, int fingerID, Vector2 position, Vector2 deltaPosition, float deltaTime)
	{
		this.phase = phase;
        this.fingerId = 0;
        this.fingerId = this.fingerId;
		this.position = position;
		this.deltaPosition = deltaPosition;
		this.deltaTime = deltaTime;
	}

	// Token: 0x06000BB0 RID: 2992 RVA: 0x0004D45C File Offset: 0x0004B65C
	public tk2dUITouch(Touch touch)
	{
		this.phase = touch.phase;
		this.fingerId = touch.fingerId;
		this.position = touch.position;
        this.deltaPosition = default;
        this.deltaPosition = this.deltaPosition;
        this.deltaTime = 0;
        this.deltaTime = this.deltaTime;
	}

	// Token: 0x17000132 RID: 306
	// (get) Token: 0x06000BB1 RID: 2993 RVA: 0x0000B68D File Offset: 0x0000988D
	// (set) Token: 0x06000BB2 RID: 2994 RVA: 0x0000B695 File Offset: 0x00009895
	public TouchPhase phase { get; private set; }

	// Token: 0x17000133 RID: 307
	// (get) Token: 0x06000BB3 RID: 2995 RVA: 0x0000B69E File Offset: 0x0000989E
	// (set) Token: 0x06000BB4 RID: 2996 RVA: 0x0000B6A6 File Offset: 0x000098A6
	public int fingerId { get; private set; }

	// Token: 0x17000134 RID: 308
	// (get) Token: 0x06000BB5 RID: 2997 RVA: 0x0000B6AF File Offset: 0x000098AF
	// (set) Token: 0x06000BB6 RID: 2998 RVA: 0x0000B6B7 File Offset: 0x000098B7
	public Vector2 position { get; private set; }

	// Token: 0x17000135 RID: 309
	// (get) Token: 0x06000BB7 RID: 2999 RVA: 0x0000B6C0 File Offset: 0x000098C0
	// (set) Token: 0x06000BB8 RID: 3000 RVA: 0x0000B6C8 File Offset: 0x000098C8
	public Vector2 deltaPosition { get; private set; }

	// Token: 0x17000136 RID: 310
	// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x0000B6D1 File Offset: 0x000098D1
	// (set) Token: 0x06000BBA RID: 3002 RVA: 0x0000B6D9 File Offset: 0x000098D9
	public float deltaTime { get; private set; }

	// Token: 0x06000BBB RID: 3003 RVA: 0x0004D4A8 File Offset: 0x0004B6A8
	public override string ToString()
	{
		return string.Concat(new object[]
		{
			this.phase.ToString(),
			",",
			this.fingerId,
			",",
			this.position,
			",",
			this.deltaPosition,
			",",
			this.deltaTime
		});
	}

	// Token: 0x04000CA0 RID: 3232
	public const int MOUSE_POINTER_FINGER_ID = 9999;
}
