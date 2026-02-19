using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

namespace HarmonyLib;

public class CodeMatch : CodeInstruction
{
	public string name;

	public List<OpCode> opcodes = new List<OpCode>();

	public List<object> operands = new List<object>();

	public List<int> jumpsFrom = new List<int>();

	public List<int> jumpsTo = new List<int>();

	public Func<CodeInstruction, bool> predicate;

	internal CodeMatch Set(object operand, string name)
	{
		if (base.operand == null)
		{
			base.operand = operand;
		}
		if (this.name == null)
		{
			this.name = name;
		}
		return this;
	}

	public CodeMatch(OpCode? opcode = null, object operand = null, string name = null)
	{
		if (opcode.HasValue)
		{
			OpCode item = (base.opcode = opcode.GetValueOrDefault());
			opcodes.Add(item);
		}
		if (operand != null)
		{
			operands.Add(operand);
			base.operand = operand;
		}
		this.name = name;
	}

	public CodeMatch(CodeInstruction instruction, string name = null)
		: this(instruction.opcode, instruction.operand, name)
	{
	}

	public CodeMatch(Func<CodeInstruction, bool> predicate, string name = null)
	{
		this.predicate = predicate;
		this.name = name;
	}

	internal bool Matches(List<CodeInstruction> codes, CodeInstruction instruction)
	{
		if (predicate != null)
		{
			return predicate(instruction);
		}
		if (opcodes.Count > 0 && !opcodes.Contains(instruction.opcode))
		{
			return false;
		}
		if (opcode.Size != 0 && opcode.Value != instruction.opcode.Value)
		{
			return false;
		}
		if (operands.Count > 0 && !operands.Contains(instruction.operand))
		{
			return false;
		}
		if (operand != null && !operand.Equals(instruction.operand))
		{
			return false;
		}
		if (labels.Count > 0 && !labels.Intersect(instruction.labels).Any())
		{
			return false;
		}
		if (blocks.Count > 0 && !blocks.Intersect(instruction.blocks).Any())
		{
			return false;
		}
		if (jumpsFrom.Count > 0 && !jumpsFrom.Select((int index) => codes[index].operand).OfType<Label>().Intersect(instruction.labels)
			.Any())
		{
			return false;
		}
		if (jumpsTo.Count > 0)
		{
			object obj = instruction.operand;
			if (obj == null || obj.GetType() != typeof(Label))
			{
				return false;
			}
			Label label = (Label)obj;
			IEnumerable<int> second = from idx in Enumerable.Range(0, codes.Count)
				where codes[idx].labels.Contains(label)
				select idx;
			if (!jumpsTo.Intersect(second).Any())
			{
				return false;
			}
		}
		return true;
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (name != null)
		{
			stringBuilder.Append(name + ": ");
		}
		if (opcodes.Count > 0)
		{
			stringBuilder.Append("opcodes=" + (from o in opcodes.Union(new OpCode[1] { opcode })
				where o.Size != 0
				select o).Join() + " ");
		}
		if (operands.Count > 0)
		{
			stringBuilder.Append("operands=" + (from o in operands.Union(new object[1] { operand })
				where o != null
				select o).Join() + " ");
		}
		if (labels.Count > 0)
		{
			stringBuilder.Append("labels=" + labels.Join() + " ");
		}
		if (blocks.Count > 0)
		{
			stringBuilder.Append("blocks=" + blocks.Join() + " ");
		}
		if (jumpsFrom.Count > 0)
		{
			stringBuilder.Append("jumpsFrom=" + jumpsFrom.Join() + " ");
		}
		if (jumpsTo.Count > 0)
		{
			stringBuilder.Append("jumpsTo=" + jumpsTo.Join() + " ");
		}
		if (predicate != null)
		{
			stringBuilder.Append("predicate=yes ");
		}
		if (stringBuilder.Length == 0)
		{
			return base.ToString();
		}
		return "[" + stringBuilder.ToString().Trim() + "]";
	}

	public static implicit operator CodeMatch(OpCode opcode)
	{
		return new CodeMatch(opcode);
	}
}
