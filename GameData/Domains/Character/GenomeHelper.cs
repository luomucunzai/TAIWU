using System;
using Redzen.Random;

namespace GameData.Domains.Character
{
	// Token: 0x02000812 RID: 2066
	public static class GenomeHelper
	{
		// Token: 0x06007482 RID: 29826 RVA: 0x004447AC File Offset: 0x004429AC
		public static void Inherit(IRandomSource randomSource, Character mother, Character father, ref Genome offspringGenome)
		{
			bool flag = mother != null && father != null;
			if (flag)
			{
				Genome.Inherit(randomSource, mother.GetGenome(), father.GetGenome(), ref offspringGenome);
			}
			else
			{
				bool flag2 = mother == null && father == null;
				if (flag2)
				{
					Genome.CreateRandom(randomSource, ref offspringGenome);
				}
				else
				{
					Genome virtualParentGenome;
					Genome.CreateRandom(randomSource, ref virtualParentGenome);
					ref Genome motherGenome = (mother != null) ? mother.GetGenome() : ref virtualParentGenome;
					ref Genome fatherGenome = (father != null) ? father.GetGenome() : ref virtualParentGenome;
					Genome.Inherit(randomSource, ref motherGenome, ref fatherGenome, ref offspringGenome);
				}
			}
		}
	}
}
