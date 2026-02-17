using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Agile
{
	// Token: 0x020003E1 RID: 993
	public class ShangTianTi : AgileSkillBase
	{
		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060037FC RID: 14332 RVA: 0x002382C3 File Offset: 0x002364C3
		private static int MaxAddPower
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x060037FD RID: 14333 RVA: 0x002382C7 File Offset: 0x002364C7
		public ShangTianTi()
		{
		}

		// Token: 0x060037FE RID: 14334 RVA: 0x002382D1 File Offset: 0x002364D1
		public ShangTianTi(CombatSkillKey skillKey) : base(skillKey, 4405)
		{
		}

		// Token: 0x060037FF RID: 14335 RVA: 0x002382E4 File Offset: 0x002364E4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._distanceAccumulator = 0;
			this._addPower = 0;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003800 RID: 14336 RVA: 0x00238346 File Offset: 0x00236546
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
		}

		// Token: 0x06003801 RID: 14337 RVA: 0x00238364 File Offset: 0x00236564
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || distance == 0;
			if (!flag)
			{
				bool rightDirection = base.IsDirect ? (distance < 0) : (distance > 0);
				bool flag2 = rightDirection;
				if (flag2)
				{
					this._distanceAccumulator += (int)Math.Abs(distance);
					bool powerChanged = false;
					while (this._distanceAccumulator >= 5)
					{
						this._distanceAccumulator -= 5;
						bool flag3 = this._addPower >= ShangTianTi.MaxAddPower || !base.CanAffect;
						if (!flag3)
						{
							this._addPower += 10;
							powerChanged = true;
						}
					}
					bool flag4 = powerChanged;
					if (flag4)
					{
						DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
						base.ShowSpecialEffectTips(0);
					}
				}
				else
				{
					this._distanceAccumulator = 0;
					this._addPower = 0;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
				}
			}
		}

		// Token: 0x06003802 RID: 14338 RVA: 0x00238474 File Offset: 0x00236674
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || Config.CombatSkill.Instance[dataKey.CombatSkillId].EquipType != 1;
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

		// Token: 0x0400105A RID: 4186
		private const sbyte RequireMoveDistance = 5;

		// Token: 0x0400105B RID: 4187
		private const int MaxUnit = 4;

		// Token: 0x0400105C RID: 4188
		private const sbyte AddPowerUnit = 10;

		// Token: 0x0400105D RID: 4189
		private int _distanceAccumulator;

		// Token: 0x0400105E RID: 4190
		private int _addPower;
	}
}
