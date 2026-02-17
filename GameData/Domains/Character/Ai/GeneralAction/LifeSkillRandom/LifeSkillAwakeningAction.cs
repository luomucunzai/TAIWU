using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.LifeSkillRandom
{
	// Token: 0x02000893 RID: 2195
	public class LifeSkillAwakeningAction : IGeneralAction
	{
		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06007820 RID: 30752 RVA: 0x004625C1 File Offset: 0x004607C1
		public sbyte ActionEnergyType
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06007821 RID: 30753 RVA: 0x004625C4 File Offset: 0x004607C4
		public unsafe bool CheckValid(Character selfChar, Character targetChar)
		{
			LifeSkillShorts targetQualifications = *targetChar.GetBaseLifeSkillQualifications();
			return *(ref targetQualifications.Items.FixedElementField + (IntPtr)this.IncreasedLifeSkillType * 2) < 90;
		}

		// Token: 0x06007822 RID: 30754 RVA: 0x00462600 File Offset: 0x00460800
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			bool flag = this.IncreasedLifeSkillType >= 0;
			if (flag)
			{
				MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
				sbyte awakeningLifeSkillType = this.AwakeningLifeSkillType;
				sbyte b = awakeningLifeSkillType;
				if (b != 12)
				{
					if (b != 13)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 3);
						defaultInterpolatedStringHandler.AppendFormatted<int>(selfChar.GetId());
						defaultInterpolatedStringHandler.AppendLiteral(" is trying to awake ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(targetChar.GetId());
						defaultInterpolatedStringHandler.AppendLiteral("'s ");
						defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.AwakeningLifeSkillType);
						defaultInterpolatedStringHandler.AppendLiteral(", which is neither Buddhism nor Taoism");
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					monthlyNotifications.AddEnlightenedByBuddhism(selfChar.GetId(), selfChar.GetLocation(), targetChar.GetId(), this.IncreasedLifeSkillType);
				}
				else
				{
					monthlyNotifications.AddEnlightenedByDaoism(selfChar.GetId(), selfChar.GetLocation(), targetChar.GetId(), this.IncreasedLifeSkillType);
				}
			}
			this.ApplyChanges(context, selfChar, targetChar);
		}

		// Token: 0x06007823 RID: 30755 RVA: 0x004626FC File Offset: 0x004608FC
		public unsafe void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool flag = this.IncreasedLifeSkillType >= 0;
			if (flag)
			{
				sbyte awakeningLifeSkillType = this.AwakeningLifeSkillType;
				sbyte b = awakeningLifeSkillType;
				if (b != 12)
				{
					if (b != 13)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 3);
						defaultInterpolatedStringHandler.AppendFormatted<int>(selfChar.GetId());
						defaultInterpolatedStringHandler.AppendLiteral(" is trying to awake ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(targetChar.GetId());
						defaultInterpolatedStringHandler.AppendLiteral("'s ");
						defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.AwakeningLifeSkillType);
						defaultInterpolatedStringHandler.AppendLiteral(", which is neither Buddhism nor Taoism");
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					lifeRecordCollection.AddBuddismAwakeningSucceed(selfCharId, currDate, targetCharId, location);
				}
				else
				{
					lifeRecordCollection.AddTaoismAwakeningSucceed(selfCharId, currDate, targetCharId, location);
				}
				LifeSkillShorts baseLifeSkillQualifications = *targetChar.GetBaseLifeSkillQualifications();
				ref short ptr = ref baseLifeSkillQualifications.Items.FixedElementField + (IntPtr)this.IncreasedLifeSkillType * 2;
				ptr += 1;
				targetChar.SetBaseLifeSkillQualifications(ref baseLifeSkillQualifications, context);
			}
			else
			{
				sbyte awakeningLifeSkillType2 = this.AwakeningLifeSkillType;
				sbyte b2 = awakeningLifeSkillType2;
				if (b2 != 12)
				{
					if (b2 != 13)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(61, 3);
						defaultInterpolatedStringHandler.AppendFormatted<int>(selfChar.GetId());
						defaultInterpolatedStringHandler.AppendLiteral(" is trying to awake ");
						defaultInterpolatedStringHandler.AppendFormatted<int>(targetChar.GetId());
						defaultInterpolatedStringHandler.AppendLiteral("'s ");
						defaultInterpolatedStringHandler.AppendFormatted<sbyte>(this.AwakeningLifeSkillType);
						defaultInterpolatedStringHandler.AppendLiteral(", which is neither Buddhism nor Taoism");
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					lifeRecordCollection.AddBuddismAwakeningFail(selfCharId, currDate, targetCharId, location);
				}
				else
				{
					lifeRecordCollection.AddTaoismAwakeningFail(selfCharId, currDate, targetCharId, location);
				}
			}
		}

		// Token: 0x0400214E RID: 8526
		public sbyte AwakeningLifeSkillType;

		// Token: 0x0400214F RID: 8527
		public sbyte IncreasedLifeSkillType;
	}
}
