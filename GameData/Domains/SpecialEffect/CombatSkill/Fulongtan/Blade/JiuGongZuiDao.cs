using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Blade
{
	// Token: 0x0200052D RID: 1325
	public class JiuGongZuiDao : CombatSkillEffectBase
	{
		// Token: 0x06003F6D RID: 16237 RVA: 0x00259C2F File Offset: 0x00257E2F
		public JiuGongZuiDao()
		{
		}

		// Token: 0x06003F6E RID: 16238 RVA: 0x00259C39 File Offset: 0x00257E39
		public JiuGongZuiDao(CombatSkillKey skillKey) : base(skillKey, 14201, -1)
		{
		}

		// Token: 0x06003F6F RID: 16239 RVA: 0x00259C4C File Offset: 0x00257E4C
		public unsafe override void OnEnable(DataContext context)
		{
			CombatCharacter affectChar = base.IsDirect ? base.CombatChar : DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true);
			List<sbyte> trickRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
			trickRandomPool.Clear();
			trickRandomPool.AddRange(affectChar.GetTricks().Tricks.Values);
			trickRandomPool.RemoveAll((sbyte type) => affectChar.IsTrickUsable(type) == this.IsDirect);
			bool flag = trickRandomPool.Count > 0;
			if (flag)
			{
				bool isDrunk = (*this.CharObj.GetEatingItems()).ContainsWine();
				int removeCount = Math.Min(isDrunk ? 3 : 2, trickRandomPool.Count);
				List<NeedTrick> removeTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
				removeTricks.Clear();
				for (int i = 0; i < removeCount; i++)
				{
					sbyte trickType = trickRandomPool[context.Random.Next(0, trickRandomPool.Count)];
					trickRandomPool.Remove(trickType);
					removeTricks.Add(new NeedTrick(trickType, 1));
				}
				bool flag2 = isDrunk;
				if (flag2)
				{
					this._addPower = removeCount * 10;
					this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
					this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
				}
				DomainManager.Combat.RemoveTrick(context, affectChar, removeTricks, base.IsDirect, false, -1);
				base.ShowSpecialEffectTips(0);
				ObjectPool<List<NeedTrick>>.Instance.Return(removeTricks);
			}
			ObjectPool<List<sbyte>>.Instance.Return(trickRandomPool);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F70 RID: 16240 RVA: 0x00259E07 File Offset: 0x00258007
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003F71 RID: 16241 RVA: 0x00259E1C File Offset: 0x0025801C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003F72 RID: 16242 RVA: 0x00259E54 File Offset: 0x00258054
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
				bool flag2 = dataKey.FieldId == 199;
				if (flag2)
				{
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040012AE RID: 4782
		private const sbyte NormalRemoveTrickCount = 2;

		// Token: 0x040012AF RID: 4783
		private const sbyte DrunkRemoveTrickCount = 3;

		// Token: 0x040012B0 RID: 4784
		private const sbyte DrunkAddPowerUnit = 10;

		// Token: 0x040012B1 RID: 4785
		private int _addPower;
	}
}
