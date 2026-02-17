using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Assist
{
	// Token: 0x020005AD RID: 1453
	public class BuffTeammateCommand : AssistSkillBase
	{
		// Token: 0x06004336 RID: 17206 RVA: 0x0026A7A1 File Offset: 0x002689A1
		protected BuffTeammateCommand()
		{
		}

		// Token: 0x06004337 RID: 17207 RVA: 0x0026A7AB File Offset: 0x002689AB
		protected BuffTeammateCommand(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
			this.SetConstAffectingOnCombatBegin = DomainManager.Combat.IsMainCharacter(base.CombatChar);
		}

		// Token: 0x06004338 RID: 17208 RVA: 0x0026A7D0 File Offset: 0x002689D0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool flag = DomainManager.Combat.IsMainCharacter(base.CombatChar);
			if (flag)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 183 : 184, -1, -1, -1, -1), base.IsDirect ? EDataModifyType.AddPercent : EDataModifyType.Add);
			}
			else
			{
				Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			}
		}

		// Token: 0x06004339 RID: 17209 RVA: 0x0026A858 File Offset: 0x00268A58
		private void OnCombatBegin(DataContext context)
		{
			base.RemoveSelf(context);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x0600433A RID: 17210 RVA: 0x0026A875 File Offset: 0x00268A75
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			base.SetConstAffecting(context, base.CanAffect);
		}

		// Token: 0x0600433B RID: 17211 RVA: 0x0026A888 File Offset: 0x00268A88
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this.AffectImplement((ETeammateCommandImplement)dataKey.CustomParam0) || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 183;
				if (flag2)
				{
					result = -50;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 184;
					if (flag3)
					{
						result = (int)this.CommandPowerUpPercent;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x0600433C RID: 17212 RVA: 0x0026A8FC File Offset: 0x00268AFC
		private bool AffectImplement(ETeammateCommandImplement implement)
		{
			foreach (ETeammateCommandImplement teammateCommandImplement in this.AffectTeammateCommandType)
			{
				bool flag = implement == teammateCommandImplement;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040013EF RID: 5103
		private const sbyte RequirePrepareValueReduce = -50;

		// Token: 0x040013F0 RID: 5104
		protected ETeammateCommandImplement[] AffectTeammateCommandType;

		// Token: 0x040013F1 RID: 5105
		protected sbyte CommandPowerUpPercent;
	}
}
