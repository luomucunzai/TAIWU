using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation;

[SerializableGameData(NotForDisplayModule = true)]
public abstract class OperationPointBase : OperationGridBase
{
	public int AdditionalPoint;

	private readonly List<sbyte> _effectiveEffectCardTemplateIds = new List<sbyte>();

	private readonly List<Book> _effectiveBookStates = new List<Book>();

	public readonly List<int> UseFriendlyCharacterIds = new List<int>();

	public int BasePoint { get; internal set; }

	public int Point => Math.Max(RawPoint, 0);

	public int RawPoint => BasePoint + AdditionalPoint;

	public IReadOnlyList<sbyte> EffectiveEffectCardTemplateIds => _effectiveEffectCardTemplateIds;

	public IReadOnlyList<Book> EffectiveBookStates => _effectiveBookStates;

	public OperationPointBase()
	{
	}

	public override string Inspect()
	{
		string text = $"{base.Inspect()} Point[{BasePoint} + {AdditionalPoint}]";
		string text2 = string.Empty;
		foreach (Book effectiveBookState in _effectiveBookStates)
		{
			if (!string.IsNullOrEmpty(text2))
			{
				text2 += ", ";
			}
			text2 += LifeSkill.Instance[effectiveBookState.LifeSkill.SkillTemplateId].Name;
		}
		text = text + " Books[" + text2 + "]";
		string text3 = string.Empty;
		foreach (sbyte effectiveEffectCardTemplateId in _effectiveEffectCardTemplateIds)
		{
			if (!string.IsNullOrEmpty(text3))
			{
				text3 += ", ";
			}
			text3 += LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId].Name;
		}
		return text + " Cards[" + text3 + "]";
	}

	public void GetEffectiveEffectCardTemplateIds(List<sbyte> receiver, bool withClear = true)
	{
		if (withClear)
		{
			receiver.Clear();
		}
		receiver.AddRange(_effectiveEffectCardTemplateIds);
	}

	public void GetEffectiveBookStates(List<Book> receiver, bool withClear = true)
	{
		if (withClear)
		{
			receiver.Clear();
		}
		receiver.AddRange(_effectiveBookStates);
	}

	public override int GetSerializedSize()
	{
		int num = 0;
		num += base.GetSerializedSize();
		num += 4;
		num += 4;
		num += 2 + _effectiveEffectCardTemplateIds.Count;
		num += 2;
		foreach (Book effectiveBookState in _effectiveBookStates)
		{
			num += effectiveBookState.GetSerializedSize();
		}
		return num + (2 + 4 * UseFriendlyCharacterIds.Count);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Serialize(ptr);
		*(int*)ptr = BasePoint;
		ptr += 4;
		*(int*)ptr = AdditionalPoint;
		ptr += 4;
		*(ushort*)ptr = (ushort)_effectiveEffectCardTemplateIds.Count;
		ptr += 2;
		int i = 0;
		for (int count = _effectiveEffectCardTemplateIds.Count; i < count; i++)
		{
			*ptr = (byte)_effectiveEffectCardTemplateIds[i];
			ptr++;
		}
		*(ushort*)ptr = (ushort)_effectiveBookStates.Count;
		ptr += 2;
		int j = 0;
		for (int count2 = _effectiveBookStates.Count; j < count2; j++)
		{
			ptr += _effectiveBookStates[j].Serialize(ptr);
		}
		*(ushort*)ptr = (ushort)UseFriendlyCharacterIds.Count;
		ptr += 2;
		int k = 0;
		for (int count3 = UseFriendlyCharacterIds.Count; k < count3; k++)
		{
			*(int*)ptr = UseFriendlyCharacterIds[k];
			ptr += 4;
		}
		return (int)(ptr - pData);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Deserialize(ptr);
		BasePoint = *(int*)ptr;
		ptr += 4;
		AdditionalPoint = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		_effectiveEffectCardTemplateIds.Clear();
		for (int i = 0; i < num; i++)
		{
			_effectiveEffectCardTemplateIds.Add((sbyte)(*ptr));
			ptr++;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		_effectiveBookStates.Clear();
		for (int j = 0; j < num2; j++)
		{
			Book item = default(Book);
			ptr += item.Deserialize(ptr);
			_effectiveBookStates.Add(item);
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		UseFriendlyCharacterIds.Clear();
		for (int k = 0; k < num3; k++)
		{
			UseFriendlyCharacterIds.Add(*(int*)ptr);
			ptr += 4;
		}
		return (int)(ptr - pData);
	}

	public void ChangeBasePoint(int delta)
	{
		BasePoint += delta;
	}

	public OperationPointBase CommitUsedBookStates(IEnumerable<Book> usedBookStates, IEnumerable<(int CharacterId, bool Usable, int BasePoint)> usedCharacterStates, bool withPoint = true)
	{
		if (usedBookStates != null)
		{
			foreach (Book usedBookState in usedBookStates)
			{
				_effectiveBookStates.Add(usedBookState);
				if (withPoint)
				{
					BasePoint += usedBookState.BasePoint;
				}
			}
		}
		if (usedCharacterStates != null)
		{
			foreach (var usedCharacterState in usedCharacterStates)
			{
				UseFriendlyCharacterIds.Add(usedCharacterState.CharacterId);
				if (withPoint)
				{
					BasePoint += usedCharacterState.BasePoint;
				}
			}
		}
		return this;
	}

	public OperationPointBase ClearUsedBookStates(Player player, bool withPoint = true)
	{
		if (withPoint)
		{
			foreach (Book effectiveBookState in EffectiveBookStates)
			{
				BasePoint -= effectiveBookState.BasePoint;
			}
		}
		if (withPoint)
		{
			foreach (var item in UseFriendlyCharacterIds.Select((int id) => player.FriendlyCharacterStates.First(((int CharacterId, bool Usable, int BasePoint) p) => p.CharacterId == id)))
			{
				BasePoint -= item.BasePoint;
			}
		}
		_effectiveBookStates.Clear();
		UseFriendlyCharacterIds.Clear();
		return this;
	}

	public abstract IEnumerable<sbyte> PickEffectiveEffectCards(IEnumerable<sbyte> wantUseEffectCardIds);

	internal void DropEffectiveEffectCards()
	{
		_effectiveEffectCardTemplateIds.Clear();
	}

	internal void RegisterEffectiveEffectCards(sbyte effectCardTemplateId)
	{
		_effectiveEffectCardTemplateIds.Add(effectCardTemplateId);
	}

	public void DropCard(sbyte cardTemplateId)
	{
		_effectiveEffectCardTemplateIds.Remove(cardTemplateId);
	}

	public virtual void ProcessOnGridActiveFixed(Match lifeSkillCombat, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
	{
		Grid grid = lifeSkillCombat.GetGrid(base.GridIndex);
		Player player = lifeSkillCombat.GetPlayer(base.PlayerId);
		bool flag = false;
		foreach (sbyte effectiveEffectCardTemplateId in EffectiveEffectCardTemplateIds)
		{
			ELifeSkillCombatEffectSubEffect subEffect = LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId].SubEffect;
			ELifeSkillCombatEffectSubEffect eLifeSkillCombatEffectSubEffect = subEffect;
			if (eLifeSkillCombatEffectSubEffect == ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndRecycleCardAndExchangeOperation)
			{
				flag = true;
			}
		}
		for (int num = EffectiveEffectCardTemplateIds.Count - 1; num >= 0; num--)
		{
			sbyte index = EffectiveEffectCardTemplateIds[num];
			switch (LifeSkillCombatEffect.Instance[index].SubEffect)
			{
			case ELifeSkillCombatEffectSubEffect.SelfTrapedInCell:
				_effectiveEffectCardTemplateIds.RemoveAt(num);
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		List<sbyte> list = grid.ProcessOnGridActiveFixedRecycle();
		foreach (sbyte item in list)
		{
			_effectiveEffectCardTemplateIds.Remove(item);
		}
		player.RecruitEffectCards(lifeSkillCombat, list);
	}

	protected OperationPointBase(sbyte playerId, int stamp, int gridIndex, int basePoint, IEnumerable<sbyte> wantUseEffectCards)
		: base(playerId, stamp, gridIndex)
	{
		BasePoint = basePoint;
		AdditionalPoint = 0;
	}
}
