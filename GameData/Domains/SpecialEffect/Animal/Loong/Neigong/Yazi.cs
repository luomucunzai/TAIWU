using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Neigong
{
	// Token: 0x02000605 RID: 1541
	public class Yazi : AnimalEffectBase
	{
		// Token: 0x0600452E RID: 17710 RVA: 0x00271B98 File Offset: 0x0026FD98
		public Yazi()
		{
		}

		// Token: 0x0600452F RID: 17711 RVA: 0x00271BA2 File Offset: 0x0026FDA2
		public Yazi(CombatSkillKey skillKey) : base(skillKey)
		{
		}

		// Token: 0x06004530 RID: 17712 RVA: 0x00271BAD File Offset: 0x0026FDAD
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(282, EDataModifyType.Custom, -1);
			base.CreateAffectedData(89, EDataModifyType.Custom, -1);
			Events.RegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x00271BE3 File Offset: 0x0026FDE3
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectDamageValue(new Events.OnAddDirectDamageValue(this.OnAddDirectDamageValue));
			base.OnDisable(context);
		}

		// Token: 0x06004532 RID: 17714 RVA: 0x00271C00 File Offset: 0x0026FE00
		private void OnAddDirectDamageValue(DataContext context, int attackerId, int defenderId, sbyte bodyPart, bool isInner, int damageValue, short combatSkillId)
		{
			bool flag = defenderId != base.CharacterId || damageValue <= 0;
			if (!flag)
			{
				if (isInner)
				{
					this._addingDamageValue.Inner = this._addingDamageValue.Inner + damageValue;
				}
				else
				{
					this._addingDamageValue.Outer = this._addingDamageValue.Outer + damageValue;
				}
				base.ShowSpecialEffectTipsOnceInFrame(0);
				this.UpdateAffecting();
			}
		}

		// Token: 0x06004533 RID: 17715 RVA: 0x00271C64 File Offset: 0x0026FE64
		private void UpdateAffecting()
		{
			bool affecting = this._addingDamageValue.IsNonZero;
			bool flag = affecting == this._affecting;
			if (!flag)
			{
				this._affecting = affecting;
				bool flag2 = !this._affecting;
				if (flag2)
				{
					DomainManager.Combat.AddToCheckFallenSet(base.CharacterId);
				}
			}
		}

		// Token: 0x06004534 RID: 17716 RVA: 0x00271CB4 File Offset: 0x0026FEB4
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
					this.UpdateAffecting();
				}
				result = dataValue;
			}
			else
			{
				result = base.GetModifiedValue(dataKey, dataValue);
			}
			return result;
		}

		// Token: 0x06004535 RID: 17717 RVA: 0x00271D38 File Offset: 0x0026FF38
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 282;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				result = (dataValue || this._affecting);
			}
			return result;
		}

		// Token: 0x04001472 RID: 5234
		private OuterAndInnerInts _addingDamageValue;

		// Token: 0x04001473 RID: 5235
		private bool _affecting;
	}
}
