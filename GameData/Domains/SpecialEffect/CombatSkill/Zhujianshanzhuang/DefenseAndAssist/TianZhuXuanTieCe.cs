using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.GameDataBridge;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.DefenseAndAssist
{
	// Token: 0x020001D8 RID: 472
	public class TianZhuXuanTieCe : AssistSkillBase
	{
		// Token: 0x06002D63 RID: 11619 RVA: 0x0020B8F6 File Offset: 0x00209AF6
		public TianZhuXuanTieCe()
		{
		}

		// Token: 0x06002D64 RID: 11620 RVA: 0x0020B90B File Offset: 0x00209B0B
		public TianZhuXuanTieCe(CombatSkillKey skillKey) : base(skillKey, 9706)
		{
		}

		// Token: 0x06002D65 RID: 11621 RVA: 0x0020B928 File Offset: 0x00209B28
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(315, EDataModifyType.Add, -1);
			base.CreateAffectedData(base.IsDirect ? 181 : 182, EDataModifyType.Custom, -1);
			this._dataUid = base.ParseCharDataUid(57);
			GameDataBridge.AddPostDataModificationHandler(this._dataUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnEquipmentChanged));
			this._affecting = false;
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002D66 RID: 11622 RVA: 0x0020B9AE File Offset: 0x00209BAE
		public override void OnDisable(DataContext context)
		{
			GameDataBridge.RemovePostDataModificationHandler(this._dataUid, base.DataHandlerKey);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			base.OnDisable(context);
		}

		// Token: 0x06002D67 RID: 11623 RVA: 0x0020B9DD File Offset: 0x00209BDD
		private void OnCombatBegin(DataContext context)
		{
			this.UpdateCanAffect(context);
			this.UpdateAddedPower(context);
		}

		// Token: 0x06002D68 RID: 11624 RVA: 0x0020B9F0 File Offset: 0x00209BF0
		protected override void OnCanUseChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context);
		}

		// Token: 0x06002D69 RID: 11625 RVA: 0x0020B9FA File Offset: 0x00209BFA
		private void OnEquipmentChanged(DataContext context, DataUid dataUid)
		{
			this.UpdateCanAffect(context);
			this.UpdateAddedPower(context);
		}

		// Token: 0x06002D6A RID: 11626 RVA: 0x0020BA10 File Offset: 0x00209C10
		private bool IsItemAffected(int itemId)
		{
			int power;
			return this._itemAddedPower.TryGetValue(itemId, out power) && power >= 50;
		}

		// Token: 0x06002D6B RID: 11627 RVA: 0x0020BA38 File Offset: 0x00209C38
		private void UpdateCanAffect(DataContext context)
		{
			bool canAffect = base.CanAffect;
			bool flag = canAffect == this._affecting;
			if (!flag)
			{
				this._affecting = canAffect;
				base.SetConstAffecting(context, canAffect);
				bool flag2 = canAffect;
				if (flag2)
				{
					base.ShowEffectTips(context);
				}
			}
		}

		// Token: 0x06002D6C RID: 11628 RVA: 0x0020BA7C File Offset: 0x00209C7C
		private void UpdateAddedPower(DataContext context)
		{
			bool anyChanged = false;
			foreach (ItemKey key in this.CharObj.GetEquipment())
			{
				bool flag = !key.IsValid();
				if (!flag)
				{
					bool prevIsAffected = this.IsItemAffected(key.Id);
					int addPower = this.CalcAddPowerValue(key);
					anyChanged = (anyChanged || addPower != this._itemAddedPower.GetOrDefault(key.Id));
					this._itemAddedPower[key.Id] = addPower;
					bool currIsAffected = this.IsItemAffected(key.Id);
					bool flag2 = !prevIsAffected && currIsAffected && this._affecting;
					if (flag2)
					{
						base.ShowSpecialEffectTipsOnceInFrame(0);
					}
				}
			}
			bool flag3 = anyChanged;
			if (flag3)
			{
				base.InvalidateCache(context, 315);
			}
		}

		// Token: 0x06002D6D RID: 11629 RVA: 0x0020BB58 File Offset: 0x00209D58
		private int CalcAddPowerValue(ItemKey key)
		{
			bool flag = key.ItemType != (base.IsDirect ? 0 : 1);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				sbyte grade = ItemTemplateHelper.GetGrade(key.ItemType, key.TemplateId);
				int requireValue = TianZhuXuanTieCe.RequireAttainments[(int)grade];
				int power = 0;
				foreach (sbyte lifeSkillType in TianZhuXuanTieCe.RequireLifeSkillTypes)
				{
					short attainment = this.CharObj.GetLifeSkillAttainment(lifeSkillType);
					power += Math.Min((int)(attainment * 100) / requireValue, 100);
				}
				result = power / TianZhuXuanTieCe.RequireLifeSkillTypes.Length;
			}
			return result;
		}

		// Token: 0x06002D6E RID: 11630 RVA: 0x0020BBF8 File Offset: 0x00209DF8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !this._affecting;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				ushort fieldId = dataKey.FieldId;
				if (!true)
				{
				}
				int num;
				if (fieldId != 315)
				{
					num = 0;
				}
				else
				{
					num = this._itemAddedPower.GetOrDefault(dataKey.CustomParam0);
				}
				if (!true)
				{
				}
				result = num;
			}
			return result;
		}

		// Token: 0x06002D6F RID: 11631 RVA: 0x0020BC50 File Offset: 0x00209E50
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = !this._affecting;
			bool flag2 = flag;
			if (!flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId - 181 <= 1;
				flag2 = !flag3;
			}
			bool flag4 = flag2;
			bool result;
			if (flag4)
			{
				result = dataValue;
			}
			else
			{
				result = this.IsItemAffected(dataKey.CustomParam0);
			}
			return result;
		}

		// Token: 0x04000DA3 RID: 3491
		private const int MaxAddPower = 100;

		// Token: 0x04000DA4 RID: 3492
		private const int GodRequirePower = 50;

		// Token: 0x04000DA5 RID: 3493
		private static readonly sbyte[] RequireLifeSkillTypes = new sbyte[]
		{
			6,
			7,
			11,
			10
		};

		// Token: 0x04000DA6 RID: 3494
		private static readonly int[] RequireAttainments = new int[]
		{
			80,
			120,
			160,
			200,
			240,
			280,
			320,
			360,
			400
		};

		// Token: 0x04000DA7 RID: 3495
		private readonly Dictionary<int, int> _itemAddedPower = new Dictionary<int, int>();

		// Token: 0x04000DA8 RID: 3496
		private bool _affecting;

		// Token: 0x04000DA9 RID: 3497
		private DataUid _dataUid;
	}
}
