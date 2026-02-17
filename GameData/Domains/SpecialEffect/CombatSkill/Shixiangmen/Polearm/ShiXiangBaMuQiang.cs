using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm
{
	// Token: 0x020003ED RID: 1005
	public class ShiXiangBaMuQiang : CombatSkillEffectBase
	{
		// Token: 0x06003845 RID: 14405 RVA: 0x00239B24 File Offset: 0x00237D24
		public ShiXiangBaMuQiang()
		{
		}

		// Token: 0x06003846 RID: 14406 RVA: 0x00239B2E File Offset: 0x00237D2E
		public ShiXiangBaMuQiang(CombatSkillKey skillKey) : base(skillKey, 6302, -1)
		{
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x00239B3F File Offset: 0x00237D3F
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x00239B54 File Offset: 0x00237D54
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x00239B6C File Offset: 0x00237D6C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = power > 0;
				if (flag2)
				{
					sbyte stateType = base.IsDirect ? 1 : 2;
					CombatStateCollection stateCollection = base.CombatChar.GetCombatStateCollection(stateType);
					bool flag3 = stateCollection.StateDict.Count > 0;
					if (flag3)
					{
						List<short> randomPool = ObjectPool<List<short>>.Instance.Get();
						randomPool.Clear();
						randomPool.AddRange(stateCollection.StateDict.Keys);
						short stateId = randomPool[context.Random.Next(0, randomPool.Count)];
						int changePower = (int)Math.Ceiling((double)((float)(stateCollection.StateDict[stateId].Item1 * 10 * (short)power) / 1000f));
						ObjectPool<List<short>>.Instance.Return(randomPool);
						bool flag4 = changePower > 0;
						if (flag4)
						{
							DomainManager.Combat.AddCombatState(context, base.CombatChar, stateType, stateId, base.IsDirect ? changePower : (-changePower), false, false);
							base.ShowSpecialEffectTips(0);
						}
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04001075 RID: 4213
		private const sbyte StatePowerChangePercent = 10;
	}
}
