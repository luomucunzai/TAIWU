using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Agile
{
	// Token: 0x020004A1 RID: 1185
	public class CaoShangFei : AgileSkillBase
	{
		// Token: 0x06003C7F RID: 15487 RVA: 0x0024DD18 File Offset: 0x0024BF18
		public CaoShangFei()
		{
		}

		// Token: 0x06003C80 RID: 15488 RVA: 0x0024DD22 File Offset: 0x0024BF22
		public CaoShangFei(CombatSkillKey skillKey) : base(skillKey, 10500)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x06003C81 RID: 15489 RVA: 0x0024DD3C File Offset: 0x0024BF3C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._addDistance = 0;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 165, -1, -1, -1, -1), EDataModifyType.Add);
			Events.RegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
		}

		// Token: 0x06003C82 RID: 15490 RVA: 0x0024DDAF File Offset: 0x0024BFAF
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_DistanceChanged(new Events.OnDistanceChanged(this.OnDistanceChanged));
			DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
		}

		// Token: 0x06003C83 RID: 15491 RVA: 0x0024DDE4 File Offset: 0x0024BFE4
		private void OnDistanceChanged(DataContext context, CombatCharacter mover, short distance, bool isMove, bool isForced)
		{
			bool flag = mover != base.CombatChar || !isMove || isForced || !(base.IsDirect ? (distance < 0) : (distance > 0)) || this._addDistance >= 10 || !this._affecting || DomainManager.Combat.IsMovedByTeammate(base.CombatChar);
			if (!flag)
			{
				this._addDistance += 5;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 165);
			}
		}

		// Token: 0x06003C84 RID: 15492 RVA: 0x0024DE6C File Offset: 0x0024C06C
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 165);
				bool flag2 = canAffect;
				if (flag2)
				{
					DomainManager.Combat.EnableJumpMove(base.CombatChar, base.SkillTemplateId, base.IsDirect);
				}
				else
				{
					DomainManager.Combat.DisableJumpMove(context, base.CombatChar, base.SkillTemplateId);
				}
			}
		}

		// Token: 0x06003C85 RID: 15493 RVA: 0x0024DEEC File Offset: 0x0024C0EC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId || !this._affecting;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 165;
				if (flag2)
				{
					result = this._addDistance;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040011CE RID: 4558
		private const sbyte AddDistanceUnit = 5;

		// Token: 0x040011CF RID: 4559
		private const sbyte MaxAddDistance = 10;

		// Token: 0x040011D0 RID: 4560
		private int _addDistance;

		// Token: 0x040011D1 RID: 4561
		private bool _affecting;
	}
}
