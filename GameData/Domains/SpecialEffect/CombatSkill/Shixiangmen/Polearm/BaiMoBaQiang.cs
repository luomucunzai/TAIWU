using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Polearm
{
	// Token: 0x020003E7 RID: 999
	public class BaiMoBaQiang : CombatSkillEffectBase
	{
		// Token: 0x06003821 RID: 14369 RVA: 0x00238E8E File Offset: 0x0023708E
		public BaiMoBaQiang()
		{
		}

		// Token: 0x06003822 RID: 14370 RVA: 0x00238E98 File Offset: 0x00237098
		public BaiMoBaQiang(CombatSkillKey skillKey) : base(skillKey, 6308, -1)
		{
		}

		// Token: 0x06003823 RID: 14371 RVA: 0x00238EAC File Offset: 0x002370AC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			bool targetIsAlly = base.IsDirect ? base.CombatChar.IsAlly : (!base.CombatChar.IsAlly);
			CombatCharacter mainChar = DomainManager.Combat.GetMainCharacter(targetIsAlly);
			foreach (CombatCharacter character in DomainManager.Combat.GetCharacters(targetIsAlly))
			{
				bool flag = mainChar == character;
				if (!flag)
				{
					this._castWithTeammate = true;
					this._addHits += character.GetCharacter().GetHitValues();
					this._addAttacks += character.GetCharacter().GetPenetrations();
				}
			}
			bool castWithTeammate = this._castWithTeammate;
			if (castWithTeammate)
			{
				base.ShowSpecialEffectTips(0);
				base.CreateAffectedData(44, EDataModifyType.Add, -1);
				base.CreateAffectedData(45, EDataModifyType.Add, -1);
				for (int i = 0; i < 4; i++)
				{
					base.CreateAffectedData((ushort)(32 + i), EDataModifyType.Add, -1);
				}
			}
			else
			{
				base.ShowSpecialEffectTips(2);
				base.CreateAffectedData(145, EDataModifyType.Add, -1);
				base.CreateAffectedData(146, EDataModifyType.Add, -1);
				base.CreateAffectedData(251, EDataModifyType.Custom, -1);
				base.CreateAffectedData(248, EDataModifyType.Custom, -1);
			}
		}

		// Token: 0x06003824 RID: 14372 RVA: 0x00239028 File Offset: 0x00237228
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06003825 RID: 14373 RVA: 0x00239048 File Offset: 0x00237248
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool _)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId);
			if (!flag)
			{
				bool flag2 = base.PowerMatchAffectRequire((int)power, 0) && this._castWithTeammate;
				if (flag2)
				{
					this.DoChangeTeammateCommandCd(context);
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x00239098 File Offset: 0x00237298
		private void DoChangeTeammateCommandCd(DataContext context)
		{
			bool flag = !DomainManager.Combat.IsMainCharacter(base.CombatChar);
			if (!flag)
			{
				bool targetIsAlly = base.IsDirect ? base.CombatChar.IsAlly : (!base.CombatChar.IsAlly);
				CombatCharacter mainChar = DomainManager.Combat.GetMainCharacter(targetIsAlly);
				List<ValueTuple<CombatCharacter, int>> pool = new List<ValueTuple<CombatCharacter, int>>();
				bool showTransferInjury = mainChar.GetShowTransferInjuryCommand();
				foreach (CombatCharacter character in DomainManager.Combat.GetCharacters(targetIsAlly))
				{
					bool flag2 = mainChar == character || base.CombatChar == character;
					if (!flag2)
					{
						List<sbyte> cmdTypes = character.GetCurrTeammateCommands();
						List<byte> cdPercent = character.GetTeammateCommandCdPercent();
						for (int i = 0; i < cmdTypes.Count; i++)
						{
							bool flag3 = cmdTypes[i] < 0 || (showTransferInjury && i > 0) || i == character.ExecutingTeammateCommandIndex;
							if (!flag3)
							{
								bool flag4 = base.IsDirect ? (cdPercent[i] == 0) : (cdPercent[i] > 0);
								if (!flag4)
								{
									pool.Add(new ValueTuple<CombatCharacter, int>(character, i));
								}
							}
						}
					}
				}
				foreach (ValueTuple<CombatCharacter, int> valueTuple in RandomUtils.GetRandomUnrepeated<ValueTuple<CombatCharacter, int>>(context.Random, int.MaxValue, pool, null))
				{
					CombatCharacter target = valueTuple.Item1;
					int index = valueTuple.Item2;
					base.ShowSpecialEffectTipsOnceInFrame(1);
					bool isDirect = base.IsDirect;
					if (isDirect)
					{
						target.ClearTeammateCommandCd(context, index);
					}
					else
					{
						target.ResetTeammateCommandCd(context, index, -1, false, false);
					}
				}
			}
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x00239288 File Offset: 0x00237488
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			ushort fieldId = dataKey.FieldId;
			if (!true)
			{
			}
			int result;
			if (fieldId <= 44)
			{
				switch (fieldId)
				{
				case 32:
					result = this._addHits[0];
					goto IL_AE;
				case 33:
					result = this._addHits[1];
					goto IL_AE;
				case 34:
					result = this._addHits[2];
					goto IL_AE;
				case 35:
					result = this._addHits[3];
					goto IL_AE;
				default:
					if (fieldId == 44)
					{
						result = this._addAttacks.Outer;
						goto IL_AE;
					}
					break;
				}
			}
			else
			{
				if (fieldId == 45)
				{
					result = this._addAttacks.Inner;
					goto IL_AE;
				}
				if (fieldId - 145 <= 1)
				{
					result = 20;
					goto IL_AE;
				}
			}
			result = base.GetModifyValue(dataKey, currModifyValue);
			IL_AE:
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x0023934C File Offset: 0x0023754C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.SkillKey == this.SkillKey;
			bool flag2 = flag;
			if (flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId == 248 || fieldId == 251;
				flag2 = flag3;
			}
			bool flag4 = flag2;
			return flag4 || base.GetModifiedValue(dataKey, dataValue);
		}

		// Token: 0x0400106A RID: 4202
		private const int AddAttackRange = 20;

		// Token: 0x0400106B RID: 4203
		private const int ChangeCdCount = 2147483647;

		// Token: 0x0400106C RID: 4204
		private bool _castWithTeammate;

		// Token: 0x0400106D RID: 4205
		private HitOrAvoidInts _addHits;

		// Token: 0x0400106E RID: 4206
		private OuterAndInnerInts _addAttacks;
	}
}
