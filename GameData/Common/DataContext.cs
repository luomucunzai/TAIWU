using System;
using System.Runtime.CompilerServices;
using GameData.Domains.Character.Ai;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Common
{
	// Token: 0x020008F0 RID: 2288
	public class DataContext
	{
		// Token: 0x06008218 RID: 33304 RVA: 0x004D995B File Offset: 0x004D7B5B
		public DataContext(int threadId)
		{
			this.ThreadId = threadId;
			this.ParallelModificationsRecorder = new ParallelModificationsRecorder();
			this.Random = RandomDefaults.CreateRandomSource();
			this.Equipping = new Equipping();
			this.AdvanceMonthRelatedData = new AdvanceMonthRelatedData();
		}

		// Token: 0x06008219 RID: 33305 RVA: 0x004D9998 File Offset: 0x004D7B98
		public IRandomSource SwitchRandomSource(string purposeText)
		{
			ulong seed = this.Random.NextULong();
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Random seed switched to ");
			defaultInterpolatedStringHandler.AppendFormatted<ulong>(seed);
			defaultInterpolatedStringHandler.AppendLiteral(" for ");
			defaultInterpolatedStringHandler.AppendFormatted(purposeText);
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			return this.SwitchRandomSource(seed);
		}

		// Token: 0x0600821A RID: 33306 RVA: 0x004D9A00 File Offset: 0x004D7C00
		public IRandomSource SwitchRandomSource(IRandomSource random)
		{
			bool flag = this._randomSourceBackup == null;
			if (flag)
			{
				this._randomSourceBackup = this.Random;
			}
			else
			{
				AdaptableLog.TagWarning("Random", "Switching to a new random source before the previous one was restored.", false);
			}
			IRandomSource oriRandom = this.Random;
			this.Random = random;
			return oriRandom;
		}

		// Token: 0x0600821B RID: 33307 RVA: 0x004D9A50 File Offset: 0x004D7C50
		public IRandomSource SwitchRandomSource(ulong seed)
		{
			bool flag = this._randomSourceBackup == null;
			if (flag)
			{
				this._randomSourceBackup = this.Random;
			}
			else
			{
				AdaptableLog.TagWarning("Random", "Switching to a new random source before the previous one was restored.", false);
			}
			IRandomSource oriRandom = this.Random;
			this.Random = RandomDefaults.CreateRandomSource(seed);
			return oriRandom;
		}

		// Token: 0x0600821C RID: 33308 RVA: 0x004D9AA4 File Offset: 0x004D7CA4
		public void RestoreRandomSource()
		{
			bool flag = this._randomSourceBackup == null;
			if (flag)
			{
				AdaptableLog.TagWarning("Random", "Restoring random source when backup doesn't exist.", false);
			}
			else
			{
				this.Random = this._randomSourceBackup;
			}
			this._randomSourceBackup = null;
		}

		// Token: 0x04002420 RID: 9248
		public readonly int ThreadId;

		// Token: 0x04002421 RID: 9249
		public readonly ParallelModificationsRecorder ParallelModificationsRecorder;

		// Token: 0x04002422 RID: 9250
		public IRandomSource Random;

		// Token: 0x04002423 RID: 9251
		public readonly Equipping Equipping;

		// Token: 0x04002424 RID: 9252
		public readonly AdvanceMonthRelatedData AdvanceMonthRelatedData;

		// Token: 0x04002425 RID: 9253
		private IRandomSource _randomSourceBackup;
	}
}
