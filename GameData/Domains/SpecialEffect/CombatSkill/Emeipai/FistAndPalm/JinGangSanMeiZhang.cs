using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.FistAndPalm
{
	// Token: 0x02000550 RID: 1360
	public class JinGangSanMeiZhang : CombatSkillEffectBase
	{
		// Token: 0x06004040 RID: 16448 RVA: 0x0025D386 File Offset: 0x0025B586
		public JinGangSanMeiZhang()
		{
		}

		// Token: 0x06004041 RID: 16449 RVA: 0x0025D390 File Offset: 0x0025B590
		public JinGangSanMeiZhang(CombatSkillKey skillKey) : base(skillKey, 2106, -1)
		{
		}

		// Token: 0x06004042 RID: 16450 RVA: 0x0025D3A4 File Offset: 0x0025B5A4
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 215, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 74, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06004043 RID: 16451 RVA: 0x0025D43C File Offset: 0x0025B63C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06004044 RID: 16452 RVA: 0x0025D478 File Offset: 0x0025B678
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._canAffect = true;
			}
		}

		// Token: 0x06004045 RID: 16453 RVA: 0x0025D4AC File Offset: 0x0025B6AC
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				DamageCompareData damageCompareData = DomainManager.Combat.GetDamageCompareData();
				bool flag2 = damageCompareData.AvoidValue[0] > damageCompareData.HitValue[0];
				if (flag2)
				{
					this.TryReduceNeiliAllocation(context);
				}
			}
		}

		// Token: 0x06004046 RID: 16454 RVA: 0x0025D508 File Offset: 0x0025B708
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index > 2;
			if (!flag)
			{
				DamageCompareData damageCompareData = DomainManager.Combat.GetDamageCompareData();
				bool flag2 = index < 2 && damageCompareData.AvoidValue[index + 1] > damageCompareData.HitValue[index + 1];
				if (flag2)
				{
					this.TryReduceNeiliAllocation(context);
				}
				bool flag3 = index == 2 && this._canAffect;
				if (flag3)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						damageCompareData.OuterDefendValue -= Math.Min(damageCompareData.InnerDefendValue * 50 / 100, damageCompareData.OuterDefendValue / 2);
					}
					else
					{
						damageCompareData.InnerDefendValue -= Math.Min(damageCompareData.OuterDefendValue * 50 / 100, damageCompareData.InnerDefendValue / 2);
					}
					DomainManager.Combat.SetDamageCompareData(damageCompareData, context);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06004047 RID: 16455 RVA: 0x0025D600 File Offset: 0x0025B800
		private unsafe void TryReduceNeiliAllocation(DataContext context)
		{
			bool flag = *(ref base.CombatChar.GetNeiliAllocation().Items.FixedElementField + (IntPtr)3 * 2) >= 5;
			if (flag)
			{
				base.CombatChar.ChangeNeiliAllocation(context, 3, -5, true, true);
			}
			else
			{
				this._canAffect = false;
			}
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x0025D654 File Offset: 0x0025B854
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 215;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x06004049 RID: 16457 RVA: 0x0025D6A8 File Offset: 0x0025B8A8
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || !this._canAffect;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 74;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x040012E2 RID: 4834
		private const sbyte ReduceNeiliAllocation = 5;

		// Token: 0x040012E3 RID: 4835
		private const sbyte ReducePenetrateResistPercent = 50;

		// Token: 0x040012E4 RID: 4836
		private bool _canAffect;
	}
}
