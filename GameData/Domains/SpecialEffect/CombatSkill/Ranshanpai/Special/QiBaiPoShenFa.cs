using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Special
{
	// Token: 0x02000450 RID: 1104
	public class QiBaiPoShenFa : CurseSilenceCombatSkill
	{
		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06003A82 RID: 14978 RVA: 0x00243CEB File Offset: 0x00241EEB
		private int CalcReducePower
		{
			get
			{
				return base.IsDirect ? 50 : 25;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06003A83 RID: 14979 RVA: 0x00243CFB File Offset: 0x00241EFB
		protected override sbyte TargetEquipType
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06003A84 RID: 14980 RVA: 0x00243CFE File Offset: 0x00241EFE
		public QiBaiPoShenFa()
		{
		}

		// Token: 0x06003A85 RID: 14981 RVA: 0x00243D08 File Offset: 0x00241F08
		public QiBaiPoShenFa(CombatSkillKey skillKey) : base(skillKey, 7306)
		{
		}

		// Token: 0x06003A86 RID: 14982 RVA: 0x00243D18 File Offset: 0x00241F18
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedAllEnemyData(38, EDataModifyType.Add, -1);
			base.CreateAffectedAllEnemyData(39, EDataModifyType.Add, -1);
			base.CreateAffectedAllEnemyData(40, EDataModifyType.Add, -1);
			base.CreateAffectedAllEnemyData(41, EDataModifyType.Add, -1);
			base.CreateAffectedAllEnemyData(46, EDataModifyType.Add, -1);
			base.CreateAffectedAllEnemyData(47, EDataModifyType.Add, -1);
		}

		// Token: 0x06003A87 RID: 14983 RVA: 0x00243D70 File Offset: 0x00241F70
		protected override void OnSilenceBegin(DataContext context, CombatSkillKey _)
		{
			this.UpdateReduceValues(context);
			base.ShowSpecialEffectTips(2);
		}

		// Token: 0x06003A88 RID: 14984 RVA: 0x00243D83 File Offset: 0x00241F83
		protected override void OnSilenceEnd(DataContext context, CombatSkillKey skillKey)
		{
			this.UpdateReduceValues(context);
		}

		// Token: 0x06003A89 RID: 14985 RVA: 0x00243D90 File Offset: 0x00241F90
		private void UpdateReduceValues(DataContext context)
		{
			this._reduceAvoid.Initialize();
			this._reducePenetrateResist = OuterAndInnerInts.Zero;
			foreach (CombatSkillKey skillKey in base.SilencingSkills)
			{
				CombatSkill skill = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey);
				HitOrAvoidInts skillAvoids = CombatSkillDomain.CalcAddAvoidValueOnCast(skill, this.CalcReducePower);
				OuterAndInnerInts skillResists = CombatSkillDomain.CalcAddPenetrateResist(skill, this.CalcReducePower);
				this._reduceAvoid -= skillAvoids;
				this._reducePenetrateResist -= skillResists;
			}
			base.InvalidateAllEnemyCache(context, 38);
			base.InvalidateAllEnemyCache(context, 39);
			base.InvalidateAllEnemyCache(context, 40);
			base.InvalidateAllEnemyCache(context, 41);
			base.InvalidateAllEnemyCache(context, 46);
			base.InvalidateAllEnemyCache(context, 47);
		}

		// Token: 0x06003A8A RID: 14986 RVA: 0x00243E84 File Offset: 0x00242084
		public unsafe override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			ushort fieldId = dataKey.FieldId;
			if (!true)
			{
			}
			int result;
			switch (fieldId)
			{
			case 38:
				result = this._reduceAvoid.Items.FixedElementField;
				goto IL_C2;
			case 39:
				result = *(ref this._reduceAvoid.Items.FixedElementField + 4);
				goto IL_C2;
			case 40:
				result = *(ref this._reduceAvoid.Items.FixedElementField + (IntPtr)2 * 4);
				goto IL_C2;
			case 41:
				result = *(ref this._reduceAvoid.Items.FixedElementField + (IntPtr)3 * 4);
				goto IL_C2;
			case 46:
				result = this._reducePenetrateResist.Outer;
				goto IL_C2;
			case 47:
				result = this._reducePenetrateResist.Inner;
				goto IL_C2;
			}
			result = base.GetModifyValue(dataKey, currModifyValue);
			IL_C2:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x04001125 RID: 4389
		private HitOrAvoidInts _reduceAvoid;

		// Token: 0x04001126 RID: 4390
		private OuterAndInnerInts _reducePenetrateResist;
	}
}
