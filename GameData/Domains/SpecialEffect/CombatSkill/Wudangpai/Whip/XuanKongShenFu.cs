using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Whip
{
	// Token: 0x020003BB RID: 955
	public class XuanKongShenFu : CombatSkillEffectBase
	{
		// Token: 0x06003726 RID: 14118 RVA: 0x0023439E File Offset: 0x0023259E
		public XuanKongShenFu()
		{
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x002343A8 File Offset: 0x002325A8
		public XuanKongShenFu(CombatSkillKey skillKey) : base(skillKey, 4307, -1)
		{
		}

		// Token: 0x06003728 RID: 14120 RVA: 0x002343B9 File Offset: 0x002325B9
		public override void OnEnable(DataContext context)
		{
			this.DoAffect(context);
			base.CreateAffectedData(199, EDataModifyType.Add, -1);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003729 RID: 14121 RVA: 0x002343E4 File Offset: 0x002325E4
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x002343FC File Offset: 0x002325FC
		private void DoAffect(DataContext context)
		{
			int removePowerCharId = base.IsDirect ? base.CharacterId : base.CurrEnemyChar.GetId();
			List<CombatSkillKey> pool = ObjectPool<List<CombatSkillKey>>.Instance.Get();
			Dictionary<CombatSkillKey, SkillPowerChangeCollection> powerDict = base.IsDirect ? DomainManager.Combat.GetAllSkillPowerReduceInCombat() : DomainManager.Combat.GetAllSkillPowerAddInCombat();
			pool.Clear();
			foreach (CombatSkillKey skillKey in powerDict.Keys)
			{
				bool flag = skillKey.CharId == removePowerCharId;
				if (flag)
				{
					pool.Add(skillKey);
				}
			}
			bool flag2 = pool.Count > 0;
			if (flag2)
			{
				this.DoAffectImplement(context, pool);
			}
			ObjectPool<List<CombatSkillKey>>.Instance.Return(pool);
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x002344D8 File Offset: 0x002326D8
		private void DoAffectImplement(DataContext context, List<CombatSkillKey> pool)
		{
			int removeCount = Math.Min(3, pool.Count);
			int reducePower = 0;
			for (int i = 0; i < removeCount; i++)
			{
				int index = context.Random.Next(0, pool.Count);
				SkillPowerChangeCollection removedCollection = base.IsDirect ? DomainManager.Combat.RemoveSkillPowerReduceInCombat(context, pool[index]) : DomainManager.Combat.RemoveSkillPowerAddInCombat(context, pool[index]);
				pool.RemoveAt(index);
				bool flag = removedCollection != null;
				if (flag)
				{
					reducePower += Math.Abs(removedCollection.GetTotalChangeValue());
				}
			}
			this._addPower = reducePower * 500 / 100;
			int changeDisorderOfQi = 10 * reducePower;
			bool flag2 = changeDisorderOfQi > 0;
			if (flag2)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, base.CombatChar, -changeDisorderOfQi, false);
				}
				else
				{
					DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, base.CurrEnemyChar, changeDisorderOfQi, false);
				}
			}
			bool flag3 = !base.IsDirect;
			if (flag3)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CurrEnemyChar.GetId(), 199);
			}
			bool flag4 = reducePower > 0;
			if (flag4)
			{
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x00234608 File Offset: 0x00232808
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x00234640 File Offset: 0x00232840
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey == this.SkillKey && dataKey.FieldId == 199;
			int result;
			if (flag)
			{
				result = this._addPower;
			}
			else
			{
				result = base.GetModifyValue(dataKey, currModifyValue);
			}
			return result;
		}

		// Token: 0x0400101C RID: 4124
		private const sbyte AffectSkillCount = 3;

		// Token: 0x0400101D RID: 4125
		private const int AddPowerPercent = 500;

		// Token: 0x0400101E RID: 4126
		private const int ChangeDisorderOfQiUnit = 10;

		// Token: 0x0400101F RID: 4127
		private int _addPower;
	}
}
