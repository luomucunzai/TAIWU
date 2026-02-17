using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.LongYuFu
{
	// Token: 0x020002FD RID: 765
	public class HuoLongFenXin : CombatSkillEffectBase
	{
		// Token: 0x060033A6 RID: 13222 RVA: 0x00226129 File Offset: 0x00224329
		public HuoLongFenXin()
		{
		}

		// Token: 0x060033A7 RID: 13223 RVA: 0x00226133 File Offset: 0x00224333
		public HuoLongFenXin(CombatSkillKey skillKey) : base(skillKey, 17122, -1)
		{
		}

		// Token: 0x060033A8 RID: 13224 RVA: 0x00226144 File Offset: 0x00224344
		public override void OnEnable(DataContext context)
		{
			bool flag = base.CombatChar.GetInjuries().Get(2, false) < 4;
			if (flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				Dictionary<SkillEffectKey, short> effectDict = enemyChar.GetSkillEffectCollection().EffectDict;
				bool flag2 = effectDict != null && effectDict.Count > 0;
				if (flag2)
				{
					List<SkillEffectKey> effectRandomPool = ObjectPool<List<SkillEffectKey>>.Instance.Get();
					int affectCount = Math.Min(3, effectDict.Count);
					effectRandomPool.Clear();
					effectRandomPool.AddRange(effectDict.Keys);
					for (int i = 0; i < affectCount; i++)
					{
						int index = context.Random.Next(effectRandomPool.Count);
						SkillEffectKey effectKey = effectRandomPool[index];
						effectRandomPool.RemoveAt(index);
						DomainManager.Combat.ChangeSkillEffectToMinCount(context, enemyChar, effectKey);
						DomainManager.Combat.AddGoneMadInjury(context, enemyChar, effectKey.SkillId, 0);
					}
					ObjectPool<List<SkillEffectKey>>.Instance.Return(effectRandomPool);
					DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
					base.ShowSpecialEffectTips(0);
				}
				DomainManager.Combat.AddInjury(context, base.CombatChar, 2, false, 1, true, false);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060033A9 RID: 13225 RVA: 0x00226298 File Offset: 0x00224498
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060033AA RID: 13226 RVA: 0x002262B0 File Offset: 0x002244B0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F46 RID: 3910
		private const sbyte InjuryThreshold = 4;

		// Token: 0x04000F47 RID: 3911
		private const sbyte AffectSkillCount = 3;
	}
}
