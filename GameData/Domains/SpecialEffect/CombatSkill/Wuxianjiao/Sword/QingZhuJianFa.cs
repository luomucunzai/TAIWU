using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Sword
{
	// Token: 0x02000389 RID: 905
	public class QingZhuJianFa : CombatSkillEffectBase
	{
		// Token: 0x06003621 RID: 13857 RVA: 0x0022F5E9 File Offset: 0x0022D7E9
		public QingZhuJianFa()
		{
		}

		// Token: 0x06003622 RID: 13858 RVA: 0x0022F5F3 File Offset: 0x0022D7F3
		public QingZhuJianFa(CombatSkillKey skillKey) : base(skillKey, 12306, -1)
		{
		}

		// Token: 0x06003623 RID: 13859 RVA: 0x0022F604 File Offset: 0x0022D804
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x0022F61C File Offset: 0x0022D81C
		public override void OnDisable(DataContext context)
		{
			bool flag = this._affectSkillList != null;
			if (flag)
			{
				ObjectPool<List<short>>.Instance.Return(this._affectSkillList);
			}
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x0022F65C File Offset: 0x0022D85C
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.IsSrcSkillPerformed;
			if (flag)
			{
				bool flag2 = charId == base.CharacterId && skillId == base.SkillTemplateId;
				if (flag2)
				{
					bool flag3 = base.PowerMatchAffectRequire((int)power, 0);
					if (flag3)
					{
						List<short> skillRandomPool = ObjectPool<List<short>>.Instance.Get();
						List<short> attackSkillList = base.CurrEnemyChar.GetAttackSkillList();
						this._affectEnemyId = base.CurrEnemyChar.GetId();
						skillRandomPool.Clear();
						for (int i = 0; i < attackSkillList.Count; i++)
						{
							short enemySkillId = attackSkillList[i];
							bool flag4 = enemySkillId >= 0;
							if (flag4)
							{
								CombatSkillKey skillKey = new CombatSkillKey(this._affectEnemyId, enemySkillId);
								sbyte innerRatio = DomainManager.CombatSkill.GetElement_CombatSkills(skillKey).GetCurrInnerRatio();
								bool flag5 = base.IsDirect ? (innerRatio < 50) : (innerRatio > 50);
								if (flag5)
								{
									skillRandomPool.Add(enemySkillId);
								}
							}
						}
						bool flag6 = skillRandomPool.Count > 0;
						if (flag6)
						{
							this.IsSrcSkillPerformed = true;
							this._affectSkillList = ObjectPool<List<short>>.Instance.Get();
							this._affectSkillList.Clear();
							bool flag7 = skillRandomPool.Count <= 3;
							if (flag7)
							{
								this._affectSkillList.AddRange(skillRandomPool);
							}
							else
							{
								for (int j = 0; j < 3; j++)
								{
									int index = context.Random.Next(0, skillRandomPool.Count);
									this._affectSkillList.Add(skillRandomPool[index]);
									skillRandomPool.RemoveAt(index);
								}
							}
							base.AppendAffectedCurrEnemyData(context, 199, EDataModifyType.TotalPercent, -1);
							base.ShowSpecialEffectTips(0);
						}
						else
						{
							base.RemoveSelf(context);
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
				bool flag8 = charId == this._affectEnemyId && this._affectSkillList.Contains(skillId) && !interrupted;
				if (flag8)
				{
					this._affectSkillList.Remove(skillId);
					bool flag9 = this._affectSkillList.Count == 0;
					if (flag9)
					{
						base.RemoveSelf(context);
					}
					else
					{
						DomainManager.SpecialEffect.InvalidateCache(context, this._affectEnemyId, 199);
					}
				}
				else
				{
					bool flag10 = charId == base.CharacterId && skillId == base.SkillTemplateId && base.PowerMatchAffectRequire((int)power, 0);
					if (flag10)
					{
						base.RemoveSelf(context);
					}
				}
			}
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x0022F8E0 File Offset: 0x0022DAE0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId == 199 && dataKey.CharId == this._affectEnemyId && this._affectSkillList.Contains(dataKey.CombatSkillId);
			int result;
			if (flag)
			{
				result = -50;
			}
			else
			{
				result = 0;
			}
			return result;
		}

		// Token: 0x04000FC5 RID: 4037
		private const sbyte AffectSkillCount = 3;

		// Token: 0x04000FC6 RID: 4038
		private int _affectEnemyId;

		// Token: 0x04000FC7 RID: 4039
		private List<short> _affectSkillList;
	}
}
