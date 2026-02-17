using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.PestleEffect
{
	// Token: 0x020004AE RID: 1198
	public class BuDongMingWangChu : PestleEffectBase
	{
		// Token: 0x06003CBA RID: 15546 RVA: 0x0024EB8C File Offset: 0x0024CD8C
		public BuDongMingWangChu()
		{
		}

		// Token: 0x06003CBB RID: 15547 RVA: 0x0024EB96 File Offset: 0x0024CD96
		public BuDongMingWangChu(int charId) : base(charId, 11405)
		{
		}

		// Token: 0x06003CBC RID: 15548 RVA: 0x0024EBA8 File Offset: 0x0024CDA8
		public override void OnEnable(DataContext context)
		{
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 145, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 146, -1, -1, -1, -1), EDataModifyType.Add);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 9, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			base.OnEnable(context);
		}

		// Token: 0x06003CBD RID: 15549 RVA: 0x0024EC59 File Offset: 0x0024CE59
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_ChangeWeapon(new Events.OnChangeWeapon(this.OnChangeWeapon));
			base.OnDisable(context);
		}

		// Token: 0x06003CBE RID: 15550 RVA: 0x0024EC78 File Offset: 0x0024CE78
		private void OnChangeWeapon(DataContext context, int charId, bool isAlly, CombatWeaponData newWeapon, CombatWeaponData oldWeapon)
		{
			bool flag = charId == base.CharacterId;
			if (flag)
			{
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 145);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 146);
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 9);
			}
		}

		// Token: 0x06003CBF RID: 15551 RVA: 0x0024ECD8 File Offset: 0x0024CED8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				bool flag2 = fieldId - 145 <= 1;
				bool flag3 = flag2;
				if (flag3)
				{
					result = 15;
				}
				else
				{
					bool flag4 = dataKey.FieldId == 9;
					if (flag4)
					{
						result = (base.IsDirect ? -80 : 80);
					}
					else
					{
						bool flag5 = dataKey.FieldId == 102;
						if (flag5)
						{
							result = (base.IsDirect ? -40 : 40);
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

		// Token: 0x040011DF RID: 4575
		private const int ChangeAttackRange = 15;

		// Token: 0x040011E0 RID: 4576
		private const sbyte ChangeMoveSpeed = 80;

		// Token: 0x040011E1 RID: 4577
		private const sbyte ChangeDamage = 40;
	}
}
