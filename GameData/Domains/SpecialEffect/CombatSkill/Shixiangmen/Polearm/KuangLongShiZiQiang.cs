using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm
{
	// Token: 0x020003EB RID: 1003
	public class KuangLongShiZiQiang : CombatSkillEffectBase
	{
		// Token: 0x06003838 RID: 14392 RVA: 0x0023971A File Offset: 0x0023791A
		public KuangLongShiZiQiang()
		{
		}

		// Token: 0x06003839 RID: 14393 RVA: 0x00239724 File Offset: 0x00237924
		public KuangLongShiZiQiang(CombatSkillKey skillKey) : base(skillKey, 6306, -1)
		{
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x00239738 File Offset: 0x00237938
		public override void OnEnable(DataContext context)
		{
			this._reduceInjuryAffected = false;
			this._moveSkillUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 62U);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 114, -1, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x002397C4 File Offset: 0x002379C4
		public override void OnDisable(DataContext context)
		{
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar);
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			bool isSrcSkillPerformed = this.IsSrcSkillPerformed;
			if (isSrcSkillPerformed)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._moveSkillUid, base.DataHandlerKey);
			}
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x00239838 File Offset: 0x00237A38
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						base.AddMaxEffectCount(true);
						this.OnMoveSkillChanged(context, this._moveSkillUid);
						GameDataBridge.AddPostDataModificationHandler(this._moveSkillUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnMoveSkillChanged));
						bool reduceInjuryAffected = this._reduceInjuryAffected;
						if (reduceInjuryAffected)
						{
							base.ShowSpecialEffectTips(0);
						}
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
				else
				{
					bool flag4 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag4)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x002398FC File Offset: 0x00237AFC
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover.GetId() != base.CharacterId || !this._jumpMoveAffecting || !isMove || isForced;
			if (!flag)
			{
				bool flag2 = base.IsDirect ? (distance < 0) : (distance > 0);
				if (flag2)
				{
					base.ReduceEffectCount(1);
				}
			}
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x00239950 File Offset: 0x00237B50
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x002399A0 File Offset: 0x00237BA0
		private void OnMoveSkillChanged(DataContext context, DataUid dataUid)
		{
			this._jumpMoveAffecting = (base.CombatChar.GetAffectingMoveSkillId() < 0);
			bool jumpMoveAffecting = this._jumpMoveAffecting;
			if (jumpMoveAffecting)
			{
				DomainManager.Combat.EnableJumpMove(base.CombatChar, base.SkillTemplateId, base.IsDirect);
			}
			else
			{
				DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
			}
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x00239A04 File Offset: 0x00237C04
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			EDamageType damageType = (EDamageType)dataKey.CustomParam0;
			bool flag = dataKey.CharId != base.CharacterId || this.IsSrcSkillPerformed || damageType != EDamageType.Bounce;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 114 && dataValue > 0L;
				if (flag2)
				{
					this._reduceInjuryAffected = true;
					bool isInner = dataKey.CustomParam1 == 1;
					sbyte bodyPart = (sbyte)dataKey.CustomParam2;
					int clampedDamageValue = (int)Math.Clamp(dataValue, 0L, 2147483647L);
					CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
					DomainManager.Combat.AddInjuryDamageValue(enemyChar, enemyChar, bodyPart, isInner ? 0 : clampedDamageValue, isInner ? clampedDamageValue : 0, base.SkillTemplateId, true);
					result = 0L;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04001072 RID: 4210
		private bool _reduceInjuryAffected;

		// Token: 0x04001073 RID: 4211
		private bool _jumpMoveAffecting;

		// Token: 0x04001074 RID: 4212
		private DataUid _moveSkillUid;
	}
}
