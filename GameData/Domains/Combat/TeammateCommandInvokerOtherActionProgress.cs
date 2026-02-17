using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x02000703 RID: 1795
	public class TeammateCommandInvokerOtherActionProgress : TeammateCommandInvokerBase
	{
		// Token: 0x060067EB RID: 26603 RVA: 0x003B2AF0 File Offset: 0x003B0CF0
		public TeammateCommandInvokerOtherActionProgress(int charId, int index) : base(charId, index)
		{
			int[] autoProgress = base.CmdConfig.AutoProgress;
			Tester.Assert(autoProgress != null && autoProgress.Length > 0, "CmdConfig.AutoProgress is { Length: > 0 }");
		}

		// Token: 0x060067EC RID: 26604 RVA: 0x003B2B35 File Offset: 0x003B0D35
		private DataUid Parse(ushort fieldId)
		{
			return new DataUid(8, 10, (ulong)((long)this.MainCharId), (uint)fieldId);
		}

		// Token: 0x060067ED RID: 26605 RVA: 0x003B2B48 File Offset: 0x003B0D48
		private void Listen(ushort fieldId, Action<DataContext, DataUid> handler)
		{
			DataUid uid = this.Parse(fieldId);
			GameDataBridge.AddPostDataModificationHandler(uid, base.DataHandlerKey, handler);
			this._dataUids.Add(uid);
		}

		// Token: 0x060067EE RID: 26606 RVA: 0x003B2B79 File Offset: 0x003B0D79
		public override void Setup()
		{
			this.Listen(70, new Action<DataContext, DataUid>(this.OnOtherActionPreparePercentChanged));
			this.Listen(74, new Action<DataContext, DataUid>(this.OnUseItemPreparePercentChanged));
		}

		// Token: 0x060067EF RID: 26607 RVA: 0x003B2BA8 File Offset: 0x003B0DA8
		public override void Close()
		{
			foreach (DataUid uid in this._dataUids)
			{
				GameDataBridge.RemovePostDataModificationHandler(uid, base.DataHandlerKey);
			}
			this._dataUids.Clear();
		}

		// Token: 0x060067F0 RID: 26608 RVA: 0x003B2C10 File Offset: 0x003B0E10
		private void OnOtherActionPreparePercentChanged(DataContext context, DataUid arg2)
		{
			this.OnProgressChanged(context, ref this._lastProgressOtherAction, (int)base.MainChar.GetOtherActionPreparePercent());
		}

		// Token: 0x060067F1 RID: 26609 RVA: 0x003B2C2C File Offset: 0x003B0E2C
		private void OnUseItemPreparePercentChanged(DataContext context, DataUid arg2)
		{
			this.OnProgressChanged(context, ref this._lastProgressUseItem, (int)base.MainChar.GetUseItemPreparePercent());
		}

		// Token: 0x060067F2 RID: 26610 RVA: 0x003B2C48 File Offset: 0x003B0E48
		private void OnProgressChanged(DataContext context, ref int lastProgress, int newProgress)
		{
			bool anyCheck = false;
			foreach (int progress in base.CmdConfig.AutoProgress)
			{
				bool flag = newProgress >= progress && lastProgress < progress;
				if (flag)
				{
					anyCheck = true;
				}
			}
			bool flag2 = anyCheck && context.Random.CheckPercentProb(base.CmdConfig.AutoProb);
			if (flag2)
			{
				base.Execute(context);
			}
			lastProgress = newProgress;
		}

		// Token: 0x04001C58 RID: 7256
		private readonly List<DataUid> _dataUids = new List<DataUid>();

		// Token: 0x04001C59 RID: 7257
		private int _lastProgressOtherAction;

		// Token: 0x04001C5A RID: 7258
		private int _lastProgressUseItem;
	}
}
