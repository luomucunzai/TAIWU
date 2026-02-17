using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.LegendaryBook.Neigong
{
	// Token: 0x02000155 RID: 341
	public class YongJi : CombatSkillEffectBase
	{
		// Token: 0x17000120 RID: 288
		// (get) Token: 0x06002AEF RID: 10991 RVA: 0x00204533 File Offset: 0x00202733
		private sbyte SelfType
		{
			get
			{
				return Config.CombatSkill.Instance[base.SkillTemplateId].FiveElements;
			}
		}

		// Token: 0x06002AF0 RID: 10992 RVA: 0x0020454A File Offset: 0x0020274A
		public YongJi()
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002AF1 RID: 10993 RVA: 0x0020455B File Offset: 0x0020275B
		public YongJi(CombatSkillKey skillKey) : base(skillKey, 40002, -1)
		{
			this.IsLegendaryBookEffect = true;
		}

		// Token: 0x06002AF2 RID: 10994 RVA: 0x00204573 File Offset: 0x00202773
		public override void OnEnable(DataContext context)
		{
			this._equipSkillUid = base.ParseCharDataUid(117);
			GameDataBridge.AddPostDataModificationHandler(this._equipSkillUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDataChanged));
			base.CreateAffectedData(199, EDataModifyType.Add, -1);
		}

		// Token: 0x06002AF3 RID: 10995 RVA: 0x002045B0 File Offset: 0x002027B0
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._equipSkillUid, base.DataHandlerKey);
		}

		// Token: 0x06002AF4 RID: 10996 RVA: 0x002045C5 File Offset: 0x002027C5
		private void OnDataChanged(DataContext context, DataUid dataUid)
		{
			DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
		}

		// Token: 0x06002AF5 RID: 10997 RVA: 0x002045DF File Offset: 0x002027DF
		private bool IsProduce(sbyte fiveElements)
		{
			return this.SelfType == FiveElementsType.Producing[(int)fiveElements] || this.SelfType == FiveElementsType.Produced[(int)fiveElements];
		}

		// Token: 0x06002AF6 RID: 10998 RVA: 0x00204604 File Offset: 0x00202804
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId == base.SkillTemplateId || dataKey.FieldId != 199;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = !this.CharObj.IsCombatSkillEquipped(base.SkillTemplateId) || !this.CharObj.GetCombatSkillCanAffect(base.SkillTemplateId);
				if (flag2)
				{
					result = 0;
				}
				else
				{
					sbyte otherSkillType = Config.CombatSkill.Instance[dataKey.CombatSkillId].FiveElements;
					bool flag3 = this.SelfType == otherSkillType;
					if (flag3)
					{
						result = 10;
					}
					else
					{
						bool flag4 = this.SelfType == 5 || otherSkillType == 5;
						if (flag4)
						{
							result = 0;
						}
						else
						{
							result = (this.IsProduce(otherSkillType) ? 5 : -5);
						}
					}
				}
			}
			return result;
		}

		// Token: 0x04000D2A RID: 3370
		private const sbyte SameAddPower = 10;

		// Token: 0x04000D2B RID: 3371
		private const sbyte ProduceAddPower = 5;

		// Token: 0x04000D2C RID: 3372
		private const sbyte CounterReducePower = -5;

		// Token: 0x04000D2D RID: 3373
		private DataUid _equipSkillUid;
	}
}
