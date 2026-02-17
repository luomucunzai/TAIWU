using System;
using System.Collections.Generic;
using Config;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000793 RID: 1939
	[AiCondition(EAiConditionType.OptionTeammateCommand)]
	public class AiConditionOptionTeammateCommand : AiConditionCombatBase
	{
		// Token: 0x060069D8 RID: 27096 RVA: 0x003BB2B9 File Offset: 0x003B94B9
		public AiConditionOptionTeammateCommand(IReadOnlyList<int> ints)
		{
			this._implement = (ETeammateCommandImplement)ints[0];
		}

		// Token: 0x060069D9 RID: 27097 RVA: 0x003BB2D0 File Offset: 0x003B94D0
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.GetShowTransferInjuryCommand() && this._implement != ETeammateCommandImplement.TransferInjury;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool[] allowAutoUse = DomainManager.Combat.AiOptions.AutoUseTeammateCommand;
				ETeammateCommandOption option = CombatDomain.TeammateCommandOptions.GetValueOrDefault(this._implement, ETeammateCommandOption.Invalid);
				bool flag2 = combatChar.IsAlly && (!allowAutoUse.CheckIndex((int)option) || !allowAutoUse[(int)option]);
				if (flag2)
				{
					result = false;
				}
				else
				{
					foreach (CombatCharacter teammate in DomainManager.Combat.GetTeammateCharacters(combatChar.GetId()))
					{
						List<sbyte> cmdTypes = teammate.GetCurrTeammateCommands();
						List<bool> cmdCanUse = teammate.GetTeammateCommandCanUse();
						for (int i = 0; i < cmdTypes.Count; i++)
						{
							bool flag3 = !cmdCanUse.CheckIndex(i) || !cmdCanUse[i];
							if (!flag3)
							{
								bool flag4 = combatChar.GetShowTransferInjuryCommand() || this.IsMatch(cmdTypes[i]);
								if (flag4)
								{
									return true;
								}
							}
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060069DA RID: 27098 RVA: 0x003BB41C File Offset: 0x003B961C
		private bool IsMatch(sbyte cmdType)
		{
			return TeammateCommand.Instance[cmdType].Implement == this._implement;
		}

		// Token: 0x04001D37 RID: 7479
		private readonly ETeammateCommandImplement _implement;
	}
}
