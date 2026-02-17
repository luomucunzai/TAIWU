using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Polearm
{
	// Token: 0x02000419 RID: 1049
	public class WuHuQunYangGun : CombatSkillEffectBase
	{
		// Token: 0x0600393F RID: 14655 RVA: 0x0023DDFA File Offset: 0x0023BFFA
		public WuHuQunYangGun()
		{
		}

		// Token: 0x06003940 RID: 14656 RVA: 0x0023DE04 File Offset: 0x0023C004
		public WuHuQunYangGun(CombatSkillKey skillKey) : base(skillKey, 1304, -1)
		{
		}

		// Token: 0x06003941 RID: 14657 RVA: 0x0023DE18 File Offset: 0x0023C018
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 223, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, base.IsDirect ? 206 : 205, base.SkillTemplateId, -1, -1, -1), EDataModifyType.TotalPercent);
			this._defendSkillUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 63U);
			GameDataBridge.AddPostDataModificationHandler(this._defendSkillUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefendSkillChanged));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06003942 RID: 14658 RVA: 0x0023DED3 File Offset: 0x0023C0D3
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._defendSkillUid, base.DataHandlerKey);
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x06003943 RID: 14659 RVA: 0x0023DEFC File Offset: 0x0023C0FC
		private void OnDefendSkillChanged(DataContext context, DataUid dataUid)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, base.IsDirect ? 206 : 205);
			bool flag = base.CombatChar.GetAffectingDefendSkillId() >= 0;
			if (flag)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003944 RID: 14660 RVA: 0x0023DF50 File Offset: 0x0023C150
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || base.CombatChar.GetAffectingDefendSkillId() < 0;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
			}
		}

		// Token: 0x06003945 RID: 14661 RVA: 0x0023DFAC File Offset: 0x0023C1AC
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
				bool flag2 = dataKey.FieldId == 206 || dataKey.FieldId == 205;
				if (flag2)
				{
					result = ((base.CombatChar.GetAffectingDefendSkillId() >= 0) ? -100 : 0);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06003946 RID: 14662 RVA: 0x0023E020 File Offset: 0x0023C220
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
				bool flag2 = dataKey.FieldId == 223;
				result = (flag2 || dataValue);
			}
			return result;
		}

		// Token: 0x040010BF RID: 4287
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x040010C0 RID: 4288
		private DataUid _defendSkillUid;
	}
}
