using System;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.FistAndPalm
{
	// Token: 0x0200048D RID: 1165
	public class DanShaShenZhang : CombatSkillEffectBase
	{
		// Token: 0x06003BFF RID: 15359 RVA: 0x0024B735 File Offset: 0x00249935
		public DanShaShenZhang()
		{
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x0024B73F File Offset: 0x0024993F
		public DanShaShenZhang(CombatSkillKey skillKey) : base(skillKey, 10104, -1)
		{
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x0024B750 File Offset: 0x00249950
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x0024B7A8 File Offset: 0x002499A8
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x0024B800 File Offset: 0x00249A00
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this.IsSrcSkillPerformed || attacker.GetId() != base.CharacterId || !hit;
			if (!flag)
			{
				this.AddPoison(context, attacker, defender);
				base.ShowSpecialEffectTips(1);
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x0024B84C File Offset: 0x00249A4C
		private unsafe void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				CombatCharacter poisonChar = base.IsDirect ? base.CurrEnemyChar : base.CombatChar;
				PoisonInts poisons = *poisonChar.GetPoison();
				PoisonInts oldPoisons = *poisonChar.GetOldPoison();
				this._poisonValue = 0;
				for (sbyte type = 0; type < 6; type += 1)
				{
					int poisonValue = *(ref poisons.Items.FixedElementField + (IntPtr)type * 4) - *(ref oldPoisons.Items.FixedElementField + (IntPtr)type * 4);
					bool flag2 = poisonValue > this._poisonValue;
					if (flag2)
					{
						this._poisonType = type;
						this._poisonLevel = PoisonsAndLevels.CalcPoisonedLevel(*(ref poisons.Items.FixedElementField + (IntPtr)type * 4));
						this._poisonValue = poisonValue;
					}
				}
			}
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x0024B93C File Offset: 0x00249B3C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = skillId == base.SkillTemplateId;
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						bool flag4 = base.PowerMatchAffectRequire((int)power, 0) && this._poisonValue > 0;
						if (flag4)
						{
							CombatCharacter poisonChar = base.IsDirect ? base.CurrEnemyChar : base.CombatChar;
							this._poisonValue = DomainManager.Combat.ReducePoison(context, poisonChar, this._poisonType, this._poisonValue, true, false);
							bool flag5 = this._poisonValue <= 0;
							if (flag5)
							{
								base.RemoveSelf(context);
							}
							else
							{
								int markCount = (int)PoisonsAndLevels.CalcPoisonedLevel(this._poisonValue);
								bool flag6 = markCount > 0;
								if (flag6)
								{
									bool isInner = this._poisonType == 1 || this._poisonType == 2 || this._poisonType == 5;
									DomainManager.Combat.AddRandomInjury(context, poisonChar, isInner, markCount, 1, false, -1);
								}
								base.ShowSpecialEffectTips(0);
								base.AddMaxEffectCount(true);
							}
						}
						else
						{
							base.RemoveSelf(context);
						}
					}
				}
				else
				{
					bool flag7 = Config.CombatSkill.Instance[skillId].EquipType == 1 && power > 0;
					if (flag7)
					{
						this.AddPoison(context, base.CombatChar, base.CurrEnemyChar);
						base.ShowSpecialEffectTips(1);
						base.ReduceEffectCount(1);
						bool flag8 = skillId == base.SkillTemplateId && base.PowerMatchAffectRequire((int)power, 0);
						if (flag8)
						{
							base.RemoveSelf(context);
						}
					}
				}
			}
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x0024BAE0 File Offset: 0x00249CE0
		private void AddPoison(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			DomainManager.Combat.AddPoison(context, attacker, defender, this._poisonType, this._poisonLevel, this._poisonValue * 100 / 100, base.SkillTemplateId, false, true, default(ItemKey), false, true, false);
		}

		// Token: 0x06003C07 RID: 15367 RVA: 0x0024BB28 File Offset: 0x00249D28
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x040011A3 RID: 4515
		private const sbyte AddPoisonPercent = 100;

		// Token: 0x040011A4 RID: 4516
		private sbyte _poisonType;

		// Token: 0x040011A5 RID: 4517
		private sbyte _poisonLevel;

		// Token: 0x040011A6 RID: 4518
		private int _poisonValue;
	}
}
