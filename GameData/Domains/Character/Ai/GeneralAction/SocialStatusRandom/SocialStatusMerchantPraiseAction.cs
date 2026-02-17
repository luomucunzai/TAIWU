using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom
{
	// Token: 0x0200088F RID: 2191
	public class SocialStatusMerchantPraiseAction : IGeneralAction
	{
		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x0600780C RID: 30732 RVA: 0x00461F5C File Offset: 0x0046015C
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x0600780D RID: 30733 RVA: 0x00461F60 File Offset: 0x00460160
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return true;
		}

		// Token: 0x0600780E RID: 30734 RVA: 0x00461F74 File Offset: 0x00460174
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddAdviseMerchantFavor(selfCharId, location, targetChar.GetId());
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x0600780F RID: 30735 RVA: 0x00461FB4 File Offset: 0x004601B4
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
