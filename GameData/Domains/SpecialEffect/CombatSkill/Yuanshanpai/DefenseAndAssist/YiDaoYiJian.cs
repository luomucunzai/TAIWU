using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.DefenseAndAssist
{
	// Token: 0x0200020A RID: 522
	public class YiDaoYiJian : AssistSkillBase
	{
		// Token: 0x06002ED1 RID: 11985 RVA: 0x00210CD0 File Offset: 0x0020EED0
		private static bool IsMatchAffect(sbyte skillType, short weaponType)
		{
			if (!true)
			{
			}
			bool result;
			if (skillType != 7)
			{
				if (skillType == 8)
				{
					if (weaponType == 8)
					{
						result = true;
						goto IL_2C;
					}
				}
			}
			else if (weaponType == 9)
			{
				result = true;
				goto IL_2C;
			}
			result = false;
			IL_2C:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06002ED2 RID: 11986 RVA: 0x00210D12 File Offset: 0x0020EF12
		public YiDaoYiJian()
		{
		}

		// Token: 0x06002ED3 RID: 11987 RVA: 0x00210D1C File Offset: 0x0020EF1C
		public YiDaoYiJian(CombatSkillKey skillKey) : base(skillKey, 5601)
		{
		}

		// Token: 0x06002ED4 RID: 11988 RVA: 0x00210D2C File Offset: 0x0020EF2C
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(200, EDataModifyType.Add, -1);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
		}

		// Token: 0x06002ED5 RID: 11989 RVA: 0x00210D7E File Offset: 0x0020EF7E
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
		}

		// Token: 0x06002ED6 RID: 11990 RVA: 0x00210DB8 File Offset: 0x0020EFB8
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || !base.CanAffect || !base.IsDirect;
			if (!flag)
			{
				sbyte skillType = Config.CombatSkill.Instance[skillId].Type;
				short weaponType = DomainManager.Combat.GetUsingWeapon(base.CombatChar).GetItemSubType();
				bool flag2 = !YiDaoYiJian.IsMatchAffect(skillType, weaponType);
				if (!flag2)
				{
					this._affectingSkillId = skillId;
					base.InvalidateCache(context, 200);
					base.ShowSpecialEffectTips(0);
					base.ShowEffectTips(context);
				}
			}
		}

		// Token: 0x06002ED7 RID: 11991 RVA: 0x00210E48 File Offset: 0x0020F048
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != this._affectingSkillId;
			if (!flag)
			{
				this._affectingSkillId = -1;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 200);
			}
		}

		// Token: 0x06002ED8 RID: 11992 RVA: 0x00210E94 File Offset: 0x0020F094
		private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			bool flag = charId != base.CharacterId || base.IsDirect;
			if (!flag)
			{
				bool flag2 = newWeapon.Template.ItemSubType == oldWeapon.Template.ItemSubType;
				if (!flag2)
				{
					int value = DomainManager.Character.CalcWeaponChangeTrickValue(base.CharacterId, newWeapon.Item.GetItemKey(), true, true);
					value *= 3;
					DomainManager.Combat.ChangeChangeTrickProgress(context, base.CombatChar, value);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06002ED9 RID: 11993 RVA: 0x00210F18 File Offset: 0x0020F118
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != this._affectingSkillId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 200)
				{
					num = 0;
				}
				else
				{
					num = 40;
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x04000DED RID: 3565
		private const sbyte AddMaxPower = 40;

		// Token: 0x04000DEE RID: 3566
		private const int ChangeTrickValueCount = 3;

		// Token: 0x04000DEF RID: 3567
		private short _affectingSkillId;
	}
}
