using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip
{
	// Token: 0x02000382 RID: 898
	public class WuGongSuo : CombatSkillEffectBase
	{
		// Token: 0x060035FE RID: 13822 RVA: 0x0022EBEE File Offset: 0x0022CDEE
		public WuGongSuo()
		{
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x0022EBF8 File Offset: 0x0022CDF8
		public WuGongSuo(CombatSkillKey skillKey) : base(skillKey, 12401, -1)
		{
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x0022EC09 File Offset: 0x0022CE09
		public override void OnEnable(DataContext context)
		{
			this._affectBodyPart = -1;
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x0022EC49 File Offset: 0x0022CE49
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x0022EC84 File Offset: 0x0022CE84
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker != base.CombatChar || skillId != base.SkillTemplateId;
			if (!flag)
			{
				FlawOrAcupointCollection flawCollection = defender.GetFlawCollection();
				List<sbyte> maxFlawPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
				int maxFlawLevelSum = 0;
				maxFlawPartRandomPool.Clear();
				for (sbyte part = 0; part < 7; part += 1)
				{
					List<ValueTuple<sbyte, int, int>> flawList = flawCollection.BodyPartDict[part];
					int levelSum = 0;
					for (int i = 0; i < flawList.Count; i++)
					{
						levelSum += (int)(flawList[i].Item1 + 1);
					}
					bool flag2 = levelSum == 0;
					if (!flag2)
					{
						bool flag3 = levelSum >= maxFlawLevelSum;
						if (flag3)
						{
							bool flag4 = levelSum > maxFlawLevelSum;
							if (flag4)
							{
								maxFlawPartRandomPool.Clear();
								maxFlawLevelSum = levelSum;
							}
							maxFlawPartRandomPool.Add(part);
						}
					}
				}
				bool flag5 = maxFlawPartRandomPool.Count > 0;
				if (flag5)
				{
					this._affectBodyPart = maxFlawPartRandomPool[context.Random.Next(0, maxFlawPartRandomPool.Count)];
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						attacker.SkillAttackBodyPart = this._affectBodyPart;
					}
				}
				ObjectPool<List<sbyte>>.Instance.Return(maxFlawPartRandomPool);
			}
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x0022EDC4 File Offset: 0x0022CFC4
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index != 3 || this._affectBodyPart < 0 || context.Attacker.GetAttackSkillPower() == 0;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					FlawOrAcupointCollection flawCollection = context.Defender.GetFlawCollection();
					List<ValueTuple<sbyte, int, int>> flawList = flawCollection.BodyPartDict[this._affectBodyPart];
					for (int i = 0; i < flawList.Count; i++)
					{
						ValueTuple<sbyte, int, int> flaw = flawList[i];
						flaw.Item3 = flaw.Item2;
						flawList[i] = flaw;
					}
					context.Defender.SetFlawCollection(flawCollection, context);
				}
				else
				{
					DomainManager.Combat.DoSkillHit(context.Attacker, context.Defender, base.SkillTemplateId, this._affectBodyPart, hitType);
				}
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x0022EEC0 File Offset: 0x0022D0C0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000FBB RID: 4027
		private sbyte _affectBodyPart;
	}
}
