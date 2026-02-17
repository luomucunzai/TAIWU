using System;
using NLog;
using NLog.Targets;

namespace GameData.GameDataBridge
{
	// Token: 0x0200001E RID: 30
	[Target("ErrorMessages")]
	public class ErrorMessagesTarget : TargetWithLayout
	{
		// Token: 0x06000CDE RID: 3294 RVA: 0x000DC5B2 File Offset: 0x000DA7B2
		public ErrorMessagesTarget(string name)
		{
			base.Name = name;
			base.OptimizeBufferReuse = true;
		}

		// Token: 0x06000CDF RID: 3295 RVA: 0x000DC5CC File Offset: 0x000DA7CC
		protected override void Write(LogEventInfo logEvent)
		{
			string logMessage = this.Layout.Render(logEvent);
			GameDataBridge.AppendErrorMessage(logMessage);
		}
	}
}
