using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Throw
{
	// Token: 0x0200021B RID: 539
	public class JinChanWuDuSha : CombatSkillEffectBase
	{
		// Token: 0x06002F20 RID: 12064 RVA: 0x00211EE5 File Offset: 0x002100E5
		public JinChanWuDuSha()
		{
		}

		// Token: 0x06002F21 RID: 12065 RVA: 0x00211EEF File Offset: 0x002100EF
		public JinChanWuDuSha(CombatSkillKey skillKey) : base(skillKey, 15404, -1)
		{
		}

		// Token: 0x06002F22 RID: 12066 RVA: 0x00211F00 File Offset: 0x00210100
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002F23 RID: 12067 RVA: 0x00211F27 File Offset: 0x00210127
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002F24 RID: 12068 RVA: 0x00211F50 File Offset: 0x00210150
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !DomainManager.Combat.InAttackRange(base.CombatChar);
			if (!flag)
			{
				OuterAndInnerInts penetrations = base.CurrEnemyChar.GetCharacter().GetPenetrations();
				this._reduceProperty = -(base.IsDirect ? penetrations.Outer : penetrations.Inner) * 20 / 100;
				base.AppendAffectedData(context, base.CurrEnemyChar.GetId(), base.IsDirect ? 46 : 47, EDataModifyType.Add, -1);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06002F25 RID: 12069 RVA: 0x00211FEC File Offset: 0x002101EC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06002F26 RID: 12070 RVA: 0x00212024 File Offset: 0x00210224
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CurrEnemyChar.GetId();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == (base.IsDirect ? 46 : 47);
				if (flag2)
				{
					result = this._reduceProperty;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E00 RID: 3584
		private const int PropertyReducePercent = 20;

		// Token: 0x04000E01 RID: 3585
		private int _reduceProperty;
	}
}
