using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.DefenseAndAssist
{
	// Token: 0x020003AC RID: 940
	public class XueOuPoShaFa : DefenseSkillBase
	{
		// Token: 0x060036D5 RID: 14037 RVA: 0x002326C0 File Offset: 0x002308C0
		public XueOuPoShaFa()
		{
		}

		// Token: 0x060036D6 RID: 14038 RVA: 0x002326D5 File Offset: 0x002308D5
		public XueOuPoShaFa(CombatSkillKey skillKey) : base(skillKey, 12704)
		{
		}

		// Token: 0x060036D7 RID: 14039 RVA: 0x002326F0 File Offset: 0x002308F0
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 114, -1, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x0023272E File Offset: 0x0023092E
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x00232744 File Offset: 0x00230944
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || !hit || attacker != base.CombatChar || base.CombatChar.GetAffectingDefendSkillId() != base.SkillTemplateId || !base.CanAffect;
			if (!flag)
			{
				CombatCharacter enemyChar = base.CurrEnemyChar;
				DomainManager.Combat.AddCombatState(context, enemyChar, 2, base.IsDirect ? 64 : 65, 50);
				base.ShowSpecialEffectTips(0);
				bool flag2 = !this._affectEnemyList.Contains(enemyChar.GetId());
				if (flag2)
				{
					this._affectEnemyList.Add(enemyChar.GetId());
					DomainManager.Combat.AddCombatState(context, enemyChar, 0, base.IsDirect ? 66 : 67, 100, false, true, base.CharacterId);
				}
			}
		}

		// Token: 0x060036DA RID: 14042 RVA: 0x00232808 File Offset: 0x00230A08
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			EDamageType damageType = (EDamageType)dataKey.CustomParam0;
			bool flag = dataKey.CharId != base.CharacterId || damageType != EDamageType.Direct || dataKey.CustomParam1 != (base.IsDirect ? 0 : 1) || this._affectEnemyList.Count == 0;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				sbyte bodyPart = (sbyte)dataKey.CustomParam2;
				int damageValue = (int)dataValue;
				DataContext context = DomainManager.Combat.Context;
				for (int i = 0; i < this._affectEnemyList.Count; i++)
				{
					CombatCharacter enemyChar = DomainManager.Combat.GetElement_CombatCharacterDict(this._affectEnemyList[i]);
					DomainManager.Combat.RemoveCombatState(context, enemyChar, 0, base.IsDirect ? 66 : 67);
					DomainManager.Combat.AddInjuryDamageValue(base.CombatChar, enemyChar, bodyPart, base.IsDirect ? damageValue : 0, base.IsDirect ? 0 : damageValue, -1, true);
				}
				this._affectEnemyList.Clear();
				base.ShowSpecialEffectTips(1);
				result = 0L;
			}
			return result;
		}

		// Token: 0x04000FFD RID: 4093
		private const sbyte StatePowerUnit = 50;

		// Token: 0x04000FFE RID: 4094
		private readonly List<int> _affectEnemyList = new List<int>();
	}
}
