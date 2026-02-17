using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special
{
	// Token: 0x0200044A RID: 1098
	public class DingShenFa : CurseSilenceCombatSkill
	{
		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06003A44 RID: 14916 RVA: 0x00242CA2 File Offset: 0x00240EA2
		protected override sbyte TargetEquipType
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x06003A45 RID: 14917 RVA: 0x00242CA5 File Offset: 0x00240EA5
		public DingShenFa()
		{
		}

		// Token: 0x06003A46 RID: 14918 RVA: 0x00242CBA File Offset: 0x00240EBA
		public DingShenFa(CombatSkillKey skillKey) : base(skillKey, 7301)
		{
		}

		// Token: 0x06003A47 RID: 14919 RVA: 0x00242CD5 File Offset: 0x00240ED5
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedAllEnemyData(69, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003A48 RID: 14920 RVA: 0x00242CFD File Offset: 0x00240EFD
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06003A49 RID: 14921 RVA: 0x00242D1C File Offset: 0x00240F1C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			CombatSkillKey skillKey = new CombatSkillKey(charId, skillId);
			this._reducingSkillKeys.Remove(skillKey);
		}

		// Token: 0x06003A4A RID: 14922 RVA: 0x00242D41 File Offset: 0x00240F41
		protected override void OnSilenceBegin(DataContext context, CombatSkillKey skillKey)
		{
			this._reducingSkillKeys.Add(skillKey);
		}

		// Token: 0x06003A4B RID: 14923 RVA: 0x00242D51 File Offset: 0x00240F51
		protected override void OnSilenceEnd(DataContext context, CombatSkillKey skillKey)
		{
		}

		// Token: 0x06003A4C RID: 14924 RVA: 0x00242D54 File Offset: 0x00240F54
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			ushort fieldId = dataKey.FieldId;
			if (!true)
			{
			}
			int result;
			if (fieldId == 69)
			{
				if (this._reducingSkillKeys.Contains(dataKey.SkillKey))
				{
					result = -40;
					goto IL_39;
				}
			}
			result = base.GetModifyValue(dataKey, currModifyValue);
			IL_39:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x0400110D RID: 4365
		private const int ReduceDamagePercent = -40;

		// Token: 0x0400110E RID: 4366
		private readonly HashSet<CombatSkillKey> _reducingSkillKeys = new HashSet<CombatSkillKey>();
	}
}
