using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.DefenseAndAssist
{
	// Token: 0x020003A8 RID: 936
	public class MieGuZhou : DefenseSkillBase
	{
		// Token: 0x060036B5 RID: 14005 RVA: 0x00231AAC File Offset: 0x0022FCAC
		public MieGuZhou()
		{
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x00231AB6 File Offset: 0x0022FCB6
		public MieGuZhou(CombatSkillKey skillKey) : base(skillKey, 12703)
		{
		}

		// Token: 0x060036B7 RID: 14007 RVA: 0x00231AC8 File Offset: 0x0022FCC8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 180, -1, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x060036B8 RID: 14008 RVA: 0x00231B24 File Offset: 0x0022FD24
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060036B9 RID: 14009 RVA: 0x00231B44 File Offset: 0x0022FD44
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || !hit || attacker != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				CombatCharacter sourceChar = base.IsDirect ? base.CombatChar : base.CurrEnemyChar;
				CombatCharacter affectChar = base.IsDirect ? base.CurrEnemyChar : base.CombatChar;
				short removedWugTemplateId = DomainManager.Combat.RemoveRandomWug(context, sourceChar, EWugReplaceType.CombatOnly);
				bool flag2 = removedWugTemplateId < 0;
				if (!flag2)
				{
					sbyte wugType = Medicine.Instance[removedWugTemplateId].WugType;
					DomainManager.Combat.AddWug(context, affectChar, wugType, base.IsDirect, base.CharacterId);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x060036BA RID: 14010 RVA: 0x00231BF4 File Offset: 0x0022FDF4
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 180 && dataKey.CustomParam0 != base.CharacterId;
				result = (!flag2 && dataValue);
			}
			return result;
		}
	}
}
