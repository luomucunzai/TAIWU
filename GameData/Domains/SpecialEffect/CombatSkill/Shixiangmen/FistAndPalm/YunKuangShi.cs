using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.FistAndPalm
{
	// Token: 0x020003FD RID: 1021
	public class YunKuangShi : CombatSkillEffectBase
	{
		// Token: 0x0600389A RID: 14490 RVA: 0x0023B0D4 File Offset: 0x002392D4
		public YunKuangShi()
		{
		}

		// Token: 0x0600389B RID: 14491 RVA: 0x0023B0DE File Offset: 0x002392DE
		public YunKuangShi(CombatSkillKey skillKey) : base(skillKey, 6106, -1)
		{
		}

		// Token: 0x0600389C RID: 14492 RVA: 0x0023B0F0 File Offset: 0x002392F0
		public override void OnEnable(DataContext context)
		{
			this._affectingSkillId = -1;
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x0023B150 File Offset: 0x00239350
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x0023B1A8 File Offset: 0x002393A8
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || !this.IsSrcSkillPerformed || Config.CombatSkill.Instance[skillId].EquipType != 1;
			if (!flag)
			{
				this._affectingSkillId = skillId;
				this._movedDist = 0;
				this._addAttackRange = 0;
				DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, 30, base.IsDirect);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				bool flag2 = skillId == base.SkillTemplateId;
				if (flag2)
				{
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x0023B244 File Offset: 0x00239444
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = skillId == base.SkillTemplateId;
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						bool flag4 = base.PowerMatchAffectRequire((int)power, 0);
						if (flag4)
						{
							base.AppendAffectedData(context, base.CharacterId, 145, EDataModifyType.Add, skillId);
							base.AppendAffectedData(context, base.CharacterId, 146, EDataModifyType.Add, skillId);
							base.AppendAffectedData(context, base.CharacterId, 199, EDataModifyType.AddPercent, skillId);
							base.AddMaxEffectCount(true);
						}
						else
						{
							base.RemoveSelf(context);
						}
					}
				}
				else
				{
					bool flag5 = this._affectingSkillId >= 0;
					if (flag5)
					{
						this._affectingSkillId = -1;
						this._addAttackRange = 0;
						bool flag6 = skillId == base.SkillTemplateId;
						if (flag6)
						{
							base.RemoveSelf(context);
						}
						else
						{
							base.ReduceEffectCount(1);
						}
					}
				}
			}
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x0023B33C File Offset: 0x0023953C
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover.GetId() != base.CharacterId || this._affectingSkillId < 0 || !isMove;
			if (!flag)
			{
				this._movedDist += distance;
				short addAttackRange = Math.Abs(this._movedDist) / 2 * 2;
				bool flag2 = this._addAttackRange != addAttackRange;
				if (flag2)
				{
					this._addAttackRange = addAttackRange;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x0023B3E0 File Offset: 0x002395E0
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x0023B430 File Offset: 0x00239630
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
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
					result = (int)this._addAttackRange;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 199 && dataKey.CombatSkillId == base.SkillTemplateId && this._affectingSkillId == base.SkillTemplateId;
					if (flag3)
					{
						result = 40;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x04001092 RID: 4242
		private const sbyte MoveDistInPrepare = 30;

		// Token: 0x04001093 RID: 4243
		private const int AffectDistanceUnit = 2;

		// Token: 0x04001094 RID: 4244
		private const sbyte AttackRangeAddUnit = 2;

		// Token: 0x04001095 RID: 4245
		private const sbyte AddPower = 40;

		// Token: 0x04001096 RID: 4246
		private short _affectingSkillId;

		// Token: 0x04001097 RID: 4247
		private short _movedDist;

		// Token: 0x04001098 RID: 4248
		private short _addAttackRange;
	}
}
