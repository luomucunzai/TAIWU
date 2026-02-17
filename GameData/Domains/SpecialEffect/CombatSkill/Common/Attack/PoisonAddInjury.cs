using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x0200059D RID: 1437
	public class PoisonAddInjury : CombatSkillEffectBase
	{
		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060042AE RID: 17070 RVA: 0x002681AE File Offset: 0x002663AE
		protected virtual bool AutoRemove
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060042AF RID: 17071 RVA: 0x002681B1 File Offset: 0x002663B1
		public PoisonAddInjury()
		{
		}

		// Token: 0x060042B0 RID: 17072 RVA: 0x002681BB File Offset: 0x002663BB
		public PoisonAddInjury(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060042B1 RID: 17073 RVA: 0x002681C8 File Offset: 0x002663C8
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(69, EDataModifyType.AddPercent, base.SkillTemplateId);
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060042B2 RID: 17074 RVA: 0x002681FF File Offset: 0x002663FF
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060042B3 RID: 17075 RVA: 0x00268228 File Offset: 0x00266428
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = DomainManager.Combat.InAttackRange(base.CombatChar);
				if (flag2)
				{
					this._addDamagePercent = (int)(15 * (base.IsDirect ? defender : attacker).GetDefeatMarkCollection().PoisonMarkList[(int)this.RequirePoisonType]);
					bool flag3 = this._addDamagePercent > 0;
					if (flag3)
					{
						base.ShowSpecialEffectTips(0);
					}
				}
				this.OnCastOwnBegin(context);
			}
		}

		// Token: 0x060042B4 RID: 17076 RVA: 0x002682B4 File Offset: 0x002664B4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					this.OnCastMaxPower(context);
				}
				bool autoRemove = this.AutoRemove;
				if (autoRemove)
				{
					base.RemoveSelf(context);
				}
				else
				{
					this._addDamagePercent = 0;
				}
			}
		}

		// Token: 0x060042B5 RID: 17077 RVA: 0x00268311 File Offset: 0x00266511
		protected virtual void OnCastOwnBegin(DataContext context)
		{
		}

		// Token: 0x060042B6 RID: 17078 RVA: 0x00268314 File Offset: 0x00266514
		protected virtual void OnCastMaxPower(DataContext context)
		{
		}

		// Token: 0x060042B7 RID: 17079 RVA: 0x00268318 File Offset: 0x00266518
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
				bool flag2 = dataKey.FieldId == 69;
				if (flag2)
				{
					result = this._addDamagePercent;
				}
				else
				{
					result = this.GetModifyValueInternal(dataKey, currModifyValue);
				}
			}
			return result;
		}

		// Token: 0x060042B8 RID: 17080 RVA: 0x00268374 File Offset: 0x00266574
		protected virtual int GetModifyValueInternal(AffectedDataKey dataKey, int currModifyValue)
		{
			return 0;
		}

		// Token: 0x040013BC RID: 5052
		private const int AddDamagePercentUnit = 15;

		// Token: 0x040013BD RID: 5053
		protected sbyte RequirePoisonType;

		// Token: 0x040013BE RID: 5054
		private int _addDamagePercent;
	}
}
