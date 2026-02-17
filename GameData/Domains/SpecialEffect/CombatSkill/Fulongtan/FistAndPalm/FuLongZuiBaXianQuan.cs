using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.FistAndPalm
{
	// Token: 0x0200051D RID: 1309
	public class FuLongZuiBaXianQuan : CombatSkillEffectBase
	{
		// Token: 0x06003F0E RID: 16142 RVA: 0x002582D3 File Offset: 0x002564D3
		public FuLongZuiBaXianQuan()
		{
		}

		// Token: 0x06003F0F RID: 16143 RVA: 0x002582DD File Offset: 0x002564DD
		public FuLongZuiBaXianQuan(CombatSkillKey skillKey) : base(skillKey, 14101, -1)
		{
		}

		// Token: 0x06003F10 RID: 16144 RVA: 0x002582F0 File Offset: 0x002564F0
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 60 : 90, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 61 : 91, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 62 : 92, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 63 : 93, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackAllBegin));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003F11 RID: 16145 RVA: 0x0025840C File Offset: 0x0025660C
		public override void OnDisable(DataContext context)
		{
			int[] enemyList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
			for (int i = 0; i < enemyList.Length; i++)
			{
				bool flag = enemyList[i] >= 0;
				if (flag)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetElement_CombatCharacterDict(enemyList[i]);
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						CombatCharacter combatCharacter = enemyChar;
						combatCharacter.ChangeAvoidTypeEffectCount -= 1;
					}
					else
					{
						CombatCharacter combatCharacter2 = enemyChar;
						combatCharacter2.ChangeHitTypeEffectCount -= 1;
					}
				}
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackAllBegin));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003F12 RID: 16146 RVA: 0x002584EC File Offset: 0x002566EC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						int[] enemyList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
						for (int i = 0; i < enemyList.Length; i++)
						{
							bool flag4 = enemyList[i] >= 0;
							if (flag4)
							{
								CombatCharacter enemyChar = DomainManager.Combat.GetElement_CombatCharacterDict(enemyList[i]);
								bool isDirect = base.IsDirect;
								if (isDirect)
								{
									CombatCharacter combatCharacter = enemyChar;
									combatCharacter.ChangeAvoidTypeEffectCount += 1;
								}
								else
								{
									CombatCharacter combatCharacter2 = enemyChar;
									combatCharacter2.ChangeHitTypeEffectCount += 1;
								}
							}
						}
						this.IsSrcSkillPerformed = true;
						base.AddMaxEffectCount(true);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x06003F13 RID: 16147 RVA: 0x002585EC File Offset: 0x002567EC
		private void OnNormalAttackAllBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = base.CombatChar != (base.IsDirect ? attacker : defender) || !this.IsSrcSkillPerformed || pursueIndex > 0;
			if (!flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003F14 RID: 16148 RVA: 0x0025862C File Offset: 0x0025682C
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = base.CombatChar != (base.IsDirect ? attacker : defender);
			if (!flag)
			{
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003F15 RID: 16149 RVA: 0x00258660 File Offset: 0x00256860
		private void OnCompareDataCalcFinished(CombatContext context, DamageCompareData compareData)
		{
			CombatCharacter attacker = context.Attacker;
			CombatCharacter defender = context.Defender;
			bool flag = !this.IsSrcSkillPerformed || context.SkillTemplateId >= 0 || base.CombatChar != (base.IsDirect ? attacker : defender);
			if (!flag)
			{
				sbyte bodyPart = context.BodyPart;
				Weapon weapon = context.Weapon;
				sbyte hitType = compareData.HitType[0];
				int minValue = (base.IsDirect ? compareData.AvoidValue : compareData.HitValue)[0];
				for (sbyte type = 0; type < 4; type += 1)
				{
					bool flag2 = type == hitType;
					if (!flag2)
					{
						minValue = Math.Min(minValue, base.IsDirect ? defender.GetAvoidValue(type, bodyPart, -1, false) : attacker.GetHitValue(weapon, type, bodyPart, 0, -1));
					}
				}
				(base.IsDirect ? compareData.AvoidValue : compareData.HitValue)[0] = minValue;
			}
		}

		// Token: 0x06003F16 RID: 16150 RVA: 0x00258754 File Offset: 0x00256954
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003F17 RID: 16151 RVA: 0x002587A4 File Offset: 0x002569A4
		public unsafe override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CustomParam0 >= 0 || !(*this.CharObj.GetEatingItems()).ContainsWine();
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = -20;
			}
			return result;
		}

		// Token: 0x04001297 RID: 4759
		private const sbyte HitAvoidChangePercent = 20;
	}
}
