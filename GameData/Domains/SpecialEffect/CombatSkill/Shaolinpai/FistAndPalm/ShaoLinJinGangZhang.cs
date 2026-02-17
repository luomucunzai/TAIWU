using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.FistAndPalm
{
	// Token: 0x02000429 RID: 1065
	public class ShaoLinJinGangZhang : CombatSkillEffectBase
	{
		// Token: 0x06003987 RID: 14727 RVA: 0x0023EEB1 File Offset: 0x0023D0B1
		public ShaoLinJinGangZhang()
		{
		}

		// Token: 0x06003988 RID: 14728 RVA: 0x0023EEBB File Offset: 0x0023D0BB
		public ShaoLinJinGangZhang(CombatSkillKey skillKey) : base(skillKey, 1101, -1)
		{
		}

		// Token: 0x06003989 RID: 14729 RVA: 0x0023EECC File Offset: 0x0023D0CC
		public override void OnEnable(DataContext context)
		{
			int currInnerRatio = (int)base.SkillInstance.GetCurrInnerRatio();
			this._changeInnerRatio = 0;
			this._innerRatioMaxChanged = (currInnerRatio == (base.IsDirect ? 100 : 0));
			bool innerRatioMaxChanged = this._innerRatioMaxChanged;
			if (innerRatioMaxChanged)
			{
				base.ShowSpecialEffectTips(1);
			}
			base.CreateAffectedData(203, EDataModifyType.Add, base.SkillTemplateId);
			base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
			base.CreateAffectedData(327, EDataModifyType.Custom, base.SkillTemplateId);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600398A RID: 14730 RVA: 0x0023EF60 File Offset: 0x0023D160
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600398B RID: 14731 RVA: 0x0023EF78 File Offset: 0x0023D178
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || power <= 0 || this._innerRatioMaxChanged;
			if (!flag)
			{
				this._changeInnerRatio += (int)(5 * power / 10 * (base.IsDirect ? 1 : -1));
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 203);
				base.ShowSpecialEffectTips(0);
				int currInnerRatio = (int)base.SkillInstance.GetCurrInnerRatio();
				this._innerRatioMaxChanged = (currInnerRatio == (base.IsDirect ? 100 : 0));
				bool innerRatioMaxChanged = this._innerRatioMaxChanged;
				if (innerRatioMaxChanged)
				{
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x0600398C RID: 14732 RVA: 0x0023F040 File Offset: 0x0023D240
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 203;
				if (flag2)
				{
					result = this._changeInnerRatio;
				}
				else
				{
					bool flag3 = !this._innerRatioMaxChanged;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						bool flag4 = dataKey.FieldId == 199;
						if (flag4)
						{
							result = 40;
						}
						else
						{
							result = 0;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600398D RID: 14733 RVA: 0x0023F0C0 File Offset: 0x0023D2C0
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey == this.SkillKey && dataKey.FieldId == 327 && dataKey.CustomParam2 == 1;
			return !flag && base.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x040010CD RID: 4301
		private const sbyte ChangeInnerRatioUnit = 5;

		// Token: 0x040010CE RID: 4302
		private const sbyte AddPower = 40;

		// Token: 0x040010CF RID: 4303
		private int _changeInnerRatio;

		// Token: 0x040010D0 RID: 4304
		private bool _innerRatioMaxChanged;
	}
}
