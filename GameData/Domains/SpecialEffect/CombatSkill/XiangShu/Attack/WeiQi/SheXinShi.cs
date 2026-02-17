using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.WeiQi
{
	// Token: 0x020002E1 RID: 737
	public class SheXinShi : CombatSkillEffectBase
	{
		// Token: 0x0600330D RID: 13069 RVA: 0x00222B76 File Offset: 0x00220D76
		public SheXinShi()
		{
		}

		// Token: 0x0600330E RID: 13070 RVA: 0x00222B8B File Offset: 0x00220D8B
		public SheXinShi(CombatSkillKey skillKey) : base(skillKey, 17055, -1)
		{
		}

		// Token: 0x0600330F RID: 13071 RVA: 0x00222BA8 File Offset: 0x00220DA8
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedData(199, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003310 RID: 13072 RVA: 0x00222BFA File Offset: 0x00220DFA
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_SkillEffectChange(new Events.OnSkillEffectChange(this.OnSkillEffectChange));
		}

		// Token: 0x06003311 RID: 13073 RVA: 0x00222C34 File Offset: 0x00220E34
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || interrupted;
			if (!flag)
			{
				IReadOnlyDictionary<int, sbyte> trickDict = base.CombatChar.GetTricks().Tricks;
				List<sbyte> trickRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
				short addEffectCount = 0;
				trickRandomPool.Clear();
				trickRandomPool.AddRange(trickDict.Values);
				int removeCount = Math.Min((int)base.MaxEffectCount, trickRandomPool.Count);
				for (int i = 0; i < removeCount; i++)
				{
					int index = context.Random.Next(trickRandomPool.Count);
					bool flag2 = DomainManager.Combat.RemoveTrick(context, base.CombatChar, trickRandomPool[index], 1, true, -1);
					if (flag2)
					{
						addEffectCount += 1;
					}
					trickRandomPool.RemoveAt(index);
				}
				ObjectPool<List<sbyte>>.Instance.Return(trickRandomPool);
				bool flag3 = addEffectCount > 0;
				if (flag3)
				{
					DomainManager.Combat.AddSkillEffect(context, base.CombatChar, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), addEffectCount, base.MaxEffectCount, true);
				}
			}
		}

		// Token: 0x06003312 RID: 13074 RVA: 0x00222D4C File Offset: 0x00220F4C
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || base.EffectCount <= 0;
			if (!flag)
			{
				base.ReduceEffectCount(1);
				bool flag2 = base.EffectCount > 0;
				if (flag2)
				{
					bool flag3 = !this._addPowerDict.TryAdd(skillId, 60);
					if (flag3)
					{
						Dictionary<short, int> addPowerDict = this._addPowerDict;
						addPowerDict[skillId] += 60;
					}
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x06003313 RID: 13075 RVA: 0x00222DE4 File Offset: 0x00220FE4
		private void OnSkillEffectChange(DataContext context, int charId, SkillEffectKey key, short oldCount, short newCount, bool removed)
		{
			bool flag = removed && charId == base.CharacterId && key.SkillId == base.SkillTemplateId && key.IsDirect == base.IsDirect;
			if (flag)
			{
				this._addPowerDict.Clear();
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06003314 RID: 13076 RVA: 0x00222E48 File Offset: 0x00221048
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			return this._addPowerDict.GetValueOrDefault(dataKey.CombatSkillId, 0);
		}

		// Token: 0x04000F18 RID: 3864
		private const sbyte AddPowerUnit = 60;

		// Token: 0x04000F19 RID: 3865
		private readonly Dictionary<short, int> _addPowerDict = new Dictionary<short, int>();
	}
}
