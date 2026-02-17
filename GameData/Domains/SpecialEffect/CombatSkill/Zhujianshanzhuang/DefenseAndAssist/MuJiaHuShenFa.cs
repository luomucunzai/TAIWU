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
	// Token: 0x020001D3 RID: 467
	public class MuJiaHuShenFa : DefenseSkillBase
	{
		// Token: 0x06002D3C RID: 11580 RVA: 0x0020ACBD File Offset: 0x00208EBD
		public MuJiaHuShenFa()
		{
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x0020ACC7 File Offset: 0x00208EC7
		public MuJiaHuShenFa(CombatSkillKey skillKey) : base(skillKey, 9701)
		{
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x0020ACD8 File Offset: 0x00208ED8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(102, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(base.IsDirect ? 144 : 143, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002D3F RID: 11583 RVA: 0x0020AD84 File Offset: 0x00208F84
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002D40 RID: 11584 RVA: 0x0020AE08 File Offset: 0x00209008
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = defender != base.CombatChar || pursueIndex > 0 || !DomainManager.Combat.InAttackRange(attacker) || !base.CanAffect;
			if (!flag)
			{
				this.UpdateAffecting(attacker.NormalAttackBodyPart);
			}
		}

		// Token: 0x06002D41 RID: 11585 RVA: 0x0020AE50 File Offset: 0x00209050
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._reduceInjuryAffected || defender != base.CombatChar;
			if (!flag)
			{
				this._reduceInjuryAffected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06002D42 RID: 11586 RVA: 0x0020AE8C File Offset: 0x0020908C
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = !this._armorBuffAffecting || defender != base.CombatChar;
			if (!flag)
			{
				this._armorBuffAffecting = false;
			}
		}

		// Token: 0x06002D43 RID: 11587 RVA: 0x0020AEC0 File Offset: 0x002090C0
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = isAlly == base.CombatChar.IsAlly || !base.CanAffect || Config.CombatSkill.Instance[skillId].EquipType != 1 || !DomainManager.Combat.InAttackRange(DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false)) || DomainManager.Combat.GetCombatCharacter(base.CombatChar.IsAlly, true) != base.CombatChar;
			if (!flag)
			{
				this.UpdateAffecting(DomainManager.Combat.GetElement_CombatCharacterDict(charId).SkillAttackBodyPart);
			}
		}

		// Token: 0x06002D44 RID: 11588 RVA: 0x0020AF60 File Offset: 0x00209160
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._reduceInjuryAffected || context.Defender != base.CombatChar;
			if (!flag)
			{
				this._reduceInjuryAffected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06002D45 RID: 11589 RVA: 0x0020AFA0 File Offset: 0x002091A0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this._armorBuffAffecting || isAlly == base.CombatChar.IsAlly || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				this._armorBuffAffecting = false;
			}
		}

		// Token: 0x06002D46 RID: 11590 RVA: 0x0020AFEC File Offset: 0x002091EC
		private void UpdateAffecting(sbyte attackBodyPart)
		{
			this._armorBuffAffecting = this.CheckCanAffect(attackBodyPart);
			bool armorBuffAffecting = this._armorBuffAffecting;
			if (armorBuffAffecting)
			{
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06002D47 RID: 11591 RVA: 0x0020B01C File Offset: 0x0020921C
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

		// Token: 0x06002D48 RID: 11592 RVA: 0x0020B0DC File Offset: 0x002092DC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102 && base.CanAffect;
				if (flag2)
				{
					this._reduceInjuryAffected = true;
					result = -15;
				}
				else
				{
					bool flag3 = dataKey.FieldId == (base.IsDirect ? 144 : 143) && this._armorBuffAffecting;
					if (flag3)
					{
						result = 50;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04000D96 RID: 3478
		private const sbyte ArmorExtraGrade = 2;

		// Token: 0x04000D97 RID: 3479
		private const sbyte ReduceDamagePercent = -15;

		// Token: 0x04000D98 RID: 3480
		private const sbyte ArmorAddValue = 50;

		// Token: 0x04000D99 RID: 3481
		private bool _reduceInjuryAffected;

		// Token: 0x04000D9A RID: 3482
		private bool _armorBuffAffecting;
	}
}
