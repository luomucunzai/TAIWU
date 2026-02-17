using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.DefenseAndAssist
{
	// Token: 0x020003DB RID: 987
	public class WuQiChaoYuanGong : DefenseSkillBase
	{
		// Token: 0x060037D7 RID: 14295 RVA: 0x00237791 File Offset: 0x00235991
		public WuQiChaoYuanGong()
		{
		}

		// Token: 0x060037D8 RID: 14296 RVA: 0x0023779B File Offset: 0x0023599B
		public WuQiChaoYuanGong(CombatSkillKey skillKey) : base(skillKey, 4504)
		{
		}

		// Token: 0x060037D9 RID: 14297 RVA: 0x002377AC File Offset: 0x002359AC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._frameCounter = ObjectPool<List<sbyte>>.Instance.Get();
			this._frameCounter.Clear();
			for (int i = 0; i < 4; i++)
			{
				this._frameCounter.Add(0);
			}
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			}
		}

		// Token: 0x060037DA RID: 14298 RVA: 0x0023782C File Offset: 0x00235A2C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			ObjectPool<List<sbyte>>.Instance.Return(this._frameCounter);
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			}
		}

		// Token: 0x060037DB RID: 14299 RVA: 0x00237884 File Offset: 0x00235A84
		private unsafe void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly != base.CombatChar.IsAlly || DomainManager.Combat.Pause;
			if (!flag)
			{
				CombatCharacter affectChar = base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				NeiliAllocation neiliAllocation = affectChar.GetNeiliAllocation();
				NeiliAllocation originNeiliAllocation = affectChar.GetOriginNeiliAllocation();
				bool showTips = false;
				for (byte type = 0; type < 4; type += 1)
				{
					short currValue = *(ref neiliAllocation.Items.FixedElementField + (IntPtr)type * 2);
					short originValue = *(ref originNeiliAllocation.Items.FixedElementField + (IntPtr)type * 2);
					bool flag2 = base.IsDirect ? (currValue < originValue) : (currValue > originValue);
					if (flag2)
					{
						List<sbyte> frameCounter = this._frameCounter;
						int index = (int)type;
						sbyte b = frameCounter[index];
						frameCounter[index] = b + 1;
						bool flag3 = this._frameCounter[(int)type] >= 30;
						if (flag3)
						{
							this._frameCounter[(int)type] = 0;
							bool canAffect = base.CanAffect;
							if (canAffect)
							{
								showTips = true;
								affectChar.ChangeNeiliAllocation(context, type, base.IsDirect ? 1 : -1, true, true);
							}
						}
					}
					else
					{
						this._frameCounter[(int)type] = 0;
					}
				}
				bool flag4 = showTips;
				if (flag4)
				{
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060037DC RID: 14300 RVA: 0x002379F4 File Offset: 0x00235BF4
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				for (int i = 0; i < 4; i++)
				{
					this._frameCounter[i] = 0;
				}
			}
		}

		// Token: 0x0400104D RID: 4173
		private const sbyte ChangeNeiliAllocationFrame = 30;

		// Token: 0x0400104E RID: 4174
		private List<sbyte> _frameCounter;
	}
}
