using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.FistAndPalm
{
	// Token: 0x02000427 RID: 1063
	public class LiuShiBaShiLuoHanQuan : CombatSkillEffectBase
	{
		// Token: 0x06003977 RID: 14711 RVA: 0x0023EA7E File Offset: 0x0023CC7E
		public LiuShiBaShiLuoHanQuan()
		{
		}

		// Token: 0x06003978 RID: 14712 RVA: 0x0023EA88 File Offset: 0x0023CC88
		public LiuShiBaShiLuoHanQuan(CombatSkillKey skillKey) : base(skillKey, 1103, -1)
		{
		}

		// Token: 0x06003979 RID: 14713 RVA: 0x0023EA9C File Offset: 0x0023CC9C
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 223, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600397A RID: 14714 RVA: 0x0023EAED File Offset: 0x0023CCED
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600397B RID: 14715 RVA: 0x0023EB04 File Offset: 0x0023CD04
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || power <= 0;
			if (!flag)
			{
				CombatCharacter defendChar = base.IsDirect ? base.CombatChar : base.CurrEnemyChar;
				bool flag2 = defendChar.GetAffectingDefendSkillId() < 0;
				if (!flag2)
				{
					int leftFrame = (int)defendChar.DefendSkillLeftFrame;
					int totalFrame = (int)defendChar.DefendSkillTotalFrame;
					defendChar.DefendSkillLeftFrame = (short)(base.IsDirect ? Math.Min(leftFrame + totalFrame * (int)power / 100, totalFrame) : Math.Max(leftFrame - totalFrame * (int)power / 100, 0));
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x0600397C RID: 14716 RVA: 0x0023EBA4 File Offset: 0x0023CDA4
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
				bool flag2 = dataKey.FieldId == 223;
				result = (flag2 || dataValue);
			}
			return result;
		}
	}
}
