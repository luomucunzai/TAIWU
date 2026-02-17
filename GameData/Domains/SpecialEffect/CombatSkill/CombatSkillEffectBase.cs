using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger;
using GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Blade;
using GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.FistAndPalm;
using GameData.Serializer;

namespace GameData.Domains.SpecialEffect.CombatSkill
{
	// Token: 0x020001B1 RID: 433
	[SerializableGameData(NotForDisplayModule = true)]
	public class CombatSkillEffectBase : SpecialEffectBase
	{
		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06002C27 RID: 11303 RVA: 0x00206C13 File Offset: 0x00204E13
		public short SkillTemplateId
		{
			get
			{
				return this.SkillKey.SkillTemplateId;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06002C28 RID: 11304 RVA: 0x00206C20 File Offset: 0x00204E20
		// (set) Token: 0x06002C29 RID: 11305 RVA: 0x00206C28 File Offset: 0x00204E28
		public bool IsDirect { get; protected set; }

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06002C2A RID: 11306 RVA: 0x00206C31 File Offset: 0x00204E31
		public GameData.Domains.CombatSkill.CombatSkill SkillInstance
		{
			get
			{
				return DomainManager.CombatSkill.GetElement_CombatSkills(this.SkillKey);
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06002C2B RID: 11307 RVA: 0x00206C43 File Offset: 0x00204E43
		protected CombatSkillData SkillData
		{
			get
			{
				return DomainManager.Combat.GetCombatSkillData(base.CharacterId, this.SkillTemplateId);
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06002C2C RID: 11308 RVA: 0x00206C5B File Offset: 0x00204E5B
		protected CombatSkillItem SkillConfig
		{
			get
			{
				return Config.CombatSkill.Instance[this.SkillTemplateId];
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06002C2D RID: 11309 RVA: 0x00206C6D File Offset: 0x00204E6D
		protected int DirectEffectId
		{
			get
			{
				return Config.CombatSkill.Instance[this.SkillTemplateId].DirectEffectID;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06002C2E RID: 11310 RVA: 0x00206C84 File Offset: 0x00204E84
		protected int ReverseEffectId
		{
			get
			{
				return Config.CombatSkill.Instance[this.SkillTemplateId].ReverseEffectID;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06002C2F RID: 11311 RVA: 0x00206C9B File Offset: 0x00204E9B
		public int EffectId
		{
			get
			{
				return this.IsDirect ? this.DirectEffectId : this.ReverseEffectId;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06002C30 RID: 11312 RVA: 0x00206CB3 File Offset: 0x00204EB3
		public SkillEffectKey EffectKey
		{
			get
			{
				return new SkillEffectKey(this.SkillTemplateId, this.IsDirect);
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06002C31 RID: 11313 RVA: 0x00206CC6 File Offset: 0x00204EC6
		protected int EffectCount
		{
			get
			{
				return (int)DomainManager.Combat.GetSkillEffectCount(base.CombatChar, this.EffectKey);
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06002C32 RID: 11314 RVA: 0x00206CDE File Offset: 0x00204EDE
		protected short MaxEffectCount
		{
			get
			{
				return SpecialEffect.Instance[this.EffectId].MaxEffectCount;
			}
		}

		// Token: 0x06002C33 RID: 11315 RVA: 0x00206CF5 File Offset: 0x00204EF5
		protected CombatSkillEffectBase()
		{
		}

		// Token: 0x06002C34 RID: 11316 RVA: 0x00206D00 File Offset: 0x00204F00
		protected CombatSkillEffectBase(CombatSkillKey skillKey, int type, sbyte direction = -1) : base(skillKey.CharId, type)
		{
			this.SkillKey = skillKey;
			bool flag = direction < 0;
			if (flag)
			{
				direction = this.SkillInstance.GetDirection();
			}
			this.IsDirect = (direction == 0);
		}

		// Token: 0x06002C35 RID: 11317 RVA: 0x00206D43 File Offset: 0x00204F43
		protected virtual void OnDirectionChanged(DataContext context)
		{
		}

		// Token: 0x06002C36 RID: 11318 RVA: 0x00206D46 File Offset: 0x00204F46
		protected void RemoveSelf(DataContext context)
		{
			DomainManager.SpecialEffect.Remove(context, this.Id);
		}

		// Token: 0x06002C37 RID: 11319 RVA: 0x00206D5C File Offset: 0x00204F5C
		protected void ReduceEffectCount(int removeCount = 1)
		{
			DataContext context = DomainManager.Combat.Context;
			DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, this.EffectKey, (short)(-(short)removeCount), true, false);
		}

		// Token: 0x06002C38 RID: 11320 RVA: 0x00206D94 File Offset: 0x00204F94
		protected void AddEffectCount(int addCount = 1)
		{
			DataContext context = DomainManager.Combat.Context;
			bool flag = DomainManager.Combat.IsSkillEffectExist(base.CombatChar, this.EffectKey);
			if (flag)
			{
				DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, this.EffectKey, (short)addCount, true, false);
			}
			else
			{
				DomainManager.Combat.AddSkillEffect(context, base.CombatChar, this.EffectKey, (short)addCount, this.MaxEffectCount, true);
			}
		}

		// Token: 0x06002C39 RID: 11321 RVA: 0x00206E08 File Offset: 0x00205008
		protected void AddMaxEffectCount(bool autoRemoveOnNoCount = true)
		{
			DataContext context = DomainManager.Combat.Context;
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, this.EffectKey, this.MaxEffectCount, this.MaxEffectCount, autoRemoveOnNoCount);
		}

		// Token: 0x06002C3A RID: 11322 RVA: 0x00206E46 File Offset: 0x00205046
		public void SetIsDirect(DataContext context, bool isDirect)
		{
			this.IsDirect = isDirect;
			this.OnDirectionChanged(context);
		}

		// Token: 0x06002C3B RID: 11323 RVA: 0x00206E5C File Offset: 0x0020505C
		protected bool FiveElementsEquals(sbyte fiveElement)
		{
			return CombatSkillDomain.FiveElementEquals(base.CharacterId, this.SkillTemplateId, fiveElement);
		}

		// Token: 0x06002C3C RID: 11324 RVA: 0x00206E80 File Offset: 0x00205080
		protected bool FiveElementsEquals(int charId, short skillId, sbyte fiveElement)
		{
			return CombatSkillDomain.FiveElementEquals(charId, skillId, fiveElement);
		}

		// Token: 0x06002C3D RID: 11325 RVA: 0x00206E9C File Offset: 0x0020509C
		public bool CombatCharPowerMatchAffectRequire(int index = 0)
		{
			return this.PowerMatchAffectRequire((int)base.CombatChar.GetAttackSkillPower(), index);
		}

		// Token: 0x06002C3E RID: 11326 RVA: 0x00206EC0 File Offset: 0x002050C0
		public bool PowerMatchAffectRequire(int power, int index = 0)
		{
			return this.SkillInstance.PowerMatchAffectRequire(power, index);
		}

		// Token: 0x06002C3F RID: 11327 RVA: 0x00206EE0 File Offset: 0x002050E0
		public int GetAffectRequirePower(int index = 0)
		{
			return this.SkillInstance.GetAffectRequirePower(index);
		}

		// Token: 0x06002C40 RID: 11328 RVA: 0x00206F00 File Offset: 0x00205100
		private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			foreach (byte index in this._affectingIndexes)
			{
				this.ShowSpecialEffectTips(index);
			}
			this._affectingIndexes.Clear();
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
		}

		// Token: 0x06002C41 RID: 11329 RVA: 0x00206F78 File Offset: 0x00205178
		public void ShowSpecialEffectTips(byte index = 0)
		{
			DomainManager.Combat.ShowSpecialEffectTips(base.CharacterId, this.EffectId, index);
		}

		// Token: 0x06002C42 RID: 11330 RVA: 0x00206F93 File Offset: 0x00205193
		public void ShowSpecialEffectTips(bool condition, byte indexTrue, byte indexFalse)
		{
			this.ShowSpecialEffectTips(condition ? indexTrue : indexFalse);
		}

		// Token: 0x06002C43 RID: 11331 RVA: 0x00206FA4 File Offset: 0x002051A4
		public void ShowSpecialEffectTipsByDisplayEvent(byte index = 0)
		{
			DomainManager.Combat.ShowSpecialEffectTipsByDisplayEvent(base.CharacterId, this.EffectId, index);
		}

		// Token: 0x06002C44 RID: 11332 RVA: 0x00206FC0 File Offset: 0x002051C0
		public void ShowSpecialEffectTipsOnceInFrame(byte index = 0)
		{
			if (this._affectingIndexes == null)
			{
				this._affectingIndexes = new List<byte>();
			}
			bool flag = this._affectingIndexes.Contains(index);
			if (!flag)
			{
				this._affectingIndexes.Add(index);
				bool flag2 = this._affectingIndexes.Count > 1;
				if (!flag2)
				{
					Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
				}
			}
		}

		// Token: 0x06002C45 RID: 11333 RVA: 0x00207028 File Offset: 0x00205228
		protected DataUid ParseCombatSkillDataUid(ushort fieldId)
		{
			return new DataUid(8, 29, (ulong)this.SkillKey, (uint)fieldId);
		}

		// Token: 0x06002C46 RID: 11334 RVA: 0x00207050 File Offset: 0x00205250
		protected override int GetSubClassSerializedSize()
		{
			return 3;
		}

		// Token: 0x06002C47 RID: 11335 RVA: 0x00207064 File Offset: 0x00205264
		protected unsafe override int SerializeSubClass(byte* pData)
		{
			*(short*)pData = this.SkillTemplateId;
			byte* pCurrData = pData + 2;
			*pCurrData = (this.IsDirect ? 1 : 0);
			pCurrData++;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06002C48 RID: 11336 RVA: 0x00207098 File Offset: 0x00205298
		protected unsafe override int DeserializeSubClass(byte* pData)
		{
			short skillTemplateId = *(short*)pData;
			this.SkillKey = new CombatSkillKey(base.CharacterId, skillTemplateId);
			byte* pCurrData = pData + 2;
			this.IsDirect = (*pCurrData != 0);
			pCurrData++;
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x06002C49 RID: 11337 RVA: 0x002070DC File Offset: 0x002052DC
		// Note: this type is marked as 'beforefieldinit'.
		static CombatSkillEffectBase()
		{
			Dictionary<short, Func<CombatSkillKey, bool, CombatSkillKey, int>> dictionary = new Dictionary<short, Func<CombatSkillKey, bool, CombatSkillKey, int>>();
			dictionary[342] = new Func<CombatSkillKey, bool, CombatSkillKey, int>(WuDangChunYangQuan.CalcInterruptOdds);
			dictionary[459] = new Func<CombatSkillKey, bool, CombatSkillKey, int>(TaiSuJueShou.CalcInterruptOdds);
			dictionary[595] = new Func<CombatSkillKey, bool, CombatSkillKey, int>(JinNiZhenMoDao.CalcInterruptOdds);
			CombatSkillEffectBase.CalcInterruptOddsFuncDict = dictionary;
		}

		// Token: 0x04000D5F RID: 3423
		public CombatSkillKey SkillKey;

		// Token: 0x04000D61 RID: 3425
		protected bool IsSrcSkillPrepared;

		// Token: 0x04000D62 RID: 3426
		protected bool IsSrcSkillPerformed;

		// Token: 0x04000D63 RID: 3427
		public bool IsLegendaryBookEffect;

		// Token: 0x04000D64 RID: 3428
		private List<byte> _affectingIndexes;

		// Token: 0x04000D65 RID: 3429
		public static readonly Dictionary<short, Func<CombatSkillKey, bool, CombatSkillKey, int>> CalcInterruptOddsFuncDict;
	}
}
