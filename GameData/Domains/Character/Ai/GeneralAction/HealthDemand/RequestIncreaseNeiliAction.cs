using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.HealthDemand
{
	// Token: 0x0200089F RID: 2207
	public class RequestIncreaseNeiliAction : IGeneralAction
	{
		// Token: 0x170004D8 RID: 1240
		// (get) Token: 0x0600785C RID: 30812 RVA: 0x004640A9 File Offset: 0x004622A9
		public sbyte ActionEnergyType
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600785D RID: 30813 RVA: 0x004640AC File Offset: 0x004622AC
		public bool CheckValid(Character selfChar, Character targetChar)
		{
			return selfChar.GetCurrNeili() < selfChar.GetMaxNeili() && targetChar.GetInventory().Items.ContainsKey(this.ItemUsed);
		}

		// Token: 0x0600785E RID: 30814 RVA: 0x004640E8 File Offset: 0x004622E8
		public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			int selfCharId = selfChar.GetId();
			Location location = targetChar.GetLocation();
			monthlyEventCollection.AddRequestNeili(selfCharId, location, targetChar.GetId(), (ulong)this.ItemUsed);
			CharacterDomain.AddLockMovementCharSet(selfCharId);
		}

		// Token: 0x0600785F RID: 30815 RVA: 0x00464130 File Offset: 0x00462330
		public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
		{
			int selfCharId = selfChar.GetId();
			int targetCharId = targetChar.GetId();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			bool agreeToRequest = this.AgreeToRequest;
			if (agreeToRequest)
			{
				bool flag = this.ItemUsed.ItemType != 12;
				if (flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(40, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Invalid item type ");
					defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(this.ItemUsed);
					defaultInterpolatedStringHandler.AppendLiteral(" to restore neili for ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(selfChar.GetId());
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				MiscItem miscCfg = Config.Misc.Instance[this.ItemUsed.TemplateId];
				ItemBase baseItem = DomainManager.Item.GetBaseItem(this.ItemUsed);
				targetChar.RemoveInventoryItem(context, this.ItemUsed, 1, false, false);
				selfChar.ChangeCurrNeili(context, (int)miscCfg.Neili);
				selfChar.ChangeHappiness(context, (int)baseItem.GetHappinessChange());
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseItem.GetFavorabilityChange() * 5);
				lifeRecordCollection.AddRequestNeiliSucceed(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId);
				SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset = secretInformationCollection.AddAcceptRequestIncreaseNeili(targetCharId, selfCharId);
				DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset, true);
			}
			else
			{
				selfChar.ChangeHappiness(context, -3);
				DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, -6000);
				lifeRecordCollection.AddRequestNeiliFail(selfCharId, currDate, targetCharId, location, this.ItemUsed.ItemType, this.ItemUsed.TemplateId);
				SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
				int secretInfoOffset2 = secretInformationCollection2.AddRefuseRequestIncreaseNeili(targetCharId, selfCharId);
				DomainManager.Information.AddSecretInformationMetaData(context, secretInfoOffset2, true);
			}
		}

		// Token: 0x0400216C RID: 8556
		public ItemKey ItemUsed;

		// Token: 0x0400216D RID: 8557
		public bool AgreeToRequest;
	}
}
