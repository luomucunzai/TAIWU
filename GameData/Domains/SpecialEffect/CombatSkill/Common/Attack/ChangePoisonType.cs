using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000597 RID: 1431
	public class ChangePoisonType : CombatSkillEffectBase
	{
		// Token: 0x06004277 RID: 17015 RVA: 0x00267018 File Offset: 0x00265218
		protected ChangePoisonType()
		{
		}

		// Token: 0x06004278 RID: 17016 RVA: 0x00267022 File Offset: 0x00265222
		protected ChangePoisonType(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004279 RID: 17017 RVA: 0x00267030 File Offset: 0x00265230
		public unsafe override void OnEnable(DataContext context)
		{
			PoisonInts poisons = *(base.IsDirect ? base.CurrEnemyChar : base.CombatChar).GetPoison();
			List<sbyte> poisonTypeRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
			int minPoison = int.MaxValue;
			poisonTypeRandomPool.Clear();
			for (int i = 0; i < this.CanChangePoisonType.Length; i++)
			{
				sbyte type = this.CanChangePoisonType[i];
				bool flag = *(ref poisons.Items.FixedElementField + (IntPtr)type * 4) < minPoison;
				if (flag)
				{
					minPoison = *(ref poisons.Items.FixedElementField + (IntPtr)type * 4);
					poisonTypeRandomPool.Clear();
					poisonTypeRandomPool.Add(type);
				}
				else
				{
					bool flag2 = *(ref poisons.Items.FixedElementField + (IntPtr)type * 4) == minPoison;
					if (flag2)
					{
						poisonTypeRandomPool.Add(type);
					}
				}
			}
			this._targetPoisonType = (poisonTypeRandomPool.Contains(this.AddPowerPoisonType) ? this.AddPowerPoisonType : poisonTypeRandomPool[context.Random.Next(0, poisonTypeRandomPool.Count)]);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 81, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			bool flag3 = this._targetPoisonType == this.AddPowerPoisonType;
			if (flag3)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			}
			ObjectPool<List<sbyte>>.Instance.Return(poisonTypeRandomPool);
			base.ShowSpecialEffectTips(this._targetPoisonType == this.AddPowerPoisonType, 1, 0);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600427A RID: 17018 RVA: 0x002671DB File Offset: 0x002653DB
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600427B RID: 17019 RVA: 0x002671F0 File Offset: 0x002653F0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600427C RID: 17020 RVA: 0x00267228 File Offset: 0x00265428
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 81;
				if (flag2)
				{
					result = (int)this._targetPoisonType;
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x0600427D RID: 17021 RVA: 0x0026727C File Offset: 0x0026547C
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
					result = 40;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040013A4 RID: 5028
		private const sbyte AddPower = 40;

		// Token: 0x040013A5 RID: 5029
		protected sbyte[] CanChangePoisonType;

		// Token: 0x040013A6 RID: 5030
		protected sbyte AddPowerPoisonType;

		// Token: 0x040013A7 RID: 5031
		private sbyte _targetPoisonType;
	}
}
