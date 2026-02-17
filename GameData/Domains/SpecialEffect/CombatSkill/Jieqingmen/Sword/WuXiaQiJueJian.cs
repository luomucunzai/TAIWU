using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Sword
{
	// Token: 0x020004E7 RID: 1255
	public class WuXiaQiJueJian : CombatSkillEffectBase
	{
		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06003E00 RID: 15872 RVA: 0x00254548 File Offset: 0x00252748
		private CombatCharacter TrickChar
		{
			get
			{
				return base.IsDirect ? base.CombatChar : base.EnemyChar;
			}
		}

		// Token: 0x06003E01 RID: 15873 RVA: 0x00254560 File Offset: 0x00252760
		public WuXiaQiJueJian()
		{
		}

		// Token: 0x06003E02 RID: 15874 RVA: 0x0025456A File Offset: 0x0025276A
		public WuXiaQiJueJian(CombatSkillKey skillKey) : base(skillKey, 13206, -1)
		{
		}

		// Token: 0x06003E03 RID: 15875 RVA: 0x0025457C File Offset: 0x0025277C
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(208, EDataModifyType.Custom, base.SkillTemplateId);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_GetShaTrick(new Events.OnGetShaTrick(this.OnGetShaTrick));
			Events.RegisterHandler_RemoveShaTrick(new Events.OnRemoveShaTrick(this.OnRemoveShaTrick));
			this._autoCastSkillId = -1;
			this._castNoTricks = false;
			this._tricksUid = new DataUid(0, 0, ulong.MaxValue, uint.MaxValue);
		}

		// Token: 0x06003E04 RID: 15876 RVA: 0x00254628 File Offset: 0x00252828
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatCharChanged(new Events.OnCombatCharChanged(this.OnCombatCharChanged));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_GetShaTrick(new Events.OnGetShaTrick(this.OnGetShaTrick));
			Events.UnRegisterHandler_RemoveShaTrick(new Events.OnRemoveShaTrick(this.OnRemoveShaTrick));
			GameDataBridge.RemovePostDataModificationHandler(this._tricksUid, base.DataHandlerKey);
		}

		// Token: 0x06003E05 RID: 15877 RVA: 0x002546B4 File Offset: 0x002528B4
		private void OnCombatBegin(DataContext context)
		{
			this.UpdateTrickChar(context);
		}

		// Token: 0x06003E06 RID: 15878 RVA: 0x002546BF File Offset: 0x002528BF
		private void OnCombatCharChanged(DataContext context, bool isAlly)
		{
			this.UpdateTrickChar(context);
		}

		// Token: 0x06003E07 RID: 15879 RVA: 0x002546CC File Offset: 0x002528CC
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != this._autoCastSkillId;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * WuXiaQiJueJian.ProgressPercent);
			}
		}

		// Token: 0x06003E08 RID: 15880 RVA: 0x00254720 File Offset: 0x00252920
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId;
			if (!flag)
			{
				bool flag2 = skillId == this._autoCastSkillId;
				if (flag2)
				{
					this._autoCastSkillId = -1;
				}
				else
				{
					bool flag3 = skillId != base.SkillTemplateId || !base.PowerMatchAffectRequire((int)power, 0);
					if (!flag3)
					{
						base.AddMaxEffectCount(true);
					}
				}
			}
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x00254780 File Offset: 0x00252980
		private void OnGetShaTrick(DataContext context, int charId, bool isAlly, bool real)
		{
			bool flag = charId == this.TrickChar.GetId();
			if (flag)
			{
				this.DoAutoCast(context);
			}
		}

		// Token: 0x06003E0A RID: 15882 RVA: 0x002547A8 File Offset: 0x002529A8
		private void OnRemoveShaTrick(DataContext context, int charId)
		{
			bool flag = charId == this.TrickChar.GetId();
			if (flag)
			{
				this.DoAutoCast(context);
			}
		}

		// Token: 0x06003E0B RID: 15883 RVA: 0x002547D0 File Offset: 0x002529D0
		private void UpdateTrickChar(DataContext context)
		{
			int charId = this.TrickChar.GetId();
			bool flag = charId == (int)this._tricksUid.SubId0;
			if (!flag)
			{
				bool flag2 = this._tricksUid.SubId0 != ulong.MaxValue;
				if (flag2)
				{
					GameDataBridge.RemovePostDataModificationHandler(this._tricksUid, base.DataHandlerKey);
				}
				this._tricksUid = base.ParseCombatCharacterDataUid(charId, 28);
				GameDataBridge.AddPostDataModificationHandler(this._tricksUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnTrickChanged));
				this.OnTrickChanged(context, this._tricksUid);
			}
		}

		// Token: 0x06003E0C RID: 15884 RVA: 0x00254864 File Offset: 0x00252A64
		private void OnTrickChanged(DataContext context, DataUid dataUid)
		{
			bool castNoTricks = this.TrickChar.GetTrickCount(19) >= 7;
			bool flag = castNoTricks == this._castNoTricks;
			if (!flag)
			{
				this._castNoTricks = castNoTricks;
				DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
			}
		}

		// Token: 0x06003E0D RID: 15885 RVA: 0x002548B4 File Offset: 0x00252AB4
		private void DoAutoCast(DataContext context)
		{
			bool flag = base.EffectCount <= 0 || base.SkillData.GetSilencing();
			if (!flag)
			{
				int continueTrickCount = this.TrickChar.GetContinueTricksAtStart(19);
				int maxSkillGrade = Math.Min(3 + continueTrickCount, 5);
				short skillId = DomainManager.Combat.GetRandomAttackSkill(base.CombatChar, 7, (sbyte)maxSkillGrade, context.Random, true, -1);
				bool flag2 = skillId < 0 || Config.CombatSkill.Instance[skillId].Grade < 3;
				if (!flag2)
				{
					this._autoCastSkillId = skillId;
					DomainManager.Combat.SilenceSkill(context, base.CombatChar, base.SkillTemplateId, 180, -1);
					DomainManager.Combat.CastSkillFree(context, base.CombatChar, this._autoCastSkillId, ECombatCastFreePriority.Normal);
					base.ShowSpecialEffectTips(0);
					base.ShowSpecialEffectTips(1);
					base.ReduceEffectCount(1);
				}
			}
		}

		// Token: 0x06003E0E RID: 15886 RVA: 0x00254990 File Offset: 0x00252B90
		public override List<NeedTrick> GetModifiedValue(AffectedDataKey dataKey, List<NeedTrick> dataValue)
		{
			bool flag = dataKey.SkillKey == this.SkillKey && dataKey.FieldId == 208 && this._castNoTricks;
			if (flag)
			{
				dataValue.Clear();
			}
			return base.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x0400124E RID: 4686
		private const int CastNoTricksRequireShaTrickCount = 7;

		// Token: 0x0400124F RID: 4687
		private const sbyte MinSkillGrade = 3;

		// Token: 0x04001250 RID: 4688
		private const sbyte MaxSkillGrade = 5;

		// Token: 0x04001251 RID: 4689
		private const short CdFrame = 180;

		// Token: 0x04001252 RID: 4690
		private static readonly CValuePercent ProgressPercent = 100;

		// Token: 0x04001253 RID: 4691
		private short _autoCastSkillId;

		// Token: 0x04001254 RID: 4692
		private DataUid _tricksUid;

		// Token: 0x04001255 RID: 4693
		private bool _castNoTricks;
	}
}
