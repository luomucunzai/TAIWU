using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.WeiQi
{
	// Token: 0x020002DE RID: 734
	public class LongZhuShi : CombatSkillEffectBase
	{
		// Token: 0x060032F3 RID: 13043 RVA: 0x00221EAA File Offset: 0x002200AA
		public LongZhuShi()
		{
		}

		// Token: 0x060032F4 RID: 13044 RVA: 0x00221EBF File Offset: 0x002200BF
		public LongZhuShi(CombatSkillKey skillKey) : base(skillKey, 17052, -1)
		{
		}

		// Token: 0x060032F5 RID: 13045 RVA: 0x00221EDB File Offset: 0x002200DB
		public override void OnEnable(DataContext context)
		{
			base.CreateAffectedAllEnemyData(199, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x060032F6 RID: 13046 RVA: 0x00221F10 File Offset: 0x00220110
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x060032F7 RID: 13047 RVA: 0x00221F38 File Offset: 0x00220138
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId || !base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				IReadOnlyDictionary<int, sbyte> trickDict = enemyChar.GetTricks().Tricks;
				List<sbyte> trickRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
				int addEffectCount = 0;
				trickRandomPool.Clear();
				trickRandomPool.AddRange(trickDict.Values.Where(new Func<sbyte, bool>(enemyChar.IsTrickUsable)));
				int removeCount = Math.Min((int)base.MaxEffectCount, trickRandomPool.Count);
				foreach (sbyte trick in RandomUtils.GetRandomUnrepeated<sbyte>(context.Random, removeCount, trickRandomPool, null))
				{
					bool flag2 = DomainManager.Combat.RemoveTrick(context, enemyChar, trick, 1, true, -1);
					if (flag2)
					{
						addEffectCount++;
					}
				}
				ObjectPool<List<sbyte>>.Instance.Return(trickRandomPool);
				bool flag3 = addEffectCount > 0;
				if (flag3)
				{
					base.AddEffectCount(addEffectCount);
				}
			}
		}

		// Token: 0x060032F8 RID: 13048 RVA: 0x00222064 File Offset: 0x00220264
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = this.SkillKey.IsMatch(charId, skillId);
			if (flag)
			{
				this.ClearPowers(context);
			}
			bool flag2 = base.CombatChar.IsAlly == isAlly || base.EffectCount <= 0;
			if (!flag2)
			{
				CombatSkillKey skillKey = new CombatSkillKey(charId, skillId);
				this._reducePowerDict[skillKey] = this._reducePowerDict.GetOrDefault(skillKey) + -30;
				DomainManager.SpecialEffect.InvalidateCache(context, charId, 199);
				base.ReduceEffectCount(1);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060032F9 RID: 13049 RVA: 0x002220F7 File Offset: 0x002202F7
		private void ClearPowers(DataContext context)
		{
			this._reducePowerDict.Clear();
			base.InvalidateAllEnemyCache(context, 199);
		}

		// Token: 0x060032FA RID: 13050 RVA: 0x00222114 File Offset: 0x00220314
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			CombatSkillKey skillKey = new CombatSkillKey(dataKey.CharId, dataKey.CombatSkillId);
			return this._reducePowerDict.GetValueOrDefault(skillKey, 0);
		}

		// Token: 0x04000F11 RID: 3857
		private const sbyte ReducePowerUnit = -30;

		// Token: 0x04000F12 RID: 3858
		private readonly Dictionary<CombatSkillKey, int> _reducePowerDict = new Dictionary<CombatSkillKey, int>();
	}
}
