using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Implement;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x0200060D RID: 1549
	public class LoongWoodImplementRange : ISpecialEffectImplement, ISpecialEffectModifier
	{
		// Token: 0x06004558 RID: 17752 RVA: 0x0027212B File Offset: 0x0027032B
		public LoongWoodImplementRange(int markToReduceRange, int markToAddRange)
		{
			this._markToReduceRange = markToReduceRange;
			this._markToAddRange = markToAddRange;
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06004559 RID: 17753 RVA: 0x00272143 File Offset: 0x00270343
		// (set) Token: 0x0600455A RID: 17754 RVA: 0x0027214B File Offset: 0x0027034B
		public CombatSkillEffectBase EffectBase { get; set; }

		// Token: 0x0600455B RID: 17755 RVA: 0x00272154 File Offset: 0x00270354
		public void OnEnable(DataContext context)
		{
			this.EffectBase.CreateAffectedData(145, EDataModifyType.Add, -1);
			this.EffectBase.CreateAffectedData(146, EDataModifyType.Add, -1);
			this.EffectBase.CreateAffectedAllEnemyData(273, EDataModifyType.Add, -1);
			this._defeatMarkUid = new DataUid(8, 10, (ulong)((long)this.EffectBase.CharacterId), 50U);
			GameDataBridge.AddPostDataModificationHandler(this._defeatMarkUid, this.EffectBase.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefeatMarkChanged));
		}

		// Token: 0x0600455C RID: 17756 RVA: 0x002721DA File Offset: 0x002703DA
		public void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._defeatMarkUid, this.EffectBase.DataHandlerKey);
		}

		// Token: 0x0600455D RID: 17757 RVA: 0x002721F4 File Offset: 0x002703F4
		private void OnDefeatMarkChanged(DataContext context, DataUid _)
		{
			DefeatMarkCollection defeatMarkCollection = this.EffectBase.CombatChar.GetDefeatMarkCollection();
			int injuryMarkCount = defeatMarkCollection.GetTotalInjuryCount();
			bool flag = injuryMarkCount == this._injuryMarkCount;
			if (!flag)
			{
				this._injuryMarkCount = injuryMarkCount;
				DomainManager.SpecialEffect.InvalidateCache(context, this.EffectBase.CharacterId, 145);
				DomainManager.SpecialEffect.InvalidateCache(context, this.EffectBase.CharacterId, 146);
				int[] enemyTeam = DomainManager.Combat.GetCharacterList(!this.EffectBase.CombatChar.IsAlly);
				foreach (int enemyCharId in enemyTeam)
				{
					bool flag2 = enemyCharId >= 0;
					if (flag2)
					{
						DomainManager.SpecialEffect.InvalidateCache(context, enemyCharId, 273);
					}
				}
			}
		}

		// Token: 0x0600455E RID: 17758 RVA: 0x002722C8 File Offset: 0x002704C8
		public int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag = fieldId - 145 <= 1;
			bool flag2 = flag && dataKey.CharId == this.EffectBase.CharacterId;
			int result;
			if (flag2)
			{
				result = this._injuryMarkCount * this._markToAddRange;
			}
			else
			{
				bool flag3 = dataKey.FieldId == 273 && dataKey.CharId != this.EffectBase.CharacterId;
				if (flag3)
				{
					int minDist = dataKey.CustomParam0;
					int maxDist = dataKey.CustomParam1;
					int remainDist = maxDist - minDist;
					result = Math.Min((remainDist - 2) / 2, this._injuryMarkCount * this._markToReduceRange);
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400147F RID: 5247
		private const int MinRangeSpace = 2;

		// Token: 0x04001480 RID: 5248
		private readonly int _markToReduceRange;

		// Token: 0x04001481 RID: 5249
		private readonly int _markToAddRange;

		// Token: 0x04001482 RID: 5250
		private DataUid _defeatMarkUid;

		// Token: 0x04001483 RID: 5251
		private int _injuryMarkCount;
	}
}
