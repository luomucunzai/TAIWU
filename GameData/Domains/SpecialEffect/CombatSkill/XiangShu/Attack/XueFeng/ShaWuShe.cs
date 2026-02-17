using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XueFeng
{
	// Token: 0x020002D0 RID: 720
	public class ShaWuShe : CombatSkillEffectBase
	{
		// Token: 0x0600329D RID: 12957 RVA: 0x002200CD File Offset: 0x0021E2CD
		public ShaWuShe()
		{
		}

		// Token: 0x0600329E RID: 12958 RVA: 0x002200D7 File Offset: 0x0021E2D7
		public ShaWuShe(CombatSkillKey skillKey) : base(skillKey, 17073, -1)
		{
		}

		// Token: 0x0600329F RID: 12959 RVA: 0x002200E8 File Offset: 0x0021E2E8
		public override void OnEnable(DataContext context)
		{
			this._affected = false;
			this._autoCasting = false;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 77, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060032A0 RID: 12960 RVA: 0x00220168 File Offset: 0x0021E368
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_PrepareSkillEnd(new Events.OnPrepareSkillEnd(this.OnPrepareSkillEnd));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060032A1 RID: 12961 RVA: 0x002201A4 File Offset: 0x0021E3A4
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !this._autoCasting;
			if (!flag)
			{
				DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 75 / 100);
			}
		}

		// Token: 0x060032A2 RID: 12962 RVA: 0x002201FC File Offset: 0x0021E3FC
		private void OnPrepareSkillEnd(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				ValueTuple<sbyte, sbyte> injuries = base.CurrEnemyChar.GetInjuries().Get(base.CombatChar.SkillAttackBodyPart);
				this._injuriesBeforeAttack.Outer = (int)injuries.Item1;
				this._injuriesBeforeAttack.Inner = (int)injuries.Item2;
			}
		}

		// Token: 0x060032A3 RID: 12963 RVA: 0x0022026C File Offset: 0x0021E46C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || interrupted;
			if (!flag)
			{
				bool affected = this._affected;
				if (affected)
				{
					base.ShowSpecialEffectTips(0);
					this._affected = false;
				}
				this._autoCasting = false;
				ValueTuple<sbyte, sbyte> currInjuries = base.CurrEnemyChar.GetInjuries().Get(base.CombatChar.SkillAttackBodyPart);
				bool flag2 = (this._injuriesBeforeAttack.Outer < 6 && currInjuries.Item1 >= 6) || (this._injuriesBeforeAttack.Inner < 6 && currInjuries.Item2 >= 6);
				if (flag2)
				{
					this._autoCasting = true;
					DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x060032A4 RID: 12964 RVA: 0x00220348 File Offset: 0x0021E548
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
				bool flag2 = dataKey.FieldId == 77;
				if (flag2)
				{
					this._affected = true;
					result = true;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x04000EF9 RID: 3833
		private const sbyte PrepareProgressPercent = 75;

		// Token: 0x04000EFA RID: 3834
		private OuterAndInnerInts _injuriesBeforeAttack;

		// Token: 0x04000EFB RID: 3835
		private bool _affected;

		// Token: 0x04000EFC RID: 3836
		private bool _autoCasting;
	}
}
