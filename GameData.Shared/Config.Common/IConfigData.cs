using System.Collections.Generic;

namespace Config.Common;

public interface IConfigData
{
	IReadOnlyDictionary<string, int> RefNameMap { get; }

	void Init();

	int GetItemId(string refName);

	int AddExtraItem(string identifier, string refName, object configItem);
}
