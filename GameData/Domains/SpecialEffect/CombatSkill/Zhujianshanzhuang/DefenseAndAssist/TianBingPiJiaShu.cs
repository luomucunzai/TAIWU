using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.DefenseAndAssist
{
	// Token: 0x020001D7 RID: 471
	public class TianBingPiJiaShu : DefenseSkillBase
	{
		// Token: 0x06002D58 RID: 11608 RVA: 0x0020B55A File Offset: 0x0020975A
		public TianBingPiJiaShu()
		{
		}

		// Token: 0x06002D59 RID: 11609 RVA: 0x0020B564 File Offset: 0x00209764
		public TianBingPiJiaShu(CombatSkillKey skillKey) : base(skillKey, 9704)
		{
		}

		// Token: 0x06002D5A RID: 11610 RVA: 0x0020B574 File Offset: 0x00209774
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(102, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002D5B RID: 11611 RVA: 0x0020B5E0 File Offset: 0x002097E0
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002D5C RID: 11612 RVA: 0x0020B640 File Offset: 0x00209840
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = defender != base.CombatChar || pursueIndex > 0 || !DomainManager.Combat.InAttackRange(attacker) || !base.CanAffect;
			if (!flag)
			{
				this.UpdateAffecting(attacker.NormalAttackBodyPart);
			}
		}

		// Token: 0x06002D5D RID: 11613 RVA: 0x0020B688 File Offset: 0x00209888
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = !this._affecting || defender != base.CombatChar;
			if (!flag)
			{
				this._affecting = false;
			}
		}

		// Token: 0x06002D5E RID: 11614 RVA: 0x0020B6BC File Offset: 0x002098BC
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = isAlly == base.CombatChar.IsAlly || !base.CanAffect || Config.CombatSkill.Instance[skillId].EquipType != 1 || !DomainManager.Combat.InAttackRange(DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false)) || DomainManager.Combat.GetCombatCharacter(base.CombatChar.IsAlly, true) != base.CombatChar;
			if (!flag)
			{
				this.UpdateAffecting(DomainManager.Combat.GetElement_CombatCharacterDict(charId).SkillAttackBodyPart);
			}
		}

		// Token: 0x06002D5F RID: 11615 RVA: 0x0020B75C File Offset: 0x0020995C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this._affecting || isAlly == base.CombatChar.IsAlly || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				this._affecting = false;
			}
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x0020B7A8 File Offset: 0x002099A8
		private void UpdateAffecting(sbyte attackBodyPart)
		{
			this._affecting = this.CheckCanAffect(attackBodyPart);
			bool affecting = this._affecting;
			if (affecting)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x0020B7D8 File Offset: 0x002099D8
		private bool CheckCanAffect(sbyte attackBodyPart)
		{
			bool flag = attackBodyPart < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = base.CurrEnemyChar.GetCharacter().GetConsummateLevel() <= this.CharObj.GetConsummateLevel();
				if (flag2)
				{
					result = true;
				}
				else
				{
					sbyte weaponGrade = DomainManager.Combat.GetUsingWeapon(base.CurrEnemyChar).GetGrade();
					int armorGrade = -1;
					ItemKey armorKey = base.CombatChar.Armors[(int)attackBodyPart];
					bool flag3 = armorKey.IsValid();
					if (flag3)
					{
						GameData.Domains.Item.Armor armor = DomainManager.Item.GetElement_Armors(armorKey.Id);
						bool flag4 = armor.GetCurrDurability() > 0;
						if (flag4)
						{
							armorGrade = (int)armor.GetGrade();
						}
					}
					result = (armorGrade >= 0 && armorGrade + 2 > (int)weaponGrade);
				}
			}
			return result;
		}

		// Token: 0x06002D62 RID: 11618 RVA: 0x0020B898 File Offset: 0x00209A98
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._affecting || dataKey.CustomParam0 != (base.IsDirect ? 0 : 1);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102;
				if (flag2)
				{
					result = -60;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000DA0 RID: 3488
		private const sbyte ArmorExtraGrade = 2;

		// Token: 0x04000DA1 RID: 3489
		private const sbyte ReduceDamagePercent = -60;

		// Token: 0x04000DA2 RID: 3490
		private bool _affecting;
	}
}
