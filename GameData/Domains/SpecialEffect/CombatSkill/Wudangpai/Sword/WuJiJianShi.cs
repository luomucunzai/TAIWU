using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Sword
{
	// Token: 0x020003C2 RID: 962
	public class WuJiJianShi : CombatSkillEffectBase
	{
		// Token: 0x0600374D RID: 14157 RVA: 0x00234CC4 File Offset: 0x00232EC4
		public WuJiJianShi()
		{
		}

		// Token: 0x0600374E RID: 14158 RVA: 0x00234CCE File Offset: 0x00232ECE
		public WuJiJianShi(CombatSkillKey skillKey) : base(skillKey, 4208, -1)
		{
		}

		// Token: 0x0600374F RID: 14159 RVA: 0x00234CE0 File Offset: 0x00232EE0
		public override void OnEnable(DataContext context)
		{
			this._affecting = true;
			this._attacking = false;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 208, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 74, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 85, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.RegisterHandler_OverflowTrickRemoved(new Events.OnOverflowTrickRemoved(this.OnOverflowTrickRemoved));
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this._tricksUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 28U);
				GameDataBridge.AddPostDataModificationHandler(this._tricksUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAffecting));
			}
			else
			{
				this._tricksUid = new DataUid(0, 0, ulong.MaxValue, uint.MaxValue);
				Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.UpdateEnemyChar));
				Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			}
		}

		// Token: 0x06003750 RID: 14160 RVA: 0x00234EA8 File Offset: 0x002330A8
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			Events.UnRegisterHandler_OverflowTrickRemoved(new Events.OnOverflowTrickRemoved(this.OnOverflowTrickRemoved));
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.UpdateEnemyChar));
				Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			}
			GameDataBridge.RemovePostDataModificationHandler(this._tricksUid, base.DataHandlerKey);
		}

		// Token: 0x06003751 RID: 14161 RVA: 0x00234F44 File Offset: 0x00233144
		private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			bool flag = isAlly == base.IsDirect;
			if (flag)
			{
				this.UpdateAffecting(context, default(DataUid));
			}
		}

		// Token: 0x06003752 RID: 14162 RVA: 0x00234F70 File Offset: 0x00233170
		private void OnOverflowTrickRemoved(DataContext context, int charId, bool isAlly, int removedCount)
		{
			bool flag = isAlly == base.IsDirect;
			if (flag)
			{
				this.UpdateAffecting(context, default(DataUid));
			}
		}

		// Token: 0x06003753 RID: 14163 RVA: 0x00234F9C File Offset: 0x0023319C
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._attacking = true;
				this.InvalidateCaches(context, false);
				bool affecting = this._affecting;
				if (affecting)
				{
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003754 RID: 14164 RVA: 0x00234FEC File Offset: 0x002331EC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				this._attacking = false;
				this.InvalidateCaches(context, false);
			}
		}

		// Token: 0x06003755 RID: 14165 RVA: 0x0023502C File Offset: 0x0023322C
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			bool flag = isAlly != base.CombatChar.IsAlly;
			if (flag)
			{
				this.UpdateEnemyChar(context);
			}
		}

		// Token: 0x06003756 RID: 14166 RVA: 0x00235058 File Offset: 0x00233258
		private void UpdateEnemyChar(DataContext context)
		{
			CombatCharacter currEnemy = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			bool flag = this._tricksUid.SubId0 != ulong.MaxValue;
			if (flag)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._tricksUid, base.DataHandlerKey);
			}
			this._tricksUid = new DataUid(8, 10, (ulong)((long)currEnemy.GetId()), 28U);
			GameDataBridge.AddPostDataModificationHandler(this._tricksUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.UpdateAffecting));
			this.UpdateAffecting(context, this._tricksUid);
		}

		// Token: 0x06003757 RID: 14167 RVA: 0x002350EC File Offset: 0x002332EC
		private void UpdateAffecting(DataContext context, DataUid dataUid)
		{
			CombatCharacter trickChar = base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			bool canAffect = !trickChar.AnyUsableTrick;
			bool flag = this._affecting != canAffect;
			if (flag)
			{
				this._affecting = canAffect;
				this.InvalidateCaches(context, true);
			}
		}

		// Token: 0x06003758 RID: 14168 RVA: 0x00235150 File Offset: 0x00233350
		private void InvalidateCaches(DataContext context, bool updateTricks = false)
		{
			if (updateTricks)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 208);
				DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
			}
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 74);
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 85);
		}

		// Token: 0x06003759 RID: 14169 RVA: 0x00235204 File Offset: 0x00233404
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || !this._affecting || !this._attacking;
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
					result = 40;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 145 || dataKey.FieldId == 146;
					if (flag3)
					{
						result = 10000;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x0600375A RID: 14170 RVA: 0x00235290 File Offset: 0x00233490
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || !this._affecting || !this._attacking;
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

		// Token: 0x0600375B RID: 14171 RVA: 0x002352F0 File Offset: 0x002334F0
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || !this._affecting || !this._attacking;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 74;
				if (flag2)
				{
					result = -1;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x0600375C RID: 14172 RVA: 0x00235350 File Offset: 0x00233550
		public override List<NeedTrick> GetModifiedValue(AffectedDataKey dataKey, List<NeedTrick> dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || !this._affecting;
			List<NeedTrick> result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 208;
				if (flag2)
				{
					dataValue.Clear();
				}
				result = dataValue;
			}
			return result;
		}

		// Token: 0x04001026 RID: 4134
		private const sbyte AddPower = 40;

		// Token: 0x04001027 RID: 4135
		private DataUid _tricksUid;

		// Token: 0x04001028 RID: 4136
		private bool _affecting;

		// Token: 0x04001029 RID: 4137
		private bool _attacking;
	}
}
