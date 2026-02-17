using System;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.FistAndPalm
{
	// Token: 0x02000236 RID: 566
	public class XueHouMoZhang : PoisonAddInjury
	{
		// Token: 0x06002F92 RID: 12178 RVA: 0x00213949 File Offset: 0x00211B49
		public XueHouMoZhang()
		{
		}

		// Token: 0x06002F93 RID: 12179 RVA: 0x00213953 File Offset: 0x00211B53
		public XueHouMoZhang(CombatSkillKey skillKey) : base(skillKey, 15107)
		{
			this.RequirePoisonType = 3;
		}

		// Token: 0x06002F94 RID: 12180 RVA: 0x0021396C File Offset: 0x00211B6C
		protected override void OnCastMaxPower(DataContext context)
		{
			CombatCharacter enemyChar = base.CurrEnemyChar;
			byte poisonMarkCount = (base.IsDirect ? enemyChar : base.CombatChar).GetDefeatMarkCollection().PoisonMarkList[(int)this.RequirePoisonType];
			bool flag = poisonMarkCount > 0;
			if (flag)
			{
				DomainManager.Combat.AppendFatalDamageMark(context, enemyChar, (int)poisonMarkCount, -1, -1, false, EDamageType.None);
				for (int i = 0; i < (int)poisonMarkCount; i++)
				{
					Injuries injuries = enemyChar.GetInjuries();
					bool flag2 = injuries.AllPartsFully(true);
					bool isInner;
					if (flag2)
					{
						isInner = false;
					}
					else
					{
						bool flag3 = injuries.AllPartsFully(false);
						isInner = (flag3 || context.Random.CheckPercentProb(50));
					}
					sbyte bodyPart = injuries.GetLightestPart(isInner, true);
					bool flag4 = bodyPart >= 0;
					if (flag4)
					{
						DomainManager.Combat.AddInjury(context, enemyChar, bodyPart, isInner, 1, true, false);
					}
				}
				DomainManager.Combat.AddToCheckFallenSet(enemyChar.GetId());
			}
			base.ShowSpecialEffectTips(1);
		}
	}
}
