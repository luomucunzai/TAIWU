using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.FistAndPalm
{
	// Token: 0x02000396 RID: 918
	public class QingZhuChanHunShou : CombatSkillEffectBase
	{
		// Token: 0x0600365F RID: 13919 RVA: 0x00230895 File Offset: 0x0022EA95
		public QingZhuChanHunShou()
		{
		}

		// Token: 0x06003660 RID: 13920 RVA: 0x002308AA File Offset: 0x0022EAAA
		public QingZhuChanHunShou(CombatSkillKey skillKey) : base(skillKey, 12104, -1)
		{
		}

		// Token: 0x06003661 RID: 13921 RVA: 0x002308C8 File Offset: 0x0022EAC8
		public override void OnEnable(DataContext context)
		{
			this.ClearKey();
			this._enemyCasting = false;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 154, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003662 RID: 13922 RVA: 0x0023095F File Offset: 0x0022EB5F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003663 RID: 13923 RVA: 0x00230988 File Offset: 0x0022EB88
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = !this.IsSrcSkillPerformed;
			if (!flag)
			{
				bool flag2 = this.IsEnemyKey(charId, skillId);
				if (flag2)
				{
					this._enemyCasting = true;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 154);
					DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
					base.ShowSpecialEffectTips(1);
				}
				else
				{
					bool flag3 = charId == base.CharacterId && skillId == base.SkillTemplateId;
					if (flag3)
					{
						this._addPower = this.GetKeyPower();
						bool flag4 = this._addPower > 0;
						if (flag4)
						{
							DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
							base.ShowSpecialEffectTips(2);
						}
						bool enemyCasting = this._enemyCasting;
						if (enemyCasting)
						{
							DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
						}
					}
				}
			}
		}

		// Token: 0x06003664 RID: 13924 RVA: 0x00230A80 File Offset: 0x0022EC80
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.IsSrcSkillPerformed;
			if (flag)
			{
				bool flag2 = charId != base.CharacterId || skillId != base.SkillTemplateId;
				if (!flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						this.IsSrcSkillPerformed = true;
						int enemyId = base.CurrEnemyChar.GetId();
						List<short> skillRandomPool = ObjectPool<List<short>>.Instance.Get();
						skillRandomPool.Clear();
						skillRandomPool.AddRange(base.CurrEnemyChar.GetAttackSkillList());
						skillRandomPool.RemoveAll((short id) => id < 0);
						bool flag4 = skillRandomPool.Count > 0;
						if (flag4)
						{
							this.SelectKey(enemyId, skillRandomPool);
							base.ShowSpecialEffectTips(0);
						}
						ObjectPool<List<short>>.Instance.Return(skillRandomPool);
					}
					else
					{
						base.RemoveSelf(context);
					}
				}
			}
			else
			{
				bool flag5 = this._enemyCasting && this.IsEnemyKey(charId, skillId);
				if (flag5)
				{
					this._enemyCasting = false;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 154);
					DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
				}
				else
				{
					bool flag6 = charId == base.CharacterId && skillId == base.SkillTemplateId && base.PowerMatchAffectRequire((int)power, 0);
					if (flag6)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x06003665 RID: 13925 RVA: 0x00230BFC File Offset: 0x0022EDFC
		private int GetEnemySkillPowerChange(int enemyId, short skillId)
		{
			CombatSkillKey skillKey = new CombatSkillKey(enemyId, skillId);
			SkillPowerChangeCollection powerCollection;
			return (base.IsDirect ? DomainManager.Combat.TryGetElement_SkillPowerAddInCombat(skillKey, out powerCollection) : DomainManager.Combat.TryGetElement_SkillPowerReduceInCombat(skillKey, out powerCollection)) ? Math.Abs(powerCollection.GetTotalChangeValue()) : 0;
		}

		// Token: 0x06003666 RID: 13926 RVA: 0x00230C4D File Offset: 0x0022EE4D
		private void ClearKey()
		{
			this._enemySkillKey.Clear();
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x00230C5C File Offset: 0x0022EE5C
		private void SelectKey(int enemyId, List<short> skillRandomPool)
		{
			QingZhuChanHunShou.<>c__DisplayClass12_0 CS$<>8__locals1 = new QingZhuChanHunShou.<>c__DisplayClass12_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.enemyId = enemyId;
			bool flag = skillRandomPool.Count == 0;
			if (!flag)
			{
				bool flag2 = skillRandomPool.Count > 2;
				if (flag2)
				{
					skillRandomPool.Sort(new Comparison<short>(CS$<>8__locals1.<SelectKey>g__Comparison|0));
				}
				this._enemySkillKey.Clear();
				this._enemySkillKey.Add(new CombatSkillKey(CS$<>8__locals1.enemyId, skillRandomPool[skillRandomPool.Count - 1]));
				bool flag3 = skillRandomPool.Count > 1;
				if (flag3)
				{
					this._enemySkillKey.Add(new CombatSkillKey(CS$<>8__locals1.enemyId, skillRandomPool[skillRandomPool.Count - 2]));
				}
			}
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x00230D14 File Offset: 0x0022EF14
		private int GetKeyPower()
		{
			return (from x in this._enemySkillKey
			select this.GetEnemySkillPowerChange(x.CharId, x.SkillTemplateId)).Sum();
		}

		// Token: 0x06003669 RID: 13929 RVA: 0x00230D44 File Offset: 0x0022EF44
		private bool IsEnemyKey(int charId, short skillId)
		{
			return this._enemySkillKey.Any((CombatSkillKey x) => x.CharId == charId && x.SkillTemplateId == skillId);
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x00230D84 File Offset: 0x0022EF84
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

		// Token: 0x0600366B RID: 13931 RVA: 0x00230DDC File Offset: 0x0022EFDC
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || !this._enemyCasting;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 154;
				result = (!flag2 && dataValue);
			}
			return result;
		}

		// Token: 0x04000FDD RID: 4061
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x04000FDE RID: 4062
		private readonly List<CombatSkillKey> _enemySkillKey = new List<CombatSkillKey>();

		// Token: 0x04000FDF RID: 4063
		private bool _enemyCasting;

		// Token: 0x04000FE0 RID: 4064
		private int _addPower;
	}
}
