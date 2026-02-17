using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.XueFeng
{
	// Token: 0x020002D3 RID: 723
	public class XueYuYuBaHuang : CombatSkillEffectBase
	{
		// Token: 0x060032AF RID: 12975 RVA: 0x00220524 File Offset: 0x0021E724
		public XueYuYuBaHuang()
		{
		}

		// Token: 0x060032B0 RID: 12976 RVA: 0x0022052E File Offset: 0x0021E72E
		public XueYuYuBaHuang(CombatSkillKey skillKey) : base(skillKey, 17075, -1)
		{
		}

		// Token: 0x060032B1 RID: 12977 RVA: 0x00220540 File Offset: 0x0021E740
		public override void OnEnable(DataContext context)
		{
			this._addPower = 10 * base.CombatChar.GetOldInjuries().GetSum();
			bool flag = this._addPower > 0;
			if (flag)
			{
				base.CreateAffectedData(199, EDataModifyType.AddPercent, base.SkillTemplateId);
				base.ShowSpecialEffectTips(0);
			}
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060032B2 RID: 12978 RVA: 0x002205A6 File Offset: 0x0021E7A6
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x060032B3 RID: 12979 RVA: 0x002205BC File Offset: 0x0021E7BC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0);
				if (flag2)
				{
					List<ValueTuple<sbyte, bool>> injuryRandomPool = ObjectPool<List<ValueTuple<sbyte, bool>>>.Instance.Get();
					List<sbyte> bodyPartRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
					Injuries oldInjuries = base.CombatChar.GetOldInjuries();
					injuryRandomPool.Clear();
					for (sbyte part = 0; part < 7; part += 1)
					{
						ValueTuple<sbyte, sbyte> injury = oldInjuries.Get(part);
						for (int i = 0; i < (int)injury.Item1; i++)
						{
							injuryRandomPool.Add(new ValueTuple<sbyte, bool>(part, false));
						}
						for (int j = 0; j < (int)injury.Item2; j++)
						{
							injuryRandomPool.Add(new ValueTuple<sbyte, bool>(part, true));
						}
					}
					while (injuryRandomPool.Count > 8)
					{
						injuryRandomPool.RemoveAt(context.Random.Next(injuryRandomPool.Count));
					}
					bool flag3 = injuryRandomPool.Count > 0;
					if (flag3)
					{
						int[] enemyCharIds = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
						List<CombatCharacter> enemyCharList = new List<CombatCharacter>();
						CombatCharacter enemyCurrChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
						for (int k = 0; k < enemyCharIds.Length; k++)
						{
							bool flag4 = enemyCharIds[k] >= 0;
							if (flag4)
							{
								enemyCharList.Add(DomainManager.Combat.GetElement_CombatCharacterDict(enemyCharIds[k]));
							}
						}
						for (int l = 0; l < injuryRandomPool.Count; l++)
						{
							ValueTuple<sbyte, bool> injuryInfo = injuryRandomPool[l];
							int charIndex = context.Random.Next(enemyCharList.Count);
							CombatCharacter enemyChar = enemyCharList[charIndex];
							Injuries enemyInjuries = enemyChar.GetInjuries();
							bodyPartRandomPool.Clear();
							for (sbyte part2 = 0; part2 < 7; part2 += 1)
							{
								bool flag5 = enemyInjuries.Get(part2, injuryInfo.Item2) < 6;
								if (flag5)
								{
									bodyPartRandomPool.Add(part2);
								}
							}
							bool flag6 = bodyPartRandomPool.Count > 0;
							if (flag6)
							{
								sbyte bodyPart = bodyPartRandomPool[context.Random.Next(bodyPartRandomPool.Count)];
								DomainManager.Combat.RemoveInjury(context, base.CombatChar, injuryInfo.Item1, injuryInfo.Item2, 1, false, true);
								DomainManager.Combat.AddInjury(context, enemyChar, bodyPart, injuryInfo.Item2, 1, true, true);
							}
							else
							{
								enemyCharList.RemoveAt(charIndex);
								bool flag7 = enemyCharList.Count > 0;
								if (!flag7)
								{
									break;
								}
								l--;
							}
						}
						DomainManager.Combat.UpdateBodyDefeatMark(context, base.CombatChar);
						DomainManager.Combat.AddToCheckFallenSet(enemyCurrChar.GetId());
						bool flag8 = enemyCurrChar != base.CurrEnemyChar;
						if (flag8)
						{
							DomainManager.Combat.AddToCheckFallenSet(base.CurrEnemyChar.GetId());
						}
						base.ShowSpecialEffectTips(1);
					}
					ObjectPool<List<ValueTuple<sbyte, bool>>>.Instance.Return(injuryRandomPool);
					ObjectPool<List<sbyte>>.Instance.Return(bodyPartRandomPool);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x060032B4 RID: 12980 RVA: 0x002208FC File Offset: 0x0021EAFC
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

		// Token: 0x04000EFF RID: 3839
		private const sbyte AddPowerUnit = 10;

		// Token: 0x04000F00 RID: 3840
		private const sbyte TransferInjuryCount = 8;

		// Token: 0x04000F01 RID: 3841
		private int _addPower;
	}
}
