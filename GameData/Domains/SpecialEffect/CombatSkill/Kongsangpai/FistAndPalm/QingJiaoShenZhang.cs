using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.FistAndPalm
{
	// Token: 0x0200048F RID: 1167
	public class QingJiaoShenZhang : CombatSkillEffectBase
	{
		// Token: 0x06003C10 RID: 15376 RVA: 0x0024BE9B File Offset: 0x0024A09B
		public QingJiaoShenZhang()
		{
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x0024BEA5 File Offset: 0x0024A0A5
		public QingJiaoShenZhang(CombatSkillKey skillKey) : base(skillKey, 10105, -1)
		{
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x0024BEB8 File Offset: 0x0024A0B8
		public override void OnEnable(DataContext context)
		{
			DefeatMarkCollection markCollection = (base.IsDirect ? DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true) : base.CombatChar).GetDefeatMarkCollection();
			this._addPower = 4 * (markCollection.OuterInjuryMarkList.Sum() + markCollection.InnerInjuryMarkList.Sum());
			bool flag = this._addPower > 0;
			if (flag)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x0024BF6E File Offset: 0x0024A16E
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x0024BF84 File Offset: 0x0024A184
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					int changeCount = DomainManager.Combat.ChangeToOldInjury(context, base.CurrEnemyChar, 18, null);
					bool flag3 = changeCount > 0;
					if (flag3)
					{
						base.ShowSpecialEffectTips(1);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003C15 RID: 15381 RVA: 0x0024BFF0 File Offset: 0x0024A1F0
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

		// Token: 0x040011AA RID: 4522
		private const sbyte AddPowerUnit = 4;

		// Token: 0x040011AB RID: 4523
		private const sbyte ChangeToOldInjuryCount = 18;

		// Token: 0x040011AC RID: 4524
		private int _addPower;
	}
}
