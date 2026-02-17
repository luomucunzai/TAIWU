using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Shot
{
	// Token: 0x020005BC RID: 1468
	public class ShiErXianQiZhenGong : CombatSkillEffectBase
	{
		// Token: 0x06004393 RID: 17299 RVA: 0x0026BD29 File Offset: 0x00269F29
		public ShiErXianQiZhenGong()
		{
		}

		// Token: 0x06004394 RID: 17300 RVA: 0x0026BD33 File Offset: 0x00269F33
		public ShiErXianQiZhenGong(CombatSkillKey skillKey) : base(skillKey, 3208, -1)
		{
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0026BD44 File Offset: 0x00269F44
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(301, EDataModifyType.Add, -1);
			Events.RegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_ChangeTrickCountChanged(new Events.OnChangeTrickCountChanged(this.OnChangeTrickCountChanged));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x0026BDBC File Offset: 0x00269FBC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_ChangeTrickCountChanged(new Events.OnChangeTrickCountChanged(this.OnChangeTrickCountChanged));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
		}

		// Token: 0x06004397 RID: 17303 RVA: 0x0026BE24 File Offset: 0x0026A024
		private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightback, bool isMind)
		{
			bool flag = attacker.GetId() != base.CharacterId || !hit || !attacker.GetChangeTrickAttack() || this._affectingEffectCount <= 0 || pursueIndex > 0;
			if (!flag)
			{
				sbyte fiveElementsType = BodyPartType.TransferToFiveElementsType(attacker.NormalAttackBodyPart);
				bool flag2 = fiveElementsType < 0;
				if (!flag2)
				{
					defender.ChangeToProportion(context, fiveElementsType, 12 * this._affectingEffectCount);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x0026BE94 File Offset: 0x0026A094
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = !this.SkillKey.IsMatch(attacker.GetId(), skillId);
			if (!flag)
			{
				int costedChangeTrickCount = Math.Min((int)base.CombatChar.GetChangeTrickCount(), 12);
				int unit = costedChangeTrickCount / 4;
				costedChangeTrickCount = unit * 4;
				bool flag2 = costedChangeTrickCount <= 0;
				if (!flag2)
				{
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.AddAcupoint(context, defender, 2, this.SkillKey, base.CombatChar.SkillAttackBodyPart, unit, true);
					}
					else
					{
						DomainManager.Combat.AddFlaw(context, defender, 2, this.SkillKey, base.CombatChar.SkillAttackBodyPart, unit, true);
					}
					DomainManager.Combat.ChangeChangeTrickCount(context, base.CombatChar, -costedChangeTrickCount, false);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x0026BF58 File Offset: 0x0026A158
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				int newEffectCount = (int)base.MaxEffectCount - base.EffectCount;
				bool flag2 = newEffectCount == 0;
				if (!flag2)
				{
					base.AddMaxEffectCount(true);
					DomainManager.Combat.ChangeChangeTrickCount(context, base.CombatChar, newEffectCount, false);
				}
			}
		}

		// Token: 0x0600439A RID: 17306 RVA: 0x0026BFC0 File Offset: 0x0026A1C0
		private void OnChangeTrickCountChanged(DataContext context, CombatCharacter character, int addValue, bool bySelectChangeTrick)
		{
			bool flag = character != base.CombatChar || addValue >= 0 || base.EffectCount <= 0;
			if (!flag)
			{
				base.ReduceEffectCount(Math.Abs(addValue));
				bool flag2 = !bySelectChangeTrick;
				if (!flag2)
				{
					this._affectingEffectCount += Math.Abs(addValue);
				}
			}
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x0026C01C File Offset: 0x0026A21C
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = attacker.GetId() == base.CharacterId;
			if (flag)
			{
				this._affectingEffectCount = 0;
			}
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x0026C044 File Offset: 0x0026A244
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 301;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = base.EffectCount;
			}
			return result;
		}

		// Token: 0x0400140D RID: 5133
		private const int CostMaxChangeTrickCount = 12;

		// Token: 0x0400140E RID: 5134
		private const int CostChangeTrickCountUnit = 4;

		// Token: 0x0400140F RID: 5135
		private const int FlawOrAcupointLevel = 2;

		// Token: 0x04001410 RID: 5136
		private const int TransferFiveElementsValue = 12;

		// Token: 0x04001411 RID: 5137
		private int _affectingEffectCount;
	}
}
