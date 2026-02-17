using System;
using System.Collections.Generic;

// Token: 0x0200000C RID: 12
public class AnimData
{
	// Token: 0x0600002A RID: 42 RVA: 0x00002725 File Offset: 0x00000925
	public AnimData(string name, float duration, Dictionary<string, float[]> events)
	{
		this.Name = name;
		this.Duration = duration;
		this.Events = events;
	}

	// Token: 0x0400000F RID: 15
	public readonly string Name;

	// Token: 0x04000010 RID: 16
	public readonly float Duration;

	// Token: 0x04000011 RID: 17
	public readonly Dictionary<string, float[]> Events;
}
