using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x0200058B RID: 1419
	public class AddPowerAndNeiliAllocationByMoving : CombatSkillEffectBase
	{
		// Token: 0x17000287 RID: 647
		// (get) Token: 0x060041FF RID: 16895 RVA: 0x00264F32 File Offset: 0x00263132
		protected static sbyte MoveDistInPrepare
		{
			get
			{
				return 40;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06004200 RID: 16896 RVA: 0x00264F36 File Offset: 0x00263136
		protected static sbyte AffectDistanceUnit
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06004201 RID: 16897 RVA: 0x00264F39 File Offset: 0x00263139
		protected virtual sbyte AddPowerUnit
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06004202 RID: 16898 RVA: 0x00264F3C File Offset: 0x0026313C
		protected virtual int MoveCostMobilityAddPercent
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06004203 RID: 16899 RVA: 0x00264F3F File Offset: 0x0026313F
		protected virtual sbyte AddNeiliAllocationUnit
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06004204 RID: 16900 RVA: 0x00264F42 File Offset: 0x00263142
		protected AddPowerAndNeiliAllocationByMoving()
		{
		}

		// Token: 0x06004205 RID: 16901 RVA: 0x00264F4C File Offset: 0x0026314C
		protected AddPowerAndNeiliAllocationByMoving(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06004206 RID: 16902 RVA: 0x00264F5C File Offset: 0x0026315C
		public override void OnEnable(DataContext context)
		{
			DomainManager.Combat.AddMoveDistInSkillPrepare(base.CombatChar, (short)AddPowerAndNeiliAllocationByMoving.MoveDistInPrepare, base.IsDirect);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 175, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004207 RID: 16903 RVA: 0x00264FFC File Offset: 0x002631FC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06004208 RID: 16904 RVA: 0x00265024 File Offset: 0x00263224
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover.GetId() != base.CharacterId || !isMove;
			if (!flag)
			{
				int prevUnit = (int)(Math.Abs(this._movedDist) / (short)AddPowerAndNeiliAllocationByMoving.AffectDistanceUnit);
				this._movedDist += distance;
				int currUnit = (int)(Math.Abs(this._movedDist) / (short)AddPowerAndNeiliAllocationByMoving.AffectDistanceUnit);
				bool flag2 = prevUnit == currUnit;
				if (!flag2)
				{
					this._addPower = (short)(currUnit * (int)this.AddPowerUnit);
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
					base.ShowSpecialEffectTips(0);
					int unit = currUnit - prevUnit;
					for (int i = 0; i < unit; i++)
					{
						this.OnDistanceChangedAddNeiliAllocation(context);
					}
				}
			}
		}

		// Token: 0x06004209 RID: 16905 RVA: 0x002650E0 File Offset: 0x002632E0
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x0600420A RID: 16906 RVA: 0x00265115 File Offset: 0x00263315
		protected virtual void OnDistanceChangedAddNeiliAllocation(DataContext context)
		{
			base.CombatChar.ChangeNeiliAllocation(context, this.AddNeiliAllocationType, (int)this.AddNeiliAllocationUnit, true, true);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x0600420B RID: 16907 RVA: 0x0026513C File Offset: 0x0026333C
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
				bool flag2 = dataKey.FieldId == 175;
				if (flag2)
				{
					result = this.MoveCostMobilityAddPercent;
				}
				else
				{
					bool flag3 = dataKey.CombatSkillId != base.SkillTemplateId;
					if (flag3)
					{
						result = 0;
					}
					else
					{
						bool flag4 = dataKey.FieldId == 199;
						if (flag4)
						{
							result = (int)this._addPower;
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

		// Token: 0x0400137A RID: 4986
		protected byte AddNeiliAllocationType;

		// Token: 0x0400137B RID: 4987
		private short _movedDist;

		// Token: 0x0400137C RID: 4988
		private short _addPower;
	}
}
