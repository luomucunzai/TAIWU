using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Domains.Character;

namespace GameData.Domains.Organization
{
	// Token: 0x0200064D RID: 1613
	public static class SettlementPrisonHelper
	{
		// Token: 0x0600485F RID: 18527 RVA: 0x0028CEC4 File Offset: 0x0028B0C4
		public static PrisonType GetPrisonType(this SettlementPrisoner prisoner)
		{
			Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(prisoner.CharId, out character);
			PrisonType result;
			if (flag)
			{
				result = PrisonType.Invalid;
			}
			else
			{
				bool flag2 = character.IsCompletelyInfected();
				if (flag2)
				{
					result = PrisonType.Infected;
				}
				else
				{
					bool flag3 = prisoner.PunishmentSeverity == 0;
					if (flag3)
					{
						result = PrisonType.Invalid;
					}
					else
					{
						bool flag4 = prisoner.PunishmentType == 39;
						if (flag4)
						{
							result = PrisonType.Infected;
						}
						else
						{
							result = (PrisonType)(character.GetOrganizationInfo().Grade / 3);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x06004860 RID: 18528 RVA: 0x0028CF35 File Offset: 0x0028B135
		public static IEnumerable<SettlementPrisoner> GetPrisonLow(this SettlementPrison prison)
		{
			return from x in prison.Prisoners
			where x.GetPrisonType() == PrisonType.Low
			select x;
		}

		// Token: 0x06004861 RID: 18529 RVA: 0x0028CF61 File Offset: 0x0028B161
		public static IEnumerable<SettlementPrisoner> GetPrisonMid(this SettlementPrison prison)
		{
			return from x in prison.Prisoners
			where x.GetPrisonType() == PrisonType.Mid
			select x;
		}

		// Token: 0x06004862 RID: 18530 RVA: 0x0028CF8D File Offset: 0x0028B18D
		public static IEnumerable<SettlementPrisoner> GetPrisonHigh(this SettlementPrison prison)
		{
			return from x in prison.Prisoners
			where x.GetPrisonType() == PrisonType.High
			select x;
		}

		// Token: 0x06004863 RID: 18531 RVA: 0x0028CFB9 File Offset: 0x0028B1B9
		public static IEnumerable<SettlementPrisoner> GetPrisonInfected(this SettlementPrison prison)
		{
			return from x in prison.Prisoners
			where x.GetPrisonType() == PrisonType.Infected
			select x;
		}
	}
}
