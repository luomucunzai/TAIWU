using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007EA RID: 2026
	[AiAction(EAiActionType.UseTeammateCommand)]
	public class AiActionUseTeammateCommand : AiActionCombatBase
	{
		// Token: 0x06006A9E RID: 27294 RVA: 0x003BD039 File Offset: 0x003BB239
		public AiActionUseTeammateCommand(IReadOnlyList<int> ints)
		{
			this._implement = (ETeammateCommandImplement)ints[0];
		}

		// Token: 0x06006A9F RID: 27295 RVA: 0x003BD050 File Offset: 0x003BB250
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.GetShowTransferInjuryCommand() && this._implement != ETeammateCommandImplement.TransferInjury;
			if (!flag)
			{
				DataContext context = DomainManager.Combat.Context;
				foreach (CombatCharacter teammate in DomainManager.Combat.GetTeammateCharacters(combatChar.GetId()))
				{
					List<sbyte> cmdTypes = teammate.GetCurrTeammateCommands();
					List<bool> cmdCanUse = teammate.GetTeammateCommandCanUse();
					for (int i = 0; i < cmdTypes.Count; i++)
					{
						bool flag2 = !cmdCanUse.CheckIndex(i) || !cmdCanUse[i];
						if (!flag2)
						{
							bool flag3 = combatChar.GetShowTransferInjuryCommand() || this.IsMatch(cmdTypes[i]);
							if (flag3)
							{
								DomainManager.Combat.ExecuteTeammateCommand(context, combatChar.IsAlly, i, teammate.GetId());
							}
						}
					}
				}
			}
		}

		// Token: 0x06006AA0 RID: 27296 RVA: 0x003BD164 File Offset: 0x003BB364
		private bool IsMatch(sbyte cmdType)
		{
			return TeammateCommand.Instance[cmdType].Implement == this._implement;
		}

		// Token: 0x04001D71 RID: 7537
		private readonly ETeammateCommandImplement _implement;
	}
}
