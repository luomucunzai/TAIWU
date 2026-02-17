using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Information;
using GameData.Domains.Item;

namespace GameData.Domains.TaiwuEvent.EventLog
{
	// Token: 0x020000D2 RID: 210
	public class EventLogCharacterData
	{
		// Token: 0x06001CD7 RID: 7383 RVA: 0x00187224 File Offset: 0x00185424
		public EventLogCharacterData(bool isTaiwu)
		{
			this.Happiness = 0;
			this.Fame = 0;
			this.Infection = 0;
			this.InfectionStatus = 216;
			this.Item = new Dictionary<ItemKey, int>();
			this.Resource = new int[8];
			this.Exp = 0;
			this.Health = 0;
			this.MainAttribute = default(MainAttributes);
			this.Injury = default(Injuries);
			this.Poison = default(PoisonInts);
			this.DisorderOfQi = 0;
			this.CombatSkills = new List<short>();
			this.LifeSkills = new List<LifeSkillItem>();
			this.Relation = new List<ValueTuple<bool, int, int, ushort>>();
			this.Feature = new List<short>();
			if (isTaiwu)
			{
				this.SecretInformation = new List<int>();
				this.NormalInformation = new List<NormalInformation>();
				this.Teammate = new ValueTuple<bool, int>(false, -1);
				this.Profession = new Dictionary<int, int>();
			}
		}

		// Token: 0x040006A1 RID: 1697
		public int Happiness;

		// Token: 0x040006A2 RID: 1698
		public sbyte Fame;

		// Token: 0x040006A3 RID: 1699
		public byte Infection;

		// Token: 0x040006A4 RID: 1700
		public short InfectionStatus;

		// Token: 0x040006A5 RID: 1701
		public Dictionary<ItemKey, int> Item;

		// Token: 0x040006A6 RID: 1702
		public int[] Resource;

		// Token: 0x040006A7 RID: 1703
		public int Exp;

		// Token: 0x040006A8 RID: 1704
		public short Health;

		// Token: 0x040006A9 RID: 1705
		public MainAttributes MainAttribute;

		// Token: 0x040006AA RID: 1706
		public Injuries Injury;

		// Token: 0x040006AB RID: 1707
		public PoisonInts Poison;

		// Token: 0x040006AC RID: 1708
		public short DisorderOfQi;

		// Token: 0x040006AD RID: 1709
		public List<short> CombatSkills;

		// Token: 0x040006AE RID: 1710
		public List<LifeSkillItem> LifeSkills;

		// Token: 0x040006AF RID: 1711
		public List<short> Feature;

		// Token: 0x040006B0 RID: 1712
		public short FavorabilityToTaiwu;

		// Token: 0x040006B1 RID: 1713
		public int ApprovedTaiwu;

		// Token: 0x040006B2 RID: 1714
		public List<int> SecretInformation;

		// Token: 0x040006B3 RID: 1715
		public List<NormalInformation> NormalInformation;

		// Token: 0x040006B4 RID: 1716
		public ValueTuple<sbyte, int> Combat;

		// Token: 0x040006B5 RID: 1717
		public int SpiritualDebt;

		// Token: 0x040006B6 RID: 1718
		public ValueTuple<bool, int> Teammate;

		// Token: 0x040006B7 RID: 1719
		public Dictionary<int, int> Profession;

		// Token: 0x040006B8 RID: 1720
		public List<ValueTuple<bool, int, int, ushort>> Relation;
	}
}
