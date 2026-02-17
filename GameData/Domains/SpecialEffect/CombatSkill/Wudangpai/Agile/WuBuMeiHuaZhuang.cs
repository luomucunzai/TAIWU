using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Agile
{
	// Token: 0x020003E4 RID: 996
	public class WuBuMeiHuaZhuang : AgileSkillBase
	{
		// Token: 0x06003812 RID: 14354 RVA: 0x00238B0E File Offset: 0x00236D0E
		public WuBuMeiHuaZhuang()
		{
		}

		// Token: 0x06003813 RID: 14355 RVA: 0x00238B18 File Offset: 0x00236D18
		public WuBuMeiHuaZhuang(CombatSkillKey skillKey) : base(skillKey, 4400)
		{
		}

		// Token: 0x06003814 RID: 14356 RVA: 0x00238B28 File Offset: 0x00236D28
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 74 : 107, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003815 RID: 14357 RVA: 0x00238B97 File Offset: 0x00236D97
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003816 RID: 14358 RVA: 0x00238BC8 File Offset: 0x00236DC8
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || base.CombatChar != (base.IsDirect ? attacker : defender);
			if (!flag)
			{
				this._affected = false;
				bool flag2 = pursueIndex == 0;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003817 RID: 14359 RVA: 0x00238C18 File Offset: 0x00236E18
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || base.CombatChar != (base.IsDirect ? context.Attacker : context.Defender);
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003818 RID: 14360 RVA: 0x00238C6C File Offset: 0x00236E6C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				DamageCompareData damageCompareData = DomainManager.Combat.GetDamageCompareData();
				int index = damageCompareData.HitType.IndexOf((sbyte)dataKey.CustomParam0);
				bool flag2 = index < 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					bool flag3 = damageCompareData.HitValue[index] >= damageCompareData.AvoidValue[index];
					if (flag3)
					{
						result = 0;
					}
					else
					{
						bool flag4 = dataKey.FieldId == 74;
						if (flag4)
						{
							this._affected = true;
							result = 80;
						}
						else
						{
							bool flag5 = dataKey.FieldId == 107;
							if (flag5)
							{
								this._affected = true;
								result = -80;
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

		// Token: 0x04001068 RID: 4200
		private const sbyte ChangeHitOdds = 80;

		// Token: 0x04001069 RID: 4201
		private bool _affected;
	}
}
