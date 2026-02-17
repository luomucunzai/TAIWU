using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Music
{
	// Token: 0x020005C8 RID: 1480
	public class TianDiXiao : CombatSkillEffectBase
	{
		// Token: 0x060043D7 RID: 17367 RVA: 0x0026CE90 File Offset: 0x0026B090
		public TianDiXiao()
		{
		}

		// Token: 0x060043D8 RID: 17368 RVA: 0x0026CEA2 File Offset: 0x0026B0A2
		public TianDiXiao(CombatSkillKey skillKey) : base(skillKey, 3306, -1)
		{
		}

		// Token: 0x060043D9 RID: 17369 RVA: 0x0026CEBB File Offset: 0x0026B0BB
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043DA RID: 17370 RVA: 0x0026CED0 File Offset: 0x0026B0D0
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043DB RID: 17371 RVA: 0x0026CEE8 File Offset: 0x0026B0E8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					CombatCharacter affectChar = base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
					Dictionary<CombatSkillKey, SkillPowerChangeCollection> powerDict = base.IsDirect ? DomainManager.Combat.GetAllSkillPowerAddInCombat() : DomainManager.Combat.GetAllSkillPowerReduceInCombat();
					SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
					List<short> skillRandomPool = ObjectPool<List<short>>.Instance.Get();
					bool affected = false;
					sbyte equipType2;
					sbyte equipType;
					for (equipType = 1; equipType < 5; equipType = equipType2 + 1)
					{
						skillRandomPool.Clear();
						bool flag3 = affectChar.BossConfig == null;
						if (flag3)
						{
							skillRandomPool.AddRange(affectChar.GetCombatSkillList(equipType));
						}
						else
						{
							skillRandomPool.AddRange(affectChar.GetCharacter().GetLearnedCombatSkills().FindAll((short id) => Config.CombatSkill.Instance[id].EquipType == equipType));
						}
						for (int i = skillRandomPool.Count - 1; i >= 0; i--)
						{
							short affectSkill = skillRandomPool[i];
							bool flag4 = affectSkill < 0;
							if (flag4)
							{
								skillRandomPool.RemoveAt(i);
							}
							else
							{
								CombatSkillKey skillKey = new CombatSkillKey(affectChar.GetId(), affectSkill);
								bool flag5 = powerDict.ContainsKey(skillKey) && powerDict[skillKey].EffectDict != null && powerDict[skillKey].EffectDict.ContainsKey(effectKey);
								if (flag5)
								{
									skillRandomPool.RemoveAt(i);
								}
							}
						}
						bool flag6 = skillRandomPool.Count > 0;
						if (flag6)
						{
							bool isDirect = base.IsDirect;
							if (isDirect)
							{
								DomainManager.Combat.AddSkillPowerInCombat(context, new CombatSkillKey(affectChar.GetId(), skillRandomPool[context.Random.Next(0, skillRandomPool.Count)]), effectKey, (int)this.ChangePower);
							}
							else
							{
								DomainManager.Combat.ReduceSkillPowerInCombat(context, new CombatSkillKey(affectChar.GetId(), skillRandomPool[context.Random.Next(0, skillRandomPool.Count)]), effectKey, (int)(-(int)this.ChangePower));
							}
							affected = true;
						}
						equipType2 = equipType;
					}
					bool flag7 = affected;
					if (flag7)
					{
						base.ShowSpecialEffectTips(0);
					}
					ObjectPool<List<short>>.Instance.Return(skillRandomPool);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04001426 RID: 5158
		private sbyte ChangePower = 40;
	}
}
