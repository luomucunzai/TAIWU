using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiYiHou
{
	// Token: 0x020002BF RID: 703
	public class AttackExtraPart : CombatSkillEffectBase
	{
		// Token: 0x0600325F RID: 12895 RVA: 0x0021F529 File Offset: 0x0021D729
		protected AttackExtraPart()
		{
		}

		// Token: 0x06003260 RID: 12896 RVA: 0x0021F533 File Offset: 0x0021D733
		protected AttackExtraPart(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06003261 RID: 12897 RVA: 0x0021F540 File Offset: 0x0021D740
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003262 RID: 12898 RVA: 0x0021F567 File Offset: 0x0021D767
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003263 RID: 12899 RVA: 0x0021F590 File Offset: 0x0021D790
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index != 3 || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
				bodyPartRandomPool.Clear();
				for (sbyte part = 0; part < 7; part += 1)
				{
					bool flag2 = part != base.CombatChar.SkillAttackBodyPart;
					if (flag2)
					{
						bodyPartRandomPool.Add(part);
					}
				}
				for (int i = 0; i < (int)this.AttackExtraPartCount; i++)
				{
					int partIndex = context.Random.Next(0, bodyPartRandomPool.Count);
					DomainManager.Combat.DoSkillHit(context.Attacker, context.Defender, base.SkillTemplateId, bodyPartRandomPool[partIndex], hitType);
					bodyPartRandomPool.RemoveAt(partIndex);
				}
				ObjectPool<List<sbyte>>.Instance.Return(bodyPartRandomPool);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003264 RID: 12900 RVA: 0x0021F688 File Offset: 0x0021D888
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000EEC RID: 3820
		protected sbyte AttackExtraPartCount;
	}
}
