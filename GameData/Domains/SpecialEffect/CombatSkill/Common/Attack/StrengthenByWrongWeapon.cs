using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x020005A5 RID: 1445
	public class StrengthenByWrongWeapon : CombatSkillEffectBase
	{
		// Token: 0x060042EF RID: 17135 RVA: 0x00268FB2 File Offset: 0x002671B2
		protected StrengthenByWrongWeapon()
		{
		}

		// Token: 0x060042F0 RID: 17136 RVA: 0x00268FBC File Offset: 0x002671BC
		protected StrengthenByWrongWeapon(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060042F1 RID: 17137 RVA: 0x00268FCC File Offset: 0x002671CC
		public override void OnEnable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this._directWeaponCount = 0;
				ItemKey[] weapons = base.CombatChar.GetWeapons();
				for (int i = 0; i < 3; i++)
				{
					ItemKey weaponKey = weapons[i];
					bool flag = this.IsRequiredWeapon(weaponKey);
					if (flag)
					{
						this._directWeaponCount++;
					}
				}
				bool flag2 = this._directWeaponCount > 0;
				if (flag2)
				{
					base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
					base.CreateAffectedData(145, EDataModifyType.Add, base.SkillTemplateId);
					base.CreateAffectedData(146, EDataModifyType.Add, base.SkillTemplateId);
					base.ShowSpecialEffectTips(0);
				}
			}
			else
			{
				Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
				base.CreateAffectedAllEnemyData(287, EDataModifyType.Custom, -1);
				base.CreateAffectedAllEnemyData(285, EDataModifyType.Custom, -1);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060042F2 RID: 17138 RVA: 0x002690CC File Offset: 0x002672CC
		public override void OnDisable(DataContext context)
		{
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060042F3 RID: 17139 RVA: 0x0026910C File Offset: 0x0026730C
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId || !this.IsRequiredWeapon(DomainManager.Combat.GetUsingWeaponKey(base.CombatChar));
			if (!flag)
			{
				this._disableSkills = true;
				base.InvalidateCache(context, base.CurrEnemyChar.GetId(), 287);
				base.InvalidateCache(context, base.CurrEnemyChar.GetId(), 285);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060042F4 RID: 17140 RVA: 0x00269194 File Offset: 0x00267394
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._disableSkills = false;
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060042F5 RID: 17141 RVA: 0x002691D0 File Offset: 0x002673D0
		private bool IsRequiredWeapon(ItemKey weaponKey)
		{
			bool result = weaponKey.IsValid() && ItemTemplateHelper.GetItemSubType(weaponKey.ItemType, weaponKey.TemplateId) != this.RequireWeaponSubType;
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				result = (result && DomainManager.Combat.WeaponHasNeedTrick(base.CombatChar, base.SkillTemplateId, DomainManager.Combat.GetElement_WeaponDataDict(weaponKey.Id)));
			}
			return result;
		}

		// Token: 0x060042F6 RID: 17142 RVA: 0x00269244 File Offset: 0x00267444
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = 20 * this._directWeaponCount;
				}
				else
				{
					ushort fieldId = dataKey.FieldId;
					bool flag3 = fieldId - 145 <= 1;
					bool flag4 = flag3;
					if (flag4)
					{
						result = 5 * this._directWeaponCount;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x060042F7 RID: 17143 RVA: 0x002692CC File Offset: 0x002674CC
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag = fieldId == 285 || fieldId == 287;
			bool flag2 = !flag;
			bool result;
			if (flag2)
			{
				result = dataValue;
			}
			else
			{
				result = (dataValue && !this._disableSkills);
			}
			return result;
		}

		// Token: 0x040013CE RID: 5070
		private const sbyte AddPowerUnit = 20;

		// Token: 0x040013CF RID: 5071
		private const sbyte AddRangeUnit = 5;

		// Token: 0x040013D0 RID: 5072
		protected short RequireWeaponSubType;

		// Token: 0x040013D1 RID: 5073
		private int _directWeaponCount;

		// Token: 0x040013D2 RID: 5074
		private bool _disableSkills;
	}
}
