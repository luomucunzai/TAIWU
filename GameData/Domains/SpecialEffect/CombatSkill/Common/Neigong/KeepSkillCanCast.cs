using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x02000576 RID: 1398
	public class KeepSkillCanCast : CombatSkillEffectBase
	{
		// Token: 0x1700026C RID: 620
		// (get) Token: 0x0600414F RID: 16719 RVA: 0x002624DE File Offset: 0x002606DE
		private sbyte CurrNeiliFiveElementsType
		{
			get
			{
				return (sbyte)NeiliType.Instance[this.CharObj.GetNeiliType()].FiveElements;
			}
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x002624FB File Offset: 0x002606FB
		protected KeepSkillCanCast()
		{
		}

		// Token: 0x06004151 RID: 16721 RVA: 0x00262505 File Offset: 0x00260705
		protected KeepSkillCanCast(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x00262514 File Offset: 0x00260714
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.CreateAffectedData(216, EDataModifyType.Add, -1);
				base.CreateAffectedData(218, EDataModifyType.TotalPercent, -1);
				base.CreateAffectedData(191, EDataModifyType.Custom, -1);
			}
			else
			{
				base.CreateAffectedData(219, EDataModifyType.Custom, -1);
				base.CreateAffectedData(199, EDataModifyType.AddPercent, -1);
			}
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
				Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			}
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x002625B8 File Offset: 0x002607B8
		public override void OnDisable(DataContext context)
		{
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
				Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			}
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x002625F9 File Offset: 0x002607F9
		private bool MatchRelatedFiveElementsType(short skillId)
		{
			return this.MatchRelatedFiveElementsType(base.EnemyChar.GetId(), skillId);
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x00262610 File Offset: 0x00260810
		private bool MatchRelatedFiveElementsType(int charId, short skillId)
		{
			bool flag = this.RequireFiveElementsType == 5;
			bool result;
			if (flag)
			{
				result = base.FiveElementsEquals(charId, skillId, this.RequireFiveElementsType);
			}
			else
			{
				result = (base.FiveElementsEquals(charId, skillId, this.RequireFiveElementsType) || base.FiveElementsEquals(charId, skillId, FiveElementsType.Countering[(int)this.RequireFiveElementsType]) || base.FiveElementsEquals(charId, skillId, FiveElementsType.Produced[(int)this.RequireFiveElementsType]));
			}
			return result;
		}

		// Token: 0x06004156 RID: 16726 RVA: 0x00262680 File Offset: 0x00260880
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = base.CharacterId != charId;
			if (!flag)
			{
				bool flag2 = this.CurrNeiliFiveElementsType != this.RequireFiveElementsType;
				if (!flag2)
				{
					bool flag3 = !DomainManager.Combat.HasSkillNeedBodyPart(base.CombatChar, skillId, false);
					if (flag3)
					{
						this._affectingPowerSkillId = skillId;
						DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x002626FC File Offset: 0x002608FC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupt)
		{
			bool flag = DomainManager.Combat.GetCombatCharacter(!isAlly, true) == base.CombatChar && !interrupt;
			if (flag)
			{
				this.DoAffect(context, charId, skillId, power);
			}
			bool flag2 = base.CharacterId != charId;
			if (!flag2)
			{
				this._affectingPowerSkillId = -1;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x0026276C File Offset: 0x0026096C
		private void DoAffect(DataContext context, int charId, short skillId, sbyte power)
		{
			bool flag = !this.MatchRelatedFiveElementsType(charId, skillId) || base.PowerMatchAffectRequire((int)power, 0);
			if (!flag)
			{
				bool flag2 = !CombatSkillTemplateHelper.IsAttack(skillId);
				if (!flag2)
				{
					CombatCharacter attacker = DomainManager.Combat.GetElement_CombatCharacterDict(charId);
					DomainManager.Combat.AddGoneMadInjury(context, attacker, skillId, -50);
					base.ShowSpecialEffectTips(1);
				}
			}
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x002627CC File Offset: 0x002609CC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = this.CurrNeiliFiveElementsType != this.RequireFiveElementsType;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					ushort fieldId = dataKey.FieldId;
					bool flag3 = fieldId == 216 || fieldId == 218;
					bool flag4 = flag3;
					if (flag4)
					{
						result = -60;
					}
					else
					{
						bool flag5 = dataKey.FieldId == 199 && dataKey.CombatSkillId == this._affectingPowerSkillId;
						if (flag5)
						{
							result = -20;
						}
						else
						{
							result = 0;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x0026286C File Offset: 0x00260A6C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId < 0;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = !this.MatchRelatedFiveElementsType(dataKey.CombatSkillId);
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 191;
					if (flag3)
					{
						EDamageType damageType = (EDamageType)dataKey.CustomParam1;
						bool flag4 = damageType != EDamageType.Direct;
						if (flag4)
						{
							result = dataValue;
						}
						else
						{
							base.ShowSpecialEffectTips(0);
							result = dataValue - dataValue * 80 / 100;
						}
					}
					else
					{
						result = dataValue;
					}
				}
			}
			return result;
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x002628F8 File Offset: 0x00260AF8
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = this.CurrNeiliFiveElementsType != this.RequireFiveElementsType;
				if (flag2)
				{
					result = dataValue;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 219;
					result = (flag3 || dataValue);
				}
			}
			return result;
		}

		// Token: 0x0400133D RID: 4925
		private const sbyte ReduceInterruptSilenceOdds = -60;

		// Token: 0x0400133E RID: 4926
		private const sbyte BrokenCastReducePower = -20;

		// Token: 0x0400133F RID: 4927
		private const sbyte DirectReduceFatalDamagePercent = 80;

		// Token: 0x04001340 RID: 4928
		private const int AddGoneMadFactor = -50;

		// Token: 0x04001341 RID: 4929
		protected sbyte RequireFiveElementsType;

		// Token: 0x04001342 RID: 4930
		private short _affectingPowerSkillId;
	}
}
