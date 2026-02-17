using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XueFeng
{
	// Token: 0x020002CF RID: 719
	public class BingZhuZhanLiuHe : CombatSkillEffectBase
	{
		// Token: 0x06003297 RID: 12951 RVA: 0x0021FF18 File Offset: 0x0021E118
		public BingZhuZhanLiuHe()
		{
		}

		// Token: 0x06003298 RID: 12952 RVA: 0x0021FF22 File Offset: 0x0021E122
		public BingZhuZhanLiuHe(CombatSkillKey skillKey) : base(skillKey, 17074, -1)
		{
		}

		// Token: 0x06003299 RID: 12953 RVA: 0x0021FF34 File Offset: 0x0021E134
		public override void OnEnable(DataContext context)
		{
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 1000, true);
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 1000, false);
			base.CombatChar.CanNormalAttackInPrepareSkill = true;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 9, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 14, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600329A RID: 12954 RVA: 0x00220019 File Offset: 0x0021E219
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600329B RID: 12955 RVA: 0x00220030 File Offset: 0x0021E230
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.CombatChar.CanNormalAttackInPrepareSkill = false;
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600329C RID: 12956 RVA: 0x00220074 File Offset: 0x0021E274
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 9 || dataKey.FieldId == 14;
			int result;
			if (flag)
			{
				result = 100;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					result = 60;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 102;
					if (flag3)
					{
						result = -60;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000EF7 RID: 3831
		private const sbyte AddAttribute = 100;

		// Token: 0x04000EF8 RID: 3832
		private const sbyte ChangeDamagePercent = 60;
	}
}
