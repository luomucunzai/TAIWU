using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005F9 RID: 1529
	public class LoongMetalImplementMindMark : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x060044EB RID: 17643 RVA: 0x00270D96 File Offset: 0x0026EF96
		// (set) Token: 0x060044EC RID: 17644 RVA: 0x00270D9E File Offset: 0x0026EF9E
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x060044ED RID: 17645 RVA: 0x00270DA7 File Offset: 0x0026EFA7
		public void OnEnable(DataContext context)
		{
			Events.RegisterHandler_MoveBegin(new Events.OnMoveBegin(this.OnMoveBegin));
			Events.RegisterHandler_MoveEnd(new Events.OnMoveEnd(this.OnMoveEnd));
		}

		// Token: 0x060044EE RID: 17646 RVA: 0x00270DCE File Offset: 0x0026EFCE
		public void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_MoveBegin(new Events.OnMoveBegin(this.OnMoveBegin));
			Events.UnRegisterHandler_MoveEnd(new Events.OnMoveEnd(this.OnMoveEnd));
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x00270DF8 File Offset: 0x0026EFF8
		private void OnMoveBegin(DataContext context, CombatCharacter mover, int distance, bool isJump)
		{
			bool flag = mover.GetId() != this.EffectBase.CharacterId || !isJump;
			if (!flag)
			{
				this._distanceOnJumpBegin = (int)DomainManager.Combat.GetCurrentDistance();
			}
		}

		// Token: 0x060044F0 RID: 17648 RVA: 0x00270E38 File Offset: 0x0026F038
		private void OnMoveEnd(DataContext context, CombatCharacter mover, int distance, bool isJump)
		{
			bool flag = mover.GetId() != this.EffectBase.CharacterId || !isJump;
			if (!flag)
			{
				int distanceDelta = Math.Abs((int)DomainManager.Combat.GetCurrentDistance() - this._distanceOnJumpBegin);
				bool flag2 = distanceDelta < 10;
				if (!flag2)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!mover.IsAlly, false);
					int mindMarkCount = distanceDelta / 10;
					DomainManager.Combat.AppendMindDefeatMark(context, enemyChar, mindMarkCount, -1, false);
					this.EffectBase.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x04001461 RID: 5217
		private const int AddMindMarkRequireDistance = 10;

		// Token: 0x04001462 RID: 5218
		private int _distanceOnJumpBegin;
	}
}
