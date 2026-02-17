using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Whip
{
	// Token: 0x020003B9 RID: 953
	public class WuDangTieFuChen : CombatSkillEffectBase
	{
		// Token: 0x06003718 RID: 14104 RVA: 0x00233D6A File Offset: 0x00231F6A
		public WuDangTieFuChen()
		{
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x00233D74 File Offset: 0x00231F74
		public WuDangTieFuChen(CombatSkillKey skillKey) : base(skillKey, 4301, -1)
		{
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x00233D85 File Offset: 0x00231F85
		public override void OnEnable(DataContext context)
		{
			this._affectBodyPart = -1;
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x00233DC5 File Offset: 0x00231FC5
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x00233E00 File Offset: 0x00232000
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker != base.CombatChar || skillId != base.SkillTemplateId;
			if (!flag)
			{
				FlawOrAcupointCollection acupointCollection = defender.GetAcupointCollection();
				List<sbyte> maxAcupointPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
				int maxAcupointLevelSum = 0;
				maxAcupointPartRandomPool.Clear();
				for (sbyte part = 0; part < 7; part += 1)
				{
					List<ValueTuple<sbyte, int, int>> acupointList = acupointCollection.BodyPartDict[part];
					int levelSum = 0;
					for (int i = 0; i < acupointList.Count; i++)
					{
						levelSum += (int)(acupointList[i].Item1 + 1);
					}
					bool flag2 = levelSum == 0;
					if (!flag2)
					{
						bool flag3 = levelSum >= maxAcupointLevelSum;
						if (flag3)
						{
							bool flag4 = levelSum > maxAcupointLevelSum;
							if (flag4)
							{
								maxAcupointPartRandomPool.Clear();
								maxAcupointLevelSum = levelSum;
							}
							maxAcupointPartRandomPool.Add(part);
						}
					}
				}
				bool flag5 = maxAcupointPartRandomPool.Count > 0;
				if (flag5)
				{
					this._affectBodyPart = maxAcupointPartRandomPool[context.Random.Next(0, maxAcupointPartRandomPool.Count)];
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						attacker.SkillAttackBodyPart = this._affectBodyPart;
					}
				}
				ObjectPool<List<sbyte>>.Instance.Return(maxAcupointPartRandomPool);
			}
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x00233F40 File Offset: 0x00232140
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index != 3 || this._affectBodyPart < 0 || context.Attacker.GetAttackSkillPower() == 0;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					FlawOrAcupointCollection acupointCollection = context.Defender.GetAcupointCollection();
					List<ValueTuple<sbyte, int, int>> acupointList = acupointCollection.BodyPartDict[this._affectBodyPart];
					for (int i = 0; i < acupointList.Count; i++)
					{
						ValueTuple<sbyte, int, int> acupoint = acupointList[i];
						acupoint.Item3 = acupoint.Item2;
						acupointList[i] = acupoint;
					}
					context.Defender.SetAcupointCollection(acupointCollection, context);
				}
				else
				{
					DomainManager.Combat.DoSkillHit(context.Attacker, context.Defender, base.SkillTemplateId, this._affectBodyPart, hitType);
				}
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x0023403C File Offset: 0x0023223C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04001015 RID: 4117
		private sbyte _affectBodyPart;
	}
}
