using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using ParseModXIV.Classes;
using ParseModXIV.Stats;

namespace ParseModTests
{
    public class StatGroupTest : nspec
    {
        void describe_stat_groups()
        {
            context["given a stat group with no children"] = () =>
			{
				before = () => sg_top = new StatGroup("Top-Level StatGroup");
				context["and a counter stat"] = () =>
				{
					before = () => sg_top.Stats.Add(new CounterStat("My Counter", 1));
					it["should report that the stat exists"] = () => sg_top.Stats.HasStat("My Counter").should_be_true();
					it["should return the value for the stat"] = () => sg_top.Stats.GetStat("My Counter").Value.should_be(1);
				};
			};
			context["given a stat group with nested levels of children"] = () =>
			{
				before = () => {
					sg4 = new StatGroup("3rd level StatGroup");
					sg3 = new StatGroup("2nd level StatGroup with a child", sg4);
					sg2 = new StatGroup("2nd level StatGroup");
					sg_top = new StatGroup("Top-Level StatGroup", sg2, sg3);
				};
				it["should contain two children"] = () => sg_top.Count.should_be(2);
				it["should contain zero stats"] = () => sg_top.Stats.Count.should_be(0);
				it["should find the first 2nd level group"] = () => sg_top.GetGroup("2nd level StatGroup").Name.should_be("2nd level StatGroup");
				it["should find the second 2nd level group"] = () => sg_top.HasGroup("2nd level StatGroup with a child").should_be_true();
				it["should find the nested group"] = () => sg_top.GetGroup("2nd level StatGroup with a child").HasGroup("3rd level StatGroup").should_be_true();
				context["and a nested group contains a stat"] = () => {
					before = () => sg4.Stats.Add(new CounterStat("My Counter",1));
					it["should report that it has the stat"] = () => sg_top.GetGroup("2nd level StatGroup with a child").GetGroup("3rd level StatGroup").Stats.HasStat("My Counter").should_be_true();
					it["should return the value for the stat"] = () => sg_top.GetGroup("2nd level StatGroup with a child").GetGroup("3rd level StatGroup").Stats.GetStat("My Counter").Value.should_be(1);
				};
			};
        }
        private StatGroup sg_top, sg2, sg3, sg4;
    }
}
