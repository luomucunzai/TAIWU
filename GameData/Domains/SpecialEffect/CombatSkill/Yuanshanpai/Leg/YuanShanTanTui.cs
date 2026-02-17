using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Leg
{
	// Token: 0x02000202 RID: 514
	public class YuanShanTanTui : CombatSkillEffectBase
	{
		// Token: 0x06002EA0 RID: 11936 RVA: 0x00210068 File Offset: 0x0020E268
		public YuanShanTanTui()
		{
		}

		// Token: 0x06002EA1 RID: 11937 RVA: 0x00210072 File Offset: 0x0020E272
		public YuanShanTanTui(CombatSkillKey skillKey) : base(skillKey, 5100, -1)
		{
		}

		// Token: 0x06002EA2 RID: 11938 RVA: 0x00210084 File Offset: 0x0020E284
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 209, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002EA3 RID: 11939 RVA: 0x002100F9 File Offset: 0x0020E2F9
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002EA4 RID: 11940 RVA: 0x00210134 File Offset: 0x0020E334
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = attacker.GetId() != base.CharacterId || attacker.PursueAttackCount < 2;
			if (!flag)
			{
				ItemKey weaponKey = DomainManager.Combat.GetUsingWeaponKey(attacker);
				bool flag2 = ItemTemplateHelper.GetItemSubType(weaponKey.ItemType, weaponKey.TemplateId) == (base.IsDirect ? 9 : 8) && DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, true);
				if (flag2)
				{
					DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06002EA5 RID: 11941 RVA: 0x002101D0 File Offset: 0x0020E3D0
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.CombatChar.GetAutoCastingSkill();
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
			}
		}

		// Token: 0x06002EA6 RID: 11942 RVA: 0x0021022C File Offset: 0x0020E42C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.ShowSpecialEffectTips(1);
				base.IsDirect = !base.IsDirect;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 209);
			}
		}

		// Token: 0x06002EA7 RID: 11943 RVA: 0x00210288 File Offset: 0x0020E488
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 209;
				if (flag2)
				{
					result = (base.IsDirect ? 0 : 1);
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04000DE1 RID: 3553
		private const sbyte RequirePursueCount = 2;

		// Token: 0x04000DE2 RID: 3554
		private const sbyte PrepareProgressPercent = 50;
	}
}
