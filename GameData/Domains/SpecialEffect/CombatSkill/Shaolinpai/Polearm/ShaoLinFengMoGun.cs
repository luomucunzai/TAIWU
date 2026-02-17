using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Polearm
{
	// Token: 0x02000414 RID: 1044
	public class ShaoLinFengMoGun : CombatSkillEffectBase
	{
		// Token: 0x06003928 RID: 14632 RVA: 0x0023D7A5 File Offset: 0x0023B9A5
		public ShaoLinFengMoGun()
		{
		}

		// Token: 0x06003929 RID: 14633 RVA: 0x0023D7B6 File Offset: 0x0023B9B6
		public ShaoLinFengMoGun(CombatSkillKey skillKey) : base(skillKey, 1305, -1)
		{
		}

		// Token: 0x0600392A RID: 14634 RVA: 0x0023D7D0 File Offset: 0x0023B9D0
		public override void OnEnable(DataContext context)
		{
			base.CombatChar.CanNormalAttackInPrepareSkill = true;
			base.CreateAffectedData(base.IsDirect ? 44 : 45, EDataModifyType.AddPercent, -1);
			base.CreateAffectedData(145, EDataModifyType.Add, base.SkillTemplateId);
			base.CreateAffectedData(146, EDataModifyType.Add, base.SkillTemplateId);
			base.CreateAffectedData(283, EDataModifyType.TotalPercent, -1);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600392B RID: 14635 RVA: 0x0023D880 File Offset: 0x0023BA80
		public override void OnDisable(DataContext context)
		{
			base.CombatChar.CanNormalAttackInPrepareSkill = false;
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600392C RID: 14636 RVA: 0x0023D8E4 File Offset: 0x0023BAE4
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !hit || attacker != base.CombatChar || this._hitCount >= 6;
			if (!flag)
			{
				this._hitCount++;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 44 : 45);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
				base.ShowSpecialEffectTips(1);
				base.ShowSpecialEffectTips(2);
			}
		}

		// Token: 0x0600392D RID: 14637 RVA: 0x0023D980 File Offset: 0x0023BB80
		private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			bool flag = charId == base.CharacterId;
			if (flag)
			{
				DomainManager.Combat.InterruptSkill(context, base.CombatChar, -1);
			}
		}

		// Token: 0x0600392E RID: 14638 RVA: 0x0023D9B0 File Offset: 0x0023BBB0
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker != base.CombatChar || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600392F RID: 14639 RVA: 0x0023D9E8 File Offset: 0x0023BBE8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003930 RID: 14640 RVA: 0x0023DA20 File Offset: 0x0023BC20
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
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 44 <= 1;
				bool flag3 = flag2;
				if (flag3)
				{
					result = 10 * this._hitCount;
				}
				else
				{
					bool flag4 = dataKey.FieldId == 283;
					if (flag4)
					{
						result = -50;
					}
					else
					{
						bool flag5 = dataKey.CombatSkillId != base.SkillTemplateId;
						if (flag5)
						{
							result = 0;
						}
						else
						{
							fieldId = dataKey.FieldId;
							flag2 = (fieldId - 145 <= 1);
							bool flag6 = flag2;
							if (flag6)
							{
								result = 5 * this._hitCount;
							}
							else
							{
								result = 0;
							}
						}
					}
				}
			}
			return result;
		}

		// Token: 0x040010B9 RID: 4281
		private const sbyte AddPenetrateUnit = 10;

		// Token: 0x040010BA RID: 4282
		private const sbyte AddRangeUnit = 5;

		// Token: 0x040010BB RID: 4283
		private const int ReduceAttackPrepareFramePercent = -50;

		// Token: 0x040010BC RID: 4284
		private const sbyte MaxAffectCount = 6;

		// Token: 0x040010BD RID: 4285
		private int _hitCount = 0;
	}
}
