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

namespace GameData.Domains.SpecialEffect.CombatSkill;

[SerializableGameData(NotForDisplayModule = true)]
public class CombatSkillEffectBase : SpecialEffectBase
{
	public CombatSkillKey SkillKey;

	protected bool IsSrcSkillPrepared;

	protected bool IsSrcSkillPerformed;

	public bool IsLegendaryBookEffect;

	private List<byte> _affectingIndexes;

	public static readonly Dictionary<short, Func<CombatSkillKey, bool, CombatSkillKey, int>> CalcInterruptOddsFuncDict = new Dictionary<short, Func<CombatSkillKey, bool, CombatSkillKey, int>>
	{
		[342] = WuDangChunYangQuan.CalcInterruptOdds,
		[459] = TaiSuJueShou.CalcInterruptOdds,
		[595] = JinNiZhenMoDao.CalcInterruptOdds
	};

	public short SkillTemplateId => SkillKey.SkillTemplateId;

	public bool IsDirect { get; protected set; }

	public GameData.Domains.CombatSkill.CombatSkill SkillInstance => DomainManager.CombatSkill.GetElement_CombatSkills(SkillKey);

	protected CombatSkillData SkillData => DomainManager.Combat.GetCombatSkillData(base.CharacterId, SkillTemplateId);

	protected CombatSkillItem SkillConfig => Config.CombatSkill.Instance[SkillTemplateId];

	protected int DirectEffectId => Config.CombatSkill.Instance[SkillTemplateId].DirectEffectID;

	protected int ReverseEffectId => Config.CombatSkill.Instance[SkillTemplateId].ReverseEffectID;

	public int EffectId => IsDirect ? DirectEffectId : ReverseEffectId;

	public SkillEffectKey EffectKey => new SkillEffectKey(SkillTemplateId, IsDirect);

	protected int EffectCount => DomainManager.Combat.GetSkillEffectCount(base.CombatChar, EffectKey);

	protected short MaxEffectCount => Config.SpecialEffect.Instance[EffectId].MaxEffectCount;

	protected CombatSkillEffectBase()
	{
	}

	protected CombatSkillEffectBase(CombatSkillKey skillKey, int type, sbyte direction = -1)
		: base(skillKey.CharId, type)
	{
		SkillKey = skillKey;
		if (direction < 0)
		{
			direction = SkillInstance.GetDirection();
		}
		IsDirect = direction == 0;
	}

	protected virtual void OnDirectionChanged(DataContext context)
	{
	}

	protected void RemoveSelf(DataContext context)
	{
		DomainManager.SpecialEffect.Remove(context, Id);
	}

	protected void ReduceEffectCount(int removeCount = 1)
	{
		DataContext context = DomainManager.Combat.Context;
		DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, EffectKey, (short)(-removeCount));
	}

	protected void AddEffectCount(int addCount = 1)
	{
		DataContext context = DomainManager.Combat.Context;
		if (DomainManager.Combat.IsSkillEffectExist(base.CombatChar, EffectKey))
		{
			DomainManager.Combat.ChangeSkillEffectCount(context, base.CombatChar, EffectKey, (short)addCount);
		}
		else
		{
			DomainManager.Combat.AddSkillEffect(context, base.CombatChar, EffectKey, (short)addCount, MaxEffectCount, autoRemoveOnNoCount: true);
		}
	}

	protected void AddMaxEffectCount(bool autoRemoveOnNoCount = true)
	{
		DataContext context = DomainManager.Combat.Context;
		DomainManager.Combat.AddSkillEffect(context, base.CombatChar, EffectKey, MaxEffectCount, MaxEffectCount, autoRemoveOnNoCount);
	}

	public void SetIsDirect(DataContext context, bool isDirect)
	{
		IsDirect = isDirect;
		OnDirectionChanged(context);
	}

	protected bool FiveElementsEquals(sbyte fiveElement)
	{
		return CombatSkillDomain.FiveElementEquals(base.CharacterId, SkillTemplateId, fiveElement);
	}

	protected bool FiveElementsEquals(int charId, short skillId, sbyte fiveElement)
	{
		return CombatSkillDomain.FiveElementEquals(charId, skillId, fiveElement);
	}

	public bool CombatCharPowerMatchAffectRequire(int index = 0)
	{
		return PowerMatchAffectRequire(base.CombatChar.GetAttackSkillPower(), index);
	}

	public bool PowerMatchAffectRequire(int power, int index = 0)
	{
		return SkillInstance.PowerMatchAffectRequire(power, index);
	}

	public int GetAffectRequirePower(int index = 0)
	{
		return SkillInstance.GetAffectRequirePower(index);
	}

	private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
	{
		foreach (byte affectingIndex in _affectingIndexes)
		{
			ShowSpecialEffectTips(affectingIndex);
		}
		_affectingIndexes.Clear();
		Events.UnRegisterHandler_CombatStateMachineUpdateEnd(OnCombatStateMachineUpdateEnd);
	}

	public void ShowSpecialEffectTips(byte index = 0)
	{
		DomainManager.Combat.ShowSpecialEffectTips(base.CharacterId, EffectId, index);
	}

	public void ShowSpecialEffectTips(bool condition, byte indexTrue, byte indexFalse)
	{
		ShowSpecialEffectTips(condition ? indexTrue : indexFalse);
	}

	public void ShowSpecialEffectTipsByDisplayEvent(byte index = 0)
	{
		DomainManager.Combat.ShowSpecialEffectTipsByDisplayEvent(base.CharacterId, EffectId, index);
	}

	public void ShowSpecialEffectTipsOnceInFrame(byte index = 0)
	{
		if (_affectingIndexes == null)
		{
			_affectingIndexes = new List<byte>();
		}
		if (!_affectingIndexes.Contains(index))
		{
			_affectingIndexes.Add(index);
			if (_affectingIndexes.Count <= 1)
			{
				Events.RegisterHandler_CombatStateMachineUpdateEnd(OnCombatStateMachineUpdateEnd);
			}
		}
	}

	protected DataUid ParseCombatSkillDataUid(ushort fieldId)
	{
		return new DataUid(8, 29, (ulong)SkillKey, fieldId);
	}

	protected override int GetSubClassSerializedSize()
	{
		return 3;
	}

	protected unsafe override int SerializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = SkillTemplateId;
		ptr += 2;
		*ptr = (IsDirect ? ((byte)1) : ((byte)0));
		ptr++;
		return (int)(ptr - pData);
	}

	protected unsafe override int DeserializeSubClass(byte* pData)
	{
		byte* ptr = pData;
		short skillTemplateId = *(short*)ptr;
		SkillKey = new CombatSkillKey(base.CharacterId, skillTemplateId);
		ptr += 2;
		IsDirect = *ptr != 0;
		ptr++;
		return (int)(ptr - pData);
	}
}
