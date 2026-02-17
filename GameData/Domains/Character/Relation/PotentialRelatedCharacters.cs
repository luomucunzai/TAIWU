using System;
using System.Collections.Generic;

namespace GameData.Domains.Character.Relation
{
	// Token: 0x02000823 RID: 2083
	public class PotentialRelatedCharacters
	{
		// Token: 0x06007556 RID: 30038 RVA: 0x00449974 File Offset: 0x00447B74
		public void OfflineClear()
		{
			this.AdoptiveParents.Clear();
			this.AdoptiveChildren.Clear();
			this.AdoptiveBrothersAndSisters.Clear();
			this.SwornBrothersAndSisters.Clear();
			this.HusbandsAndWives.Clear();
			this.Mentors.Clear();
			this.Mentees.Clear();
			this.Friends.Clear();
			this.Adored.Clear();
			this.BoyAndGirlFriends.Clear();
			this.Enemies.Clear();
		}

		// Token: 0x04001F43 RID: 8003
		public readonly List<int> AdoptiveParents = new List<int>();

		// Token: 0x04001F44 RID: 8004
		public readonly List<int> AdoptiveChildren = new List<int>();

		// Token: 0x04001F45 RID: 8005
		public readonly List<int> AdoptiveBrothersAndSisters = new List<int>();

		// Token: 0x04001F46 RID: 8006
		public readonly List<int> SwornBrothersAndSisters = new List<int>();

		// Token: 0x04001F47 RID: 8007
		public readonly List<int> HusbandsAndWives = new List<int>();

		// Token: 0x04001F48 RID: 8008
		public readonly List<int> Mentors = new List<int>();

		// Token: 0x04001F49 RID: 8009
		public readonly List<int> Mentees = new List<int>();

		// Token: 0x04001F4A RID: 8010
		public readonly List<int> Friends = new List<int>();

		// Token: 0x04001F4B RID: 8011
		public readonly List<int> Adored = new List<int>();

		// Token: 0x04001F4C RID: 8012
		public readonly List<int> BoyAndGirlFriends = new List<int>();

		// Token: 0x04001F4D RID: 8013
		public readonly List<int> Enemies = new List<int>();
	}
}
