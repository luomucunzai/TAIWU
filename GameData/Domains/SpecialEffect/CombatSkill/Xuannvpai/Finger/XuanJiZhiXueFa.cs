using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Finger
{
	// Token: 0x0200027D RID: 637
	public class XuanJiZhiXueFa : CombatSkillEffectBase
	{
		// Token: 0x060030D4 RID: 12500 RVA: 0x00218AAC File Offset: 0x00216CAC
		public XuanJiZhiXueFa()
		{
		}

		// Token: 0x060030D5 RID: 12501 RVA: 0x00218AC2 File Offset: 0x00216CC2
		public XuanJiZhiXueFa(CombatSkillKey skillKey) : base(skillKey, 8206, -1)
		{
		}

		// Token: 0x060030D6 RID: 12502 RVA: 0x00218ADF File Offset: 0x00216CDF
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060030D7 RID: 12503 RVA: 0x00218B06 File Offset: 0x00216D06
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060030D8 RID: 12504 RVA: 0x00218B30 File Offset: 0x00216D30
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				FlawOrAcupointCollection srcCollection = base.IsDirect ? base.CurrEnemyChar.GetFlawCollection() : base.CurrEnemyChar.GetAcupointCollection();
				for (sbyte part = 0; part < 7; part += 1)
				{
					this._maxLevel[(int)part] = -1;
					foreach (ValueTuple<sbyte, int, int> entry in srcCollection.BodyPartDict[part])
					{
						bool flag2 = entry.Item1 > this._maxLevel[(int)part];
						if (flag2)
						{
							this._maxLevel[(int)part] = entry.Item1;
						}
					}
				}
			}
		}

		// Token: 0x060030D9 RID: 12505 RVA: 0x00218C14 File Offset: 0x00216E14
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					bool affected = false;
					for (sbyte part = 0; part < 7; part += 1)
					{
						sbyte level = this._maxLevel[(int)part];
						bool flag3 = level >= 0;
						if (flag3)
						{
							bool isDirect = base.IsDirect;
							if (isDirect)
							{
								DomainManager.Combat.AddAcupoint(context, base.CurrEnemyChar, level, this.SkillKey, part, 1, true);
							}
							else
							{
								DomainManager.Combat.AddFlaw(context, base.CurrEnemyChar, level, this.SkillKey, part, 1, true);
							}
							affected = true;
						}
					}
					bool flag4 = affected;
					if (flag4)
					{
						base.ShowSpecialEffectTips(0);
						DomainManager.Combat.AddToCheckFallenSet(base.CurrEnemyChar.GetId());
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x04000E7E RID: 3710
		private sbyte[] _maxLevel = new sbyte[7];
	}
}
