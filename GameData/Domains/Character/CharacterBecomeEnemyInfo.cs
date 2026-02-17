using System;
using GameData.Domains.Map;

namespace GameData.Domains.Character
{
	// Token: 0x02000806 RID: 2054
	public struct CharacterBecomeEnemyInfo
	{
		// Token: 0x06007015 RID: 28693 RVA: 0x003FE784 File Offset: 0x003FC984
		[Obsolete("Become enemy info must be created with a character.", true)]
		public CharacterBecomeEnemyInfo()
		{
			throw new NotImplementedException("Become enemy info must be created with a character.");
		}

		// Token: 0x06007016 RID: 28694 RVA: 0x003FE791 File Offset: 0x003FC991
		public CharacterBecomeEnemyInfo(Character selfChar)
		{
			this.Date = DomainManager.World.GetCurrDate();
			this.Location = selfChar.GetValidLocation();
			this.SecretInformationTemplateId = -1;
			this.WugTemplateId = -1;
		}

		// Token: 0x04001E4B RID: 7755
		public int Date;

		// Token: 0x04001E4C RID: 7756
		public Location Location;

		// Token: 0x04001E4D RID: 7757
		public short SecretInformationTemplateId;

		// Token: 0x04001E4E RID: 7758
		public short WugTemplateId;
	}
}
