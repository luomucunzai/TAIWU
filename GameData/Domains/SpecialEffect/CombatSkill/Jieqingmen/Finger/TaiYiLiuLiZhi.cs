using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger
{
	// Token: 0x020004F5 RID: 1269
	public class TaiYiLiuLiZhi : CombatSkillEffectBase
	{
		// Token: 0x06003E43 RID: 15939 RVA: 0x0025529C File Offset: 0x0025349C
		public TaiYiLiuLiZhi()
		{
		}

		// Token: 0x06003E44 RID: 15940 RVA: 0x002552A6 File Offset: 0x002534A6
		public TaiYiLiuLiZhi(CombatSkillKey skillKey) : base(skillKey, 13108, -1)
		{
		}

		// Token: 0x06003E45 RID: 15941 RVA: 0x002552B8 File Offset: 0x002534B8
		public override void OnEnable(DataContext context)
		{
			Character enemyChar = base.CurrEnemyChar.GetCharacter();
			int enemyCharId = enemyChar.GetId();
			OuterAndInnerInts selfResists = this.CharObj.GetPenetrationResists();
			OuterAndInnerInts enemyResists = enemyChar.GetPenetrationResists();
			this._selfTransferValue = (base.IsDirect ? selfResists.Outer : selfResists.Inner) / 2;
			this._enemyTransferValue = (base.IsDirect ? enemyResists.Outer : enemyResists.Inner) / 2;
			base.CreateAffectedData(114, EDataModifyType.Custom, -1);
			base.CreateAffectedData(base.IsDirect ? 44 : 45, EDataModifyType.Add, -1);
			base.CreateAffectedData(base.IsDirect ? 46 : 47, EDataModifyType.Add, -1);
			base.CreateAffectedData(enemyCharId, base.IsDirect ? 44 : 45, EDataModifyType.Add, -1);
			base.CreateAffectedData(enemyCharId, base.IsDirect ? 46 : 47, EDataModifyType.Add, -1);
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003E46 RID: 15942 RVA: 0x002553AB File Offset: 0x002535AB
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06003E47 RID: 15943 RVA: 0x002553C0 File Offset: 0x002535C0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003E48 RID: 15944 RVA: 0x002553F8 File Offset: 0x002535F8
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			long result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 114;
				if (flag2)
				{
					bool inner = dataKey.CustomParam1 == 1;
					bool flag3 = inner == base.IsDirect;
					if (flag3)
					{
						result = dataValue;
					}
					else
					{
						sbyte bodyPart = (sbyte)dataKey.CustomParam2;
						result = Math.Min(dataValue, (long)base.CombatChar.MarkCountChangeToDamageValue(bodyPart, inner, 2));
					}
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x06003E49 RID: 15945 RVA: 0x00255474 File Offset: 0x00253674
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			ushort fieldId = dataKey.FieldId;
			bool flag = fieldId - 44 <= 1;
			bool flag2 = flag;
			int result;
			if (flag2)
			{
				result = ((dataKey.CharId == base.CharacterId) ? this._selfTransferValue : this._enemyTransferValue);
			}
			else
			{
				fieldId = dataKey.FieldId;
				flag = (fieldId - 46 <= 1);
				bool flag3 = flag;
				if (flag3)
				{
					result = ((dataKey.CharId == base.CharacterId) ? (-this._selfTransferValue) : (-this._enemyTransferValue));
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400125F RID: 4703
		private const sbyte MaxInjuryMark = 2;

		// Token: 0x04001260 RID: 4704
		private int _selfTransferValue;

		// Token: 0x04001261 RID: 4705
		private int _enemyTransferValue;
	}
}
