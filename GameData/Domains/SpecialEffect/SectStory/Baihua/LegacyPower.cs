using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.SpecialEffect.Animal;

namespace GameData.Domains.SpecialEffect.SectStory.Baihua
{
	// Token: 0x020000F7 RID: 247
	public abstract class LegacyPower : CarrierEffectBase
	{
		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x06002994 RID: 10644
		protected abstract sbyte OrgTemplateId { get; }

		// Token: 0x06002995 RID: 10645 RVA: 0x002012FB File Offset: 0x001FF4FB
		private bool IsOrgSkill(short skillId)
		{
			return CombatSkill.Instance[skillId].SectId == this.OrgTemplateId;
		}

		// Token: 0x06002996 RID: 10646 RVA: 0x00201315 File Offset: 0x001FF515
		protected LegacyPower(int charId) : base(charId)
		{
		}

		// Token: 0x06002997 RID: 10647 RVA: 0x00201320 File Offset: 0x001FF520
		protected override void OnEnableSubClass(DataContext context)
		{
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x00201335 File Offset: 0x001FF535
		protected override void OnDisableSubClass(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x0020134A File Offset: 0x001FF54A
		private void OnCombatBegin(DataContext context)
		{
			base.AppendAffectedData(context, 199, EDataModifyType.Add, -1);
			base.AppendAffectedAllEnemyData(context, 199, EDataModifyType.Add, -1);
		}

		// Token: 0x0600299A RID: 10650 RVA: 0x0020136C File Offset: 0x001FF56C
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.FieldId != 199 || !this.IsOrgSkill(dataKey.CombatSkillId);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = ((dataKey.CharId == base.CharacterId) ? 220 : -220);
			}
			return result;
		}

		// Token: 0x04000CD6 RID: 3286
		private const int AddPower = 220;
	}
}
