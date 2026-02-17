using System;
using System.Linq;
using Config;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000738 RID: 1848
	public abstract class AiConditionOptionCastCombatSkillBase : AiConditionCombatBase
	{
		// Token: 0x06006917 RID: 26903 RVA: 0x003B9470 File Offset: 0x003B7670
		private bool IsValid(short skillId)
		{
			return skillId == this.SkillId;
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06006918 RID: 26904
		protected abstract short SkillId { get; }

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x06006919 RID: 26905 RVA: 0x003B947B File Offset: 0x003B767B
		// (set) Token: 0x0600691A RID: 26906 RVA: 0x003B9483 File Offset: 0x003B7683
		private protected AiMemoryNew Memory { protected get; private set; }

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x0600691B RID: 26907 RVA: 0x003B948C File Offset: 0x003B768C
		// (set) Token: 0x0600691C RID: 26908 RVA: 0x003B9494 File Offset: 0x003B7694
		private protected CombatCharacter CombatChar { protected get; private set; }

		// Token: 0x0600691D RID: 26909 RVA: 0x003B94A0 File Offset: 0x003B76A0
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			this.Memory = memory;
			this.CombatChar = combatChar;
			bool result = this.GetResult(combatChar);
			this.Memory = null;
			this.CombatChar = null;
			return result;
		}

		// Token: 0x0600691E RID: 26910 RVA: 0x003B94DC File Offset: 0x003B76DC
		private bool GetResult(CombatCharacter combatChar)
		{
			CombatSkillItem config = CombatSkill.Instance[this.SkillId];
			bool flag = config == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte equipType = config.EquipType;
				if (!true)
				{
				}
				int num;
				switch (equipType)
				{
				case 1:
					num = 0;
					break;
				case 2:
					num = 1;
					break;
				case 3:
					num = 2;
					break;
				default:
					num = -1;
					break;
				}
				if (!true)
				{
				}
				int autoCastIndex = num;
				bool flag2 = autoCastIndex < 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoCastSkill[autoCastIndex];
					result = (!flag3 && combatChar.GetCombatSkillIds().Where(new Func<short, bool>(this.IsValid)).Where(new Func<short, bool>(combatChar.AiCanCast)).Any<short>());
				}
			}
			return result;
		}
	}
}
