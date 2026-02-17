using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.HuanXin
{
	// Token: 0x02000311 RID: 785
	public class BiGuang : CombatSkillEffectBase
	{
		// Token: 0x060033F7 RID: 13303 RVA: 0x00227263 File Offset: 0x00225463
		public BiGuang()
		{
		}

		// Token: 0x060033F8 RID: 13304 RVA: 0x0022726D File Offset: 0x0022546D
		public BiGuang(CombatSkillKey skillKey) : base(skillKey, 17104, -1)
		{
		}

		// Token: 0x060033F9 RID: 13305 RVA: 0x00227280 File Offset: 0x00225480
		public override void OnEnable(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			Dictionary<short, ValueTuple<short, bool, int>> selfStateDict = base.CombatChar.GetDebuffCombatStateCollection().StateDict;
			Dictionary<short, ValueTuple<short, bool, int>> enemyStateDict = enemyChar.GetBuffCombatStateCollection().StateDict;
			bool flag = selfStateDict.Count > 0;
			if (flag)
			{
				List<short> stateRandomPool = ObjectPool<List<short>>.Instance.Get();
				stateRandomPool.Clear();
				stateRandomPool.AddRange(selfStateDict.Keys);
				int changeCount = Math.Min(2, stateRandomPool.Count);
				for (int i = 0; i < changeCount; i++)
				{
					int index = context.Random.Next(0, stateRandomPool.Count);
					short key = stateRandomPool[index];
					stateRandomPool.RemoveAt(index);
					DomainManager.Combat.ReverseCombatState(context, base.CombatChar, 2, key);
				}
				ObjectPool<List<short>>.Instance.Return(stateRandomPool);
			}
			bool flag2 = enemyStateDict.Count > 0;
			if (flag2)
			{
				List<short> stateRandomPool2 = ObjectPool<List<short>>.Instance.Get();
				stateRandomPool2.Clear();
				stateRandomPool2.AddRange(enemyStateDict.Keys);
				int changeCount2 = Math.Min(2, stateRandomPool2.Count);
				for (int j = 0; j < changeCount2; j++)
				{
					int index2 = context.Random.Next(0, stateRandomPool2.Count);
					short key2 = stateRandomPool2[index2];
					stateRandomPool2.RemoveAt(index2);
					DomainManager.Combat.ReverseCombatState(context, enemyChar, 1, key2);
				}
				ObjectPool<List<short>>.Instance.Return(stateRandomPool2);
			}
			bool flag3 = selfStateDict.Count > 0 || enemyStateDict.Count > 0;
			if (flag3)
			{
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060033FA RID: 13306 RVA: 0x00227447 File Offset: 0x00225647
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060033FB RID: 13307 RVA: 0x0022745C File Offset: 0x0022565C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					foreach (short banableSkillId in enemyChar.GetBanableSkillIds(3, -1))
					{
						DomainManager.Combat.SilenceSkill(context, enemyChar, banableSkillId, 600, 100);
					}
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F5D RID: 3933
		private const sbyte ChangeStateCount = 2;

		// Token: 0x04000F5E RID: 3934
		private const short SilenceFrame = 600;
	}
}
