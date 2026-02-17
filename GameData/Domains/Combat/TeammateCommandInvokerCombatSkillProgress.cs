using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x02000702 RID: 1794
	public class TeammateCommandInvokerCombatSkillProgress : TeammateCommandInvokerBase
	{
		// Token: 0x060067E6 RID: 26598 RVA: 0x003B29B0 File Offset: 0x003B0BB0
		public TeammateCommandInvokerCombatSkillProgress(int charId, int index) : base(charId, index)
		{
			int[] autoProgress = base.CmdConfig.AutoProgress;
			Tester.Assert(autoProgress != null && autoProgress.Length > 0, "CmdConfig.AutoProgress is { Length: > 0 }");
		}

		// Token: 0x060067E7 RID: 26599 RVA: 0x003B29EA File Offset: 0x003B0BEA
		public override void Setup()
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_PrepareSkillProgressChange(new Events.OnPrepareSkillProgressChange(this.OnPrepareSkillProgressChange));
		}

		// Token: 0x060067E8 RID: 26600 RVA: 0x003B2A11 File Offset: 0x003B0C11
		public override void Close()
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_PrepareSkillProgressChange(new Events.OnPrepareSkillProgressChange(this.OnPrepareSkillProgressChange));
		}

		// Token: 0x060067E9 RID: 26601 RVA: 0x003B2A38 File Offset: 0x003B0C38
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != this.MainCharId;
			if (!flag)
			{
				this._lastProgress = 0;
			}
		}

		// Token: 0x060067EA RID: 26602 RVA: 0x003B2A60 File Offset: 0x003B0C60
		private void OnPrepareSkillProgressChange(DataContext context, int charId, bool isAlly, short skillId, sbyte preparePercent)
		{
			bool flag = charId != this.MainCharId;
			if (!flag)
			{
				bool anyCheck = false;
				foreach (int progress in base.CmdConfig.AutoProgress)
				{
					bool flag2 = (int)preparePercent >= progress && this._lastProgress < progress;
					if (flag2)
					{
						anyCheck = true;
					}
				}
				bool flag3 = anyCheck && context.Random.CheckPercentProb(base.CmdConfig.AutoProb);
				if (flag3)
				{
					base.Execute(context);
				}
				this._lastProgress = (int)preparePercent;
			}
		}

		// Token: 0x04001C57 RID: 7255
		private int _lastProgress;
	}
}
