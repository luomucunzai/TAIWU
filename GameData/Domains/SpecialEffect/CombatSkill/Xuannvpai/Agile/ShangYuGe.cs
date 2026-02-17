using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Agile
{
	// Token: 0x0200028D RID: 653
	public class ShangYuGe : AgileSkillBase
	{
		// Token: 0x06003129 RID: 12585 RVA: 0x00219EFB File Offset: 0x002180FB
		public ShangYuGe()
		{
		}

		// Token: 0x0600312A RID: 12586 RVA: 0x00219F05 File Offset: 0x00218105
		public ShangYuGe(CombatSkillKey skillKey) : base(skillKey, 8401)
		{
		}

		// Token: 0x0600312B RID: 12587 RVA: 0x00219F18 File Offset: 0x00218118
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			for (sbyte hitType = 0; hitType < 3; hitType += 1)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, (ushort)((base.IsDirect ? 56 : 94) + hitType), -1, -1, -1, -1), EDataModifyType.Add);
			}
			Events.RegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
		}

		// Token: 0x0600312C RID: 12588 RVA: 0x00219F9B File Offset: 0x0021819B
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackBegin(new Events.OnNormalAttackBegin(this.OnNormalAttackBegin));
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
		}

		// Token: 0x0600312D RID: 12589 RVA: 0x00219FCC File Offset: 0x002181CC
		private void OnNormalAttackBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex)
		{
			bool flag = base.CombatChar != (base.IsDirect ? attacker : defender) || pursueIndex > 0 || attacker.NormalAttackHitType == 3 || !DomainManager.Combat.InAttackRange(base.CombatChar) || !base.CanAffect;
			if (!flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600312E RID: 12590 RVA: 0x0021A028 File Offset: 0x00218228
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = base.CombatChar != (base.IsDirect ? attacker : defender) || CombatSkillTemplateHelper.IsMindHitSkill(skillId) || !DomainManager.Combat.InAttackRange(base.CombatChar) || !base.CanAffect;
			if (!flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600312F RID: 12591 RVA: 0x0021A080 File Offset: 0x00218280
		public unsafe override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					result = *(ref this.CharObj.GetHitValues().Items.FixedElementField + (IntPtr)3 * 4) * 30 / 100;
				}
				else
				{
					result = *(ref this.CharObj.GetAvoidValues().Items.FixedElementField + (IntPtr)3 * 4) * 30 / 100;
				}
			}
			return result;
		}

		// Token: 0x04000E94 RID: 3732
		private const sbyte AddMindPercent = 30;
	}
}
