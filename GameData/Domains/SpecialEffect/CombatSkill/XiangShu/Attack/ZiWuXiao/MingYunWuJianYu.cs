using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.ZiWuXiao
{
	// Token: 0x020002BA RID: 698
	public class MingYunWuJianYu : CombatSkillEffectBase
	{
		// Token: 0x0600323A RID: 12858 RVA: 0x0021E9AF File Offset: 0x0021CBAF
		public MingYunWuJianYu()
		{
		}

		// Token: 0x0600323B RID: 12859 RVA: 0x0021E9B9 File Offset: 0x0021CBB9
		public MingYunWuJianYu(CombatSkillKey skillKey) : base(skillKey, 17115, -1)
		{
		}

		// Token: 0x0600323C RID: 12860 RVA: 0x0021E9CC File Offset: 0x0021CBCC
		public override void OnEnable(DataContext context)
		{
			SkillEffectKey effectKey = new SkillEffectKey(876, true);
			Dictionary<SkillEffectKey, short> effectDict = base.CombatChar.GetSkillEffectCollection().EffectDict;
			short count;
			int effectCount = (int)((effectDict != null && effectDict.TryGetValue(effectKey, out count)) ? count : 0);
			this._addPower = effectCount * 40;
			bool flag = this._addPower > 0;
			if (flag)
			{
				base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, effectKey, (short)(-(short)effectCount), true, false);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600323D RID: 12861 RVA: 0x0021EA6B File Offset: 0x0021CC6B
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600323E RID: 12862 RVA: 0x0021EA80 File Offset: 0x0021CC80
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					SkillEffectKey effectKey = new SkillEffectKey(874, true);
					Dictionary<SkillEffectKey, short> effectDict = base.CombatChar.GetSkillEffectCollection().EffectDict;
					bool flag3 = effectDict != null && effectDict.ContainsKey(effectKey);
					if (flag3)
					{
						DomainManager.Combat.CastSkillFree(context, base.CombatChar, 877, ECombatCastFreePriority.Normal);
						base.ShowSpecialEffectTips(1);
					}
					else
					{
						bool flag4 = base.EnemyChar.WorsenAllInjury(context, WorsenConstants.SpecialPercentMingYunWuJianYu);
						if (flag4)
						{
							base.ShowSpecialEffectTips(2);
						}
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600323F RID: 12863 RVA: 0x0021EB3C File Offset: 0x0021CD3C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey;
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

		// Token: 0x04000EE1 RID: 3809
		private const sbyte AddPowerUnit = 40;

		// Token: 0x04000EE2 RID: 3810
		private int _addPower;
	}
}
