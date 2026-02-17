using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Special
{
	// Token: 0x02000546 RID: 1350
	public class YuNvCi : CombatSkillEffectBase
	{
		// Token: 0x0600400D RID: 16397 RVA: 0x0025CB9B File Offset: 0x0025AD9B
		public YuNvCi()
		{
		}

		// Token: 0x0600400E RID: 16398 RVA: 0x0025CBA5 File Offset: 0x0025ADA5
		public YuNvCi(CombatSkillKey skillKey) : base(skillKey, 2406, -1)
		{
		}

		// Token: 0x0600400F RID: 16399 RVA: 0x0025CBB6 File Offset: 0x0025ADB6
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06004010 RID: 16400 RVA: 0x0025CBF0 File Offset: 0x0025ADF0
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06004011 RID: 16401 RVA: 0x0025CC18 File Offset: 0x0025AE18
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._addingPower = false;
				base.InvalidateCache(context, 199);
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && !base.CombatChar.GetAutoCastingSkill();
				if (flag2)
				{
					base.AddMaxEffectCount(true);
				}
			}
		}

		// Token: 0x06004012 RID: 16402 RVA: 0x0025CC84 File Offset: 0x0025AE84
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || base.EffectCount <= 0 || base.CombatChar.GetPreparingSkillId() >= 0;
			if (!flag)
			{
				bool flag2 = (base.IsDirect ? attacker : defender) != base.CombatChar;
				if (!flag2)
				{
					sbyte[] weaponTricks = base.CombatChar.GetWeaponTricks();
					DomainManager.Combat.AddTrick(context, base.CombatChar, weaponTricks[context.Random.Next(0, weaponTricks.Length)], true);
					base.ShowSpecialEffectTips(0);
					bool flag3 = (base.IsDirect ? hit : (!hit)) && DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, false);
					if (flag3)
					{
						this._addingPower = true;
						base.InvalidateCache(context, 199);
						DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
						base.ShowSpecialEffectTips(1);
					}
					base.ReduceEffectCount(1);
				}
			}
		}

		// Token: 0x06004013 RID: 16403 RVA: 0x0025CD88 File Offset: 0x0025AF88
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 199 && this._addingPower;
				if (flag2)
				{
					result = 40;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040012D8 RID: 4824
		private const sbyte AddPower = 40;

		// Token: 0x040012D9 RID: 4825
		private bool _addingPower;
	}
}
