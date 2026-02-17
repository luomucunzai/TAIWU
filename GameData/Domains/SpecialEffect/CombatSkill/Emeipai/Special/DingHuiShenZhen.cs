using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.Combat.Ai.Memory;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Special
{
	// Token: 0x0200053F RID: 1343
	public class DingHuiShenZhen : CombatSkillEffectBase
	{
		// Token: 0x06003FF1 RID: 16369 RVA: 0x0025C48F File Offset: 0x0025A68F
		public DingHuiShenZhen()
		{
		}

		// Token: 0x06003FF2 RID: 16370 RVA: 0x0025C499 File Offset: 0x0025A699
		public DingHuiShenZhen(CombatSkillKey skillKey) : base(skillKey, 2405, -1)
		{
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x0025C4AA File Offset: 0x0025A6AA
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x0025C4BF File Offset: 0x0025A6BF
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x0025C4D4 File Offset: 0x0025A6D4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				AiMemory aiMemory = base.CurrEnemyChar.AiController.Memory;
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && aiMemory != null;
				if (flag2)
				{
					SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						foreach (short combatSkillId in aiMemory.EnemyRecordDict[base.CharacterId].SkillRecord.Keys)
						{
							CombatSkillKey skillKey = new CombatSkillKey(base.CharacterId, combatSkillId);
							SkillPowerChangeCollection skillPowerChangeCollection;
							bool flag3 = DomainManager.Combat.CombatSkillDataExist(skillKey) && (!DomainManager.Combat.TryGetElement_SkillPowerAddInCombat(skillKey, out skillPowerChangeCollection) || !DomainManager.Combat.GetElement_SkillPowerAddInCombat(skillKey).EffectDict.ContainsKey(effectKey));
							if (flag3)
							{
								DomainManager.Combat.AddSkillPowerInCombat(context, skillKey, effectKey, 40);
							}
						}
					}
					else
					{
						foreach (short combatSkillId2 in aiMemory.SelfRecord.SkillRecord.Keys)
						{
							CombatSkillKey skillKey2 = new CombatSkillKey(base.CurrEnemyChar.GetId(), combatSkillId2);
							SkillPowerChangeCollection skillPowerChangeCollection;
							bool flag4 = DomainManager.Combat.CombatSkillDataExist(skillKey2) && (!DomainManager.Combat.TryGetElement_SkillPowerReduceInCombat(skillKey2, out skillPowerChangeCollection) || !DomainManager.Combat.GetElement_SkillPowerReduceInCombat(skillKey2).EffectDict.ContainsKey(effectKey));
							if (flag4)
							{
								DomainManager.Combat.ReduceSkillPowerInCombat(context, skillKey2, effectKey, -40);
							}
						}
					}
					base.CurrEnemyChar.AiController.ClearMemories();
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x040012D6 RID: 4822
		private const sbyte ChangePower = 40;
	}
}
