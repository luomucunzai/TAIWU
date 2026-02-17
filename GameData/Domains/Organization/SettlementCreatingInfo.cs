using System;
using System.Collections.Generic;

namespace GameData.Domains.Organization
{
	// Token: 0x02000649 RID: 1609
	public class SettlementCreatingInfo
	{
		// Token: 0x0600484D RID: 18509 RVA: 0x0028CAD8 File Offset: 0x0028ACD8
		public SettlementCreatingInfo(List<short> villageRandomNameIds, List<short> townRandomNameIds, List<short> walledTownRandomNameIds)
		{
			this._villageRandomNameIds = villageRandomNameIds;
			this._nextVillageRandomNameIndex = 0;
			this._townRandomNameIds = townRandomNameIds;
			this._nextTownRandomNameIndex = 0;
			this._walledTownRandomNameIds = walledTownRandomNameIds;
			this._nextWalledTownRandomNameIndex = 0;
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x0028CB0C File Offset: 0x0028AD0C
		public short GenerateRandomName(sbyte orgTemplateId)
		{
			short result;
			switch (orgTemplateId)
			{
			case 36:
			{
				short index = this._nextVillageRandomNameIndex;
				this._nextVillageRandomNameIndex += 1;
				result = this._villageRandomNameIds[(int)index];
				break;
			}
			case 37:
			{
				short index2 = this._nextTownRandomNameIndex;
				this._nextTownRandomNameIndex += 1;
				result = this._townRandomNameIds[(int)index2];
				break;
			}
			case 38:
			{
				short index3 = this._nextWalledTownRandomNameIndex;
				this._nextWalledTownRandomNameIndex += 1;
				result = this._walledTownRandomNameIds[(int)index3];
				break;
			}
			default:
				result = -1;
				break;
			}
			return result;
		}

		// Token: 0x04001511 RID: 5393
		private readonly List<short> _villageRandomNameIds;

		// Token: 0x04001512 RID: 5394
		private short _nextVillageRandomNameIndex;

		// Token: 0x04001513 RID: 5395
		private readonly List<short> _townRandomNameIds;

		// Token: 0x04001514 RID: 5396
		private short _nextTownRandomNameIndex;

		// Token: 0x04001515 RID: 5397
		private readonly List<short> _walledTownRandomNameIds;

		// Token: 0x04001516 RID: 5398
		private short _nextWalledTownRandomNameIndex;
	}
}
