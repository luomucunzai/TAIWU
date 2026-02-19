using System;
using System.Collections;
using System.Collections.Generic;

namespace GameData.Domains.CombatSkill;

public class CombatSkillCollection : IDictionary<CombatSkillKey, CombatSkill>, ICollection<KeyValuePair<CombatSkillKey, CombatSkill>>, IEnumerable<KeyValuePair<CombatSkillKey, CombatSkill>>, IEnumerable
{
	public struct Enumerator : IEnumerator<KeyValuePair<CombatSkillKey, CombatSkill>>, IEnumerator, IDisposable, IDictionaryEnumerator
	{
		private readonly Dictionary<int, Dictionary<short, CombatSkill>> _collection;

		private Dictionary<int, Dictionary<short, CombatSkill>>.Enumerator _enumerator;

		private Dictionary<short, CombatSkill>.Enumerator _subEnumerator;

		private bool _subEnumeratorExists;

		private KeyValuePair<CombatSkillKey, CombatSkill> _current;

		public KeyValuePair<CombatSkillKey, CombatSkill> Current => _current;

		object IEnumerator.Current => _current;

		public object Key => _current.Key;

		public object Value => _current.Value;

		public DictionaryEntry Entry => new DictionaryEntry(_current.Key, _current.Value);

		internal Enumerator(Dictionary<int, Dictionary<short, CombatSkill>> collection)
		{
			_collection = collection;
			_enumerator = _collection.GetEnumerator();
			_subEnumerator = default(Dictionary<short, CombatSkill>.Enumerator);
			_subEnumeratorExists = false;
			_current = default(KeyValuePair<CombatSkillKey, CombatSkill>);
		}

		public bool MoveNext()
		{
			if (_subEnumeratorExists && _subEnumerator.MoveNext())
			{
				KeyValuePair<short, CombatSkill> current = _subEnumerator.Current;
				_current = new KeyValuePair<CombatSkillKey, CombatSkill>(new CombatSkillKey(_enumerator.Current.Key, current.Key), current.Value);
				return true;
			}
			KeyValuePair<int, Dictionary<short, CombatSkill>> current2;
			do
			{
				if (!_enumerator.MoveNext())
				{
					_current = default(KeyValuePair<CombatSkillKey, CombatSkill>);
					return false;
				}
				current2 = _enumerator.Current;
				Dictionary<short, CombatSkill> value = current2.Value;
				_subEnumerator = value.GetEnumerator();
				_subEnumeratorExists = true;
			}
			while (!_subEnumerator.MoveNext());
			KeyValuePair<short, CombatSkill> current3 = _subEnumerator.Current;
			_current = new KeyValuePair<CombatSkillKey, CombatSkill>(new CombatSkillKey(current2.Key, current3.Key), current3.Value);
			return true;
		}

		public void Dispose()
		{
		}

		void IEnumerator.Reset()
		{
			_enumerator = _collection.GetEnumerator();
			_subEnumerator = default(Dictionary<short, CombatSkill>.Enumerator);
			_subEnumeratorExists = false;
			_current = default(KeyValuePair<CombatSkillKey, CombatSkill>);
		}
	}

	public readonly Dictionary<int, Dictionary<short, CombatSkill>> Collection;

	public int Count
	{
		get
		{
			int num = 0;
			foreach (KeyValuePair<int, Dictionary<short, CombatSkill>> item in Collection)
			{
				num += item.Value.Count;
			}
			return num;
		}
	}

	public bool IsReadOnly => false;

	public CombatSkill this[CombatSkillKey key]
	{
		get
		{
			return Collection[key.CharId][key.SkillTemplateId];
		}
		set
		{
			Collection[key.CharId][key.SkillTemplateId] = value;
		}
	}

	public ICollection<CombatSkillKey> Keys
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public ICollection<CombatSkill> Values
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public CombatSkillCollection(int capacity)
	{
		Collection = new Dictionary<int, Dictionary<short, CombatSkill>>(capacity);
	}

	public void RemoveCharStub(int charId)
	{
		Collection.Remove(charId);
	}

	public void Add(KeyValuePair<CombatSkillKey, CombatSkill> item)
	{
		Add(item.Key, item.Value);
	}

	public bool Contains(KeyValuePair<CombatSkillKey, CombatSkill> item)
	{
		Dictionary<short, CombatSkill> value;
		CombatSkill value2;
		return Collection.TryGetValue(item.Key.CharId, out value) && value.TryGetValue(item.Key.SkillTemplateId, out value2) && EqualityComparer<CombatSkill>.Default.Equals(value2, item.Value);
	}

	public void CopyTo(KeyValuePair<CombatSkillKey, CombatSkill>[] array, int arrayIndex)
	{
		throw new NotImplementedException();
	}

	public bool Remove(KeyValuePair<CombatSkillKey, CombatSkill> item)
	{
		return Remove(item.Key);
	}

	public void Add(CombatSkillKey key, CombatSkill value)
	{
		if (!Collection.TryGetValue(key.CharId, out var value2))
		{
			value2 = new Dictionary<short, CombatSkill>();
			Collection.Add(key.CharId, value2);
		}
		value2.Add(key.SkillTemplateId, value);
	}

	public bool ContainsKey(CombatSkillKey key)
	{
		Dictionary<short, CombatSkill> value;
		return Collection.TryGetValue(key.CharId, out value) && value.ContainsKey(key.SkillTemplateId);
	}

	public bool TryGetValue(CombatSkillKey key, out CombatSkill value)
	{
		if (Collection.TryGetValue(key.CharId, out var value2))
		{
			return value2.TryGetValue(key.SkillTemplateId, out value);
		}
		value = null;
		return false;
	}

	public bool Remove(CombatSkillKey key)
	{
		Dictionary<short, CombatSkill> value;
		return Collection.TryGetValue(key.CharId, out value) && value.Remove(key.SkillTemplateId);
	}

	public void Clear()
	{
		Collection.Clear();
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(Collection);
	}

	IEnumerator<KeyValuePair<CombatSkillKey, CombatSkill>> IEnumerable<KeyValuePair<CombatSkillKey, CombatSkill>>.GetEnumerator()
	{
		return new Enumerator(Collection);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return new Enumerator(Collection);
	}
}
