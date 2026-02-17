using System;
using System.Diagnostics.CodeAnalysis;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat
{
	// Token: 0x020006AC RID: 1708
	public readonly struct CombatContext
	{
		// Token: 0x0600628C RID: 25228 RVA: 0x0038179C File Offset: 0x0037F99C
		public static CombatContext Create([DisallowNull] CombatCharacter attacker, [AllowNull] CombatCharacter defender = null, sbyte specifyBodyPart = -1, short specifySkillId = -1, int specifyWeaponIndex = -1, CombatProperty? combatProperty = null)
		{
			return new CombatContext
			{
				Context = attacker.GetDataContext(),
				Attacker = attacker,
				Defender = (defender ?? CombatContext.CombatDomain.GetCombatCharacter(!attacker.IsAlly, true)),
				BounceSourceId = -1,
				SpecifyProperty = combatProperty,
				SpecifyBodyPart = specifyBodyPart,
				SpecifySkillId = specifySkillId,
				SpecifyWeaponIndex = specifyWeaponIndex,
				IsBounce = false,
				CriticalType = ECombatCriticalType.Uncheck
			};
		}

		// Token: 0x0600628D RID: 25229 RVA: 0x00381830 File Offset: 0x0037FA30
		public CombatContext(CombatContext context)
		{
			this.Context = context.Context;
			this.Attacker = context.Attacker;
			this.Defender = context.Defender;
			this.BounceSourceId = context.BounceSourceId;
			this.SpecifyProperty = context.SpecifyProperty;
			this.SpecifyBodyPart = context.SpecifyBodyPart;
			this.SpecifySkillId = context.SpecifySkillId;
			this.SpecifyWeaponIndex = context.SpecifyWeaponIndex;
			this.IsBounce = context.IsBounce;
			this.CriticalType = context.CriticalType;
		}

		// Token: 0x0600628E RID: 25230 RVA: 0x003818CC File Offset: 0x0037FACC
		public CombatContext Bounce()
		{
			bool isBounce = this.IsBounce;
			if (isBounce)
			{
				PredefinedLog.Show(8, "Unable to bounce a bounce");
			}
			return new CombatContext(this)
			{
				Defender = this.Attacker,
				IsBounce = true,
				BounceSourceId = this.DefenderId,
				SpecifyProperty = null,
				CriticalType = ECombatCriticalType.Uncheck
			};
		}

		// Token: 0x0600628F RID: 25231 RVA: 0x00381944 File Offset: 0x0037FB44
		public CombatContext Property(CombatProperty property)
		{
			return new CombatContext(this)
			{
				SpecifyProperty = new CombatProperty?(property)
			};
		}

		// Token: 0x06006290 RID: 25232 RVA: 0x00381974 File Offset: 0x0037FB74
		public CombatContext Critical(bool critical)
		{
			return new CombatContext(this)
			{
				CriticalType = (critical ? ECombatCriticalType.Critical : ECombatCriticalType.NoCritical)
			};
		}

		// Token: 0x06006291 RID: 25233 RVA: 0x003819A3 File Offset: 0x0037FBA3
		public static implicit operator DataContext(CombatContext context)
		{
			return context.Context;
		}

		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06006292 RID: 25234 RVA: 0x003819AC File Offset: 0x0037FBAC
		// (set) Token: 0x06006293 RID: 25235 RVA: 0x003819B4 File Offset: 0x0037FBB4
		public DataContext Context { get; private set; }

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06006294 RID: 25236 RVA: 0x003819BD File Offset: 0x0037FBBD
		// (set) Token: 0x06006295 RID: 25237 RVA: 0x003819C5 File Offset: 0x0037FBC5
		public CombatCharacter Attacker { get; private set; }

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06006296 RID: 25238 RVA: 0x003819CE File Offset: 0x0037FBCE
		// (set) Token: 0x06006297 RID: 25239 RVA: 0x003819D6 File Offset: 0x0037FBD6
		public CombatCharacter Defender { get; private set; }

		// Token: 0x17000382 RID: 898
		// (get) Token: 0x06006298 RID: 25240 RVA: 0x003819DF File Offset: 0x0037FBDF
		// (set) Token: 0x06006299 RID: 25241 RVA: 0x003819E7 File Offset: 0x0037FBE7
		public int BounceSourceId { get; set; }

		// Token: 0x17000383 RID: 899
		// (get) Token: 0x0600629A RID: 25242 RVA: 0x003819F0 File Offset: 0x0037FBF0
		// (set) Token: 0x0600629B RID: 25243 RVA: 0x003819F8 File Offset: 0x0037FBF8
		private bool IsBounce { get; set; }

		// Token: 0x17000384 RID: 900
		// (get) Token: 0x0600629C RID: 25244 RVA: 0x00381A01 File Offset: 0x0037FC01
		// (set) Token: 0x0600629D RID: 25245 RVA: 0x00381A09 File Offset: 0x0037FC09
		private sbyte SpecifyBodyPart { get; set; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600629E RID: 25246 RVA: 0x00381A12 File Offset: 0x0037FC12
		// (set) Token: 0x0600629F RID: 25247 RVA: 0x00381A1A File Offset: 0x0037FC1A
		private short SpecifySkillId { get; set; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x060062A0 RID: 25248 RVA: 0x00381A23 File Offset: 0x0037FC23
		// (set) Token: 0x060062A1 RID: 25249 RVA: 0x00381A2B File Offset: 0x0037FC2B
		private int SpecifyWeaponIndex { get; set; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x060062A2 RID: 25250 RVA: 0x00381A34 File Offset: 0x0037FC34
		// (set) Token: 0x060062A3 RID: 25251 RVA: 0x00381A3C File Offset: 0x0037FC3C
		private CombatProperty? SpecifyProperty { get; set; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x060062A4 RID: 25252 RVA: 0x00381A45 File Offset: 0x0037FC45
		// (set) Token: 0x060062A5 RID: 25253 RVA: 0x00381A4D File Offset: 0x0037FC4D
		private ECombatCriticalType CriticalType { get; set; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x060062A6 RID: 25254 RVA: 0x00381A56 File Offset: 0x0037FC56
		public IRandomSource Random
		{
			get
			{
				return this.Context.Random;
			}
		}

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x060062A7 RID: 25255 RVA: 0x00381A63 File Offset: 0x0037FC63
		public int AttackerId
		{
			get
			{
				return this.Attacker.GetId();
			}
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060062A8 RID: 25256 RVA: 0x00381A70 File Offset: 0x0037FC70
		public int DefenderId
		{
			get
			{
				return this.Defender.GetId();
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060062A9 RID: 25257 RVA: 0x00381A7D File Offset: 0x0037FC7D
		public sbyte BodyPart
		{
			get
			{
				return (this.SpecifyBodyPart >= 0) ? this.SpecifyBodyPart : (this.IsNormalAttack ? this.Attacker.NormalAttackBodyPart : this.Attacker.SkillAttackBodyPart);
			}
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060062AA RID: 25258 RVA: 0x00381AB0 File Offset: 0x0037FCB0
		public sbyte InnerRatio
		{
			get
			{
				return this.IsNormalAttack ? this.WeaponData.GetInnerRatio() : this.Skill.GetCurrInnerRatio();
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060062AB RID: 25259 RVA: 0x00381AD2 File Offset: 0x0037FCD2
		public sbyte OuterRatio
		{
			get
			{
				return 100 - this.InnerRatio;
			}
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060062AC RID: 25260 RVA: 0x00381ADE File Offset: 0x0037FCDE
		public bool IsNormalAttack
		{
			get
			{
				return this.SkillTemplateId < 0;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060062AD RID: 25261 RVA: 0x00381AE9 File Offset: 0x0037FCE9
		public bool IsFightBack
		{
			get
			{
				return this.IsNormalAttack && this.Attacker.GetIsFightBack();
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060062AE RID: 25262 RVA: 0x00381B01 File Offset: 0x0037FD01
		private static ItemDomain ItemDomain
		{
			get
			{
				return DomainManager.Item;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060062AF RID: 25263 RVA: 0x00381B08 File Offset: 0x0037FD08
		private static CombatDomain CombatDomain
		{
			get
			{
				return DomainManager.Combat;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060062B0 RID: 25264 RVA: 0x00381B0F File Offset: 0x0037FD0F
		private static CombatSkillDomain CombatSkillDomain
		{
			get
			{
				return DomainManager.CombatSkill;
			}
		}

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x060062B1 RID: 25265 RVA: 0x00381B16 File Offset: 0x0037FD16
		public short SkillTemplateId
		{
			get
			{
				return (this.SpecifySkillId >= 0) ? this.SpecifySkillId : this.Attacker.GetPerformingSkillId();
			}
		}

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x060062B2 RID: 25266 RVA: 0x00381B34 File Offset: 0x0037FD34
		public CombatSkillKey SkillKey
		{
			get
			{
				return new CombatSkillKey(this.Attacker.GetId(), this.SkillTemplateId);
			}
		}

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x060062B3 RID: 25267 RVA: 0x00381B4C File Offset: 0x0037FD4C
		public GameData.Domains.CombatSkill.CombatSkill Skill
		{
			get
			{
				GameData.Domains.CombatSkill.CombatSkill skill;
				return CombatContext.CombatSkillDomain.TryGetElement_CombatSkills(this.SkillKey, out skill) ? skill : null;
			}
		}

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x060062B4 RID: 25268 RVA: 0x00381B74 File Offset: 0x0037FD74
		public CombatSkillData SkillData
		{
			get
			{
				CombatSkillData data;
				return CombatContext.CombatDomain.TryGetCombatSkillData(this.Attacker.GetId(), this.SkillTemplateId, out data) ? data : null;
			}
		}

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x060062B5 RID: 25269 RVA: 0x00381BA4 File Offset: 0x0037FDA4
		public CombatSkillItem SkillConfig
		{
			get
			{
				return Config.CombatSkill.Instance[this.SkillTemplateId];
			}
		}

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x060062B6 RID: 25270 RVA: 0x00381BB6 File Offset: 0x0037FDB6
		public ItemKey WeaponKey
		{
			get
			{
				return (this.SpecifyWeaponIndex >= 0) ? this.Attacker.GetWeapons()[this.SpecifyWeaponIndex] : CombatContext.CombatDomain.GetUsingWeaponKey(this.Attacker);
			}
		}

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x060062B7 RID: 25271 RVA: 0x00381BE9 File Offset: 0x0037FDE9
		public GameData.Domains.Item.Weapon Weapon
		{
			get
			{
				return CombatContext.ItemDomain.GetElement_Weapons(this.WeaponKey.Id);
			}
		}

		// Token: 0x1700039B RID: 923
		// (get) Token: 0x060062B8 RID: 25272 RVA: 0x00381C00 File Offset: 0x0037FE00
		public CombatWeaponData WeaponData
		{
			get
			{
				return CombatContext.CombatDomain.GetElement_WeaponDataDict(this.WeaponKey.Id);
			}
		}

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x060062B9 RID: 25273 RVA: 0x00381C17 File Offset: 0x0037FE17
		public WeaponItem WeaponConfig
		{
			get
			{
				return Config.Weapon.Instance[this.WeaponKey.TemplateId];
			}
		}

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x060062BA RID: 25274 RVA: 0x00381C2E File Offset: 0x0037FE2E
		public sbyte WeaponPointCost
		{
			get
			{
				return this.WeaponConfig.AttackPreparePointCost;
			}
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060062BB RID: 25275 RVA: 0x00381C3B File Offset: 0x0037FE3B
		public int WeaponAttack
		{
			get
			{
				return CombatDomain.CalcWeaponAttack(this.Attacker, this.Weapon, this.SkillTemplateId);
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060062BC RID: 25276 RVA: 0x00381C54 File Offset: 0x0037FE54
		public int WeaponDefend
		{
			get
			{
				return CombatDomain.CalcWeaponDefend(this.Attacker, this.Weapon, this.SkillTemplateId);
			}
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060062BD RID: 25277 RVA: 0x00381C6D File Offset: 0x0037FE6D
		public ItemKey ArmorKey
		{
			get
			{
				return (this.BodyPart >= 0) ? this.Defender.Armors[(int)this.BodyPart] : ItemKey.Invalid;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060062BE RID: 25278 RVA: 0x00381C98 File Offset: 0x0037FE98
		public GameData.Domains.Item.Armor Armor
		{
			get
			{
				return this.ArmorKey.IsValid() ? CombatContext.ItemDomain.GetElement_Armors(this.ArmorKey.Id) : null;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060062BF RID: 25279 RVA: 0x00381CCD File Offset: 0x0037FECD
		public int ArmorAttack
		{
			get
			{
				return CombatDomain.CalcArmorAttack(this.Defender, this.Armor);
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060062C0 RID: 25280 RVA: 0x00381CE0 File Offset: 0x0037FEE0
		public int ArmorDefend
		{
			get
			{
				return CombatDomain.CalcArmorDefend(this.Defender, this.Armor);
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060062C1 RID: 25281 RVA: 0x00381CF4 File Offset: 0x0037FEF4
		public ItemKey WeaponOrShoesKey
		{
			get
			{
				return (this.SkillTemplateId >= 0 && CombatContext.CombatSkillDomain.GetSkillType(this.AttackerId, this.SkillTemplateId) == 5 && this.Attacker.LegSkillUseShoes()) ? this.Attacker.Armors[5] : this.WeaponKey;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060062C2 RID: 25282 RVA: 0x00381D49 File Offset: 0x0037FF49
		public EquipmentBase WeaponOrShoes
		{
			get
			{
				return CombatContext.ItemDomain.TryGetBaseEquipment(this.WeaponOrShoesKey);
			}
		}

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x060062C3 RID: 25283 RVA: 0x00381D5B File Offset: 0x0037FF5B
		public int OuterStep
		{
			get
			{
				return (this.BodyPart < 0) ? -1 : this.DamageStepCollection.OuterDamageSteps[(int)this.BodyPart];
			}
		}

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x060062C4 RID: 25284 RVA: 0x00381D7B File Offset: 0x0037FF7B
		public int InnerStep
		{
			get
			{
				return (this.BodyPart < 0) ? -1 : this.DamageStepCollection.InnerDamageSteps[(int)this.BodyPart];
			}
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060062C5 RID: 25285 RVA: 0x00381D9B File Offset: 0x0037FF9B
		public int OuterOrigin
		{
			get
			{
				return (this.BodyPart < 0) ? 0 : this.Defender.GetOuterDamageValue()[(int)this.BodyPart];
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060062C6 RID: 25286 RVA: 0x00381DBB File Offset: 0x0037FFBB
		public int InnerOrigin
		{
			get
			{
				return (this.BodyPart < 0) ? 0 : this.Defender.GetInnerDamageValue()[(int)this.BodyPart];
			}
		}

		// Token: 0x060062C7 RID: 25287 RVA: 0x00381DDC File Offset: 0x0037FFDC
		private bool CalcDamageCanAdd(bool inner)
		{
			bool isGodArmor = this.IsGodArmor;
			return !isGodArmor && DomainManager.SpecialEffect.ModifyData(this.DefenderId, this.SkillTemplateId, 326, true, inner ? 1 : 0, (int)this.BodyPart, (int)this.DamageType);
		}

		// Token: 0x060062C8 RID: 25288 RVA: 0x00381E2C File Offset: 0x0038002C
		private bool CalcDamageCanReduce(bool inner)
		{
			bool isGodWeapon = this.IsGodWeapon;
			return !isGodWeapon && DomainManager.SpecialEffect.ModifyData(this.AttackerId, this.SkillTemplateId, 327, true, inner ? 1 : 0, (int)this.BodyPart, (int)this.DamageType);
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060062C9 RID: 25289 RVA: 0x00381E7B File Offset: 0x0038007B
		public EDataSumType OuterSumType
		{
			get
			{
				return DataSumTypeHelper.CalcSumType(this.CalcDamageCanAdd(false), this.CalcDamageCanReduce(false));
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x060062CA RID: 25290 RVA: 0x00381E90 File Offset: 0x00380090
		public EDataSumType InnerSumType
		{
			get
			{
				return DataSumTypeHelper.CalcSumType(this.CalcDamageCanAdd(true), this.CalcDamageCanReduce(true));
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x060062CB RID: 25291 RVA: 0x00381EA8 File Offset: 0x003800A8
		public int ExtraFlawCount
		{
			get
			{
				return DomainManager.SpecialEffect.GetModifyValue(this.Attacker.GetId(), this.SkillTemplateId, 84, EDataModifyType.Add, (int)this.BodyPart, -1, -1, EDataSumType.All);
			}
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x060062CC RID: 25292 RVA: 0x00381EDC File Offset: 0x003800DC
		public EDamageType OuterDamageType
		{
			get
			{
				return (EDamageType)DomainManager.SpecialEffect.ModifyData(this.Attacker.GetId(), this.SkillTemplateId, 79, (int)this.DamageType, 0, -1, -1);
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x060062CD RID: 25293 RVA: 0x00381F04 File Offset: 0x00380104
		public EDamageType InnerDamageType
		{
			get
			{
				return (EDamageType)DomainManager.SpecialEffect.ModifyData(this.Attacker.GetId(), this.SkillTemplateId, 79, (int)this.DamageType, 1, -1, -1);
			}
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x060062CE RID: 25294 RVA: 0x00381F2C File Offset: 0x0038012C
		private bool AllChangeToOld
		{
			get
			{
				return this.Attacker.IsUnlockAttack && this.Attacker.UnlockEffect.ChangeToOld;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x060062CF RID: 25295 RVA: 0x00381F4E File Offset: 0x0038014E
		public bool OuterInjuryChangeToOld
		{
			get
			{
				return DomainManager.SpecialEffect.ModifyData(this.AttackerId, this.SkillTemplateId, 77, false, 0, -1, -1) || this.AllChangeToOld;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x060062D0 RID: 25296 RVA: 0x00381F77 File Offset: 0x00380177
		public bool InnerInjuryChangeToOld
		{
			get
			{
				return DomainManager.SpecialEffect.ModifyData(this.AttackerId, this.SkillTemplateId, 77, false, 1, -1, -1) || this.AllChangeToOld;
			}
		}

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x060062D1 RID: 25297 RVA: 0x00381FA0 File Offset: 0x003801A0
		public bool IsGodWeapon
		{
			get
			{
				return DomainManager.SpecialEffect.ModifyData(this.Attacker.GetId(), -1, 181, false, this.WeaponOrShoesKey.Id, -1, -1);
			}
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x060062D2 RID: 25298 RVA: 0x00381FCB File Offset: 0x003801CB
		public bool IsGodArmor
		{
			get
			{
				return DomainManager.SpecialEffect.ModifyData(this.Defender.GetId(), -1, 182, false, this.ArmorKey.Id, -1, -1);
			}
		}

		// Token: 0x060062D3 RID: 25299 RVA: 0x00381FF8 File Offset: 0x003801F8
		public CombatProperty CalcProperty(sbyte hitType = -1)
		{
			bool flag = this.SpecifyProperty != null;
			CombatProperty result;
			if (flag)
			{
				result = this.SpecifyProperty.Value;
			}
			else
			{
				Tester.Assert(hitType >= 0, "");
				result = CombatProperty.Create(this, hitType);
			}
			return result;
		}

		// Token: 0x060062D4 RID: 25300 RVA: 0x0038204C File Offset: 0x0038024C
		public OuterAndInnerInts CalcMixedDamage(sbyte hitType, CValuePercent power)
		{
			int damage = this.BaseDamage * this.FlawBonus * power;
			CombatProperty property = this.CalcProperty(hitType);
			return CFormula.FormulaCalcMixedDamageValue(damage, this.AttackOdds, this.InnerRatio, property.AttackValue, property.DefendValue);
		}

		// Token: 0x060062D5 RID: 25301 RVA: 0x003820A0 File Offset: 0x003802A0
		public bool CheckCritical(sbyte hitType)
		{
			bool flag = this.CriticalType > ECombatCriticalType.Uncheck;
			bool result;
			if (flag)
			{
				result = (this.CriticalType == ECombatCriticalType.Critical);
			}
			else
			{
				bool flag2 = this.BodyPart < 0;
				if (flag2)
				{
					result = false;
				}
				else
				{
					int hitOdds = this.CalcProperty(hitType).HitOdds;
					bool canCriticalHit = DomainManager.SpecialEffect.ModifyData(this.DefenderId, this.SkillTemplateId, 234, true, (int)this.BodyPart, (int)hitType, this.AttackerId);
					bool certainCriticalHit = this.Attacker.IsBreakAttacking || DomainManager.SpecialEffect.ModifyData(this.AttackerId, this.SkillTemplateId, 248, false, (int)hitType, -1, -1);
					result = (canCriticalHit && CombatDomain.CheckCritical(this.Random, this.AttackerId, hitOdds, certainCriticalHit));
				}
			}
			return result;
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x060062D6 RID: 25302 RVA: 0x0038216C File Offset: 0x0038036C
		public DamageStepCollection DamageStepCollection
		{
			get
			{
				return this.Defender.GetDamageStepCollection();
			}
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x060062D7 RID: 25303 RVA: 0x00382179 File Offset: 0x00380379
		public EDamageType DamageType
		{
			get
			{
				return this.IsBounce ? EDamageType.Bounce : (this.IsFightBack ? EDamageType.FightBack : EDamageType.Direct);
			}
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x060062D8 RID: 25304 RVA: 0x00382192 File Offset: 0x00380392
		public bool UseSkillAttackOdds
		{
			get
			{
				return !this.IsNormalAttack || this.Attacker.IsAnimal;
			}
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x060062D9 RID: 25305 RVA: 0x003821AC File Offset: 0x003803AC
		public CFormula.EAttackType AttackType
		{
			get
			{
				bool isUnlockAttack = this.Attacker.IsUnlockAttack;
				CFormula.EAttackType result;
				if (isUnlockAttack)
				{
					result = CFormula.EAttackType.Unlock;
				}
				else
				{
					bool isAutoNormalAttackingSpecial = this.Attacker.IsAutoNormalAttackingSpecial;
					if (isAutoNormalAttackingSpecial)
					{
						result = CFormula.EAttackType.Spirit;
					}
					else
					{
						bool useSkillAttackOdds = this.UseSkillAttackOdds;
						if (useSkillAttackOdds)
						{
							result = ((this.BodyPart < 0) ? CFormula.EAttackType.MindSkill : CFormula.EAttackType.Skill);
						}
						else
						{
							result = CFormula.EAttackType.Normal;
						}
					}
				}
				return result;
			}
		}

		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x060062DA RID: 25306 RVA: 0x003821FF File Offset: 0x003803FF
		public int BaseDamage
		{
			get
			{
				return CFormula.CalcBaseDamageValue(this.AttackType, this.WeaponPointCost);
			}
		}

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x060062DB RID: 25307 RVA: 0x00382212 File Offset: 0x00380412
		public int AttackOdds
		{
			get
			{
				return CFormula.CalcBaseAttackOdds(this.AttackType);
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x060062DC RID: 25308 RVA: 0x00382220 File Offset: 0x00380420
		public CValuePercentBonus FlawBonus
		{
			get
			{
				int bonus = (int)CFormula.CalcFlawDamageBonus((int)this.Defender.GetFlawCount()[(int)this.BodyPart], this.ExtraFlawCount);
				bool flag = bonus > 0;
				if (flag)
				{
					bonus *= DomainManager.SpecialEffect.GetModify(this.AttackerId, this.SkillTemplateId, 318, (int)this.BodyPart, -1, -1, EDataSumType.All);
				}
				return bonus;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x060062DD RID: 25309 RVA: 0x0038228E File Offset: 0x0038048E
		public CValuePercentBonus ConsummateBonus
		{
			get
			{
				return CFormulaHelper.CalcConsummateChangeDamagePercent(this.Attacker, this.Defender);
			}
		}

		// Token: 0x060062DE RID: 25310 RVA: 0x003822A4 File Offset: 0x003804A4
		public void ApplyWeaponAndArmorPoison(int valueMultiplier = 1)
		{
			sbyte bodyPart = this.BodyPart;
			bool flag = bodyPart < 0 || bodyPart >= 7;
			bool flag2 = flag;
			if (!flag2)
			{
				DomainManager.Combat.ApplyEquipmentPoison(this, this.Attacker, this.Defender, this.WeaponOrShoesKey, valueMultiplier);
				DomainManager.Combat.ApplyEquipmentPoison(this, this.Defender, this.Attacker, this.ArmorKey, valueMultiplier);
			}
		}

		// Token: 0x060062DF RID: 25311 RVA: 0x00382324 File Offset: 0x00380524
		public void CheckReduceWeaponDurability(sbyte breakOdds)
		{
			bool flag = breakOdds <= 0;
			if (!flag)
			{
				EquipmentBase weaponOrShoes = this.WeaponOrShoes;
				short currDurability = (weaponOrShoes != null) ? weaponOrShoes.GetCurrDurability() : 0;
				bool flag2 = currDurability <= 0;
				if (!flag2)
				{
					bool flag3 = this.ArmorAttack <= this.WeaponDefend;
					if (!flag3)
					{
						bool flag4 = this.IsGodWeapon && this.IsNormalAttack;
						if (!flag4)
						{
							bool flag5 = !CombatDomain.IsWeaponCanBreak(this.Weapon.GetItemSubType());
							if (!flag5)
							{
								this.Attacker.NeedReduceWeaponDurability = this.Random.CheckPercentProb((int)breakOdds);
							}
						}
					}
				}
			}
		}

		// Token: 0x060062E0 RID: 25312 RVA: 0x003823C4 File Offset: 0x003805C4
		public void CheckReduceArmorDurability(sbyte breakOdds)
		{
			bool flag = breakOdds <= 0;
			if (!flag)
			{
				GameData.Domains.Item.Armor armor = this.Armor;
				short currDurability = (armor != null) ? armor.GetCurrDurability() : 0;
				bool flag2 = currDurability <= 0;
				if (!flag2)
				{
					bool flag3 = this.WeaponAttack <= this.ArmorDefend;
					if (!flag3)
					{
						bool flag4 = this.IsGodArmor && this.IsNormalAttack;
						if (!flag4)
						{
							this.Defender.NeedReduceArmorDurability = this.Random.CheckPercentProb((int)breakOdds);
						}
					}
				}
			}
		}
	}
}
