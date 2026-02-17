using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Music
{
	// Token: 0x020005C6 RID: 1478
	public class FengLaiYi : CombatSkillEffectBase
	{
		// Token: 0x060043CB RID: 17355 RVA: 0x0026CAAA File Offset: 0x0026ACAA
		public FengLaiYi()
		{
		}

		// Token: 0x060043CC RID: 17356 RVA: 0x0026CAB4 File Offset: 0x0026ACB4
		public FengLaiYi(CombatSkillKey skillKey) : base(skillKey, 3302, -1)
		{
		}

		// Token: 0x060043CD RID: 17357 RVA: 0x0026CAC8 File Offset: 0x0026ACC8
		public unsafe override void OnEnable(DataContext context)
		{
			Personalities selfPersonalities = this.CharObj.GetPersonalities();
			Personalities enemyPersonalities = base.CurrEnemyChar.GetCharacter().GetPersonalities();
			bool flag = *(ref selfPersonalities.Items.FixedElementField + 5) > *(ref enemyPersonalities.Items.FixedElementField + 5);
			if (flag)
			{
				this._addPower = (int)(*(ref selfPersonalities.Items.FixedElementField + 5) * 50 / 100);
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				base.ShowSpecialEffectTips(0);
				bool flag2 = *(ref selfPersonalities.Items.FixedElementField + 6) > *(ref enemyPersonalities.Items.FixedElementField + 6);
				if (flag2)
				{
					DomainManager.Combat.AddCombatState(context, base.IsDirect ? base.CombatChar : base.CurrEnemyChar, base.IsDirect ? 1 : 2, base.IsDirect ? 37 : 38, 250);
				}
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043CE RID: 17358 RVA: 0x0026CBE7 File Offset: 0x0026ADE7
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043CF RID: 17359 RVA: 0x0026CBFC File Offset: 0x0026ADFC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060043D0 RID: 17360 RVA: 0x0026CC34 File Offset: 0x0026AE34
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
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001421 RID: 5153
		private const short StatePower = 250;

		// Token: 0x04001422 RID: 5154
		private const short LuckyPowerPercent = 50;

		// Token: 0x04001423 RID: 5155
		private int _addPower;
	}
}
