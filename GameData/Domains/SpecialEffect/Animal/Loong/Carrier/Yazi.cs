using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier
{
	// Token: 0x02000619 RID: 1561
	public class Yazi : CarrierEffectBase
	{
		// Token: 0x06004593 RID: 17811 RVA: 0x00272D2E File Offset: 0x00270F2E
		public Yazi(int charId) : base(charId)
		{
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06004594 RID: 17812 RVA: 0x00272D39 File Offset: 0x00270F39
		protected override short CombatStateId
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06004595 RID: 17813 RVA: 0x00272D40 File Offset: 0x00270F40
		protected override void OnEnableSubClass(DataContext context)
		{
			base.OnEnableSubClass(context);
			base.CreateAffectedData(89, EDataModifyType.Custom, -1);
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
		}

		// Token: 0x06004596 RID: 17814 RVA: 0x00272D68 File Offset: 0x00270F68
		protected override void OnDisableSubClass(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			base.OnDisableSubClass(context);
		}

		// Token: 0x06004597 RID: 17815 RVA: 0x00272D88 File Offset: 0x00270F88
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = defenderId != base.CharacterId || damageValue <= 0;
			if (!flag)
			{
				if (isInner)
				{
					this._addingDamageValue.Inner = this._addingDamageValue.Inner + damageValue * Yazi.AddDamagePercent;
				}
				else
				{
					this._addingDamageValue.Outer = this._addingDamageValue.Outer + damageValue * Yazi.AddDamagePercent;
				}
			}
		}

		// Token: 0x06004598 RID: 17816 RVA: 0x00272DF0 File Offset: 0x00270FF0
		public override long GetModifiedValue(AffectedDataKey dataKey, long dataValue)
		{
			bool flag = dataKey.FieldId == 89 && dataKey.CharId == base.CharacterId;
			long result;
			if (flag)
			{
				ref int value = ref (dataKey.CustomParam1 == 1) ? ref this._addingDamageValue.Inner : ref this._addingDamageValue.Outer;
				bool flag2 = value > 0;
				if (flag2)
				{
					dataValue += (long)value;
					value = 0;
				}
				result = dataValue;
			}
			else
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			return result;
		}

		// Token: 0x04001491 RID: 5265
		private static readonly CValuePercent AddDamagePercent = 33;

		// Token: 0x04001492 RID: 5266
		private OuterAndInnerInts _addingDamageValue;
	}
}
