using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.LifeSkillRandom
{
	// Token: 0x02000897 RID: 2199
	public class LifeSkillEntertainmentAction : IGeneralAction
	{
		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x06007834 RID: 30772 RVA: 0x00462D5A File Offset: 0x00460F5A
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06007835 RID: 30773 RVA: 0x00462D60 File Offset: 0x00460F60
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return true;
		}

		// Token: 0x06007836 RID: 30774 RVA: 0x00462D74 File Offset: 0x00460F74
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			int targetCharId = targetChar.GetId();
			int selfCharId = selfChar.GetId();
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			Location location = selfChar.GetLocation();
			switch (this.LifeSkillType)
			{
			case 0:
				monthlyNotifications.AddAmuseOthersByMusic(targetCharId, location, selfCharId);
				break;
			case 1:
				monthlyNotifications.AddAmuseOthersByChess(targetCharId, location, selfCharId);
				break;
			case 2:
				monthlyNotifications.AddAmuseOthersByPoem(targetCharId, location, selfCharId);
				break;
			case 3:
				monthlyNotifications.AddAmuseOthersByPainting(targetCharId, location, selfCharId);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid life skill type for entertainment ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.LifeSkillType);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			this.ApplyChanges(context, selfChar, targetChar);
		}

		// Token: 0x06007837 RID: 30775 RVA: 0x00462E34 File Offset: 0x00461034
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			int targetCharId = targetChar.GetId();
			int selfCharId = selfChar.GetId();
			short selfAttainment = Math.Max(1, selfChar.GetLifeSkillAttainment(this.LifeSkillType));
			short targetAttainment = Math.Max(1, targetChar.GetLifeSkillAttainment(this.LifeSkillType));
			int selfHappinessChange = Math.Clamp((int)(targetAttainment * 5 / selfAttainment - 5), -5, 10);
			int targetHappinessChange = Math.Clamp((int)(selfAttainment * 5 / targetAttainment - 5), -5, 10);
			selfChar.ChangeHappiness(context, selfHappinessChange);
			targetChar.ChangeHappiness(context, targetHappinessChange);
			int selfFavorabilityChange = Math.Clamp((int)(targetAttainment * 1500 / selfAttainment - 1500), -1500, 3000);
			int targetFavorabilityChange = Math.Clamp((int)(selfAttainment * 1500 / targetAttainment - 1500), -1500, 3000);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, selfFavorabilityChange);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, targetFavorabilityChange);
			switch (this.LifeSkillType)
			{
			case 0:
				lifeRecordCollection.AddEntertainWithMusic(selfCharId, currDate, targetCharId, location);
				break;
			case 1:
				lifeRecordCollection.AddEntertainWithChess(selfCharId, currDate, targetCharId, location);
				break;
			case 2:
				lifeRecordCollection.AddEntertainWithPoem(selfCharId, currDate, targetCharId, location);
				break;
			case 3:
				lifeRecordCollection.AddEntertainWithPainting(selfCharId, currDate, targetCharId, location);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid life skill type for entertainment ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.LifeSkillType);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x0400215A RID: 8538
		public sbyte LifeSkillType;
	}
}
