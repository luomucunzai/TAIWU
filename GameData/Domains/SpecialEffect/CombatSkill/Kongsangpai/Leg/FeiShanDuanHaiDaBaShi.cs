using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Leg
{
	// Token: 0x02000487 RID: 1159
	public class FeiShanDuanHaiDaBaShi : CombatSkillEffectBase
	{
		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06003BC6 RID: 15302 RVA: 0x00249A75 File Offset: 0x00247C75
		private CombatCharacter AffectingMoveChar
		{
			get
			{
				return DomainManager.Combat.GetElement_CombatCharacterDict(this._affectingMoveCharId);
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06003BC7 RID: 15303 RVA: 0x00249A87 File Offset: 0x00247C87
		private CombatCharacter AffectingCastChar
		{
			get
			{
				return DomainManager.Combat.GetElement_CombatCharacterDict(this._affectingCastCharId);
			}
		}

		// Token: 0x06003BC8 RID: 15304 RVA: 0x00249A99 File Offset: 0x00247C99
		public FeiShanDuanHaiDaBaShi()
		{
		}

		// Token: 0x06003BC9 RID: 15305 RVA: 0x00249AAB File Offset: 0x00247CAB
		public FeiShanDuanHaiDaBaShi(CombatSkillKey skillKey) : base(skillKey, 10306, -1)
		{
		}

		// Token: 0x06003BCA RID: 15306 RVA: 0x00249AC4 File Offset: 0x00247CC4
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			this._affectingMoveCharId = -1;
			this._affectingCastCharId = -1;
			this._selfAttributeUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), base.IsDirect ? 87U : 89U);
			GameDataBridge.AddPostDataModificationHandler(this._selfAttributeUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAffecting));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BCB RID: 15307 RVA: 0x00249B72 File Offset: 0x00247D72
		public override void OnDataAdded(DataContext context)
		{
			this.UpdateEnemyDataUid(context, true);
			this.UpdateAffecting(context, this._selfAttributeUid);
		}

		// Token: 0x06003BCC RID: 15308 RVA: 0x00249B8C File Offset: 0x00247D8C
		public override void OnDisable(DataContext context)
		{
			bool flag = this._affectingMoveCharId >= 0;
			if (flag)
			{
				this.AffectingMoveChar.SetMobilityLockEffectCount(this.AffectingMoveChar.GetMobilityLockEffectCount() - 1, context);
			}
			bool flag2 = this._affectingCastCharId >= 0;
			if (flag2)
			{
				this.AffectingCastChar.PreventCastSkillEffectCount--;
				DomainManager.Combat.UpdateSkillCanUse(context, this.AffectingCastChar);
			}
			GameDataBridge.RemovePostDataModificationHandler(this._selfAttributeUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._enemyAttributeUid, base.DataHandlerKey);
			GameDataBridge.RemovePostDataModificationHandler(this._enemyAgileSkillUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003BCD RID: 15309 RVA: 0x00249C58 File Offset: 0x00247E58
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly == base.CombatChar.IsAlly;
			if (!flag)
			{
				this.UpdateEnemyDataUid(context, false);
				this.UpdateAffecting(context, this._selfAttributeUid);
			}
		}

		// Token: 0x06003BCE RID: 15310 RVA: 0x00249C94 File Offset: 0x00247E94
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = power > 0;
				if (flag2)
				{
					DomainManager.Combat.AddCombatState(context, base.CurrEnemyChar, 2, base.IsDirect ? 51 : 52, (int)(this.StatePowerUnit * power / 10));
					base.ShowSpecialEffectTips(1);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x00249D0C File Offset: 0x00247F0C
		private void UpdateEnemyDataUid(DataContext context, bool init = false)
		{
			bool flag = !init;
			if (flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._enemyAttributeUid, base.DataHandlerKey);
				GameDataBridge.RemovePostDataModificationHandler(this._enemyAgileSkillUid, base.DataHandlerKey);
				base.ClearAffectedData(context);
			}
			CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			this._enemyAttributeUid = new DataUid(4, 0, (ulong)((long)currEnemy.GetId()), base.IsDirect ? 87U : 89U);
			this._enemyAgileSkillUid = new DataUid(8, 10, (ulong)((long)currEnemy.GetId()), 62U);
			base.AppendAffectedData(context, currEnemy.GetId(), 151, EDataModifyType.Custom, -1);
			GameDataBridge.AddPostDataModificationHandler(this._enemyAttributeUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAffecting));
			GameDataBridge.AddPostDataModificationHandler(this._enemyAgileSkillUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAffecting));
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x00249DF4 File Offset: 0x00247FF4
		private void UpdateAffecting(DataContext context, DataUid dataUid)
		{
			CombatCharacter enemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			Character enemyChar = enemy.GetCharacter();
			short selfValue = base.IsDirect ? this.CharObj.GetRecoveryOfFlaw() : this.CharObj.GetRecoveryOfBlockedAcupoint();
			short enemyValue = base.IsDirect ? enemyChar.GetRecoveryOfFlaw() : enemyChar.GetRecoveryOfBlockedAcupoint();
			bool canAffectCast = selfValue > enemyValue;
			bool canAffectMove = canAffectCast && DomainManager.SpecialEffect.ModifyData(enemy.GetId(), -1, 147, true, -1, -1, -1);
			bool flag = this._affectingMoveCharId >= 0 != canAffectMove;
			if (flag)
			{
				CombatCharacter affectChar = canAffectMove ? enemy : this.AffectingMoveChar;
				bool flag2 = canAffectMove;
				if (flag2)
				{
					affectChar.SetMobilityLockEffectCount(affectChar.GetMobilityLockEffectCount() + 1, context);
				}
				else
				{
					affectChar.SetMobilityLockEffectCount(affectChar.GetMobilityLockEffectCount() - 1, context);
				}
				this._affectingMoveCharId = (canAffectMove ? enemy.GetId() : -1);
			}
			bool flag3 = this._affectingCastCharId >= 0 != canAffectCast;
			if (flag3)
			{
				CombatCharacter affectChar2 = canAffectCast ? enemy : this.AffectingCastChar;
				bool flag4 = canAffectCast;
				if (flag4)
				{
					affectChar2.PreventCastSkillEffectCount++;
				}
				else
				{
					affectChar2.PreventCastSkillEffectCount--;
				}
				DomainManager.Combat.UpdateSkillCanUse(context, affectChar2);
				this._affectingCastCharId = (canAffectCast ? enemy.GetId() : -1);
			}
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x00249F64 File Offset: 0x00248164
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.FieldId == 151 && this._affectingMoveCharId >= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = dataValue;
			}
			return result;
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x00249F9C File Offset: 0x0024819C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || !base.CombatCharPowerMatchAffectRequire(0);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 69 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					int delta = (int)(base.IsDirect ? (base.CombatChar.GetCharacter().GetRecoveryOfFlaw() - base.CurrEnemyChar.GetCharacter().GetRecoveryOfFlaw()) : (base.CombatChar.GetCharacter().GetRecoveryOfBlockedAcupoint() - base.CurrEnemyChar.GetCharacter().GetRecoveryOfBlockedAcupoint()));
					bool flag3 = delta > 0;
					if (flag3)
					{
						base.ShowSpecialEffectTips(2);
						return delta / 8;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x04001181 RID: 4481
		private const int AddDamageValueBasePercentDivisor = 8;

		// Token: 0x04001182 RID: 4482
		private sbyte StatePowerUnit = 25;

		// Token: 0x04001183 RID: 4483
		private int _affectingMoveCharId;

		// Token: 0x04001184 RID: 4484
		private int _affectingCastCharId;

		// Token: 0x04001185 RID: 4485
		private DataUid _selfAttributeUid;

		// Token: 0x04001186 RID: 4486
		private DataUid _enemyAttributeUid;

		// Token: 0x04001187 RID: 4487
		private DataUid _enemyAgileSkillUid;
	}
}
