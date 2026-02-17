using System;
using System.Collections.Generic;
using GameData.Domains;
using GameData.Domains.Global;
using GameData.Domains.Map;
using Redzen.Random;

namespace GameData.Common
{
	// Token: 0x020008F5 RID: 2293
	public class GameContext : IGameContext
	{
		// Token: 0x17000512 RID: 1298
		// (get) Token: 0x06008228 RID: 33320 RVA: 0x004D9D89 File Offset: 0x004D7F89
		public IRandomSource Random
		{
			get
			{
				return DataContextManager.GetCurrentThreadDataContext().Random;
			}
		}

		// Token: 0x17000513 RID: 1299
		// (get) Token: 0x06008229 RID: 33321 RVA: 0x004D9D95 File Offset: 0x004D7F95
		public string Language
		{
			get
			{
				return GlobalDomain.Settings.Language;
			}
		}

		// Token: 0x17000514 RID: 1300
		// (get) Token: 0x0600822A RID: 33322 RVA: 0x004D9DA1 File Offset: 0x004D7FA1
		public bool NoProfessionSkillCooldown
		{
			get
			{
				return DomainManager.Extra.NoProfessionSkillCooldown;
			}
		}

		// Token: 0x0600822B RID: 33323 RVA: 0x004D9DAD File Offset: 0x004D7FAD
		public bool IsProfessionalSkillUnlockedAndEquipped(int professionSkillTemplateId)
		{
			return DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(professionSkillTemplateId);
		}

		// Token: 0x17000515 RID: 1301
		// (get) Token: 0x0600822C RID: 33324 RVA: 0x004D9DBA File Offset: 0x004D7FBA
		public IReadOnlyDictionary<int, string> CustomTexts
		{
			get
			{
				return DomainManager.World.GetCustomTexts();
			}
		}

		// Token: 0x0600822D RID: 33325 RVA: 0x004D9DC6 File Offset: 0x004D7FC6
		public bool GetWorldFunctionsStatus(byte worldFunctionType)
		{
			return DomainManager.World.GetWorldFunctionsStatus(worldFunctionType);
		}

		// Token: 0x0600822E RID: 33326 RVA: 0x004D9DD3 File Offset: 0x004D7FD3
		public byte GetAreaSize(short areaId)
		{
			return DomainManager.Map.GetAreaSize(areaId);
		}

		// Token: 0x0600822F RID: 33327 RVA: 0x004D9DE0 File Offset: 0x004D7FE0
		public MapBlockData GetBlockData(Location location)
		{
			return DomainManager.Map.GetBlock(location);
		}

		// Token: 0x06008230 RID: 33328 RVA: 0x004D9DED File Offset: 0x004D7FED
		public IEnumerable<short> GetGroupBlockIds(Location rootLocation, MapBlockData rootBlock)
		{
			bool flag = rootBlock.GroupBlockList == null;
			if (flag)
			{
				yield break;
			}
			foreach (MapBlockData groupBlock in rootBlock.GroupBlockList)
			{
				yield return groupBlock.BlockId;
				groupBlock = null;
			}
			List<MapBlockData>.Enumerator enumerator = default(List<MapBlockData>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x17000516 RID: 1302
		// (get) Token: 0x06008231 RID: 33329 RVA: 0x004D9E0B File Offset: 0x004D800B
		public string DataPath
		{
			get
			{
				return "../The Scroll of Taiwu_Data";
			}
		}

		// Token: 0x17000517 RID: 1303
		// (get) Token: 0x06008232 RID: 33330 RVA: 0x004D9E12 File Offset: 0x004D8012
		public bool HideTaiwuOriginalSurname
		{
			get
			{
				return DomainManager.World.GetHideTaiwuOriginalSurname();
			}
		}

		// Token: 0x17000518 RID: 1304
		// (get) Token: 0x06008233 RID: 33331 RVA: 0x004D9E1E File Offset: 0x004D801E
		public int TaiwuCharId
		{
			get
			{
				return DomainManager.Taiwu.GetTaiwuCharId();
			}
		}

		// Token: 0x17000519 RID: 1305
		// (get) Token: 0x06008234 RID: 33332 RVA: 0x004D9E2A File Offset: 0x004D802A
		public int CurrDate
		{
			get
			{
				return DomainManager.World.GetCurrDate();
			}
		}

		// Token: 0x1700051A RID: 1306
		// (get) Token: 0x06008235 RID: 33333 RVA: 0x004D9E36 File Offset: 0x004D8036
		public short MainStoryLineProgress
		{
			get
			{
				return DomainManager.World.GetMainStoryLineProgress();
			}
		}

		// Token: 0x1700051B RID: 1307
		// (get) Token: 0x06008236 RID: 33334 RVA: 0x004D9E42 File Offset: 0x004D8042
		public byte WorldResourceAmountType
		{
			get
			{
				return DomainManager.World.GetWorldResourceAmountType();
			}
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x06008237 RID: 33335 RVA: 0x004D9E4E File Offset: 0x004D804E
		public sbyte XiangshuProgress
		{
			get
			{
				return DomainManager.World.GetXiangshuProgress();
			}
		}
	}
}
