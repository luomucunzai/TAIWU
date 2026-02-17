using System;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.EventOption
{
	// Token: 0x020000B5 RID: 181
	public class OptionConditionSecretInformationArgBoxKey : TaiwuEventOptionConditionBase
	{
		// Token: 0x06001BAC RID: 7084 RVA: 0x0017D54D File Offset: 0x0017B74D
		public OptionConditionSecretInformationArgBoxKey(short id, string boxKey, string charIdKey, sbyte taiwuNameFormatIndex, Func<int, string, bool> checkFunc) : base(id)
		{
			this.Key = boxKey;
			this.CharIdKey = charIdKey;
			this.TaiwuNameFormatIndex = taiwuNameFormatIndex;
			this.ConditionChecker = checkFunc;
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x0017D578 File Offset: 0x0017B778
		public override bool CheckCondition(EventArgBox box)
		{
			int secretInformationMetaDataId = -1;
			bool flag = box.Get(this.Key, ref secretInformationMetaDataId);
			return flag && this.ConditionChecker(secretInformationMetaDataId, this.CharIdKey);
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x0017D5B8 File Offset: 0x0017B7B8
		public override ValueTuple<short, string[]> GetDisplayData(EventArgBox box)
		{
			int secretInformationMetaDataId = -1;
			bool flag = box.Get(this.Key, ref secretInformationMetaDataId);
			if (flag)
			{
				EventArgBox metaDataArgBox = EventHelper.GetSecretInformationParameters(secretInformationMetaDataId);
				int charId = -1;
				bool flag2 = metaDataArgBox.Get(this.CharIdKey, ref charId);
				if (flag2)
				{
					int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
					bool isTaiwu = charId == taiwuCharId;
					ValueTuple<string, string> nameTuple = DomainManager.Character.GetNameRelatedData(charId).GetDisplayName(isTaiwu);
					string charName = nameTuple.Item1 + nameTuple.Item2;
					bool flag3 = -1 == this.TaiwuNameFormatIndex;
					if (flag3)
					{
						return new ValueTuple<short, string[]>(this.Id, new string[]
						{
							charName
						});
					}
					string[] nameFormatArgs = new string[]
					{
						charName,
						charName
					};
					bool flag4 = nameFormatArgs.CheckIndex((int)this.TaiwuNameFormatIndex);
					if (flag4)
					{
						ValueTuple<string, string> taiwuNameTuple = DomainManager.Character.GetNameRelatedData(taiwuCharId).GetDisplayName(true);
						nameFormatArgs[(int)this.TaiwuNameFormatIndex] = taiwuNameTuple.Item1 + taiwuNameTuple.Item2;
					}
					return new ValueTuple<short, string[]>(this.Id, nameFormatArgs);
				}
			}
			return new ValueTuple<short, string[]>(this.Id, new string[]
			{
				"role decode error"
			});
		}

		// Token: 0x0400065C RID: 1628
		public readonly string Key;

		// Token: 0x0400065D RID: 1629
		public readonly string CharIdKey;

		// Token: 0x0400065E RID: 1630
		public readonly sbyte TaiwuNameFormatIndex;

		// Token: 0x0400065F RID: 1631
		public readonly Func<int, string, bool> ConditionChecker;
	}
}
