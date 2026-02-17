using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.FistAndPalm
{
	// Token: 0x020004C3 RID: 1219
	public class XuMiShanSiShenZhang : CombatSkillEffectBase
	{
		// Token: 0x06003D0F RID: 15631 RVA: 0x0024FB0D File Offset: 0x0024DD0D
		public XuMiShanSiShenZhang()
		{
		}

		// Token: 0x06003D10 RID: 15632 RVA: 0x0024FB17 File Offset: 0x0024DD17
		public XuMiShanSiShenZhang(CombatSkillKey skillKey) : base(skillKey, 11106, -1)
		{
		}

		// Token: 0x06003D11 RID: 15633 RVA: 0x0024FB28 File Offset: 0x0024DD28
		public override void OnEnable(DataContext context)
		{
			this._attackRangeFieldId = (base.IsDirect ? 146 : 145);
			base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
			base.CreateAffectedData(this._attackRangeFieldId, EDataModifyType.Add, base.SkillTemplateId);
			base.CreateAffectedData(327, EDataModifyType.Custom, -1);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.RegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
		}

		// Token: 0x06003D12 RID: 15634 RVA: 0x0024FBBB File Offset: 0x0024DDBB
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.UnRegisterHandler_CostBreathAndStance(new Events.OnCostBreathAndStance(this.OnCostBreathAndStance));
		}

		// Token: 0x06003D13 RID: 15635 RVA: 0x0024FBF4 File Offset: 0x0024DDF4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.EffectCount == (int)base.MaxEffectCount && this._costBreathOrStance > 0;
				if (flag2)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						base.ChangeStanceValue(context, base.CombatChar, this._costBreathOrStance);
					}
					else
					{
						base.ChangeBreathValue(context, base.CombatChar, this._costBreathOrStance);
					}
					this._costBreathOrStance = 0;
					base.ShowSpecialEffectTips(3);
				}
				bool flag3 = base.PowerMatchAffectRequire((int)power, 0) && base.EffectCount < (int)base.MaxEffectCount;
				if (flag3)
				{
					SkillEffectKey key = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
					bool flag4 = base.EffectCount == 0;
					if (flag4)
					{
						DomainManager.Combat.AddSkillEffect(context, base.CombatChar, key, 1, base.MaxEffectCount, true);
					}
					else
					{
						DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, key, 1, true, false);
					}
				}
			}
		}

		// Token: 0x06003D14 RID: 15636 RVA: 0x0024FD00 File Offset: 0x0024DF00
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				bool flag2 = oldCount < 1 != newCount < 1;
				if (flag2)
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				}
				bool flag3 = newCount > oldCount;
				if (flag3)
				{
					for (int i = 1; i < 4; i++)
					{
						bool flag4 = (int)oldCount < i != (int)newCount < i;
						if (flag4)
						{
							base.ShowSpecialEffectTips((byte)(i - 1));
						}
					}
				}
				bool flag5 = newCount <= 0 && !removed;
				if (flag5)
				{
					DomainManager.Combat.RemoveSkillEffect(context, base.CombatChar, key);
				}
			}
		}

		// Token: 0x06003D15 RID: 15637 RVA: 0x0024FDD0 File Offset: 0x0024DFD0
		private void OnCostBreathAndStance(DataContext context, int charId, bool isAlly, int costBreath, int costStance, short skillId)
		{
			bool flag = base.CharacterId != charId || skillId != base.SkillTemplateId || base.EffectCount < (int)base.MaxEffectCount;
			if (!flag)
			{
				this._costBreathOrStance = (base.IsDirect ? costStance : costBreath);
			}
		}

		// Token: 0x06003D16 RID: 15638 RVA: 0x0024FE1C File Offset: 0x0024E01C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || base.EffectCount == 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199 && base.EffectCount >= 1;
				if (flag2)
				{
					result = 40;
				}
				else
				{
					bool flag3 = dataKey.FieldId == this._attackRangeFieldId && base.EffectCount >= 2;
					if (flag3)
					{
						result = 30;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x06003D17 RID: 15639 RVA: 0x0024FEAC File Offset: 0x0024E0AC
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey == this.SkillKey && dataKey.FieldId == 327 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1) && base.EffectCount >= 3 && dataKey.CustomParam2 == 1;
			return !flag && base.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x040011F0 RID: 4592
		private const sbyte AddPower = 40;

		// Token: 0x040011F1 RID: 4593
		private const sbyte AddAttackRange = 30;

		// Token: 0x040011F2 RID: 4594
		private ushort _attackRangeFieldId;

		// Token: 0x040011F3 RID: 4595
		private int _costBreathOrStance;
	}
}
