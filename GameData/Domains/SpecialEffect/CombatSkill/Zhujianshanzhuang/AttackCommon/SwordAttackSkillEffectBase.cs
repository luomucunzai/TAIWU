using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon
{
	// Token: 0x020001E5 RID: 485
	public abstract class SwordAttackSkillEffectBase : SwordUnlockEffectBase
	{
		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06002DE4 RID: 11748
		protected abstract ushort FieldId { get; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06002DE5 RID: 11749
		protected abstract int AddValue { get; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06002DE6 RID: 11750 RVA: 0x0020D454 File Offset: 0x0020B654
		private int EffectAddValue
		{
			get
			{
				return this.AddValue;
			}
		}

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06002DE7 RID: 11751 RVA: 0x0020D45C File Offset: 0x0020B65C
		private int SelfAddValue
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? (this.AddValue * 2) : this.AddValue;
			}
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x0020D476 File Offset: 0x0020B676
		protected SwordAttackSkillEffectBase()
		{
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x0020D480 File Offset: 0x0020B680
		protected SwordAttackSkillEffectBase(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x0020D48C File Offset: 0x0020B68C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(this.FieldId, EDataModifyType.AddPercent, -1);
			this._addingValue = 0;
			this._addingSkillId = -1;
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x0020D4E3 File Offset: 0x0020B6E3
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x0020D514 File Offset: 0x0020B714
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId;
			if (!flag)
			{
				bool flag2 = skillId == base.SkillTemplateId && base.IsReverseOrUsingDirectWeapon;
				if (flag2)
				{
					this._addingValue += this.SelfAddValue;
				}
				bool flag3 = base.EffectCount > 0;
				if (flag3)
				{
					base.ReduceEffectCount(1);
					this._addingValue += this.EffectAddValue;
				}
				bool flag4 = this._addingValue <= 0;
				if (!flag4)
				{
					this._addingSkillId = skillId;
					base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
				}
			}
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x0020D5B8 File Offset: 0x0020B7B8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != this._addingSkillId;
			if (!flag)
			{
				this._addingValue = 0;
				this._addingSkillId = -1;
			}
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x0020D5F4 File Offset: 0x0020B7F4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != this._addingSkillId || dataKey.FieldId != this.FieldId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this._addingValue;
			}
			return result;
		}

		// Token: 0x04000DB7 RID: 3511
		private int _addingValue;

		// Token: 0x04000DB8 RID: 3512
		private short _addingSkillId;
	}
}
