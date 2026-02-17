using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x020005A3 RID: 1443
	public class ReduceEnemyTrick : CombatSkillEffectBase
	{
		// Token: 0x060042DF RID: 17119 RVA: 0x00268933 File Offset: 0x00266B33
		protected ReduceEnemyTrick()
		{
		}

		// Token: 0x060042E0 RID: 17120 RVA: 0x0026893D File Offset: 0x00266B3D
		protected ReduceEnemyTrick(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060042E1 RID: 17121 RVA: 0x0026894A File Offset: 0x00266B4A
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060042E2 RID: 17122 RVA: 0x00268971 File Offset: 0x00266B71
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x060042E3 RID: 17123 RVA: 0x00268998 File Offset: 0x00266B98
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
						bool isDirect = base.IsDirect;
						if (isDirect)
						{
							CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
							bool flag4 = DomainManager.Combat.IsCurrentCombatCharacter(enemyChar) && enemyChar.GetTrickCount(this.AffectTrickType) > 0 && DomainManager.Combat.StealTrick(context, base.CombatChar, enemyChar, this.AffectTrickType, 1);
							if (flag4)
							{
								base.ShowSpecialEffectTips(0);
							}
							base.RemoveSelf(context);
						}
						else
						{
							this.IsSrcSkillPerformed = true;
							base.AppendAffectedAllEnemyData(context, 138, EDataModifyType.Custom, -1);
							base.AddMaxEffectCount(true);
						}
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag5 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag5)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x060042E4 RID: 17124 RVA: 0x00268AB0 File Offset: 0x00266CB0
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060042E5 RID: 17125 RVA: 0x00268B00 File Offset: 0x00266D00
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.FieldId == 138 && dataKey.CustomParam2 == 1;
			if (flag)
			{
				DataContext context = DomainManager.Combat.Context;
				bool flag2 = dataKey.CustomParam0 == (int)this.AffectTrickType;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
					base.ReduceEffectCount(1);
					return false;
				}
			}
			return dataValue;
		}

		// Token: 0x040013C9 RID: 5065
		protected sbyte AffectTrickType;
	}
}
