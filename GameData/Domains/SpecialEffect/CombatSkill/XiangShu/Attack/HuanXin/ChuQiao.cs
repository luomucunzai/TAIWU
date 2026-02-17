using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.HuanXin
{
	// Token: 0x02000313 RID: 787
	public class ChuQiao : CombatSkillEffectBase
	{
		// Token: 0x06003401 RID: 13313 RVA: 0x0022768C File Offset: 0x0022588C
		public ChuQiao()
		{
		}

		// Token: 0x06003402 RID: 13314 RVA: 0x00227696 File Offset: 0x00225896
		public ChuQiao(CombatSkillKey skillKey) : base(skillKey, 17101, -1)
		{
		}

		// Token: 0x06003403 RID: 13315 RVA: 0x002276A8 File Offset: 0x002258A8
		public override void OnEnable(DataContext context)
		{
			Dictionary<short, ValueTuple<short, bool, int>> stateDict = base.CombatChar.GetDebuffCombatStateCollection().StateDict;
			bool flag = stateDict.Count > 0;
			if (flag)
			{
				List<short> stateRandomPool = ObjectPool<List<short>>.Instance.Get();
				stateRandomPool.Clear();
				stateRandomPool.AddRange(stateDict.Keys);
				int changeCount = Math.Min(2, stateRandomPool.Count);
				for (int i = 0; i < changeCount; i++)
				{
					int index = context.Random.Next(0, stateRandomPool.Count);
					short key = stateRandomPool[index];
					stateRandomPool.RemoveAt(index);
					DomainManager.Combat.ReverseCombatState(context, base.CombatChar, 2, key);
				}
				ObjectPool<List<short>>.Instance.Return(stateRandomPool);
				bool flag2 = changeCount > 0;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
				}
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003404 RID: 13316 RVA: 0x00227789 File Offset: 0x00225989
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003405 RID: 13317 RVA: 0x002277A0 File Offset: 0x002259A0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000F60 RID: 3936
		private const sbyte ChangeStateCount = 2;
	}
}
