using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.LifeRecord;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect
{
	// Token: 0x0200036D RID: 877
	public class IceSilkwormBase : WugEffectBase
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06003582 RID: 13698 RVA: 0x0022CE3B File Offset: 0x0022B03B
		private int ChangeSilenceFramePercent
		{
			get
			{
				return base.IsGood ? -30 : 60;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06003583 RID: 13699 RVA: 0x0022CE4B File Offset: 0x0022B04B
		private int ChangeSilenceOddsPercent
		{
			get
			{
				return (!base.IsElite) ? 0 : (base.IsGood ? -50 : 50);
			}
		}

		// Token: 0x06003584 RID: 13700 RVA: 0x0022CE68 File Offset: 0x0022B068
		public static bool CanGrown(Character character)
		{
			return character.GetDisorderOfQi() == DisorderLevelOfQi.MaxValue;
		}

		// Token: 0x06003585 RID: 13701 RVA: 0x0022CE87 File Offset: 0x0022B087
		protected IceSilkwormBase()
		{
		}

		// Token: 0x06003586 RID: 13702 RVA: 0x0022CE91 File Offset: 0x0022B091
		protected IceSilkwormBase(int charId, int type, short wugTemplateId, short effectId) : base(charId, type, wugTemplateId, effectId)
		{
			this.CostWugCount = 8;
		}

		// Token: 0x06003587 RID: 13703 RVA: 0x0022CEA8 File Offset: 0x0022B0A8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				base.CreateAffectedData(266, EDataModifyType.Custom, -1);
				base.CreateAffectedData(298, EDataModifyType.Custom, -1);
				Events.RegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			}
			bool canChangeToGrown = base.CanChangeToGrown;
			if (canChangeToGrown)
			{
				this._disorderOfQiUid = new DataUid(4, 0, (ulong)((long)base.CharacterId), 21U);
				GameDataBridge.AddPostDataModificationHandler(this._disorderOfQiUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnDisorderOfQiChanged));
				this.OnDisorderOfQiChanged(context, default(DataUid));
			}
		}

		// Token: 0x06003588 RID: 13704 RVA: 0x0022CF4C File Offset: 0x0022B14C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool isGrown = base.IsGrown;
			if (isGrown)
			{
				Events.UnRegisterHandler_AdvanceMonthFinish(new Events.OnAdvanceMonthFinish(this.OnAdvanceMonthFinish));
			}
			bool canChangeToGrown = base.CanChangeToGrown;
			if (canChangeToGrown)
			{
				GameDataBridge.RemovePostDataModificationHandler(this._disorderOfQiUid, base.DataHandlerKey);
			}
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x0022CF9C File Offset: 0x0022B19C
		protected override void AddAffectDataAndEvent(DataContext context)
		{
			base.AppendAffectedData(context, base.CharacterId, 265, EDataModifyType.AddPercent, -1);
			base.AppendAffectedData(context, base.CharacterId, 264, EDataModifyType.AddPercent, -1);
			bool flag = !base.IsGrown;
			if (flag)
			{
				base.AppendAffectedData(context, base.CharacterId, 218, EDataModifyType.TotalPercent, -1);
			}
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x0022CFF8 File Offset: 0x0022B1F8
		protected override void ClearAffectDataAndEvent(DataContext context)
		{
			base.RemoveAffectedData(context, base.CharacterId, 265);
			base.RemoveAffectedData(context, base.CharacterId, 264);
			bool flag = !base.IsGrown;
			if (flag)
			{
				base.RemoveAffectedData(context, base.CharacterId, 218);
			}
		}

		// Token: 0x0600358B RID: 13707 RVA: 0x0022D04C File Offset: 0x0022B24C
		private void OnAdvanceMonthFinish(DataContext context)
		{
			bool flag = !this._affectedOnMonthChange;
			if (!flag)
			{
				this._affectedOnMonthChange = false;
				LifeRecordCollection lifeRecord = DomainManager.LifeRecord.GetLifeRecordCollection();
				base.AddLifeRecord(new WugEffectBase.LifeRecordAddTemplate(lifeRecord.AddWugKingIceSilkwormLoseNeili));
			}
		}

		// Token: 0x0600358C RID: 13708 RVA: 0x0022D090 File Offset: 0x0022B290
		private void OnDisorderOfQiChanged(DataContext context, DataUid _)
		{
			bool flag = IceSilkwormBase.CanGrown(this.CharObj);
			if (flag)
			{
				this.ChangeToGrown(context);
			}
		}

		// Token: 0x0600358D RID: 13709 RVA: 0x0022D0B8 File Offset: 0x0022B2B8
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
				bool flag2 = fieldId - 264 <= 1;
				bool flag3 = flag2;
				if (flag3)
				{
					base.ShowEffectTips(DomainManager.Combat.Context, 1);
					base.CostWugInCombat(DomainManager.Combat.Context);
					result = this.ChangeSilenceFramePercent;
				}
				else
				{
					bool flag4 = dataKey.FieldId == 218;
					if (flag4)
					{
						base.ShowEffectTips(DomainManager.Combat.Context, 1);
						result = this.ChangeSilenceOddsPercent;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x0600358E RID: 13710 RVA: 0x0022D16C File Offset: 0x0022B36C
		public override int GetModifiedValue(AffectedDataKey dataKey, int dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 298 || !base.CanAffect || !base.IsElite;
			int result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				this._affectedOnMonthChange = true;
				int maxNeili = this.CharObj.GetMaxNeili();
				result = -maxNeili * IceSilkwormBase.CostNeiliPercent;
			}
			return result;
		}

		// Token: 0x0600358F RID: 13711 RVA: 0x0022D1D4 File Offset: 0x0022B3D4
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 266 || !base.CanAffect;
			return !flag || dataValue;
		}

		// Token: 0x04000FA7 RID: 4007
		private static readonly CValuePercent CostNeiliPercent = 50;

		// Token: 0x04000FA8 RID: 4008
		private DataUid _disorderOfQiUid;

		// Token: 0x04000FA9 RID: 4009
		private bool _affectedOnMonthChange;
	}
}
