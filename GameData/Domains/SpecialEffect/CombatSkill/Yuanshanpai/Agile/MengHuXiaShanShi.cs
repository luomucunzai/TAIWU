using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Agile
{
	// Token: 0x02000214 RID: 532
	public class MengHuXiaShanShi : AgileSkillBase
	{
		// Token: 0x06002EF6 RID: 12022 RVA: 0x0021119F File Offset: 0x0020F39F
		public MengHuXiaShanShi()
		{
		}

		// Token: 0x06002EF7 RID: 12023 RVA: 0x002111A9 File Offset: 0x0020F3A9
		public MengHuXiaShanShi(CombatSkillKey skillKey) : base(skillKey, 5401)
		{
		}

		// Token: 0x06002EF8 RID: 12024 RVA: 0x002111B9 File Offset: 0x0020F3B9
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CastLegSkillWithAgile(new Events.OnCastLegSkillWithAgile(this.OnCastLegSkillWithAgile));
		}

		// Token: 0x06002EF9 RID: 12025 RVA: 0x002111D6 File Offset: 0x0020F3D6
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CastLegSkillWithAgile(new Events.OnCastLegSkillWithAgile(this.OnCastLegSkillWithAgile));
		}

		// Token: 0x06002EFA RID: 12026 RVA: 0x002111F4 File Offset: 0x0020F3F4
		private void OnCastLegSkillWithAgile(DataContext context, CombatCharacter combatChar, short legSkillId)
		{
			bool flag = !base.CanAffect || combatChar != base.CombatChar;
			if (!flag)
			{
				this.AutoRemove = false;
				this._affectingLegSkill = legSkillId;
				bool flag2 = this.AffectDatas == null || this.AffectDatas.Count == 0;
				if (flag2)
				{
					base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 64 : 65, EDataModifyType.TotalPercent, legSkillId);
				}
				base.ShowSpecialEffectTips(0);
				Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			}
		}

		// Token: 0x06002EFB RID: 12027 RVA: 0x00211284 File Offset: 0x0020F484
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != this._affectingLegSkill;
			if (!flag)
			{
				Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
				bool agileSkillChanged = this.AgileSkillChanged;
				if (agileSkillChanged)
				{
					base.RemoveSelf(context);
				}
				else
				{
					this.AutoRemove = true;
				}
			}
		}

		// Token: 0x06002EFC RID: 12028 RVA: 0x002112E0 File Offset: 0x0020F4E0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != this._affectingLegSkill;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 64 || dataKey.FieldId == 65;
				if (flag2)
				{
					result = 60;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000DF1 RID: 3569
		private const sbyte PenetrateAddPercent = 60;

		// Token: 0x04000DF2 RID: 3570
		private short _affectingLegSkill;
	}
}
