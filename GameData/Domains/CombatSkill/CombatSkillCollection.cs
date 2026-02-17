using System;
using System.Collections;
using System.Collections.Generic;

namespace GameData.Domains.CombatSkill
{
	// Token: 0x02000801 RID: 2049
	public class CombatSkillCollection : IDictionary<CombatSkillKey, CombatSkill>, ICollection<KeyValuePair<CombatSkillKey, CombatSkill>>, IEnumerable<KeyValuePair<CombatSkillKey, CombatSkill>>, IEnumerable
	{
		// Token: 0x06006B55 RID: 27477 RVA: 0x003C2312 File Offset: 0x003C0512
		public CombatSkillCollection(int capacity)
		{
			this.Collection = new Dictionary<int, Dictionary<short, CombatSkill>>(capacity);
		}

		// Token: 0x06006B56 RID: 27478 RVA: 0x003C2328 File Offset: 0x003C0528
		public void RemoveCharStub(int charId)
		{
			this.Collection.Remove(charId);
		}

		// Token: 0x06006B57 RID: 27479 RVA: 0x003C2338 File Offset: 0x003C0538
		public void Add(KeyValuePair<CombatSkillKey, CombatSkill> item)
		{
			this.Add(item.Key, item.Value);
		}

		// Token: 0x06006B58 RID: 27480 RVA: 0x003C2350 File Offset: 0x003C0550
		public bool Contains(KeyValuePair<CombatSkillKey, CombatSkill> item)
		{
			Dictionary<short, CombatSkill> subCollection;
			CombatSkill skill;
			return this.Collection.TryGetValue(item.Key.CharId, out subCollection) && subCollection.TryGetValue(item.Key.SkillTemplateId, out skill) && EqualityComparer<CombatSkill>.Default.Equals(skill, item.Value);
		}

		// Token: 0x06006B59 RID: 27481 RVA: 0x003C23A8 File Offset: 0x003C05A8
		public void CopyTo(KeyValuePair<CombatSkillKey, CombatSkill>[] array, int arrayIndex)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06006B5A RID: 27482 RVA: 0x003C23B0 File Offset: 0x003C05B0
		public bool Remove(KeyValuePair<CombatSkillKey, CombatSkill> item)
		{
			return this.Remove(item.Key);
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06006B5B RID: 27483 RVA: 0x003C23D0 File Offset: 0x003C05D0
		public int Count
		{
			get
			{
				int count = 0;
				foreach (KeyValuePair<int, Dictionary<short, CombatSkill>> entry in this.Collection)
				{
					count += entry.Value.Count;
				}
				return count;
			}
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06006B5C RID: 27484 RVA: 0x003C2438 File Offset: 0x003C0638
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06006B5D RID: 27485 RVA: 0x003C243C File Offset: 0x003C063C
		public void Add(CombatSkillKey key, CombatSkill value)
		{
			Dictionary<short, CombatSkill> subCollection;
			bool flag = !this.Collection.TryGetValue(key.CharId, out subCollection);
			if (flag)
			{
				subCollection = new Dictionary<short, CombatSkill>();
				this.Collection.Add(key.CharId, subCollection);
			}
			subCollection.Add(key.SkillTemplateId, value);
		}

		// Token: 0x17000471 RID: 1137
		public CombatSkill this[CombatSkillKey key]
		{
			get
			{
				return this.Collection[key.CharId][key.SkillTemplateId];
			}
			set
			{
				this.Collection[key.CharId][key.SkillTemplateId] = value;
			}
		}

		// Token: 0x06006B60 RID: 27488 RVA: 0x003C24CC File Offset: 0x003C06CC
		public bool ContainsKey(CombatSkillKey key)
		{
			Dictionary<short, CombatSkill> subCollection;
			return this.Collection.TryGetValue(key.CharId, out subCollection) && subCollection.ContainsKey(key.SkillTemplateId);
		}

		// Token: 0x06006B61 RID: 27489 RVA: 0x003C2504 File Offset: 0x003C0704
		public bool TryGetValue(CombatSkillKey key, out CombatSkill value)
		{
			Dictionary<short, CombatSkill> subCollection;
			bool flag = this.Collection.TryGetValue(key.CharId, out subCollection);
			bool result;
			if (flag)
			{
				result = subCollection.TryGetValue(key.SkillTemplateId, out value);
			}
			else
			{
				value = null;
				result = false;
			}
			return result;
		}

		// Token: 0x06006B62 RID: 27490 RVA: 0x003C2544 File Offset: 0x003C0744
		public bool Remove(CombatSkillKey key)
		{
			Dictionary<short, CombatSkill> subCollection;
			return this.Collection.TryGetValue(key.CharId, out subCollection) && subCollection.Remove(key.SkillTemplateId);
		}

		// Token: 0x06006B63 RID: 27491 RVA: 0x003C257A File Offset: 0x003C077A
		public void Clear()
		{
			this.Collection.Clear();
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06006B64 RID: 27492 RVA: 0x003C2589 File Offset: 0x003C0789
		public ICollection<CombatSkillKey> Keys
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x06006B65 RID: 27493 RVA: 0x003C2590 File Offset: 0x003C0790
		public ICollection<CombatSkill> Values
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06006B66 RID: 27494 RVA: 0x003C2598 File Offset: 0x003C0798
		public CombatSkillCollection.Enumerator GetEnumerator()
		{
			return new CombatSkillCollection.Enumerator(this.Collection);
		}

		// Token: 0x06006B67 RID: 27495 RVA: 0x003C25B8 File Offset: 0x003C07B8
		IEnumerator<KeyValuePair<CombatSkillKey, CombatSkill>> IEnumerable<KeyValuePair<CombatSkillKey, CombatSkill>>.GetEnumerator()
		{
			return new CombatSkillCollection.Enumerator(this.Collection);
		}

		// Token: 0x06006B68 RID: 27496 RVA: 0x003C25DC File Offset: 0x003C07DC
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new CombatSkillCollection.Enumerator(this.Collection);
		}

		// Token: 0x04001D9F RID: 7583
		public readonly Dictionary<int, Dictionary<short, CombatSkill>> Collection;

		// Token: 0x02000BB8 RID: 3000
		public struct Enumerator : IEnumerator<KeyValuePair<CombatSkillKey, CombatSkill>>, IEnumerator, IDisposable, IDictionaryEnumerator
		{
			// Token: 0x06008CF1 RID: 36081 RVA: 0x004F80A3 File Offset: 0x004F62A3
			internal Enumerator(Dictionary<int, Dictionary<short, CombatSkill>> collection)
			{
				this._collection = collection;
				this._enumerator = this._collection.GetEnumerator();
				this._subEnumerator = default(Dictionary<short, CombatSkill>.Enumerator);
				this._subEnumeratorExists = false;
				this._current = default(KeyValuePair<CombatSkillKey, CombatSkill>);
			}

			// Token: 0x06008CF2 RID: 36082 RVA: 0x004F80E0 File Offset: 0x004F62E0
			public bool MoveNext()
			{
				bool flag = this._subEnumeratorExists && this._subEnumerator.MoveNext();
				bool result;
				if (flag)
				{
					KeyValuePair<short, CombatSkill> subCurrent = this._subEnumerator.Current;
					KeyValuePair<int, Dictionary<short, CombatSkill>> keyValuePair = this._enumerator.Current;
					this._current = new KeyValuePair<CombatSkillKey, CombatSkill>(new CombatSkillKey(keyValuePair.Key, subCurrent.Key), subCurrent.Value);
					result = true;
				}
				else
				{
					KeyValuePair<int, Dictionary<short, CombatSkill>> current;
					for (;;)
					{
						bool flag2 = !this._enumerator.MoveNext();
						if (flag2)
						{
							break;
						}
						current = this._enumerator.Current;
						Dictionary<short, CombatSkill> subCollection = current.Value;
						this._subEnumerator = subCollection.GetEnumerator();
						this._subEnumeratorExists = true;
						bool flag3 = !this._subEnumerator.MoveNext();
						if (!flag3)
						{
							goto IL_CB;
						}
					}
					this._current = default(KeyValuePair<CombatSkillKey, CombatSkill>);
					return false;
					IL_CB:
					KeyValuePair<short, CombatSkill> subCurrent2 = this._subEnumerator.Current;
					this._current = new KeyValuePair<CombatSkillKey, CombatSkill>(new CombatSkillKey(current.Key, subCurrent2.Key), subCurrent2.Value);
					result = true;
				}
				return result;
			}

			// Token: 0x17000642 RID: 1602
			// (get) Token: 0x06008CF3 RID: 36083 RVA: 0x004F81F7 File Offset: 0x004F63F7
			public KeyValuePair<CombatSkillKey, CombatSkill> Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x17000643 RID: 1603
			// (get) Token: 0x06008CF4 RID: 36084 RVA: 0x004F81FF File Offset: 0x004F63FF
			object IEnumerator.Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x17000644 RID: 1604
			// (get) Token: 0x06008CF5 RID: 36085 RVA: 0x004F820C File Offset: 0x004F640C
			public object Key
			{
				get
				{
					return this._current.Key;
				}
			}

			// Token: 0x17000645 RID: 1605
			// (get) Token: 0x06008CF6 RID: 36086 RVA: 0x004F821E File Offset: 0x004F641E
			public object Value
			{
				get
				{
					return this._current.Value;
				}
			}

			// Token: 0x17000646 RID: 1606
			// (get) Token: 0x06008CF7 RID: 36087 RVA: 0x004F822B File Offset: 0x004F642B
			public DictionaryEntry Entry
			{
				get
				{
					return new DictionaryEntry(this._current.Key, this._current.Value);
				}
			}

			// Token: 0x06008CF8 RID: 36088 RVA: 0x004F824D File Offset: 0x004F644D
			public void Dispose()
			{
			}

			// Token: 0x06008CF9 RID: 36089 RVA: 0x004F8250 File Offset: 0x004F6450
			void IEnumerator.Reset()
			{
				this._enumerator = this._collection.GetEnumerator();
				this._subEnumerator = default(Dictionary<short, CombatSkill>.Enumerator);
				this._subEnumeratorExists = false;
				this._current = default(KeyValuePair<CombatSkillKey, CombatSkill>);
			}

			// Token: 0x0400326E RID: 12910
			private readonly Dictionary<int, Dictionary<short, CombatSkill>> _collection;

			// Token: 0x0400326F RID: 12911
			private Dictionary<int, Dictionary<short, CombatSkill>>.Enumerator _enumerator;

			// Token: 0x04003270 RID: 12912
			private Dictionary<short, CombatSkill>.Enumerator _subEnumerator;

			// Token: 0x04003271 RID: 12913
			private bool _subEnumeratorExists;

			// Token: 0x04003272 RID: 12914
			private KeyValuePair<CombatSkillKey, CombatSkill> _current;
		}
	}
}
