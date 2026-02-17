using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Sword
{
	// Token: 0x020001F6 RID: 502
	public class WuShangPuTiJian : CombatSkillEffectBase
	{
		// Token: 0x06002E56 RID: 11862 RVA: 0x0020E6F4 File Offset: 0x0020C8F4
		private bool IsAffectChar(int charId)
		{
			return DomainManager.Combat.IsCurrentCombatCharacter(base.CombatChar) && (base.IsDirect ? (charId == base.CharacterId) : (DomainManager.Combat.GetElement_CombatCharacterDict(charId).IsAlly != base.CombatChar.IsAlly));
		}

		// Token: 0x06002E57 RID: 11863 RVA: 0x0020E749 File Offset: 0x0020C949
		public WuShangPuTiJian()
		{
		}

		// Token: 0x06002E58 RID: 11864 RVA: 0x0020E753 File Offset: 0x0020C953
		public WuShangPuTiJian(CombatSkillKey skillKey) : base(skillKey, 5206, -1)
		{
		}

		// Token: 0x06002E59 RID: 11865 RVA: 0x0020E764 File Offset: 0x0020C964
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(85, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(292, EDataModifyType.Add, -1);
			base.CreateAffectedAllEnemyData(292, EDataModifyType.Add, -1);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002E5A RID: 11866 RVA: 0x0020E7C2 File Offset: 0x0020C9C2
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002E5B RID: 11867 RVA: 0x0020E7EC File Offset: 0x0020C9EC
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = Config.CombatSkill.Instance[skillId].EquipType != 1 || !this.IsAffectChar(charId) || base.EffectCount <= 0;
			if (!flag)
			{
				this._affectingSkill = new CombatSkillKey(charId, skillId);
				base.ReduceEffectCount(1);
				base.ShowSpecialEffectTips(0);
				base.InvalidateCache(context, charId, 292);
			}
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x0020E858 File Offset: 0x0020CA58
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this._affectingSkill.IsMatch(charId, skillId);
			if (flag)
			{
				this._affectingSkill = CombatSkillKey.Invalid;
				base.InvalidateCache(context, charId, 292);
			}
			bool flag2 = this.SkillKey.IsMatch(charId, skillId) && base.PowerMatchAffectRequire((int)power, 0);
			if (flag2)
			{
				base.AddMaxEffectCount(true);
			}
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x0020E8BC File Offset: 0x0020CABC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this._affectingSkill || dataKey.FieldId != 292;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = (base.IsDirect ? 1 : -1);
			}
			return result;
		}

		// Token: 0x06002E5E RID: 11870 RVA: 0x0020E90C File Offset: 0x0020CB0C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 85;
			return flag && dataValue;
		}

		// Token: 0x04000DC6 RID: 3526
		private CombatSkillKey _affectingSkill;
	}
}
