using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Leg
{
	// Token: 0x020001FE RID: 510
	public class HuWeiGong : CombatSkillEffectBase
	{
		// Token: 0x06002E7C RID: 11900 RVA: 0x0020EFF6 File Offset: 0x0020D1F6
		public HuWeiGong()
		{
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x0020F000 File Offset: 0x0020D200
		public HuWeiGong(CombatSkillKey skillKey) : base(skillKey, 5102, -1)
		{
		}

		// Token: 0x06002E7E RID: 11902 RVA: 0x0020F014 File Offset: 0x0020D214
		public override void OnEnable(DataContext context)
		{
			base.AddMaxEffectCount(false);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002E7F RID: 11903 RVA: 0x0020F0B7 File Offset: 0x0020D2B7
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002E80 RID: 11904 RVA: 0x0020F0F0 File Offset: 0x0020D2F0
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = defender.GetId() != base.CharacterId || base.EffectCount <= 0;
			if (!flag)
			{
				this.TryAutoCast(context);
			}
		}

		// Token: 0x06002E81 RID: 11905 RVA: 0x0020F12C File Offset: 0x0020D32C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.CombatChar.GetAutoCastingSkill();
			if (!flag)
			{
				base.ReduceEffectCount(1);
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
			}
		}

		// Token: 0x06002E82 RID: 11906 RVA: 0x0020F190 File Offset: 0x0020D390
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId == base.CharacterId && skillId == base.SkillTemplateId;
			if (flag)
			{
				this._addingRange = false;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && !base.CombatChar.GetAutoCastingSkill();
				if (flag2)
				{
					DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 1, true, false);
				}
			}
			else
			{
				bool flag3 = isAlly != base.CombatChar.IsAlly && base.EffectCount > 0 && Config.CombatSkill.Instance[skillId].EquipType == 1;
				if (flag3)
				{
					this.TryAutoCast(context);
				}
			}
		}

		// Token: 0x06002E83 RID: 11907 RVA: 0x0020F274 File Offset: 0x0020D474
		private void TryAutoCast(DataContext context)
		{
			OuterAndInnerShorts attackRange = base.CombatChar.GetAttackRange();
			short currDistance = DomainManager.Combat.GetCurrentDistance();
			bool flag = !(base.IsDirect ? (currDistance > attackRange.Inner) : (currDistance < attackRange.Outer));
			if (!flag)
			{
				this._addingRange = true;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
				bool flag2 = DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, false);
				if (flag2)
				{
					DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
					base.ShowSpecialEffectTips(0);
				}
				else
				{
					this._addingRange = false;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
				}
			}
		}

		// Token: 0x06002E84 RID: 11908 RVA: 0x0020F370 File Offset: 0x0020D570
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 145 || dataKey.FieldId == 146;
				if (flag2)
				{
					result = (this._addingRange ? 30 : 0);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04000DD3 RID: 3539
		private const sbyte AddAttackRange = 30;

		// Token: 0x04000DD4 RID: 3540
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x04000DD5 RID: 3541
		private bool _addingRange;
	}
}
