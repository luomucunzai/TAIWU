using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Special
{
	// Token: 0x020004AA RID: 1194
	public class LuoChaChuFa : CombatSkillEffectBase
	{
		// Token: 0x06003CA1 RID: 15521 RVA: 0x0024E3F2 File Offset: 0x0024C5F2
		public LuoChaChuFa()
		{
		}

		// Token: 0x06003CA2 RID: 15522 RVA: 0x0024E3FC File Offset: 0x0024C5FC
		public LuoChaChuFa(CombatSkillKey skillKey) : base(skillKey, 11302, -1)
		{
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x0024E410 File Offset: 0x0024C610
		public override void OnEnable(DataContext context)
		{
			SkillEffectKey effectKey = DomainManager.Combat.GetUsingWeaponData(base.CombatChar).GetPestleEffect();
			this._addDamage = (int)((effectKey.SkillId >= 0) ? (10 * (Config.CombatSkill.Instance[effectKey.SkillId].Grade + 1)) : 0);
			bool flag = this._addDamage > 0;
			if (flag)
			{
				base.ShowSpecialEffectTips(0);
			}
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x0024E4B5 File Offset: 0x0024C6B5
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x0024E4CC File Offset: 0x0024C6CC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !interrupted;
				if (flag2)
				{
					DomainManager.Combat.GetUsingWeaponData(base.CombatChar).RemovePestleEffect(context);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x0024E524 File Offset: 0x0024C724
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || dataKey.CustomParam0 != (base.IsDirect ? 0 : 1);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					result = this._addDamage;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040011D6 RID: 4566
		private const sbyte AddDamageUnit = 10;

		// Token: 0x040011D7 RID: 4567
		private int _addDamage;
	}
}
