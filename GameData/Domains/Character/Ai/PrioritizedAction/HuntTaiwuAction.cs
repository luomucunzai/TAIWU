using System;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x0200085E RID: 2142
	[SerializableGameData(NotForDisplayModule = true, IsExtensible = true, NoCopyConstructors = true)]
	public class HuntTaiwuAction : ExtensiblePrioritizedAction
	{
		// Token: 0x17000499 RID: 1177
		// (get) Token: 0x060076FB RID: 30459 RVA: 0x0045A120 File Offset: 0x00458320
		public override short ActionType
		{
			get
			{
				return 22;
			}
		}

		// Token: 0x060076FC RID: 30460 RVA: 0x0045A124 File Offset: 0x00458324
		public override bool CheckValid(Character selfChar)
		{
			bool flag = DomainManager.World.GetMainStoryLineProgress() >= 27;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !DomainManager.Character.IsCharacterAlive(selfChar.GetId());
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = !DomainManager.Character.IsCharacterAlive(this.Target.TargetCharId);
					result = (!flag3 && base.CheckValid(selfChar));
				}
			}
			return result;
		}

		// Token: 0x060076FD RID: 30461 RVA: 0x0045A194 File Offset: 0x00458394
		public override void OnStart(DataContext context, Character selfChar)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
			defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
			defaultInterpolatedStringHandler.AppendLiteral(" start huntTaiwuAction,target is ");
			defaultInterpolatedStringHandler.AppendFormatted<Character>(DomainManager.Character.GetElement_Objects(this.Target.TargetCharId));
			defaultInterpolatedStringHandler.AppendLiteral(".");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			int selfCharId = selfChar.GetId();
			int targetCharId = this.Target.TargetCharId;
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecords = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecords.AddJieQingPunishmentAssassinSetOut(selfCharId, currDate, targetCharId);
		}

		// Token: 0x060076FE RID: 30462 RVA: 0x0045A22C File Offset: 0x0045842C
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			Character targetChar;
			string message;
			if (!DomainManager.Character.TryGetElement_Objects(this.Target.TargetCharId, out targetChar))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
				defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
				defaultInterpolatedStringHandler.AppendLiteral(" end huntTaiwuAction ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Target.TargetCharId);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				message = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
				defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
				defaultInterpolatedStringHandler.AppendLiteral(" end huntTaiwuAction ");
				defaultInterpolatedStringHandler.AppendFormatted<Character>(targetChar);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				message = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			AdaptableLog.Info(message);
			int selfCharId = selfChar.GetId();
			int targetCharId = this.Target.TargetCharId;
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecords = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecords.AddJieQingPunishmentAssassinGiveUp(selfCharId, currDate, targetCharId);
		}

		// Token: 0x060076FF RID: 30463 RVA: 0x0045A314 File Offset: 0x00458514
		public override bool Execute(DataContext context, Character selfChar)
		{
			bool flag = DomainManager.World.ClearMonthlyEventCollectionNotEndGame();
			bool result;
			if (flag)
			{
				this.OnInterrupt(context, selfChar);
				result = true;
			}
			else
			{
				Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
				Location selfLocation = selfChar.GetLocation();
				bool flag2 = taiwuLocation.Equals(selfLocation);
				if (flag2)
				{
					DomainManager.Taiwu.JieqingPunishmentAssassinAlreadyAdd = true;
					MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
					monthlyEventCollection.AddJieQingPunishmentAssassin(DomainManager.Taiwu.GetTaiwuCharId(), selfLocation, selfChar.GetId());
					result = true;
				}
				else
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(98, 3);
					defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
					defaultInterpolatedStringHandler.AppendLiteral(" cannot hunt taiwu, because taiwu is not in the same location.selfLocation is ");
					defaultInterpolatedStringHandler.AppendFormatted<Location>(selfLocation);
					defaultInterpolatedStringHandler.AppendLiteral(", taiwuLocation is ");
					defaultInterpolatedStringHandler.AppendFormatted<Location>(taiwuLocation);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
					result = false;
				}
			}
			return result;
		}
	}
}
