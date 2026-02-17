using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.RanChenZi
{
	// Token: 0x020002EF RID: 751
	public class YiJianRongChenYin : CombatSkillEffectBase
	{
		// Token: 0x06003362 RID: 13154 RVA: 0x00224B83 File Offset: 0x00222D83
		public YiJianRongChenYin()
		{
		}

		// Token: 0x06003363 RID: 13155 RVA: 0x00224B8D File Offset: 0x00222D8D
		public YiJianRongChenYin(CombatSkillKey skillKey) : base(skillKey, 17130, -1)
		{
		}

		// Token: 0x06003364 RID: 13156 RVA: 0x00224BA0 File Offset: 0x00222DA0
		public override void OnEnable(DataContext context)
		{
			this._affected = false;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 74, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 107, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.RegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003365 RID: 13157 RVA: 0x00224C44 File Offset: 0x00222E44
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
			Events.UnRegisterHandler_NormalAttackAllEnd(new Events.OnNormalAttackAllEnd(this.OnNormalAttackAllEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003366 RID: 13158 RVA: 0x00224C9C File Offset: 0x00222E9C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = !this.IsSrcSkillPerformed;
				if (flag2)
				{
					this.IsSrcSkillPerformed = true;
					base.AddMaxEffectCount(true);
					base.ShowSpecialEffectTips(0);
					sbyte taskStatus = DomainManager.World.GetElement_XiangshuAvatarTaskStatuses(6).JuniorXiangshuTaskStatus;
					bool flag3 = taskStatus > 4;
					if (flag3)
					{
						bool goodEnding = taskStatus == 6;
						bool flag4 = goodEnding;
						if (flag4)
						{
							DomainManager.Combat.RemoveAllAcupoint(context, base.CurrEnemyChar);
						}
						else
						{
							DomainManager.Combat.AddAcupoint(context, base.CurrEnemyChar, 3, this.SkillKey, -1, 1, true);
						}
						base.ShowSpecialEffectTips(goodEnding, 1, 2);
					}
				}
				else
				{
					base.RemoveSelf(context);
				}
			}
		}

		// Token: 0x06003367 RID: 13159 RVA: 0x00224D64 File Offset: 0x00222F64
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && this.IsSrcSkillPerformed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003368 RID: 13160 RVA: 0x00224DB4 File Offset: 0x00222FB4
		private void OnNormalAttackAllEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x06003369 RID: 13161 RVA: 0x00224DE0 File Offset: 0x00222FE0
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected;
			if (!flag)
			{
				this._affected = false;
				base.ReduceEffectCount(1);
			}
		}

		// Token: 0x0600336A RID: 13162 RVA: 0x00224E0C File Offset: 0x0022300C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				this._affected = true;
				result = ((dataKey.FieldId == 74) ? 100 : -50);
			}
			return result;
		}

		// Token: 0x04000F31 RID: 3889
		private const sbyte AddHitOdds = 100;

		// Token: 0x04000F32 RID: 3890
		private const sbyte ReduceHitOdds = -50;

		// Token: 0x04000F33 RID: 3891
		private bool _affected;
	}
}
