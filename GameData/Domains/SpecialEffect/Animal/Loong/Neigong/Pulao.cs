using System;
using System.Runtime.CompilerServices;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x02000602 RID: 1538
	public class Pulao : AnimalEffectBase
	{
		// Token: 0x06004519 RID: 17689 RVA: 0x0027169C File Offset: 0x0026F89C
		private bool IsDamageValid(Pulao.EAcceptDamageType damageType)
		{
			bool beAttacked = this._beAttacked;
			bool result;
			if (beAttacked)
			{
				result = (damageType == this._acceptDamageType);
			}
			else
			{
				Pulao.EAcceptDamageType acceptDamageType = this._acceptDamageType;
				if (!true)
				{
				}
				bool flag;
				switch (acceptDamageType)
				{
				case Pulao.EAcceptDamageType.None:
					flag = true;
					break;
				case Pulao.EAcceptDamageType.Outer:
				{
					bool flag2 = damageType - Pulao.EAcceptDamageType.Inner <= 1;
					flag = flag2;
					break;
				}
				case Pulao.EAcceptDamageType.Inner:
				{
					bool flag2 = damageType == Pulao.EAcceptDamageType.Outer || damageType == Pulao.EAcceptDamageType.Mind;
					flag = flag2;
					break;
				}
				case Pulao.EAcceptDamageType.Mind:
				{
					bool flag2 = damageType - Pulao.EAcceptDamageType.Outer <= 1;
					flag = flag2;
					break;
				}
				default:
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unexpected acceptDamageType ");
					defaultInterpolatedStringHandler.AppendFormatted<Pulao.EAcceptDamageType>(this._acceptDamageType);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				}
				if (!true)
				{
				}
				result = flag;
			}
			return result;
		}

		// Token: 0x0600451A RID: 17690 RVA: 0x00271768 File Offset: 0x0026F968
		public Pulao()
		{
		}

		// Token: 0x0600451B RID: 17691 RVA: 0x00271779 File Offset: 0x0026F979
		public Pulao(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x0600451C RID: 17692 RVA: 0x0027178C File Offset: 0x0026F98C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(114, EDataModifyType.Custom, -1);
			base.CreateAffectedData(276, EDataModifyType.Custom, -1);
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.RegisterHandler_AddMindDamage(new Events.OnAddMindDamage(this.OnAddMindDamageValue));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600451D RID: 17693 RVA: 0x00271804 File Offset: 0x0026FA04
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			Events.UnRegisterHandler_AddMindDamage(new Events.OnAddMindDamage(this.OnAddMindDamageValue));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x0600451E RID: 17694 RVA: 0x00271864 File Offset: 0x0026FA64
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = defenderId != base.CharacterId || damageValue <= 0 || this._beAttacked;
			if (!flag)
			{
				this._beAttacked = true;
				this._acceptDamageType = (isInner ? Pulao.EAcceptDamageType.Inner : Pulao.EAcceptDamageType.Outer);
			}
		}

		// Token: 0x0600451F RID: 17695 RVA: 0x002718A4 File Offset: 0x0026FAA4
		private void OnAddMindDamageValue(DataContext context, int attackerId, int defenderId, int damageValue, int markCount, short combatSkillId)
		{
			bool flag = defenderId != base.CharacterId || damageValue <= 0 || this._beAttacked;
			if (!flag)
			{
				this._beAttacked = true;
				this._acceptDamageType = Pulao.EAcceptDamageType.Mind;
			}
		}

		// Token: 0x06004520 RID: 17696 RVA: 0x002718E0 File Offset: 0x0026FAE0
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = defender.GetId() == base.CharacterId && this._beAttacked;
			if (flag)
			{
				this._beAttacked = false;
			}
		}

		// Token: 0x06004521 RID: 17697 RVA: 0x00271910 File Offset: 0x0026FB10
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId && this._beAttacked;
			if (flag)
			{
				this._beAttacked = false;
			}
		}

		// Token: 0x06004522 RID: 17698 RVA: 0x0027193C File Offset: 0x0026FB3C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 276;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = this.IsDamageValid(Pulao.EAcceptDamageType.Mind);
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					base.ShowSpecialEffectTips(0);
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06004523 RID: 17699 RVA: 0x00271990 File Offset: 0x0026FB90
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 114;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				EDamageType damageType = (EDamageType)dataKey.CustomParam0;
				bool flag2 = damageType != EDamageType.Direct;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					bool flag3 = this.IsDamageValid((dataKey.CustomParam1 == 1) ? Pulao.EAcceptDamageType.Inner : Pulao.EAcceptDamageType.Outer);
					if (flag3)
					{
						result = dataValue;
					}
					else
					{
						base.ShowSpecialEffectTips(0);
						result = 0L;
					}
				}
			}
			return result;
		}

		// Token: 0x0400146F RID: 5231
		private bool _beAttacked;

		// Token: 0x04001470 RID: 5232
		private Pulao.EAcceptDamageType _acceptDamageType = Pulao.EAcceptDamageType.None;

		// Token: 0x02000A72 RID: 2674
		private enum EAcceptDamageType
		{
			// Token: 0x04002AB2 RID: 10930
			None,
			// Token: 0x04002AB3 RID: 10931
			Outer,
			// Token: 0x04002AB4 RID: 10932
			Inner,
			// Token: 0x04002AB5 RID: 10933
			Mind
		}
	}
}
