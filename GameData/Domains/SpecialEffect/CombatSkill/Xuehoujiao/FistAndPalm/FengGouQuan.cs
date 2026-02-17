using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.FistAndPalm
{
	// Token: 0x02000232 RID: 562
	public class FengGouQuan : CombatSkillEffectBase
	{
		// Token: 0x06002F83 RID: 12163 RVA: 0x00213669 File Offset: 0x00211869
		public FengGouQuan()
		{
		}

		// Token: 0x06002F84 RID: 12164 RVA: 0x00213673 File Offset: 0x00211873
		public FengGouQuan(CombatSkillKey skillKey) : base(skillKey, 15100, -1)
		{
		}

		// Token: 0x06002F85 RID: 12165 RVA: 0x00213684 File Offset: 0x00211884
		public override void OnEnable(DataContext context)
		{
			this._firstCast = true;
			this._costedSkillId = -1;
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.CastAttackSkillBegin));
		}

		// Token: 0x06002F86 RID: 12166 RVA: 0x002136D6 File Offset: 0x002118D6
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.CastAttackSkillBegin));
		}

		// Token: 0x06002F87 RID: 12167 RVA: 0x00213710 File Offset: 0x00211910
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool firstCast = this._firstCast;
				if (firstCast)
				{
					this._firstCast = false;
					base.AppendAffectedData(context, base.CharacterId, base.IsDirect ? 69 : 102, EDataModifyType.AddPercent, -1);
					base.AddMaxEffectCount(true);
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x06002F88 RID: 12168 RVA: 0x00213782 File Offset: 0x00211982
		private void CastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			this._costedSkillId = -1;
		}

		// Token: 0x06002F89 RID: 12169 RVA: 0x0021378C File Offset: 0x0021198C
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && !this._firstCast && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06002F8A RID: 12170 RVA: 0x002137DC File Offset: 0x002119DC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CombatSkillId < 0 || Config.CombatSkill.Instance[dataKey.CombatSkillId].EquipType != 1;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == (base.IsDirect ? 69 : 102) && (!base.IsDirect || (DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) && base.CombatChar.TeammateBeforeMainChar < 0));
				if (flag2)
				{
					DataContext context = DomainManager.Combat.Context;
					bool flag3 = dataKey.CombatSkillId != this._costedSkillId;
					if (flag3)
					{
						base.ShowSpecialEffectTips(0);
						base.ReduceEffectCount(1);
						this._costedSkillId = dataKey.CombatSkillId;
					}
					result = (context.Random.CheckPercentProb(60) ? (base.IsDirect ? 90 : -90) : (base.IsDirect ? -90 : 90));
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000E17 RID: 3607
		private const short DamageChangePercent = 90;

		// Token: 0x04000E18 RID: 3608
		private const sbyte BuffOdds = 60;

		// Token: 0x04000E19 RID: 3609
		private bool _firstCast;

		// Token: 0x04000E1A RID: 3610
		private short _costedSkillId;
	}
}
