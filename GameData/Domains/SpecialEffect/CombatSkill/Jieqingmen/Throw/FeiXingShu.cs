using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw
{
	// Token: 0x020004DB RID: 1243
	public class FeiXingShu : CombatSkillEffectBase
	{
		// Token: 0x06003DA0 RID: 15776 RVA: 0x00252928 File Offset: 0x00250B28
		public FeiXingShu()
		{
		}

		// Token: 0x06003DA1 RID: 15777 RVA: 0x00252932 File Offset: 0x00250B32
		public FeiXingShu(CombatSkillKey skillKey) : base(skillKey, 13303, -1)
		{
		}

		// Token: 0x06003DA2 RID: 15778 RVA: 0x00252943 File Offset: 0x00250B43
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(318, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_PrepareSkillEffectNotYetCreated(new Events.OnPrepareSkillEffectNotYetCreated(this.OnPrepareSkillEffectNotYetCreated));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x00252978 File Offset: 0x00250B78
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEffectNotYetCreated(new Events.OnPrepareSkillEffectNotYetCreated(this.OnPrepareSkillEffectNotYetCreated));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x002529A0 File Offset: 0x00250BA0
		private void OnPrepareSkillEffectNotYetCreated(DataContext context, CombatCharacter character, short skillId)
		{
			bool flag = character.GetId() == base.CharacterId && CombatSkillTemplateHelper.IsAttack(skillId);
			if (flag)
			{
				this.DoRearrangeTrick(context);
			}
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x002529D4 File Offset: 0x00250BD4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					base.AddMaxEffectCount(true);
				}
			}
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x00252A18 File Offset: 0x00250C18
		private void DoRearrangeTrick(DataContext context)
		{
			CombatCharacter affectChar = base.IsDirect ? base.CombatChar : base.EnemyChar;
			bool flag = affectChar.GetTrickCount(19) <= 0 || base.EffectCount <= 0;
			if (!flag)
			{
				TrickCollection tricks = affectChar.GetTricks();
				tricks.RearrangeTrick(19);
				affectChar.SetTricks(tricks, context);
				Events.RaiseRearrangeTrick(context, affectChar.GetId(), affectChar.IsAlly);
				base.ReduceEffectCount(1);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x00252A9C File Offset: 0x00250C9C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.IsNormalAttack || dataKey.FieldId != 318;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				byte shaTrickCount = (base.IsDirect ? base.CombatChar : base.EnemyChar).GetTrickCount(19);
				bool flag2 = shaTrickCount <= 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					base.ShowSpecialEffectTipsOnceInFrame(1);
					result = (int)(shaTrickCount * 10);
				}
			}
			return result;
		}

		// Token: 0x04001228 RID: 4648
		private const int ShaAddFlawFactor = 10;
	}
}
