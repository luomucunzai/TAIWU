using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Blade
{
	// Token: 0x020004D3 RID: 1235
	public class MoHeJiaLuoDao : CombatSkillEffectBase
	{
		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06003D77 RID: 15735 RVA: 0x00251CFB File Offset: 0x0024FEFB
		private int ChangeNeiliAllocationDirection
		{
			get
			{
				return base.IsDirect ? 1 : -1;
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06003D78 RID: 15736 RVA: 0x00251D09 File Offset: 0x0024FF09
		private bool CanBuffEnemy
		{
			get
			{
				return base.EffectCount >= 18;
			}
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x00251D18 File Offset: 0x0024FF18
		public MoHeJiaLuoDao()
		{
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x00251D2E File Offset: 0x0024FF2E
		public MoHeJiaLuoDao(CombatSkillKey skillKey) : base(skillKey, 11208, -1)
		{
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x00251D4C File Offset: 0x0024FF4C
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(322, EDataModifyType.Custom, -1);
			base.CreateAffectedData(114, EDataModifyType.Custom, -1);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003D7C RID: 15740 RVA: 0x00251DA9 File Offset: 0x0024FFA9
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003D7D RID: 15741 RVA: 0x00251DE4 File Offset: 0x0024FFE4
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._delaying = true;
				this._delayFatalDamages = 0;
				for (int i = 0; i < this._delayDamages.Length; i++)
				{
					this._delayDamages[i] = default(OuterAndInnerInts);
				}
				DomainManager.Combat.AddGoneMadInjury(context, base.CombatChar, skillId, 100);
			}
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x00251E60 File Offset: 0x00250060
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId && (!this.CanBuffEnemy || defender.GetId() != base.CharacterId);
			if (!flag)
			{
				int canAddCount = Math.Min(6, base.EffectCount);
				bool flag2 = canAddCount <= 0;
				if (!flag2)
				{
					base.ShowSpecialEffectTips(attacker.GetId() == base.CharacterId, 2, 3);
					base.ReduceEffectCount(canAddCount);
					int delta = canAddCount * 5 * this.ChangeNeiliAllocationDirection;
					CombatCharacter target = base.IsDirect ? attacker : defender;
					for (byte i = 0; i < 4; i += 1)
					{
						target.ChangeNeiliAllocation(context, i, delta, true, true);
					}
				}
			}
		}

		// Token: 0x06003D7F RID: 15743 RVA: 0x00251F18 File Offset: 0x00250118
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._delaying = false;
				int effectCount = base.EffectCount;
				bool isFullPower = base.PowerMatchAffectRequire((int)power, 0);
				bool flag2 = isFullPower;
				if (flag2)
				{
					base.ShowSpecialEffectTips(1);
				}
				bool flag3 = isFullPower;
				if (flag3)
				{
					effectCount += this._delayFatalDamages / base.CombatChar.GetDamageStepCollection().FatalDamageStep;
					effectCount += this._delayDisorderOfQi / 1000;
				}
				else
				{
					DomainManager.Combat.AddFatalDamageValue(context, base.CombatChar, this._delayFatalDamages, -1, -1, -1, EDamageType.None);
					base.CombatChar.GetCharacter().ChangeDisorderOfQi(context, this._delayDisorderOfQi);
				}
				for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
				{
					OuterAndInnerInts damage = this._delayDamages[(int)bodyPart];
					bool flag4 = isFullPower;
					if (flag4)
					{
						effectCount += this.CalcEffectCount(damage.Outer, false, bodyPart);
						effectCount += this.CalcEffectCount(damage.Inner, true, bodyPart);
					}
					else
					{
						DomainManager.Combat.AddInjuryDamageValue(base.CombatChar, base.CombatChar, bodyPart, damage.Outer, damage.Inner, -1, false);
					}
				}
				bool flag5 = isFullPower && effectCount > 0;
				if (flag5)
				{
					DomainManager.Combat.AddSkillEffect(context, base.CombatChar, base.EffectKey, (short)effectCount, (short)effectCount, true);
				}
				DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
			}
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x0025209C File Offset: 0x0025029C
		private int CalcEffectCount(int damage, bool inner, sbyte bodyPart)
		{
			DamageStepCollection damageStepCollection = base.CombatChar.GetDamageStepCollection();
			Injuries injuries = base.CombatChar.GetInjuries();
			int[] damageSteps = inner ? damageStepCollection.InnerDamageSteps : damageStepCollection.OuterDamageSteps;
			int injuryCount = damage / damageSteps[(int)bodyPart];
			bool flag = (int)injuries.Get(bodyPart, inner) + injuryCount <= 6;
			int result;
			if (flag)
			{
				result = injuryCount;
			}
			else
			{
				injuryCount = (int)(6 - injuries.Get(bodyPart, inner));
				damage -= injuryCount * damageSteps[(int)bodyPart];
				result = injuryCount + damage / damageStepCollection.FatalDamageStep;
			}
			return result;
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x0025211C File Offset: 0x0025031C
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._delaying || dataKey.FieldId != 114;
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
					sbyte part = (sbyte)dataKey.CustomParam2;
					bool inner = dataKey.CustomParam1 == 1;
					bool flag3 = inner;
					if (flag3)
					{
						OuterAndInnerInts[] delayDamages = this._delayDamages;
						sbyte b = part;
						delayDamages[(int)b].Inner = delayDamages[(int)b].Inner + (int)dataValue;
					}
					else
					{
						OuterAndInnerInts[] delayDamages2 = this._delayDamages;
						sbyte b2 = part;
						delayDamages2[(int)b2].Outer = delayDamages2[(int)b2].Outer + (int)dataValue;
					}
					base.ShowSpecialEffectTipsOnceInFrame(0);
					result = 0L;
				}
			}
			return result;
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x002521CC File Offset: 0x002503CC
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._delaying || dataKey.FieldId != 322;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool inner = dataKey.CustomParam0 == 1;
				sbyte part = (sbyte)dataKey.CustomParam1;
				bool addingDisorderOfQi = dataKey.CustomParam2 == 1;
				bool flag2 = addingDisorderOfQi;
				if (flag2)
				{
					this._delayDisorderOfQi += dataValue;
				}
				else
				{
					bool flag3 = part < 0;
					if (flag3)
					{
						this._delayFatalDamages += dataValue;
					}
					else
					{
						bool flag4 = inner;
						if (flag4)
						{
							OuterAndInnerInts[] delayDamages = this._delayDamages;
							sbyte b = part;
							delayDamages[(int)b].Inner = delayDamages[(int)b].Inner + dataValue;
						}
						else
						{
							OuterAndInnerInts[] delayDamages2 = this._delayDamages;
							sbyte b2 = part;
							delayDamages2[(int)b2].Outer = delayDamages2[(int)b2].Outer + dataValue;
						}
					}
				}
				base.ShowSpecialEffectTipsOnceInFrame(0);
				result = 0;
			}
			return result;
		}

		// Token: 0x0400121B RID: 4635
		private const short AddGoneMadInjury = 100;

		// Token: 0x0400121C RID: 4636
		private const int DisorderOfQiPerEffectCount = 1000;

		// Token: 0x0400121D RID: 4637
		private const sbyte BuffEnemyCount = 18;

		// Token: 0x0400121E RID: 4638
		private const sbyte MaxCostOnce = 6;

		// Token: 0x0400121F RID: 4639
		private const int ChangeNeiliAllocationValue = 5;

		// Token: 0x04001220 RID: 4640
		private int _delayDisorderOfQi;

		// Token: 0x04001221 RID: 4641
		private int _delayFatalDamages;

		// Token: 0x04001222 RID: 4642
		private readonly OuterAndInnerInts[] _delayDamages = new OuterAndInnerInts[7];

		// Token: 0x04001223 RID: 4643
		private bool _delaying;
	}
}
