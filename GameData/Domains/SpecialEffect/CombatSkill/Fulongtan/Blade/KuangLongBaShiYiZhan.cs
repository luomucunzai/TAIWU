using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Blade
{
	// Token: 0x0200052F RID: 1327
	public class KuangLongBaShiYiZhan : CombatSkillEffectBase
	{
		// Token: 0x06003F7A RID: 16250 RVA: 0x0025A1DC File Offset: 0x002583DC
		private static int HitCountToAddDamagePercent(int hitCount)
		{
			if (!true)
			{
			}
			int result;
			if (hitCount > 0)
			{
				if (hitCount != 1)
				{
					if (hitCount != 2)
					{
						result = KuangLongBaShiYiZhan.HitAddDamagePercent[0] + KuangLongBaShiYiZhan.HitAddDamagePercent[1] + (hitCount - 2) * KuangLongBaShiYiZhan.HitAddDamagePercent[2];
					}
					else
					{
						result = KuangLongBaShiYiZhan.HitAddDamagePercent[0] + KuangLongBaShiYiZhan.HitAddDamagePercent[1];
					}
				}
				else
				{
					result = KuangLongBaShiYiZhan.HitAddDamagePercent[0];
				}
			}
			else
			{
				result = 0;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06003F7B RID: 16251 RVA: 0x0025A245 File Offset: 0x00258445
		public KuangLongBaShiYiZhan()
		{
		}

		// Token: 0x06003F7C RID: 16252 RVA: 0x0025A25A File Offset: 0x0025845A
		public KuangLongBaShiYiZhan(CombatSkillKey skillKey) : base(skillKey, 14204, -1)
		{
		}

		// Token: 0x06003F7D RID: 16253 RVA: 0x0025A276 File Offset: 0x00258476
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F7E RID: 16254 RVA: 0x0025A29D File Offset: 0x0025849D
		public override void OnDataAdded(DataContext context)
		{
			base.AppendAffectedAllEnemyData(context, 102, EDataModifyType.AddPercent, base.SkillTemplateId);
		}

		// Token: 0x06003F7F RID: 16255 RVA: 0x0025A2B1 File Offset: 0x002584B1
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F80 RID: 16256 RVA: 0x0025A2D8 File Offset: 0x002584D8
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || !hit || base.EffectCount <= 0;
			if (!flag)
			{
				Dictionary<sbyte, int> hitCounts = this._charBodyPartHitCount.GetOrNew(context.DefenderId);
				hitCounts[context.BodyPart] = hitCounts.GetOrDefault(context.BodyPart) + 1;
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003F81 RID: 16257 RVA: 0x0025A34C File Offset: 0x0025854C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x06003F82 RID: 16258 RVA: 0x0025A384 File Offset: 0x00258584
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					sbyte bodyPart = (sbyte)dataKey.CustomParam1;
					Dictionary<sbyte, int> hitCount;
					result = (this._charBodyPartHitCount.TryGetValue(dataKey.CharId, out hitCount) ? KuangLongBaShiYiZhan.HitCountToAddDamagePercent(hitCount.GetOrDefault(bodyPart)) : 0);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040012B6 RID: 4790
		private static readonly int[] HitAddDamagePercent = new int[]
		{
			20,
			10,
			5
		};

		// Token: 0x040012B7 RID: 4791
		private readonly Dictionary<int, Dictionary<sbyte, int>> _charBodyPartHitCount = new Dictionary<int, Dictionary<sbyte, int>>();
	}
}
