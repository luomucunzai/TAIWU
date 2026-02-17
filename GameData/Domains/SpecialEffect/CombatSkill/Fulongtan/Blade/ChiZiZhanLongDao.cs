using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Blade
{
	// Token: 0x0200052A RID: 1322
	public class ChiZiZhanLongDao : CombatSkillEffectBase
	{
		// Token: 0x17000251 RID: 593
		// (get) Token: 0x06003F5A RID: 16218 RVA: 0x002598A4 File Offset: 0x00257AA4
		private int AddPower
		{
			get
			{
				return base.IsDirect ? 40 : 20;
			}
		}

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06003F5B RID: 16219 RVA: 0x002598B4 File Offset: 0x00257AB4
		private int AddHitOdds
		{
			get
			{
				return base.IsDirect ? 300 : 150;
			}
		}

		// Token: 0x06003F5C RID: 16220 RVA: 0x002598CA File Offset: 0x00257ACA
		public ChiZiZhanLongDao()
		{
		}

		// Token: 0x06003F5D RID: 16221 RVA: 0x002598D4 File Offset: 0x00257AD4
		public ChiZiZhanLongDao(CombatSkillKey skillKey) : base(skillKey, 14206, -1)
		{
		}

		// Token: 0x06003F5E RID: 16222 RVA: 0x002598E8 File Offset: 0x00257AE8
		public override void OnEnable(DataContext context)
		{
			bool flag = this.CharObj.HasVirginity() == base.IsDirect;
			if (flag)
			{
				base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
				base.CreateAffectedData(74, EDataModifyType.AddPercent, base.SkillTemplateId);
				base.CreateAffectedData(327, EDataModifyType.Custom, base.SkillTemplateId);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F5F RID: 16223 RVA: 0x0025995F File Offset: 0x00257B5F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F60 RID: 16224 RVA: 0x00259974 File Offset: 0x00257B74
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003F61 RID: 16225 RVA: 0x002599AC File Offset: 0x00257BAC
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
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 74)
				{
					if (fieldId != 199)
					{
						num = 0;
					}
					else
					{
						num = this.AddPower;
					}
				}
				else
				{
					num = this.AddHitOdds;
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06003F62 RID: 16226 RVA: 0x00259A20 File Offset: 0x00257C20
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey == this.SkillKey && dataKey.FieldId == 327 && dataKey.CustomParam2 == 1;
			return !flag && base.GetModifiedValue(dataKey, dataValue);
		}
	}
}
