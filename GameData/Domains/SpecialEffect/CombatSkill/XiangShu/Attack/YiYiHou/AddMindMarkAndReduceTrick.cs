using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiYiHou
{
	// Token: 0x020002BD RID: 701
	public class AddMindMarkAndReduceTrick : CombatSkillEffectBase
	{
		// Token: 0x06003254 RID: 12884 RVA: 0x0021F1FE File Offset: 0x0021D3FE
		protected AddMindMarkAndReduceTrick()
		{
		}

		// Token: 0x06003255 RID: 12885 RVA: 0x0021F208 File Offset: 0x0021D408
		protected AddMindMarkAndReduceTrick(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06003256 RID: 12886 RVA: 0x0021F215 File Offset: 0x0021D415
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003257 RID: 12887 RVA: 0x0021F23C File Offset: 0x0021D43C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003258 RID: 12888 RVA: 0x0021F264 File Offset: 0x0021D464
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = base.CombatChar != combatChar || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._frameCounter++;
				bool flag2 = this._frameCounter < (int)this.AffectFrameCount || !DomainManager.Combat.InAttackRange(base.CombatChar);
				if (!flag2)
				{
					this._frameCounter = 0;
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					bool flag3 = enemyChar.GetTricks().Tricks.Count > 0;
					if (flag3)
					{
						List<sbyte> trickTypeRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
						trickTypeRandomPool.Clear();
						trickTypeRandomPool.AddRange(enemyChar.GetTricks().Tricks.Values);
						DomainManager.Combat.RemoveTrick(context, enemyChar, trickTypeRandomPool[context.Random.Next(0, trickTypeRandomPool.Count)], 1, false, -1);
						ObjectPool<List<sbyte>>.Instance.Return(trickTypeRandomPool);
						base.ShowSpecialEffectTips(1);
					}
					DomainManager.Combat.AppendMindDefeatMark(context, enemyChar, 1, -1, false);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003259 RID: 12889 RVA: 0x0021F38C File Offset: 0x0021D58C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000EE9 RID: 3817
		protected short AffectFrameCount;

		// Token: 0x04000EEA RID: 3818
		private int _frameCounter;
	}
}
