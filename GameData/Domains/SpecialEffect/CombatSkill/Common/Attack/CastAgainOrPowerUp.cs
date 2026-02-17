using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Common.Attack
{
	// Token: 0x02000594 RID: 1428
	public class CastAgainOrPowerUp : CombatSkillEffectBase
	{
		// Token: 0x0600425A RID: 16986 RVA: 0x00266733 File Offset: 0x00264933
		protected CastAgainOrPowerUp()
		{
		}

		// Token: 0x0600425B RID: 16987 RVA: 0x0026673D File Offset: 0x0026493D
		protected CastAgainOrPowerUp(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600425C RID: 16988 RVA: 0x0026674C File Offset: 0x0026494C
		public override void OnEnable(DataContext context)
		{
			this._addPower = 0;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, base.SkillTemplateId, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 209, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600425D RID: 16989 RVA: 0x002667DC File Offset: 0x002649DC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600425E RID: 16990 RVA: 0x00266804 File Offset: 0x00264A04
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					bool flag2 = this._addPower > 0;
					if (flag2)
					{
						DomainManager.Combat.ChangeSkillPrepareProgress(base.CombatChar, base.CombatChar.SkillPrepareTotalProgress * 50 / 100);
					}
				}
				else
				{
					int trickCount = base.CombatChar.ReplaceUsableTrick(context, this.RequireTrickType, -1);
					bool flag3 = trickCount > 0;
					if (flag3)
					{
						this._addPower = trickCount * 10;
						DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
						base.ShowSpecialEffectTips(0);
					}
				}
			}
		}

		// Token: 0x0600425F RID: 16991 RVA: 0x002668BC File Offset: 0x00264ABC
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				bool flag2 = base.IsDirect && base.PowerMatchAffectRequire((int)power, 0) && base.CombatChar.GetTrickCount(this.RequireTrickType, true) >= 2 && DomainManager.Combat.CanCastSkill(base.CombatChar, base.SkillTemplateId, true, false);
				if (flag2)
				{
					this._addPower += 10;
					List<NeedTrick> removeTricks = ObjectPool<List<NeedTrick>>.Instance.Get();
					removeTricks.Clear();
					removeTricks.Add(new NeedTrick(this.RequireTrickType, 2));
					DomainManager.Combat.RemoveTrick(context, base.CombatChar, removeTricks, true, false, -1);
					DomainManager.Combat.CastSkillFree(context, base.CombatChar, base.SkillTemplateId, ECombatCastFreePriority.Normal);
					base.ShowSpecialEffectTips(0);
					ObjectPool<List<NeedTrick>>.Instance.Return(removeTricks);
				}
				else
				{
					base.ShowSpecialEffectTips(1);
					this._addPower = 0;
					base.IsDirect = !base.IsDirect;
					DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 209);
				}
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06004260 RID: 16992 RVA: 0x00266A00 File Offset: 0x00264C00
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
				bool flag2 = dataKey.FieldId == 209;
				if (flag2)
				{
					result = (base.IsDirect ? 0 : 1);
				}
				else
				{
					result = dataValue;
				}
			}
			return result;
		}

		// Token: 0x06004261 RID: 16993 RVA: 0x00266A60 File Offset: 0x00264C60
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
					result = this._addPower;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x0400139C RID: 5020
		private const byte DirectCostTrickCount = 2;

		// Token: 0x0400139D RID: 5021
		private const sbyte DirectAddPowerUnit = 10;

		// Token: 0x0400139E RID: 5022
		private const sbyte ReverseAddPowerUnit = 10;

		// Token: 0x0400139F RID: 5023
		private const sbyte PrepareProgressPercent = 50;

		// Token: 0x040013A0 RID: 5024
		protected sbyte RequireTrickType;

		// Token: 0x040013A1 RID: 5025
		private int _addPower;
	}
}
