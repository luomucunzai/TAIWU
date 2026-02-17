using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier
{
	// Token: 0x02000614 RID: 1556
	public class Chiwen : CarrierEffectBase
	{
		// Token: 0x06004576 RID: 17782 RVA: 0x00272659 File Offset: 0x00270859
		public Chiwen(int charId) : base(charId)
		{
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06004577 RID: 17783 RVA: 0x00272664 File Offset: 0x00270864
		protected override short CombatStateId
		{
			get
			{
				return 207;
			}
		}

		// Token: 0x06004578 RID: 17784 RVA: 0x0027266B File Offset: 0x0027086B
		protected override void OnEnableSubClass(DataContext context)
		{
			Events.RegisterHandler_CastSkillTrickCosted(new Events.OnCastSkillTrickCosted(this.OnCastSkillTrickCosted));
		}

		// Token: 0x06004579 RID: 17785 RVA: 0x00272680 File Offset: 0x00270880
		protected override void OnDisableSubClass(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillTrickCosted(new Events.OnCastSkillTrickCosted(this.OnCastSkillTrickCosted));
		}

		// Token: 0x0600457A RID: 17786 RVA: 0x00272698 File Offset: 0x00270898
		private unsafe void OnCastSkillTrickCosted(DataContext context, CombatCharacter combatChar, short skillId, List<NeedTrick> costTricks)
		{
			bool flag = combatChar.GetId() != base.CharacterId && combatChar.IsAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				bool buff = combatChar.GetId() == base.CharacterId;
				int totalCostCount = costTricks.Sum((NeedTrick x) => (int)x.NeedCount);
				NeiliAllocation neiliAllocation = combatChar.GetNeiliAllocation();
				NeiliAllocation originNeiliAllocation = combatChar.GetOriginNeiliAllocation();
				int neiliAllocationCost = totalCostCount * 2 * (buff ? 1 : -1);
				for (byte i = 0; i < 4; i += 1)
				{
					bool flag2 = buff ? (*neiliAllocation[(int)i] >= *originNeiliAllocation[(int)i]) : (*neiliAllocation[(int)i] <= *originNeiliAllocation[(int)i]);
					if (!flag2)
					{
						combatChar.ChangeNeiliAllocation(context, i, neiliAllocationCost, true, true);
					}
				}
			}
		}

		// Token: 0x04001489 RID: 5257
		private const int AddOrReduceNeiliAllocationValue = 2;
	}
}
