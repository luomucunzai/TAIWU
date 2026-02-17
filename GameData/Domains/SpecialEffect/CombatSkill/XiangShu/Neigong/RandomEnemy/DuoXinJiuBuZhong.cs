using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.RandomEnemy
{
	// Token: 0x02000291 RID: 657
	public class DuoXinJiuBuZhong : MinionBase
	{
		// Token: 0x06003142 RID: 12610 RVA: 0x0021A60F File Offset: 0x0021880F
		public DuoXinJiuBuZhong()
		{
		}

		// Token: 0x06003143 RID: 12611 RVA: 0x0021A619 File Offset: 0x00218819
		public DuoXinJiuBuZhong(CombatSkillKey skillKey) : base(skillKey, 16005)
		{
		}

		// Token: 0x06003144 RID: 12612 RVA: 0x0021A629 File Offset: 0x00218829
		public override bool IsOn(int counterType)
		{
			return MinionBase.CanAffect;
		}

		// Token: 0x06003145 RID: 12613 RVA: 0x0021A630 File Offset: 0x00218830
		protected override IEnumerable<int> CalcFrameCounterPeriods()
		{
			yield return 300;
			yield break;
		}

		// Token: 0x06003146 RID: 12614 RVA: 0x0021A640 File Offset: 0x00218840
		public unsafe override void OnProcess(DataContext context, int counterType)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			NeiliProportionOfFiveElements neiliProportion = enemyChar.GetCharacter().GetNeiliProportionOfFiveElements();
			sbyte fiveElementsType = (sbyte)NeiliType.Instance[enemyChar.GetNeiliType()].FiveElements;
			int poisonValue = (int)((fiveElementsType != 5) ? (50 * *neiliProportion[(int)fiveElementsType] / 100) : 50);
			DomainManager.Combat.AddPoison(context, base.CombatChar, enemyChar, DuoXinJiuBuZhong.PoisonTypeDict[fiveElementsType], 2, poisonValue, -1, true, true, default(ItemKey), false, false, false);
		}

		// Token: 0x06003147 RID: 12615 RVA: 0x0021A6D4 File Offset: 0x002188D4
		// Note: this type is marked as 'beforefieldinit'.
		static DuoXinJiuBuZhong()
		{
			Dictionary<sbyte, sbyte> dictionary = new Dictionary<sbyte, sbyte>();
			dictionary[0] = 3;
			dictionary[1] = 0;
			dictionary[2] = 4;
			dictionary[3] = 2;
			dictionary[4] = 1;
			dictionary[5] = 5;
			DuoXinJiuBuZhong.PoisonTypeDict = dictionary;
		}

		// Token: 0x04000E99 RID: 3737
		private const short EffectRequireFrame = 300;

		// Token: 0x04000E9A RID: 3738
		private const sbyte PoisonBaseValue = 50;

		// Token: 0x04000E9B RID: 3739
		private const sbyte PoisonLevel = 2;

		// Token: 0x04000E9C RID: 3740
		private static readonly Dictionary<sbyte, sbyte> PoisonTypeDict;
	}
}
