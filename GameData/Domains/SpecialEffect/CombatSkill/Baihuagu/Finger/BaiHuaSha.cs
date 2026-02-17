using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Finger
{
	// Token: 0x020005CC RID: 1484
	public class BaiHuaSha : CombatSkillEffectBase
	{
		// Token: 0x060043F2 RID: 17394 RVA: 0x0026D7E8 File Offset: 0x0026B9E8
		public BaiHuaSha()
		{
		}

		// Token: 0x060043F3 RID: 17395 RVA: 0x0026D7F2 File Offset: 0x0026B9F2
		public BaiHuaSha(CombatSkillKey skillKey) : base(skillKey, 3107, -1)
		{
		}

		// Token: 0x060043F4 RID: 17396 RVA: 0x0026D803 File Offset: 0x0026BA03
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043F5 RID: 17397 RVA: 0x0026D82C File Offset: 0x0026BA2C
		public override void OnDisable(DataContext context)
		{
			bool flag = this._bodyPartList != null;
			if (flag)
			{
				ObjectPool<List<sbyte>>.Instance.Return(this._bodyPartList);
			}
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060043F6 RID: 17398 RVA: 0x0026D87C File Offset: 0x0026BA7C
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				SortedDictionary<sbyte, List<ValueTuple<sbyte, int, int>>> bodyPartDict = (base.IsDirect ? base.CurrEnemyChar.GetAcupointCollection() : base.CurrEnemyChar.GetFlawCollection()).BodyPartDict;
				this._bodyPartList = ObjectPool<List<sbyte>>.Instance.Get();
				this._bodyPartList.Clear();
				for (sbyte type = 0; type < 7; type += 1)
				{
					bool flag2 = bodyPartDict[type].Count > 0;
					if (flag2)
					{
						this._bodyPartList.Add(type);
					}
				}
			}
		}

		// Token: 0x060043F7 RID: 17399 RVA: 0x0026D928 File Offset: 0x0026BB28
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					Injuries injuries = base.CurrEnemyChar.GetInjuries();
					int innerOdds = (int)base.SkillInstance.GetCurrInnerRatio();
					bool anyInjuryAdded = false;
					foreach (sbyte type in this._bodyPartList)
					{
						ValueTuple<sbyte, sbyte> injury = injuries.Get(type);
						bool flag3 = injury.Item1 < 6 || injury.Item2 < 6;
						if (flag3)
						{
							bool inner = injury.Item1 >= 6 || (injury.Item2 < 6 && context.Random.CheckPercentProb(innerOdds));
							DomainManager.Combat.AddInjury(context, base.CurrEnemyChar, type, inner, 1, false, true);
							anyInjuryAdded = true;
						}
					}
					bool flag4 = anyInjuryAdded;
					if (flag4)
					{
						DomainManager.Combat.UpdateBodyDefeatMark(context, base.CurrEnemyChar);
						base.ShowSpecialEffectTips(0);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0400142E RID: 5166
		private List<sbyte> _bodyPartList;
	}
}
