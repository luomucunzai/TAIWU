using System;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x02000851 RID: 2129
	public class SectCandidateSkills
	{
		// Token: 0x0600769D RID: 30365 RVA: 0x004579EE File Offset: 0x00455BEE
		public SectCandidateSkills()
		{
			this.Initialize(-1);
		}

		// Token: 0x0600769E RID: 30366 RVA: 0x00457A00 File Offset: 0x00455C00
		public SectCandidateSkills(sbyte orgTemplateId)
		{
			this.Initialize(orgTemplateId);
		}

		// Token: 0x0600769F RID: 30367 RVA: 0x00457A12 File Offset: 0x00455C12
		public void Initialize(sbyte orgTemplateId)
		{
			this.OrgTemplateId = orgTemplateId;
			this.CombatSkillsCount = 0;
			this.MaxGrade = -1;
			this.SkillTemplateIds = new short[]
			{
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1,
				-1
			};
		}

		// Token: 0x060076A0 RID: 30368 RVA: 0x00457A44 File Offset: 0x00455C44
		public void Add(short skillTemplateId, sbyte grade)
		{
			this.CombatSkillsCount += 1;
			bool flag = this.MaxGrade < grade;
			if (flag)
			{
				this.MaxGrade = grade;
			}
			this.SkillTemplateIds[(int)grade] = skillTemplateId;
		}

		// Token: 0x040020C3 RID: 8387
		public sbyte OrgTemplateId;

		// Token: 0x040020C4 RID: 8388
		public sbyte CombatSkillsCount;

		// Token: 0x040020C5 RID: 8389
		public sbyte MaxGrade;

		// Token: 0x040020C6 RID: 8390
		public short[] SkillTemplateIds;
	}
}
