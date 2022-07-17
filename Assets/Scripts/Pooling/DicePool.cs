using System.Collections.Generic;
using System.Linq;

public class DicePool : Pool
{
	public List<Die> ActiveDice => activeItems.Cast<Die>().ToList();
}
