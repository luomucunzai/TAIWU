using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000737 RID: 1847
	public abstract class AiConditionCheckCharStatePowerSumMoreOrEqualBase : AiConditionCheckCharBase
	{
		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x06006912 RID: 26898
		protected abstract sbyte StateType { get; }

		// Token: 0x06006913 RID: 26899 RVA: 0x003B939D File Offset: 0x003B759D
		protected bool IsNameMatch(short stateId)
		{
			return CombatState.Instance[stateId].Name == this.CombatStateName;
		}

		// Token: 0x06006914 RID: 26900 RVA: 0x003B93BA File Offset: 0x003B75BA
		protected AiConditionCheckCharStatePowerSumMoreOrEqualBase(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(ints)
		{
			this.CombatStateName = strings[0];
			this.TargetPower = ints[1];
		}

		// Token: 0x06006915 RID: 26901 RVA: 0x003B93E0 File Offset: 0x003B75E0
		protected override bool Check(CombatCharacter checkChar)
		{
			CombatStateCollection stateCollection = checkChar.GetCombatStateCollection(this.StateType);
			bool flag = ((stateCollection != null) ? stateCollection.StateDict : null) == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				result = ((from x in stateCollection.StateDict
				where this.IsNameMatch(x.Key)
				select (int)x.Value.Item1).Sum() >= this.TargetPower);
			}
			return result;
		}

		// Token: 0x04001CE8 RID: 7400
		protected readonly string CombatStateName;

		// Token: 0x04001CE9 RID: 7401
		protected readonly int TargetPower;
	}
}
