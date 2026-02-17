using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Agile
{
	// Token: 0x020001ED RID: 493
	public class XingDouBiNu : AgileSkillBase
	{
		// Token: 0x06002E2F RID: 11823 RVA: 0x0020E01C File Offset: 0x0020C21C
		public XingDouBiNu()
		{
		}

		// Token: 0x06002E30 RID: 11824 RVA: 0x0020E026 File Offset: 0x0020C226
		public XingDouBiNu(CombatSkillKey skillKey) : base(skillKey, 9504)
		{
		}

		// Token: 0x06002E31 RID: 11825 RVA: 0x0020E038 File Offset: 0x0020C238
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(base.IsDirect ? 141 : 142, EDataModifyType.AddPercent, -1);
			this._affecting = false;
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002E32 RID: 11826 RVA: 0x0020E0BC File Offset: 0x0020C2BC
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002E33 RID: 11827 RVA: 0x0020E11C File Offset: 0x0020C31C
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = attacker != base.CombatChar || pursueIndex > 0 || !base.CanAffect;
			if (!flag)
			{
				this.UpdateAffecting(base.CombatChar.NormalAttackBodyPart);
			}
		}

		// Token: 0x06002E34 RID: 11828 RVA: 0x0020E15C File Offset: 0x0020C35C
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = !this._affecting || attacker != base.CombatChar;
			if (!flag)
			{
				this._affecting = false;
			}
		}

		// Token: 0x06002E35 RID: 11829 RVA: 0x0020E190 File Offset: 0x0020C390
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || !base.CanAffect || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				this.UpdateAffecting(base.CombatChar.SkillAttackBodyPart);
			}
		}

		// Token: 0x06002E36 RID: 11830 RVA: 0x0020E1E4 File Offset: 0x0020C3E4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this._affecting || charId != base.CharacterId;
			if (!flag)
			{
				this._affecting = false;
			}
		}

		// Token: 0x06002E37 RID: 11831 RVA: 0x0020E218 File Offset: 0x0020C418
		private void UpdateAffecting(sbyte attackBodyPart)
		{
			this._affecting = this.CheckCanAffect(attackBodyPart);
			bool affecting = this._affecting;
			if (affecting)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06002E38 RID: 11832 RVA: 0x0020E248 File Offset: 0x0020C448
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
					sbyte weaponGrade = DomainManager.Combat.GetUsingWeapon(base.CombatChar).GetGrade();
					int armorGrade = -1;
					ItemKey armorKey = base.CurrEnemyChar.Armors[(int)attackBodyPart];
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
					result = ((int)(weaponGrade + 2) > armorGrade);
				}
			}
			return result;
		}

		// Token: 0x06002E39 RID: 11833 RVA: 0x0020E300 File Offset: 0x0020C500
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._affecting;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = 50;
			}
			return result;
		}

		// Token: 0x04000DC2 RID: 3522
		private const sbyte WeaponExtraGrade = 2;

		// Token: 0x04000DC3 RID: 3523
		private const sbyte AddWeaponAttackOrDefense = 50;

		// Token: 0x04000DC4 RID: 3524
		private bool _affecting;
	}
}
