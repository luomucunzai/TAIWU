using System;
using GameData.Common;
using GameData.Serializer;

namespace GameData.DLC
{
	// Token: 0x020008D9 RID: 2265
	public interface IDlcEntry : ISerializableGameData
	{
		// Token: 0x06008149 RID: 33097
		void OnLoadedArchiveData();

		// Token: 0x0600814A RID: 33098
		void OnEnterNewWorld();

		// Token: 0x0600814B RID: 33099
		void OnPostAdvanceMonth(DataContext context);

		// Token: 0x0600814C RID: 33100
		void OnCrossArchive(DataContext context, IDlcEntry entryBeforeCrossArchive);

		// Token: 0x0600814D RID: 33101
		void FixAbnormalArchiveData(DataContext context);
	}
}
