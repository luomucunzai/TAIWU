using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Leg
{
	// Token: 0x02000488 RID: 1160
	public class KeJinZhenYuXiaoBaShi : CombatSkillEffectBase
	{
		// Token: 0x06003BD3 RID: 15315 RVA: 0x0024A070 File Offset: 0x00248270
		public KeJinZhenYuXiaoBaShi()
		{
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x0024A07A File Offset: 0x0024827A
		public KeJinZhenYuXiaoBaShi(CombatSkillKey skillKey) : base(skillKey, 10303, -1)
		{
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x0024A08C File Offset: 0x0024828C
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 85, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x0024A0EC File Offset: 0x002482EC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x0024A114 File Offset: 0x00248314
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker != base.CombatChar || skillId != base.SkillTemplateId;
			if (!flag)
			{
				OuterAndInnerInts penetrations = base.CombatChar.GetCharacter().GetPenetrations();
				OuterAndInnerInts penetrationResists = base.CurrEnemyChar.GetCharacter().GetPenetrationResists();
				this._canAffect = (DomainManager.Combat.InAttackRange(base.CombatChar) && (base.IsDirect ? (penetrationResists.Outer > penetrations.Outer) : (penetrationResists.Inner > penetrations.Inner)));
			}
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x0024A1A4 File Offset: 0x002483A4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool canAffect = this._canAffect;
				if (canAffect)
				{
					DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, base.IsDirect ? 47 : 49);
					DomainManager.Combat.AddCombatState(context, base.CurrEnemyChar, 2, base.IsDirect ? 48 : 50);
					base.ShowSpecialEffectTips(0);
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x0024A238 File Offset: 0x00248438
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 85;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x04001188 RID: 4488
		private bool _canAffect;
	}
}
