using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.FistAndPalm
{
	// Token: 0x020003F8 RID: 1016
	public class DaShenWeiZhang : CombatSkillEffectBase
	{
		// Token: 0x06003885 RID: 14469 RVA: 0x0023AB51 File Offset: 0x00238D51
		public DaShenWeiZhang()
		{
		}

		// Token: 0x06003886 RID: 14470 RVA: 0x0023AB5B File Offset: 0x00238D5B
		public DaShenWeiZhang(CombatSkillKey skillKey) : base(skillKey, 6107, -1)
		{
		}

		// Token: 0x06003887 RID: 14471 RVA: 0x0023AB6C File Offset: 0x00238D6C
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003888 RID: 14472 RVA: 0x0023AB93 File Offset: 0x00238D93
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003889 RID: 14473 RVA: 0x0023ABBC File Offset: 0x00238DBC
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !DomainManager.Combat.InAttackRange(base.CombatChar);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
				short enemySkillId = base.IsDirect ? enemyChar.GetAffectingDefendSkillId() : enemyChar.GetAffectingMoveSkillId();
				CombatSkill enemySkill;
				bool flag2 = DomainManager.CombatSkill.TryGetElement_CombatSkills(new CombatSkillKey(enemyChar.GetId(), enemySkillId), out enemySkill);
				if (flag2)
				{
					this._addPower = (int)(enemySkill.GetPower() * 20 / 100);
					base.AppendAffectedData(context, base.CharacterId, 199, EDataModifyType.AddPercent, base.SkillTemplateId);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x0600388A RID: 14474 RVA: 0x0023AC84 File Offset: 0x00238E84
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && (base.IsDirect ? enemyChar.GetAffectingDefendSkillId() : enemyChar.GetAffectingMoveSkillId()) >= 0;
				if (flag2)
				{
					short enemySkillId = base.IsDirect ? enemyChar.GetAffectingDefendSkillId() : enemyChar.GetAffectingMoveSkillId();
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						DomainManager.Combat.ClearAffectingDefenseSkill(context, enemyChar);
					}
					else
					{
						base.ClearAffectingAgileSkill(DomainManager.Combat.Context, enemyChar);
					}
					bool flag3 = enemySkillId >= 0;
					if (flag3)
					{
						DomainManager.Combat.SilenceSkill(context, enemyChar, enemySkillId, 3000, 100);
					}
					base.ShowSpecialEffectTips(1);
					base.ShowSpecialEffectTips(2);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600388B RID: 14475 RVA: 0x0023AD7C File Offset: 0x00238F7C
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
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400108B RID: 4235
		private const int AddPowerPercent = 20;

		// Token: 0x0400108C RID: 4236
		private const int SilenceFrame = 3000;

		// Token: 0x0400108D RID: 4237
		private int _addPower;
	}
}
