using System;
using System.Collections.Generic;

// Token: 0x02000119 RID: 281
public class DialogSceneStepsProgress
{
	// Token: 0x06000642 RID: 1602 RVA: 0x00006BD9 File Offset: 0x00004DD9
	public DialogSceneStepsProgress(List<DialogSceneStep> sceneSteps)
	{
		this.steps = sceneSteps;
		this.stepIndex = -1;
	}

	// Token: 0x0400078F RID: 1935
	public List<DialogSceneStep> steps;

	// Token: 0x04000790 RID: 1936
	public int stepIndex;
}
