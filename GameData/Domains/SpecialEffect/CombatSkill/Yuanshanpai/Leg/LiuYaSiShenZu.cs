using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Leg
{
	// Token: 0x02000200 RID: 512
	public class LiuYaSiShenZu : CombatSkillEffectBase
	{
		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x06002E8C RID: 11916 RVA: 0x0020FB99 File Offset: 0x0020DD99
		private bool IsAffect4
		{
			get
			{
				return base.EffectCount >= 4;
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x06002E8D RID: 11917 RVA: 0x0020FBA7 File Offset: 0x0020DDA7
		private bool IsAffect8
		{
			get
			{
				return base.EffectCount >= 8;
			}
		}

		// Token: 0x06002E8E RID: 11918 RVA: 0x0020FBB5 File Offset: 0x0020DDB5
		public LiuYaSiShenZu()
		{
		}

		// Token: 0x06002E8F RID: 11919 RVA: 0x0020FBBF File Offset: 0x0020DDBF
		public LiuYaSiShenZu(CombatSkillKey skillKey) : base(skillKey, 5108, -1)
		{
		}

		// Token: 0x06002E90 RID: 11920 RVA: 0x0020FBD0 File Offset: 0x0020DDD0
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(217, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(215, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(251, EDataModifyType.Custom, base.SkillTemplateId);
			base.CreateAffectedData(282, EDataModifyType.Custom, -1);
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.RegisterHandler_AddInjury(new Events.OnAddInjury(this.OnAddInjury));
			Events.RegisterHandler_AddFatalDamageMark(new Events.OnAddFatalDamageMark(this.OnAddFatalOrMindMark));
			Events.RegisterHandler_AddMindMark(new Events.OnAddMindMark(this.OnAddFatalOrMindMark));
			Events.RegisterHandler_FlawAdded(new Events.OnFlawAdded(this.OnFlawAdded));
			Events.RegisterHandler_AcuPointAdded(new Events.OnAcuPointAdded(this.OnAcuPointAdded));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002E91 RID: 11921 RVA: 0x0020FCAC File Offset: 0x0020DEAC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.UnRegisterHandler_AddInjury(new Events.OnAddInjury(this.OnAddInjury));
			Events.UnRegisterHandler_AddFatalDamageMark(new Events.OnAddFatalDamageMark(this.OnAddFatalOrMindMark));
			Events.UnRegisterHandler_AddMindMark(new Events.OnAddMindMark(this.OnAddFatalOrMindMark));
			Events.UnRegisterHandler_FlawAdded(new Events.OnFlawAdded(this.OnFlawAdded));
			Events.UnRegisterHandler_AcuPointAdded(new Events.OnAcuPointAdded(this.OnAcuPointAdded));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002E92 RID: 11922 RVA: 0x0020FD40 File Offset: 0x0020DF40
		protected override IEnumerable<int> CalcFrameCounterPeriods()
		{
			yield return 600;
			yield break;
		}

		// Token: 0x06002E93 RID: 11923 RVA: 0x0020FD50 File Offset: 0x0020DF50
		public override bool IsOn(int counterType)
		{
			return this.IsAffect8;
		}

		// Token: 0x06002E94 RID: 11924 RVA: 0x0020FD58 File Offset: 0x0020DF58
		public override void OnProcess(DataContext context, int counterType)
		{
			base.AddEffectCount(1);
		}

		// Token: 0x06002E95 RID: 11925 RVA: 0x0020FD64 File Offset: 0x0020DF64
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = charId != base.CharacterId || key != base.EffectKey || base.EffectCount != (int)base.MaxEffectCount;
			if (!flag)
			{
				this.DoChangeMark(context);
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, key, (short)(-(short)base.EffectCount), false, false);
			}
		}

		// Token: 0x06002E96 RID: 11926 RVA: 0x0020FDC8 File Offset: 0x0020DFC8
		private void OnAddInjury(DataContext context, CombatCharacter character, sbyte bodyPart, bool isInner, sbyte value, bool changeToOld)
		{
			bool flag = character.GetId() != base.CharacterId || isInner == base.IsDirect;
			if (!flag)
			{
				this.AddMarkEffectCount();
			}
		}

		// Token: 0x06002E97 RID: 11927 RVA: 0x0020FE00 File Offset: 0x0020E000
		private void OnAddFatalOrMindMark(DataContext context, CombatCharacter combatChar, int count)
		{
			bool flag = combatChar.GetId() != base.CharacterId || count <= 0;
			if (!flag)
			{
				this.AddMarkEffectCount();
			}
		}

		// Token: 0x06002E98 RID: 11928 RVA: 0x0020FE34 File Offset: 0x0020E034
		private void OnFlawAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
		{
			bool flag = combatChar.GetId() != base.CharacterId || !base.IsDirect;
			if (!flag)
			{
				this.AddMarkEffectCount();
			}
		}

		// Token: 0x06002E99 RID: 11929 RVA: 0x0020FE6C File Offset: 0x0020E06C
		private void OnAcuPointAdded(DataContext context, CombatCharacter combatChar, sbyte bodyPart, sbyte level)
		{
			bool flag = combatChar.GetId() != base.CharacterId || base.IsDirect;
			if (!flag)
			{
				this.AddMarkEffectCount();
			}
		}

		// Token: 0x06002E9A RID: 11930 RVA: 0x0020FEA0 File Offset: 0x0020E0A0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				base.AddEffectCount(2);
				base.ShowSpecialEffectTipsOnceInFrame(0);
			}
		}

		// Token: 0x06002E9B RID: 11931 RVA: 0x0020FEE4 File Offset: 0x0020E0E4
		private void AddMarkEffectCount()
		{
			bool flag = base.EffectCount <= 0 || base.SkillData.GetLeftCdFrame() != 0;
			if (!flag)
			{
				base.AddEffectCount(1);
				base.ShowSpecialEffectTipsOnceInFrame(0);
			}
		}

		// Token: 0x06002E9C RID: 11932 RVA: 0x0020FF24 File Offset: 0x0020E124
		private void DoChangeMark(DataContext context)
		{
			DomainManager.Combat.RemoveHalfFlawOrAcupoint(context, base.CombatChar, base.IsDirect);
			DomainManager.Combat.RemoveHalfInjury(context, base.CombatChar, !base.IsDirect);
			DomainManager.Combat.RemoveHalfFatalDamageMark(context, base.CombatChar);
			DomainManager.Combat.RemoveHalfMindDefeatMark(context, base.CombatChar);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x06002E9D RID: 11933 RVA: 0x0020FF94 File Offset: 0x0020E194
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId == base.CharacterId && dataKey.FieldId == 282;
			bool result;
			if (flag)
			{
				result = (dataValue || this.IsAffect8);
			}
			else
			{
				bool flag2 = dataKey.SkillKey != this.SkillKey || !this.IsAffect4;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 251;
					if (flag3)
					{
						result = true;
					}
					else
					{
						ushort fieldId = dataKey.FieldId;
						bool flag4 = fieldId == 215 || fieldId == 217;
						bool flag5 = flag4;
						result = (!flag5 && dataValue);
					}
				}
			}
			return result;
		}

		// Token: 0x04000DDE RID: 3550
		private const int AddEffectCountFrame = 600;

		// Token: 0x04000DDF RID: 3551
		private const int CastAddCount = 2;

		// Token: 0x04000DE0 RID: 3552
		private const int MarkAddCount = 1;
	}
}
