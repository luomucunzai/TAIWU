using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong
{
	// Token: 0x0200056F RID: 1391
	public class AddFiveElementsDamage : CombatSkillEffectBase
	{
		// Token: 0x06004108 RID: 16648 RVA: 0x00261311 File Offset: 0x0025F511
		protected AddFiveElementsDamage()
		{
		}

		// Token: 0x06004109 RID: 16649 RVA: 0x0026131B File Offset: 0x0025F51B
		protected AddFiveElementsDamage(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600410A RID: 16650 RVA: 0x00261328 File Offset: 0x0025F528
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 69, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 117, -1, -1, -1, -1), EDataModifyType.AddPercent);
		}

		// Token: 0x0600410B RID: 16651 RVA: 0x00261380 File Offset: 0x0025F580
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
				bool flag2 = dataKey.CombatSkillId >= 0 && base.FiveElementsEquals(dataKey, this.RequireSelfFiveElementsType);
				if (flag2)
				{
					sbyte selfNeiliFiveElementsType = (sbyte)NeiliType.Instance[this.CharObj.GetNeiliType()].FiveElements;
					bool flag3 = dataKey.FieldId == 117 && selfNeiliFiveElementsType == this.AffectFiveElementsType;
					if (flag3)
					{
						return 100;
					}
					GameData.Domains.Character.Character enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, true).GetCharacter();
					sbyte enemyNeiliFiveElementsType = (sbyte)NeiliType.Instance[enemyChar.GetNeiliType()].FiveElements;
					bool flag4 = dataKey.FieldId == 69 && enemyNeiliFiveElementsType == this.AffectFiveElementsType;
					if (flag4)
					{
						return 30;
					}
				}
				result = 0;
			}
			return result;
		}

		// Token: 0x0600410C RID: 16652 RVA: 0x0026146C File Offset: 0x0025F66C
		protected override int GetSubClassSerializedSize()
		{
			return base.GetSubClassSerializedSize() + 1;
		}

		// Token: 0x0600410D RID: 16653 RVA: 0x00261488 File Offset: 0x0025F688
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.SerializeSubClass(pData);
			*pCurrData = (byte)this.RequireSelfFiveElementsType;
			pCurrData++;
			*pCurrData = (byte)this.AffectFiveElementsType;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x0600410E RID: 16654 RVA: 0x002614C0 File Offset: 0x0025F6C0
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			byte* pCurrData = pData + base.DeserializeSubClass(pData);
			this.RequireSelfFiveElementsType = *(sbyte*)pCurrData;
			pCurrData++;
			this.AffectFiveElementsType = *(sbyte*)pCurrData;
			return this.GetSubClassSerializedSize();
		}

		// Token: 0x04001320 RID: 4896
		private const sbyte DirectDamageAddPercent = 30;

		// Token: 0x04001321 RID: 4897
		private const sbyte AddGongMadInjury = 100;

		// Token: 0x04001322 RID: 4898
		protected sbyte RequireSelfFiveElementsType;

		// Token: 0x04001323 RID: 4899
		protected sbyte AffectFiveElementsType;
	}
}
