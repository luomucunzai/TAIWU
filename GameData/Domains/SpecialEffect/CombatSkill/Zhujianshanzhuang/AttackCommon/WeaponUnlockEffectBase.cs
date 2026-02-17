using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.GameDataBridge;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon
{
	// Token: 0x020001E7 RID: 487
	public abstract class WeaponUnlockEffectBase : CombatSkillEffectBase
	{
		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06002DFA RID: 11770
		protected abstract short WeaponType { get; }

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06002DFB RID: 11771
		protected abstract CValuePercent DirectAddUnlockPercent { get; }

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06002DFC RID: 11772
		protected abstract bool ReverseEffectDoubling { get; }

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x06002DFD RID: 11773 RVA: 0x0020D6C4 File Offset: 0x0020B8C4
		protected short DirectWeaponId
		{
			get
			{
				return base.SkillConfig.FixedBestWeaponID;
			}
		}

		// Token: 0x06002DFE RID: 11774 RVA: 0x0020D6D1 File Offset: 0x0020B8D1
		protected bool IsMatchWeaponType(ItemKey weaponKey)
		{
			return ItemTemplateHelper.GetItemSubType(weaponKey.ItemType, weaponKey.TemplateId) == this.WeaponType;
		}

		// Token: 0x06002DFF RID: 11775 RVA: 0x0020D6EC File Offset: 0x0020B8EC
		protected bool IsDirectWeapon(ItemKey weaponKey)
		{
			return weaponKey.TemplateId == this.DirectWeaponId;
		}

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x06002E00 RID: 11776 RVA: 0x0020D6FC File Offset: 0x0020B8FC
		protected bool UsingDirectWeapon
		{
			get
			{
				return this.IsDirectWeapon(DomainManager.Combat.GetUsingWeaponKey(base.CombatChar));
			}
		}

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x06002E01 RID: 11777 RVA: 0x0020D714 File Offset: 0x0020B914
		protected bool IsDirectOrReverseEffectDoubling
		{
			get
			{
				return base.IsDirect || this.ReverseEffectDoubling;
			}
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06002E02 RID: 11778 RVA: 0x0020D727 File Offset: 0x0020B927
		protected bool IsReverseOrUsingDirectWeapon
		{
			get
			{
				return !base.IsDirect || this.UsingDirectWeapon;
			}
		}

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06002E03 RID: 11779 RVA: 0x0020D73A File Offset: 0x0020B93A
		// (set) Token: 0x06002E04 RID: 11780 RVA: 0x0020D742 File Offset: 0x0020B942
		private protected bool Affected { protected get; private set; }

		// Token: 0x06002E05 RID: 11781 RVA: 0x0020D74B File Offset: 0x0020B94B
		protected WeaponUnlockEffectBase()
		{
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x0020D755 File Offset: 0x0020B955
		protected WeaponUnlockEffectBase(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x0020D764 File Offset: 0x0020B964
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_UnlockAttack(new Events.OnUnlockAttack(this.OnUnlockAttack));
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			this._usingWeaponUid = base.ParseCombatCharacterDataUid(16);
			GameDataBridge.AddPostDataModificationHandler(this._usingWeaponUid, base.DataHandlerKey, new Action<DataContext, DataUid>(this.OnUsingWeaponChanged));
		}

		// Token: 0x06002E08 RID: 11784 RVA: 0x0020D7DC File Offset: 0x0020B9DC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_UnlockAttack(new Events.OnUnlockAttack(this.OnUnlockAttack));
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			GameDataBridge.RemovePostDataModificationHandler(this._usingWeaponUid, base.DataHandlerKey);
			base.OnDisable(context);
		}

		// Token: 0x06002E09 RID: 11785
		protected abstract void DoAffect(DataContext context, int weaponIndex);

		// Token: 0x06002E0A RID: 11786 RVA: 0x0020D83A File Offset: 0x0020BA3A
		private void OnCombatBegin(DataContext context)
		{
			this.OnUsingWeaponChanged(context);
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x0020D844 File Offset: 0x0020BA44
		private void OnUnlockAttack(DataContext context, CombatCharacter combatChar, int weaponIndex)
		{
			bool flag = combatChar.GetId() != base.CharacterId;
			if (!flag)
			{
				ItemKey weapon = combatChar.GetWeapons()[weaponIndex];
				bool flag2 = base.IsDirect ? this.IsDirectWeapon(weapon) : (this.IsMatchWeaponType(weapon) && this.ReverseEffectDoubling);
				if (flag2)
				{
					this.DoAffect(context, weaponIndex);
				}
			}
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x0020D8A8 File Offset: 0x0020BAA8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = !this.SkillKey.IsMatch(charId, skillId) || power <= 0;
			if (!flag)
			{
				this.OnCastAddUnlockAttackValue(context, (int)power);
			}
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x0020D8E6 File Offset: 0x0020BAE6
		private void OnUsingWeaponChanged(DataContext context, DataUid arg2)
		{
			this.OnUsingWeaponChanged(context);
		}

		// Token: 0x06002E0E RID: 11790 RVA: 0x0020D8F0 File Offset: 0x0020BAF0
		private void OnUsingWeaponChanged(DataContext context)
		{
			bool affected = this.IsReverseOrUsingDirectWeapon;
			bool flag = affected == this.Affected;
			if (!flag)
			{
				this.Affected = affected;
				this.OnAffectedChanged(context);
			}
		}

		// Token: 0x06002E0F RID: 11791 RVA: 0x0020D924 File Offset: 0x0020BB24
		protected virtual void OnAffectedChanged(DataContext context)
		{
		}

		// Token: 0x06002E10 RID: 11792 RVA: 0x0020D928 File Offset: 0x0020BB28
		protected virtual void OnCastAddUnlockAttackValue(DataContext context, CValuePercent power)
		{
			bool flag = !base.IsDirect || !this.UsingDirectWeapon;
			if (!flag)
			{
				int addValue = GlobalConfig.Instance.UnlockAttackUnit * power * this.DirectAddUnlockPercent;
				for (int i = 0; i < 3; i++)
				{
					bool flag2 = base.CombatChar.GetWeapons()[i].TemplateId == this.DirectWeaponId;
					if (flag2)
					{
						base.CombatChar.ChangeUnlockAttackValue(context, i, addValue);
					}
				}
				base.ShowSpecialEffectTipsOnceInFrame(0);
			}
		}

		// Token: 0x04000DB9 RID: 3513
		private DataUid _usingWeaponUid;
	}
}
