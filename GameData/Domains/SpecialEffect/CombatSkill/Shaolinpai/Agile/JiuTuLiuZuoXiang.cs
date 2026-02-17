using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Agile
{
	// Token: 0x0200043D RID: 1085
	public class JiuTuLiuZuoXiang : AgileSkillBase
	{
		// Token: 0x060039FC RID: 14844 RVA: 0x00241739 File Offset: 0x0023F939
		public JiuTuLiuZuoXiang()
		{
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x0024174A File Offset: 0x0023F94A
		public JiuTuLiuZuoXiang(CombatSkillKey skillKey) : base(skillKey, 1404)
		{
			this.ListenCanAffectChange = true;
		}

		// Token: 0x060039FE RID: 14846 RVA: 0x00241768 File Offset: 0x0023F968
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affecting = false;
			this.OnMoveSkillCanAffectChanged(context, default(DataUid));
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 176, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 199, -1, -1, -1, -1), EDataModifyType.TotalPercent);
			this._affectingDefendSkill = base.CombatChar.GetAffectingDefendSkillId();
			this._defendSkillUid = new DataUid(8, 10, (ulong)((long)base.CharacterId), 63U);
			GameDataBridge.AddPostDataModificationHandler(this._defendSkillUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDefendSkillChanged));
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x060039FF RID: 14847 RVA: 0x00241831 File Offset: 0x0023FA31
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			GameDataBridge.RemovePostDataModificationHandler(this._defendSkillUid, base.DataHandlerKey);
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x00241850 File Offset: 0x0023FA50
		private void OnDefendSkillChanged(DataContext context, DataUid dataUid)
		{
			this._affectingDefendSkill = base.CombatChar.GetAffectingDefendSkillId();
			bool flag = this.AgileSkillChanged && this._affectingDefendSkill < 0;
			if (flag)
			{
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x00241890 File Offset: 0x0023FA90
		protected override void OnMoveSkillChanged(DataContext context, DataUid dataUid)
		{
			bool firstMoveSkillChanged = this._firstMoveSkillChanged;
			if (firstMoveSkillChanged)
			{
				this._firstMoveSkillChanged = false;
			}
			else
			{
				bool flag = this._affectingDefendSkill < 0;
				if (flag)
				{
					base.RemoveSelf(context);
				}
				else
				{
					this.AgileSkillChanged = true;
				}
			}
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x002418D0 File Offset: 0x0023FAD0
		protected override void OnMoveSkillCanAffectChanged(DataContext context, DataUid dataUid)
		{
			bool canAffect = base.CanAffect;
			bool flag = this._affecting == canAffect;
			if (!flag)
			{
				this._affecting = canAffect;
				DomainManager.SpecialEffect.InvalidateCache(context, base.CharacterId, 199);
			}
		}

		// Token: 0x06003A03 RID: 14851 RVA: 0x00241914 File Offset: 0x0023FB14
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !this._affecting;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 176;
				if (flag2)
				{
					result = (base.IsDirect ? 50 : -50);
				}
				else
				{
					bool flag3 = dataKey.FieldId == 199 && Config.CombatSkill.Instance[dataKey.CombatSkillId].EquipType == 3;
					if (flag3)
					{
						result = (base.IsDirect ? -50 : 50);
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x040010F7 RID: 4343
		private const sbyte ChangeKeepFrames = 50;

		// Token: 0x040010F8 RID: 4344
		private const sbyte ChangePower = 50;

		// Token: 0x040010F9 RID: 4345
		private bool _affecting;

		// Token: 0x040010FA RID: 4346
		private bool _firstMoveSkillChanged = true;

		// Token: 0x040010FB RID: 4347
		private DataUid _defendSkillUid;

		// Token: 0x040010FC RID: 4348
		private short _affectingDefendSkill;
	}
}
