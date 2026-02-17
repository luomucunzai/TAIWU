using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x020005A8 RID: 1448
	public class TrickBuffFlaw : CombatSkillEffectBase
	{
		// Token: 0x0600430D RID: 17165 RVA: 0x00269BC5 File Offset: 0x00267DC5
		public TrickBuffFlaw()
		{
		}

		// Token: 0x0600430E RID: 17166 RVA: 0x00269BCF File Offset: 0x00267DCF
		public TrickBuffFlaw(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600430F RID: 17167 RVA: 0x00269BDC File Offset: 0x00267DDC
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			Events.RegisterHandler_AttackSkillAttackBegin(new Events.OnAttackSkillAttackBegin(this.OnAttackSkillAttackBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004310 RID: 17168 RVA: 0x00269C0E File Offset: 0x00267E0E
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackBegin(new Events.OnAttackSkillAttackBegin(this.OnAttackSkillAttackBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004311 RID: 17169 RVA: 0x00269C38 File Offset: 0x00267E38
		private void OnAttackSkillAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId, int index, bool hit)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId || !hit || index < 3 || !base.IsDirect || !base.CombatCharPowerMatchAffectRequire(0);
			if (!flag)
			{
				int trickCount = this.CalcTrickCount(attacker.GetTricks());
				int flawCount = trickCount / 3;
				bool flag2 = flawCount <= 0;
				if (!flag2)
				{
					DamageCompareData damageCompare = DomainManager.Combat.GetDamageCompareData();
					int hitValue = (int)Math.Clamp((long)damageCompare.HitValue[attacker.SkillFinalAttackHitIndex] * (long)((ulong)attacker.GetAttackSkillPower()) / 100L, 0L, 2147483647L);
					int avoidValue = damageCompare.AvoidValue[attacker.SkillFinalAttackHitIndex];
					int hitOdds = CFormula.FormulaCalcHitOdds(hitValue, avoidValue);
					int flawLevel = (int)(CFormula.CalcFlawOrAcupointLevel(hitOdds, true) + 1);
					DomainManager.Combat.AddFlaw(context, defender, (sbyte)flawLevel, this.SkillKey, attacker.SkillAttackBodyPart, flawCount, true);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06004312 RID: 17170 RVA: 0x00269D28 File Offset: 0x00267F28
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && !base.IsDirect;
				if (flag2)
				{
					sbyte[] weaponTricks = base.CombatChar.GetWeaponTricks();
					int trickCount = 0;
					foreach (sbyte trickType in weaponTricks)
					{
						bool flag3 = trickType == this.RequireTrickType;
						if (flag3)
						{
							trickCount++;
						}
					}
					bool affected = this.OnReverseAffect(context, trickCount);
					bool flag4 = affected;
					if (flag4)
					{
						base.ShowSpecialEffectTips(0);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06004313 RID: 17171 RVA: 0x00269DD4 File Offset: 0x00267FD4
		protected int CalcTrickCount(TrickCollection trickCollection)
		{
			IReadOnlyDictionary<int, sbyte> trickDict = trickCollection.Tricks;
			int trickCounter = 0;
			foreach (sbyte type in trickDict.Values)
			{
				bool flag = type == this.RequireTrickType;
				if (flag)
				{
					trickCounter++;
				}
			}
			return trickCounter;
		}

		// Token: 0x06004314 RID: 17172 RVA: 0x00269E40 File Offset: 0x00268040
		protected virtual bool OnReverseAffect(DataContext context, int trickCount)
		{
			return false;
		}

		// Token: 0x040013E1 RID: 5089
		private const sbyte DirectTrickUnit = 3;

		// Token: 0x040013E2 RID: 5090
		protected sbyte RequireTrickType;
	}
}
