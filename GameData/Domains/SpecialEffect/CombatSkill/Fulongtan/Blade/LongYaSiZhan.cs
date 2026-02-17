using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Blade
{
	// Token: 0x02000531 RID: 1329
	public class LongYaSiZhan : CombatSkillEffectBase
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06003F8C RID: 16268 RVA: 0x0025A757 File Offset: 0x00258957
		private CombatSkillItem Defend
		{
			get
			{
				return Config.CombatSkill.Instance[base.CurrEnemyChar.GetAffectingDefendSkillId()];
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06003F8D RID: 16269 RVA: 0x0025A76E File Offset: 0x0025896E
		private int AddDamagePercent
		{
			get
			{
				return (int)((this.Defend == null) ? 0 : (10 * (this.Defend.Grade + 1)));
			}
		}

		// Token: 0x06003F8E RID: 16270 RVA: 0x0025A78B File Offset: 0x0025898B
		public LongYaSiZhan()
		{
		}

		// Token: 0x06003F8F RID: 16271 RVA: 0x0025A795 File Offset: 0x00258995
		public LongYaSiZhan(CombatSkillKey skillKey) : base(skillKey, 14202, -1)
		{
		}

		// Token: 0x06003F90 RID: 16272 RVA: 0x0025A7A8 File Offset: 0x002589A8
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(69, EDataModifyType.AddPercent, base.SkillTemplateId);
			base.CreateAffectedAllEnemyData(285, EDataModifyType.Custom, -1);
			bool flag = base.EnemyChar.GetAffectingDefendSkillId() < 0;
			if (flag)
			{
				this.TryCastDefend(context);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F91 RID: 16273 RVA: 0x0025A800 File Offset: 0x00258A00
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F92 RID: 16274 RVA: 0x0025A818 File Offset: 0x00258A18
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId);
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003F93 RID: 16275 RVA: 0x0025A840 File Offset: 0x00258A40
		private void TryCastDefend(DataContext context)
		{
			CombatCharacter enemyChar = base.EnemyChar;
			int enemyCharId = enemyChar.GetId();
			List<short> skillRandomPool = ObjectPool<List<short>>.Instance.Get();
			skillRandomPool.Clear();
			skillRandomPool.AddRange(enemyChar.GetDefenceSkillList().Where(delegate(short id)
			{
				CombatSkillData data;
				return DomainManager.Combat.TryGetCombatSkillData(enemyCharId, id, out data) && data.GetCanUse();
			}));
			bool flag = skillRandomPool.Count > 0;
			if (flag)
			{
				short skillId = skillRandomPool.GetRandom(context.Random);
				DomainManager.Combat.StartPrepareSkill(context, skillId, enemyChar.IsAlly);
				base.ShowSpecialEffectTips(0);
			}
			ObjectPool<List<short>>.Instance.Return(skillRandomPool);
		}

		// Token: 0x06003F94 RID: 16276 RVA: 0x0025A8DC File Offset: 0x00258ADC
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
				bool flag2 = dataKey.FieldId == 69 && dataKey.CustomParam0 == (base.IsDirect ? 0 : 1);
				if (flag2)
				{
					result = this.AddDamagePercent;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06003F95 RID: 16277 RVA: 0x0025A948 File Offset: 0x00258B48
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			ushort fieldId = dataKey.FieldId;
			if (!true)
			{
			}
			bool result = fieldId != 285 && dataValue;
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x040012BA RID: 4794
		private const sbyte AddDamageUnit = 10;
	}
}
