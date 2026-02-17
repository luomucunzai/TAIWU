using System;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Finger
{
	// Token: 0x0200042B RID: 1067
	public class DaLiJinGangZhi : AddWeaponEquipAttackOnAttack
	{
		// Token: 0x17000209 RID: 521
		// (get) Token: 0x06003994 RID: 14740 RVA: 0x0023F424 File Offset: 0x0023D624
		protected override short AddWeaponEquipAttack
		{
			get
			{
				return 1000;
			}
		}

		// Token: 0x06003995 RID: 14741 RVA: 0x0023F42B File Offset: 0x0023D62B
		public DaLiJinGangZhi()
		{
		}

		// Token: 0x06003996 RID: 14742 RVA: 0x0023F435 File Offset: 0x0023D635
		public DaLiJinGangZhi(CombatSkillKey skillKey) : base(skillKey, 1203)
		{
		}

		// Token: 0x06003997 RID: 14743 RVA: 0x0023F448 File Offset: 0x0023D648
		protected override void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				sbyte bodyPart = base.CombatChar.SkillAttackBodyPart;
				CombatCharacter enemyChar = base.CurrEnemyChar;
				FlawOrAcupointCollection flaws = enemyChar.GetFlawCollection();
				FlawOrAcupointCollection acupoints = enemyChar.GetAcupointCollection();
				this._flawCount = flaws.BodyPartDict[bodyPart].Count;
				this._acupointCount = acupoints.BodyPartDict[bodyPart].Count;
				base.OnPrepareSkillEnd(context, charId, isAlly, skillId);
			}
		}

		// Token: 0x06003998 RID: 14744 RVA: 0x0023F4D4 File Offset: 0x0023D6D4
		protected override void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					sbyte bodyPart = base.CombatChar.SkillAttackBodyPart;
					CombatCharacter enemyChar = base.CurrEnemyChar;
					Injuries injuries = enemyChar.GetInjuries();
					bool flag3 = this._flawCount > 0 || this._acupointCount > 0;
					if (flag3)
					{
						bool flag4 = this._flawCount > 0;
						if (flag4)
						{
							int removedCount = Math.Min(3, this._flawCount);
							int injuryCount = Math.Min(removedCount, (int)(6 - injuries.Get(bodyPart, !base.IsDirect)));
							enemyChar.RemoveRandomFlawOrAcupoint(context, true, removedCount);
							DomainManager.Combat.AddInjury(context, enemyChar, bodyPart, !base.IsDirect, (sbyte)injuryCount, false, false);
							DomainManager.Combat.AppendFatalDamageMark(context, enemyChar, removedCount - injuryCount, (int)(base.IsDirect ? 0 : 1), bodyPart, false, EDamageType.None);
						}
						bool flag5 = this._acupointCount > 0;
						if (flag5)
						{
							int removedCount2 = Math.Min(3, this._flawCount);
							int injuryCount2 = Math.Min(removedCount2, (int)(6 - injuries.Get(bodyPart, !base.IsDirect)));
							enemyChar.RemoveRandomFlawOrAcupoint(context, false, removedCount2);
							DomainManager.Combat.AddInjury(context, enemyChar, bodyPart, !base.IsDirect, (sbyte)injuryCount2, false, false);
							DomainManager.Combat.AppendFatalDamageMark(context, enemyChar, removedCount2 - injuryCount2, (int)(base.IsDirect ? 0 : 1), bodyPart, false, EDamageType.None);
						}
						DomainManager.Combat.UpdateBodyDefeatMark(context, enemyChar, bodyPart);
						base.ShowSpecialEffectTips(1);
					}
				}
				base.OnCastSkillEnd(context, charId, isAlly, skillId, power, interrupted);
			}
		}

		// Token: 0x040010D6 RID: 4310
		private const int ChangeCount = 3;

		// Token: 0x040010D7 RID: 4311
		private int _flawCount;

		// Token: 0x040010D8 RID: 4312
		private int _acupointCount;
	}
}
