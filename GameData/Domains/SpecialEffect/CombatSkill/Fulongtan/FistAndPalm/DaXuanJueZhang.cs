using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm
{
	// Token: 0x02000519 RID: 1305
	public class DaXuanJueZhang : CombatSkillEffectBase
	{
		// Token: 0x06003EFA RID: 16122 RVA: 0x00257AAB File Offset: 0x00255CAB
		public DaXuanJueZhang()
		{
		}

		// Token: 0x06003EFB RID: 16123 RVA: 0x00257AB5 File Offset: 0x00255CB5
		public DaXuanJueZhang(CombatSkillKey skillKey) : base(skillKey, 14104, -1)
		{
		}

		// Token: 0x06003EFC RID: 16124 RVA: 0x00257AC8 File Offset: 0x00255CC8
		public override void OnEnable(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
			List<short> selfSkillList = ObjectPool<List<short>>.Instance.Get();
			List<short> enemySkillList = ObjectPool<List<short>>.Instance.Get();
			SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
			selfSkillList.Clear();
			selfSkillList.AddRange(base.CombatChar.GetCombatSkillList(base.IsDirect ? 3 : 2));
			selfSkillList.RemoveAll((short id) => id < 0);
			enemySkillList.Clear();
			enemySkillList.AddRange(enemyChar.GetCombatSkillList(base.IsDirect ? 3 : 2));
			enemySkillList.RemoveAll((short id) => id < 0);
			DomainManager.Combat.RemoveSkillPowerAddInCombat(context, this.SkillKey, effectKey);
			bool flag = selfSkillList.Count > 0 || enemySkillList.Count > 0;
			if (flag)
			{
				int addPower = 0;
				List<CombatSkillKey> skillKeyRandomPool = ObjectPool<List<CombatSkillKey>>.Instance.Get();
				skillKeyRandomPool.Clear();
				bool flag2 = selfSkillList.Count > 0;
				if (flag2)
				{
					short maxPower = 0;
					for (int i = 0; i < selfSkillList.Count; i++)
					{
						short skillId = selfSkillList[i];
						CombatSkillKey skillKey = new CombatSkillKey(base.CharacterId, skillId);
						short power = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey).GetPower();
						bool flag3 = power > maxPower;
						if (flag3)
						{
							skillKeyRandomPool.Clear();
							skillKeyRandomPool.Add(skillKey);
							maxPower = power;
						}
						else
						{
							bool flag4 = power == maxPower;
							if (flag4)
							{
								skillKeyRandomPool.Add(skillKey);
							}
						}
					}
					int transferPower = (int)(maxPower * 10 / 100);
					bool flag5 = transferPower > 0;
					if (flag5)
					{
						DomainManager.Combat.ReduceSkillPowerInCombat(context, skillKeyRandomPool[context.Random.Next(0, skillKeyRandomPool.Count)], effectKey, -transferPower);
						addPower += transferPower;
					}
				}
				skillKeyRandomPool.Clear();
				bool flag6 = enemySkillList.Count > 0;
				if (flag6)
				{
					short maxPower = 0;
					for (int j = 0; j < enemySkillList.Count; j++)
					{
						short skillId2 = enemySkillList[j];
						CombatSkillKey skillKey2 = new CombatSkillKey(enemyChar.GetId(), skillId2);
						short power2 = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey2).GetPower();
						bool flag7 = power2 > maxPower;
						if (flag7)
						{
							skillKeyRandomPool.Clear();
							skillKeyRandomPool.Add(skillKey2);
							maxPower = power2;
						}
						else
						{
							bool flag8 = power2 == maxPower;
							if (flag8)
							{
								skillKeyRandomPool.Add(skillKey2);
							}
						}
					}
					int transferPower2 = (int)(maxPower * 10 / 100);
					bool flag9 = transferPower2 > 0;
					if (flag9)
					{
						DomainManager.Combat.ReduceSkillPowerInCombat(context, skillKeyRandomPool[context.Random.Next(0, skillKeyRandomPool.Count)], effectKey, -transferPower2);
						addPower += transferPower2;
					}
				}
				ObjectPool<List<CombatSkillKey>>.Instance.Return(skillKeyRandomPool);
				bool flag10 = addPower > 0;
				if (flag10)
				{
					DomainManager.Combat.AddSkillPowerInCombat(context, this.SkillKey, effectKey, addPower);
					base.ShowSpecialEffectTips(0);
				}
			}
			ObjectPool<List<short>>.Instance.Return(selfSkillList);
			ObjectPool<List<short>>.Instance.Return(enemySkillList);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003EFD RID: 16125 RVA: 0x00257E2F File Offset: 0x0025602F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003EFE RID: 16126 RVA: 0x00257E44 File Offset: 0x00256044
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0400128F RID: 4751
		private const sbyte TransferPowerPercent = 10;
	}
}
