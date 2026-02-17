using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x020005A2 RID: 1442
	public abstract class ReduceEnemyNeiliAllocation : CombatSkillEffectBase
	{
		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x060042D6 RID: 17110 RVA: 0x002687F0 File Offset: 0x002669F0
		private unsafe bool EnemyBelowOrigin
		{
			get
			{
				return *base.CurrEnemyChar.GetNeiliAllocation()[(int)this.AffectNeiliAllocationType] < *base.CurrEnemyChar.GetOriginNeiliAllocation()[(int)this.AffectNeiliAllocationType];
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x060042D7 RID: 17111 RVA: 0x00268833 File Offset: 0x00266A33
		private int DirectReduceValue
		{
			get
			{
				return this.EnemyBelowOrigin ? 10 : 30;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x060042D8 RID: 17112 RVA: 0x00268843 File Offset: 0x00266A43
		private int ReverseStealValue
		{
			get
			{
				return this.EnemyBelowOrigin ? 5 : 15;
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x060042D9 RID: 17113
		protected abstract byte AffectNeiliAllocationType { get; }

		// Token: 0x060042DA RID: 17114 RVA: 0x00268852 File Offset: 0x00266A52
		protected ReduceEnemyNeiliAllocation()
		{
		}

		// Token: 0x060042DB RID: 17115 RVA: 0x0026885C File Offset: 0x00266A5C
		protected ReduceEnemyNeiliAllocation(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x060042DC RID: 17116 RVA: 0x00268869 File Offset: 0x00266A69
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060042DD RID: 17117 RVA: 0x0026887E File Offset: 0x00266A7E
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060042DE RID: 17118 RVA: 0x00268894 File Offset: 0x00266A94
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					CombatCharacter enemyChar = base.CurrEnemyChar;
					int changeValue = base.IsDirect ? this.DirectReduceValue : this.ReverseStealValue;
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						enemyChar.ChangeNeiliAllocation(context, this.AffectNeiliAllocationType, -changeValue, true, true);
					}
					else
					{
						base.CombatChar.AbsorbNeiliAllocation(context, enemyChar, this.AffectNeiliAllocationType, changeValue);
					}
					base.ShowSpecialEffectTips(0);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x040013C5 RID: 5061
		private const int ReduceValueAboveOrigin = 30;

		// Token: 0x040013C6 RID: 5062
		private const int ReduceValueBelowOrigin = 10;

		// Token: 0x040013C7 RID: 5063
		private const int StealValueAboveOrigin = 15;

		// Token: 0x040013C8 RID: 5064
		private const int StealValueBelowOrigin = 5;
	}
}
