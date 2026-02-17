using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Sword
{
	// Token: 0x02000539 RID: 1337
	public class CanHongJianShi : CombatSkillEffectBase
	{
		// Token: 0x06003FCA RID: 16330 RVA: 0x0025B856 File Offset: 0x00259A56
		public CanHongJianShi()
		{
		}

		// Token: 0x06003FCB RID: 16331 RVA: 0x0025B860 File Offset: 0x00259A60
		public CanHongJianShi(CombatSkillKey skillKey) : base(skillKey, 2305, -1)
		{
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x0025B874 File Offset: 0x00259A74
		public override void OnEnable(DataContext context)
		{
			this._lastTrickCount = 0;
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 0, base.MaxEffectCount, false);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.RegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
			}
			else
			{
				GameDataBridge.AddPostDataModificationHandler(new DataUid(8, 10, (ulong)((long)base.CharacterId), 28U), base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnTricksChanged));
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x0025B940 File Offset: 0x00259B40
		public override void OnDisable(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.UnRegisterHandler_GetTrick(new Events.OnGetTrick(this.OnGetTrick));
			}
			else
			{
				GameDataBridge.RemovePostDataModificationHandler(new DataUid(8, 10, (ulong)((long)base.CharacterId), 28U), base.DataHandlerKey);
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x0025B99C File Offset: 0x00259B9C
		private void OnGetTrick(DataContext context, int charId, bool isAlly, sbyte trickType, bool usable)
		{
			bool flag = base.CharacterId != charId || base.EffectCount >= (int)base.MaxEffectCount;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), 1, true, false);
				base.ShowSpecialEffectTips(0);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x0025BA14 File Offset: 0x00259C14
		private void OnTricksChanged(DataContext context, DataUid dataUid)
		{
			int trickCount = base.CombatChar.GetTricks().Tricks.Count;
			bool flag = trickCount < this._lastTrickCount && base.EffectCount < (int)base.MaxEffectCount;
			if (flag)
			{
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), (short)Math.Min(this._lastTrickCount - trickCount, (int)base.MaxEffectCount - base.EffectCount), true, false);
				base.ShowSpecialEffectTips(0);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
			this._lastTrickCount = trickCount;
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x0025BAC0 File Offset: 0x00259CC0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || power < 100;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillEffectToMinCount(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect));
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06003FD1 RID: 16337 RVA: 0x0025BB2C File Offset: 0x00259D2C
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
					result = 5 * base.EffectCount;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040012CB RID: 4811
		private const int AddPowerUnit = 5;

		// Token: 0x040012CC RID: 4812
		private int _lastTrickCount;
	}
}
