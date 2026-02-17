using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction
{
	// Token: 0x02000858 RID: 2136
	[SerializableGameData(NotForDisplayModule = true, IsExtensible = true, NoCopyConstructors = true)]
	public class EscortPrisonerAction : ExtensiblePrioritizedAction
	{
		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060076D9 RID: 30425 RVA: 0x00459170 File Offset: 0x00457370
		public override short ActionType
		{
			get
			{
				return 20;
			}
		}

		// Token: 0x060076DA RID: 30426 RVA: 0x00459174 File Offset: 0x00457374
		public override bool CheckValid(Character selfChar)
		{
			bool flag = !selfChar.IsActiveExternalRelationState(2);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int selfCharId = selfChar.GetId();
				sbyte selfOrgTemplateId = selfChar.GetOrganizationInfo().OrgTemplateId;
				List<KidnappedCharacter> kidnappedCharList = DomainManager.Character.GetKidnappedCharacters(selfCharId).GetCollection();
				foreach (KidnappedCharacter kidnappedChar in kidnappedCharList)
				{
					sbyte bountySectTemplateId = DomainManager.Organization.GetFugitiveBountySect(kidnappedChar.CharId);
					bool flag2 = bountySectTemplateId < 0;
					if (!flag2)
					{
						bool flag3 = bountySectTemplateId != selfOrgTemplateId;
						if (!flag3)
						{
							return base.CheckValid(selfChar);
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060076DB RID: 30427 RVA: 0x00459240 File Offset: 0x00457440
		public override void OnStart(DataContext context, Character selfChar)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(27, 2);
			defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
			defaultInterpolatedStringHandler.AppendLiteral(" start escorting prisoner ");
			defaultInterpolatedStringHandler.AppendFormatted<Character>(DomainManager.Character.GetElement_Objects(this.Target.TargetCharId));
			defaultInterpolatedStringHandler.AppendLiteral(".");
			AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			int selfCharId = selfChar.GetId();
			int targetCharId = this.Target.TargetCharId;
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			LifeRecordCollection lifeRecords = DomainManager.LifeRecord.GetLifeRecordCollection();
			short settlementId = selfChar.GetOrganizationInfo().SettlementId;
			lifeRecords.AddDecideToEscortPrisoner(selfCharId, currDate, targetCharId, location, settlementId);
		}

		// Token: 0x060076DC RID: 30428 RVA: 0x004592F4 File Offset: 0x004574F4
		public override void OnInterrupt(DataContext context, Character selfChar)
		{
			Character targetChar;
			string message;
			if (!DomainManager.Character.TryGetElement_Objects(this.Target.TargetCharId, out targetChar))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
				defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
				defaultInterpolatedStringHandler.AppendLiteral(" end escorting prisoner ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(this.Target.TargetCharId);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				message = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			else
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 2);
				defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
				defaultInterpolatedStringHandler.AppendLiteral(" end escorting prisoner ");
				defaultInterpolatedStringHandler.AppendFormatted<Character>(targetChar);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				message = defaultInterpolatedStringHandler.ToStringAndClear();
			}
			AdaptableLog.Info(message);
		}

		// Token: 0x060076DD RID: 30429 RVA: 0x004593A8 File Offset: 0x004575A8
		public override bool Execute(DataContext context, Character selfChar)
		{
			int selfCharId = selfChar.GetId();
			sbyte selfOrgTemplateId = selfChar.GetOrganizationInfo().OrgTemplateId;
			KidnappedCharacterList kidnappedChars = DomainManager.Character.GetKidnappedCharacters(selfCharId);
			List<KidnappedCharacter> kidnappedCharList = kidnappedChars.GetCollection();
			for (int index = kidnappedCharList.Count - 1; index >= 0; index--)
			{
				KidnappedCharacter kidnappedChar = kidnappedCharList[index];
				sbyte bountySectTemplateId = DomainManager.Organization.GetFugitiveBountySect(kidnappedChar.CharId);
				bool flag = bountySectTemplateId < 0;
				if (!flag)
				{
					bool flag2 = bountySectTemplateId != selfOrgTemplateId;
					if (!flag2)
					{
						int currDate = DomainManager.World.GetCurrDate();
						Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(bountySectTemplateId);
						SettlementBounty bounty = sect.Prison.GetBounty(kidnappedChar.CharId);
						Character targetChar = DomainManager.Character.GetElement_Objects(kidnappedChar.CharId);
						DomainManager.Character.RemoveKidnappedCharacter(context, selfChar, kidnappedChars, index, false);
						DomainManager.Organization.PunishSectMember(context, sect, targetChar, bounty.PunishmentSeverity, bounty.PunishmentType, true);
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
						defaultInterpolatedStringHandler.AppendFormatted<Character>(selfChar);
						defaultInterpolatedStringHandler.AppendLiteral(" successfully escorted ");
						defaultInterpolatedStringHandler.AppendFormatted<Character>(targetChar);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
						int targetCharId = kidnappedChar.CharId;
						Location location = selfChar.GetLocation();
						LifeRecordCollection lifeRecords = DomainManager.LifeRecord.GetLifeRecordCollection();
						lifeRecords.AddEscortPrisonerSucceed(selfCharId, currDate, targetCharId, location);
						selfChar.RecordFameAction(context, 83, targetCharId, bounty.CaptorFameActionMultiplier, true);
					}
				}
			}
			return true;
		}
	}
}
