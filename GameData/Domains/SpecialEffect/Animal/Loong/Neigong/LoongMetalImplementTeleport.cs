using System;
using System.Runtime.CompilerServices;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x020005F8 RID: 1528
	public class LoongMetalImplementTeleport : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x060044DC RID: 17628 RVA: 0x00270A43 File Offset: 0x0026EC43
		// (set) Token: 0x060044DD RID: 17629 RVA: 0x00270A4B File Offset: 0x0026EC4B
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x060044DE RID: 17630 RVA: 0x00270A54 File Offset: 0x0026EC54
		public void OnEnable(DataContext context)
		{
			this.EffectBase.CreateAffectedData(157, EDataModifyType.Custom, -1);
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
			Events.RegisterHandler_NormalAttackPrepareEnd(new Events.OnNormalAttackPrepareEnd(this.OnNormalAttackPrepareEnd));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060044DF RID: 17631 RVA: 0x00270AD0 File Offset: 0x0026ECD0
		public void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.AddDirectDamageValue));
			Events.UnRegisterHandler_NormalAttackPrepareEnd(new Events.OnNormalAttackPrepareEnd(this.OnNormalAttackPrepareEnd));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060044E0 RID: 17632 RVA: 0x00270B38 File Offset: 0x0026ED38
		private void AddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = defenderId != this.EffectBase.CharacterId || damageValue == 0;
			if (!flag)
			{
				this._affecting = true;
			}
		}

		// Token: 0x060044E1 RID: 17633 RVA: 0x00270B69 File Offset: 0x0026ED69
		private void OnNormalAttackPrepareEnd(DataContext context, int charId, bool isAlly)
		{
			this._affecting = false;
		}

		// Token: 0x060044E2 RID: 17634 RVA: 0x00270B73 File Offset: 0x0026ED73
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			this.DoTeleport(context);
		}

		// Token: 0x060044E3 RID: 17635 RVA: 0x00270B7E File Offset: 0x0026ED7E
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			this._affecting = false;
		}

		// Token: 0x060044E4 RID: 17636 RVA: 0x00270B88 File Offset: 0x0026ED88
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			this.DoTeleport(context);
		}

		// Token: 0x060044E5 RID: 17637 RVA: 0x00270B94 File Offset: 0x0026ED94
		private void DoTeleport(DataContext context)
		{
			bool flag = !this._affecting;
			if (!flag)
			{
				this._affecting = false;
				bool flag2 = this.EffectBase.CombatChar.GetMobilityValue() < MoveSpecialConstants.MaxMobility * LoongMetalImplementTeleport.MinMobilityPercent;
				if (!flag2)
				{
					LoongMetalImplementTeleport.<>c__DisplayClass15_0 CS$<>8__locals1;
					CS$<>8__locals1.moveRange = DomainManager.Combat.GetDistanceRange();
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!this.EffectBase.CombatChar.IsAlly, false);
					CS$<>8__locals1.enemyAttackRange = enemyChar.GetAttackRange();
					int forwardSpace = Math.Abs((int)((short)CS$<>8__locals1.moveRange.Item1 - CS$<>8__locals1.enemyAttackRange.Outer));
					int backwardSpace = Math.Abs((int)((short)CS$<>8__locals1.moveRange.Item2 - CS$<>8__locals1.enemyAttackRange.Inner));
					bool flag3 = forwardSpace < backwardSpace;
					int targetDistance;
					if (flag3)
					{
						targetDistance = LoongMetalImplementTeleport.<DoTeleport>g__BackwardDistance|15_1(ref CS$<>8__locals1);
					}
					else
					{
						bool flag4 = forwardSpace > backwardSpace;
						if (flag4)
						{
							targetDistance = LoongMetalImplementTeleport.<DoTeleport>g__ForwardDistance|15_0(ref CS$<>8__locals1);
						}
						else
						{
							targetDistance = (context.Random.CheckPercentProb(50) ? LoongMetalImplementTeleport.<DoTeleport>g__ForwardDistance|15_0(ref CS$<>8__locals1) : LoongMetalImplementTeleport.<DoTeleport>g__BackwardDistance|15_1(ref CS$<>8__locals1));
						}
					}
					int addDistance = targetDistance - (int)DomainManager.Combat.GetCurrentDistance();
					this._teleporting = true;
					DomainManager.Combat.ChangeDistance(context, this.EffectBase.CombatChar, addDistance, false, false);
					this._teleporting = false;
					this.EffectBase.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060044E6 RID: 17638 RVA: 0x00270CF4 File Offset: 0x0026EEF4
		public bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != this.EffectBase.CharacterId || dataKey.FieldId != 157;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				result = (!this._teleporting && dataValue);
			}
			return result;
		}

		// Token: 0x060044E9 RID: 17641 RVA: 0x00270D56 File Offset: 0x0026EF56
		[CompilerGenerated]
		internal static int <DoTeleport>g__ForwardDistance|15_0(ref LoongMetalImplementTeleport.<>c__DisplayClass15_0 A_0)
		{
			return Math.Max((int)A_0.moveRange.Item2, (int)(A_0.enemyAttackRange.Outer - 10));
		}

		// Token: 0x060044EA RID: 17642 RVA: 0x00270D76 File Offset: 0x0026EF76
		[CompilerGenerated]
		internal static int <DoTeleport>g__BackwardDistance|15_1(ref LoongMetalImplementTeleport.<>c__DisplayClass15_0 A_0)
		{
			return Math.Min((int)A_0.moveRange.Item2, (int)(A_0.enemyAttackRange.Inner + 10));
		}

		// Token: 0x0400145C RID: 5212
		private const int MoveRangeDelta = 10;

		// Token: 0x0400145D RID: 5213
		private static readonly CValuePercent MinMobilityPercent = 50;

		// Token: 0x0400145E RID: 5214
		private bool _affecting;

		// Token: 0x0400145F RID: 5215
		private bool _teleporting;
	}
}
