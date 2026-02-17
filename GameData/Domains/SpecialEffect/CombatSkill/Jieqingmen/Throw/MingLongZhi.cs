using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw
{
	// Token: 0x020004DE RID: 1246
	public class MingLongZhi : CombatSkillEffectBase
	{
		// Token: 0x06003DB6 RID: 15798 RVA: 0x00252E82 File Offset: 0x00251082
		public MingLongZhi()
		{
		}

		// Token: 0x06003DB7 RID: 15799 RVA: 0x00252E8C File Offset: 0x0025108C
		public MingLongZhi(CombatSkillKey skillKey) : base(skillKey, 13306, -1)
		{
		}

		// Token: 0x06003DB8 RID: 15800 RVA: 0x00252EA0 File Offset: 0x002510A0
		public override void OnEnable(DataContext context)
		{
			CombatCharacter trickChar = base.IsDirect ? base.CombatChar : base.EnemyChar;
			this._addPower = 20 * trickChar.GetContinueTricksAtStart(19);
			bool flag = this._addPower > 0;
			if (flag)
			{
				base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DB9 RID: 15801 RVA: 0x00252F24 File Offset: 0x00251124
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastAttackSkillBegin(new Events.OnCastAttackSkillBegin(this.OnCastAttackSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003DBA RID: 15802 RVA: 0x00252F4C File Offset: 0x0025114C
		private void OnCastAttackSkillBegin(DataContext context, CombatCharacter attacker, CombatCharacter defender, short skillId)
		{
			bool flag = attacker.GetId() != base.CharacterId || skillId != base.SkillTemplateId || !DomainManager.Combat.InAttackRange(base.CombatChar);
			if (!flag)
			{
				CombatCharacter trickChar = base.IsDirect ? base.CombatChar : base.EnemyChar;
				int maxCostTrick = Math.Min(3, (int)trickChar.GetTrickCount(19));
				bool flag2 = maxCostTrick <= 0;
				if (!flag2)
				{
					List<int> preferIndexes = ObjectPool<List<int>>.Instance.Get();
					List<int> alterIndexes = ObjectPool<List<int>>.Instance.Get();
					preferIndexes.Clear();
					alterIndexes.Clear();
					TrickCollection tricks = trickChar.GetTricks();
					foreach (KeyValuePair<int, sbyte> keyValuePair in tricks.Tricks)
					{
						int num;
						sbyte b;
						keyValuePair.Deconstruct(out num, out b);
						int index = num;
						sbyte trick = b;
						bool flag3 = trick == 19;
						if (!flag3)
						{
							bool flag4 = trickChar.IsTrickUsable(trick);
							if (flag4)
							{
								(base.IsDirect ? alterIndexes : preferIndexes).Add(index);
							}
							else
							{
								(base.IsDirect ? preferIndexes : alterIndexes).Add(index);
							}
						}
					}
					int trulyCostTrick = Math.Min(maxCostTrick, preferIndexes.Count + alterIndexes.Count);
					for (int i = 0; i < trulyCostTrick; i++)
					{
						int index2 = (i < preferIndexes.Count) ? preferIndexes[i] : alterIndexes[i - preferIndexes.Count];
						tricks.RemoveTrick(index2);
					}
					ObjectPool<List<int>>.Instance.Return(preferIndexes);
					ObjectPool<List<int>>.Instance.Return(alterIndexes);
					bool flag5 = trulyCostTrick <= 0;
					if (!flag5)
					{
						trickChar.SetTricks(tricks, context);
						this._addDamagePercent = 20 * trulyCostTrick;
						base.AppendAffectedData(context, base.CharacterId, 69, EDataModifyType.AddPercent, base.SkillTemplateId);
						base.ShowSpecialEffectTips(1);
					}
				}
			}
		}

		// Token: 0x06003DBB RID: 15803 RVA: 0x00253150 File Offset: 0x00251350
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003DBC RID: 15804 RVA: 0x00253188 File Offset: 0x00251388
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
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 69)
				{
					if (fieldId != 199)
					{
						num = 0;
					}
					else
					{
						num = this._addPower;
					}
				}
				else
				{
					num = this._addDamagePercent;
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x0400122E RID: 4654
		private const sbyte AddPowerUnit = 20;

		// Token: 0x0400122F RID: 4655
		private const int MaxCostTrick = 3;

		// Token: 0x04001230 RID: 4656
		private const sbyte AddDamagePercentUnit = 20;

		// Token: 0x04001231 RID: 4657
		private int _addPower;

		// Token: 0x04001232 RID: 4658
		private int _addDamagePercent;
	}
}
