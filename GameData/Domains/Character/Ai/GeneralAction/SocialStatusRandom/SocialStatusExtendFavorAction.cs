using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom
{
	// Token: 0x0200088D RID: 2189
	public class SocialStatusExtendFavorAction : IGeneralAction
	{
		// Token: 0x170004C6 RID: 1222
		// (get) Token: 0x06007802 RID: 30722 RVA: 0x00461DC2 File Offset: 0x0045FFC2
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06007803 RID: 30723 RVA: 0x00461DC8 File Offset: 0x0045FFC8
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return true;
		}

		// Token: 0x06007804 RID: 30724 RVA: 0x00461DDC File Offset: 0x0045FFDC
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddAdviseExtendFavours(selfCharId, location, targetChar.GetId());
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x06007805 RID: 30725 RVA: 0x00461E1C File Offset: 0x0046001C
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
