using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using ParseModXIV.Stats;
using ParseModXIV.Classes;

namespace ParseModTests
{
    public class NumericStatTest : nspec
    {
        void describe_Stat()
        {
			context["given a numericNumericStat"] = () => {
				before = () => numericStat = new NumericStat("TestNumericStat",0);
				it["should have an initial value of zero"]  = () => numericStat.Value.should_be(0);
				context["after setting the value"] = () => {
					before = () => {
						gotEvent = false;
						numericStatValue = 0;
						oldValue = 12345;
						numericStat.OnValueChanged += delegate(object sender,StatChangedEvent sce) {
							oldValue = Convert.ToDecimal(sce.OldValue);
							numericStatValue = Convert.ToDecimal(sce.NewValue);
							gotEvent = true;
						};
						numericStat.Value = 1;
					};
					it["should return the new value"] = () => numericStat.Value.should_be(1);
					it["should publish an event"] = () =>  gotEvent.should_be_true();
					it["should send the old value in the event args"] = () => oldValue.should_be(0);
					it["should send the new value in the event args"] = () => numericStatValue.should_be(1);
				};
				
			};
        }
				
		void describe_Stat_dependencies() {
			Decimal oldValueCheck=12345,newValueCheck=6789;
			context["given a NumericStat"] = () => {
				before = () => {
					linkedStat = new TotalStat("TestNumericStat With Dependencies");
					gotEvent = false;
					gotDependencyEvent = false;
					linkedStat.OnValueChanged += delegate {
						gotEvent = true;
					};
					linkedStat.OnDependencyValueChanged += delegate(object sender,StatChangedEvent sce) {
						gotDependencyEvent = true;
						oldValueCheck = Convert.ToDecimal(sce.OldValue);
						newValueCheck = Convert.ToDecimal(sce.NewValue);
						numericStat.Value = newValueCheck;
					};
				};
				context["when a dependency is added"] = () => {
					before = () => {
						dependencyStat = new NumericStat("Dependency",1);
						linkedStat.AddDependency(dependencyStat);
					};
					context["and the dependency's value is updated"] = () => {
						before = () => dependencyStat.Value = 2;
						it["should receive the update event"] = () => gotDependencyEvent.should_be_true();
						it["should receive the old value in the event"] = () => oldValueCheck.should_be(1);
						it["should receive the new value in the event"] = () => newValueCheck.should_be(2);
						it["should publish its own value changed event"] = () => gotEvent.should_be_true();
					};
				};
			};
		}
		
		void describe_counter_stat() {
			context["given a counter stat"] = () => {
				before = () => {
					counterStat = new CounterStat("CounterStat");
				};
				context["when calling the increment method with no arguments"] = () => {
					before = () => counterStat.Increment();
					it["should increment method should increment the value by 1"] = () => counterStat.Value.should_be(1);
				};
				context["when calling the increment method with an amount"] = () => {
					before = () => counterStat.Increment(2);
					it["should increment the value by that amount"] = () => counterStat.Value.should_be(2);
				};
			};
		}
			
		void describe_average_stat() {
			context["given an average stat with a dependent numeric stat"] = () => {
				before = () => {
					numericStat = new NumericStat("NumericStat");
					averageStat = new PerSecondAverageStat("AverageStat", numericStat);
				};
				context["when there has been less than one second between updates"] = () => {
					before = () => numericStat.Value = 1;
					it["should not update the value"] = () => averageStat.Value.should_be(0); 
				};
				context["when the dependent stat's value is zero"] = () => {
					before = () => numericStat.Value = 0;
					it["should not update the value"] = () => averageStat.Value.should_be(0);
				};
				context["when there has been more than one second between updates"] = () => {
					before = () => {
						averageStat.FirstEventReceived = DateTime.Now.Subtract(TimeSpan.FromSeconds(10));
						averageStat.LastEventReceived = DateTime.Now;
						numericStat.Value = 100;
					};
					it["should update the average"] = () => averageStat.Value.should_be(10);
				};
			};				
		}
			
		void describe_accuracy_stat() {
			context["given an accuracy stat with dependent hit and miss stats"] = () => {
				before = () => {
					hitStat = new NumericStat("Hits",1);
					missStat = new NumericStat("Misses",1);
					accuracyStat = new AccuracyStat("Accuracy %", hitStat,missStat);
				};
				it["should initialize with the current accuracy from the dependent stats"] = () => accuracyStat.Value.should_be(50);
				context["when a dependent stat value update occurs with a value of zero"] = () => {
					before = () => {
						hitStat.Value = 0;
						missStat.Value = 0;
					};
					it["should have a value of zero"] = () => accuracyStat.Value.should_be(0);
				};
				context["when the hit stat is updated"] = () => {
					before = () => hitStat.Value = 3;
					it["should update the accuracy"] = () => accuracyStat.Value.should_be(75);
				};
				context["when the miss stat is updated"] = () => {
					before = () => missStat.Value = 3;
					it["should update the accuracy"] = () => accuracyStat.Value.should_be(25);
				};
			};
		}
		
		void describe_total_stat() {
			context["given a total stat with a dependent numeric stat"] = () => {
				before = () => {
					dependencyStat = new NumericStat("dependency",0);
					totalStat = new TotalStat("Total", dependencyStat);
				};
				context["when the dependent stat's value changes the first time"] = () => {
					before = () => dependencyStat.Value = 5;
					it["should contain the new value"] = () => totalStat.Value.should_be(5);
					context["when the dependent stat's value changes subsequent times"] = () => {
						before = () => dependencyStat.Value += 10;
						it["should update the total"] = () => totalStat.Value.should_be(15);
						context["when the dependent stat's value decreases"] = () => {
							before = () => dependencyStat.Value -= 5;
							it["should decrease the total"] = () => totalStat.Value.should_be(10);
						};
					};
				};
			};
		}
		
		void describe_max_stat() {
			context["given a max stat with a dependent numeric stat"] = () => {
				before = () => {
					dependencyStat = new NumericStat("dependency",0);
					maxStat = new MaxStat("MaxStat", dependencyStat);
					maxStat.Value = 50;
				};
				context["when the dependency stat's value is updated"] = () => {
					before = () => dependencyStat.Value = 25;
					context["and the value delta is less than the current max"] = () => {
						it["should not update the max"] = () => maxStat.Value.should_be(50);
					};
					context["and the value delta is greater than the current max"] = () => {
						before = () => dependencyStat.Value = 200;
						it["should update the max"] = () => maxStat.Value.should_be(175);
					};
				};
			};
		}

		void describe_min_stat() {
			context["given a min stat with a dependent numeric stat"] = () => {
				before = () => {
					dependencyStat = new NumericStat("dependency",0);
					minStat = new MinStat("MaxStat", dependencyStat);
				};
				context["when the dependency stat's value is updated"] = () => {
					before = () => dependencyStat.Value = 25;
					context["and the min stat has not previously received a value"] = () => {
						it["should update the min with the current delta"] = () => minStat.Value.should_be(25);
					};
					context["and the value delta is less than the current min"] = () => {
						before = () => dependencyStat.Value = 26;
						it["should update the min"] = () => minStat.Value.should_be(1);
					};
				};
			};
		}

        private LinkedStat linkedStat;
		private Boolean gotEvent,gotDependencyEvent;
		private Decimal numericStatValue,oldValue;
		private NumericStat numericStat,hitStat,missStat;
		private NumericStat dependencyStat;
		private CounterStat counterStat;
		private TotalStat totalStat;
		private MaxStat maxStat;
		private MinStat minStat;
		private PerSecondAverageStat averageStat;
		private AccuracyStat accuracyStat;
	}

}
