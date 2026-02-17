using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.RanChenZi
{
	// Token: 0x020002ED RID: 749
	public class SiWuJian : CombatSkillEffectBase
	{
		// Token: 0x06003355 RID: 13141 RVA: 0x002247C0 File Offset: 0x002229C0
		public SiWuJian()
		{
		}

		// Token: 0x06003356 RID: 13142 RVA: 0x002247CA File Offset: 0x002229CA
		public SiWuJian(CombatSkillKey skillKey) : base(skillKey, 17133, -1)
		{
		}

		// Token: 0x06003357 RID: 13143 RVA: 0x002247DC File Offset: 0x002229DC
		public override void OnEnable(DataContext context)
		{
			this._addProgress = 0;
			this._addPower = 0;
			base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			this._bossPhaseUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 100U);
			GameDataBridge.AddPostDataModificationHandler(this._bossPhaseUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnBossPhaseChanged));
			sbyte[] taskStatus = new sbyte[]
			{
				DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(2).JuniorXiangshuTaskStatus,
				DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(3).JuniorXiangshuTaskStatus
			};
			bool goodEnding = !taskStatus.Exist((sbyte status) => status != 6);
			bool badEnding = !taskStatus.Exist((sbyte status) => status != 5);
			bool flag = goodEnding || badEnding;
			if (flag)
			{
				SiWuJianAssist effect = new SiWuJianAssist(this.SkillKey, goodEnding);
				DomainManager.SpecialEffect.Add(context, effect);
				this._assistEffectId = effect.Id;
				base.ShowSpecialEffectTips(goodEnding, 2, 3);
			}
			else
			{
				this._assistEffectId = -1L;
			}
		}

		// Token: 0x06003358 RID: 13144 RVA: 0x0022492C File Offset: 0x00222B2C
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			GameDataBridge.RemovePostDataModificationHandler(this._bossPhaseUid, base.DataHandlerKey);
			bool flag = this._assistEffectId >= 0L;
			if (flag)
			{
				DomainManager.SpecialEffect.Remove(context, this._assistEffectId);
			}
		}

		// Token: 0x06003359 RID: 13145 RVA: 0x00224994 File Offset: 0x00222B94
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || this._addProgress <= 0;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * this._addProgress / 100);
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x0600335A RID: 13146 RVA: 0x002249F8 File Offset: 0x00222BF8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || interrupted;
			if (!flag)
			{
				this._addProgress += 25;
				this._addPower += 40;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600335B RID: 13147 RVA: 0x00224A68 File Offset: 0x00222C68
		private void OnBossPhaseChanged(DataContext context, DataUid dataUid)
		{
			bool flag = base.CombatChar.GetBossPhase() > 3;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600335C RID: 13148 RVA: 0x00224A90 File Offset: 0x00222C90
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

		// Token: 0x04000F29 RID: 3881
		private const sbyte AddProgressUnit = 25;

		// Token: 0x04000F2A RID: 3882
		private const sbyte AddPowerUnit = 40;

		// Token: 0x04000F2B RID: 3883
		private int _addProgress;

		// Token: 0x04000F2C RID: 3884
		private int _addPower;

		// Token: 0x04000F2D RID: 3885
		private DataUid _bossPhaseUid;

		// Token: 0x04000F2E RID: 3886
		private long _assistEffectId;
	}
}
