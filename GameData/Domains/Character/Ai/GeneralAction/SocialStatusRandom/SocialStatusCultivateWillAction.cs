using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom
{
	// Token: 0x0200088E RID: 2190
	public class SocialStatusCultivateWillAction : IGeneralAction
	{
		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06007807 RID: 30727 RVA: 0x00461E90 File Offset: 0x00460090
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06007808 RID: 30728 RVA: 0x00461E94 File Offset: 0x00460094
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return true;
		}

		// Token: 0x06007809 RID: 30729 RVA: 0x00461EA8 File Offset: 0x004600A8
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddAdviseWinPeopleSupport(selfCharId, location, targetChar.GetId());
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x0600780A RID: 30730 RVA: 0x00461EE8 File Offset: 0x004600E8
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(29, 2);
			defaultInterpolatedStringHandler.AppendLiteral("targetChar ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(targetChar.GetId());
			defaultInterpolatedStringHandler.AppendLiteral(" has to be Taiwu ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(DomainManager.Taiwu.GetTaiwuCharId());
			defaultInterpolatedStringHandler.AppendLiteral(".");
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}
	}
}
