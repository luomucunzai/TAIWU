using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.LongYuFu
{
	// Token: 0x020002F9 RID: 761
	public class ChiNiZhengYi : CombatSkillEffectBase
	{
		// Token: 0x0600338D RID: 13197 RVA: 0x00225639 File Offset: 0x00223839
		public ChiNiZhengYi()
		{
		}

		// Token: 0x0600338E RID: 13198 RVA: 0x00225643 File Offset: 0x00223843
		public ChiNiZhengYi(CombatSkillKey skillKey) : base(skillKey, 17121, -1)
		{
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x00225654 File Offset: 0x00223854
		public override void OnEnable(DataContext context)
		{
			Injuries injuries = base.CombatChar.GetInjuries();
			int addedInjury = 0;
			bool flag = injuries.Get(5, false) < 4;
			if (flag)
			{
				DomainManager.Combat.AddInjury(context, base.CombatChar, 5, false, 1, true, false);
				addedInjury++;
			}
			bool flag2 = injuries.Get(6, false) < 4;
			if (flag2)
			{
				DomainManager.Combat.AddInjury(context, base.CombatChar, 6, false, 1, true, false);
				addedInjury++;
			}
			bool flag3 = addedInjury > 0;
			if (flag3)
			{
				int enemyCharId = base.CurrEnemyChar.GetId();
				Dictionary<CombatSkillKey, SkillPowerChangeCollection> powerDict = DomainManager.Combat.GetAllSkillPowerAddInCombat();
				List<CombatSkillKey> srcSkillRandomPool = ObjectPool<List<CombatSkillKey>>.Instance.Get();
				srcSkillRandomPool.Clear();
				foreach (CombatSkillKey skillKey in powerDict.Keys)
				{
					bool flag4 = skillKey.CharId == enemyCharId;
					if (flag4)
					{
						srcSkillRandomPool.Add(skillKey);
					}
				}
				bool flag5 = srcSkillRandomPool.Count > 0;
				if (flag5)
				{
					int transferCount = Math.Min(2 * addedInjury, srcSkillRandomPool.Count);
					for (int i = 0; i < transferCount; i++)
					{
						int index = context.Random.Next(0, srcSkillRandomPool.Count);
						CombatSkillKey srcKey = srcSkillRandomPool[index];
						SkillPowerChangeCollection powerAddCollection = DomainManager.Combat.RemoveSkillPowerAddInCombat(context, srcKey);
						srcSkillRandomPool.RemoveAt(index);
						bool flag6 = powerAddCollection != null;
						if (flag6)
						{
							DomainManager.Combat.AddSkillPowerInCombat(context, this.SkillKey, new SkillEffectKey(base.SkillTemplateId, true), powerAddCollection.GetTotalChangeValue());
						}
					}
					base.ShowSpecialEffectTips(0);
				}
				ObjectPool<List<CombatSkillKey>>.Instance.Return(srcSkillRandomPool);
				DomainManager.Combat.AddToCheckFallenSet(base.CombatChar.GetId());
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x0022584C File Offset: 0x00223A4C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x00225864 File Offset: 0x00223A64
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F3D RID: 3901
		private const sbyte InjuryThreshold = 4;

		// Token: 0x04000F3E RID: 3902
		private const sbyte PowerCountPerInjury = 2;
	}
}
