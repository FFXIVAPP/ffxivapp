using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSpec;
using ParseModXIV.Classes;

namespace ParseModTests
{
    class describe_EventParsing : nspec
    {
        void describe_parsing_from_xml()
        {
            EventCode ec1 = null, ec2 = null;
            bool got_ec1=false, got_ec2=false;
            EventParser parser = new EventParser(
@"<?xml version='1.0' standalone='yes'?>
<ChatCodes xmlns:xivapp='http://ffxiv-app.com/schemas/ChatCodes.xsd'>
  <Group name='Group1' type='Attack'>
    <ChatCode Desc='Testing chatcode in group1' id='0001' />
    <ChatCode Desc='Testing second chatcode in group1' id='0002' />
    <Group name='Subgroup1' subject='You' direction='By'>
        <Group name='Subgroup2' direction='On'>
            <ChatCode Desc='Testing a chatcode in a nested subgroup' id='0003' />
        </Group>
        <ChatCode Desc='Testing a chatcode in a subgroup' id='0004' />
    </Group>
  </Group>
  <Group name='Group2'>
    <ChatCode Desc='Testing Group2' id='BEEF' />
  </Group>
</ChatCodes>");
            it["should parse all the nested chatcodes"] = () => parser.EventCodes.Count.should_be(5);
            context["given an initial top level group"] = () =>
            {
                before = () =>
                {
                    got_ec1 = parser.EventCodes.TryGetValue(0x0001, out ec1);
                    got_ec2 = parser.EventCodes.TryGetValue(2, out ec2);
                };
                it["should parse the first code"] = () => got_ec1.should_be_true();
                it["should parse the second code"] = () => got_ec2.should_be_true();
                it["should set the group properly"] = () => (ec1.Group.Name == "Group1" && ec2.Group.Name == "Group1").should_be_true();
                it["should pass on the event type to children"] = () => ec1.Type.should_be(EventType.Attack);
            };
            context["given a subgroup"] = () =>
            {
                before = () =>
                {
                    got_ec1 = parser.EventCodes.TryGetValue(4, out ec1);
                };
                it["Should parse the first code"] = () => got_ec1.should_be_true();
                it["Should set the group properly"] = () => ec1.Group.Name.should_be("Subgroup1");
                it["Should set the group's parent properly"] = () => ec1.Group.Parent.Name.should_be("Group1");
                it["Should inherit the event type from its parent"] = () => ec1.Type.should_be(EventType.Attack);
                it["Should set its own direction"] = () => ec1.Direction.should_be(EventDirection.By);
                it["Should set the subject"] = () => ec1.Subject.should_be(EventSubject.You);
            };
            context["given a nested subgroup"] = () =>
            {
                before = () =>
                {
                    got_ec1 = parser.EventCodes.TryGetValue(3, out ec1);
                };
                it["Should parse the first code"] = () => got_ec1.should_be_true();
                it["Should set the group properly"] = () => ec1.Group.Name.should_be("Subgroup2");
                it["Should set the group's parent properly"] = () => ec1.Group.Parent.Name.should_be("Subgroup1");
                it["Should inherit the type from ancestors"] = () => ec1.Type.should_be(EventType.Attack);
                it["Should inherit the subject from its parent"] = () => ec1.Subject.should_be(EventSubject.You);
                it["Should override the direction"] = () => ec1.Direction.should_be(EventDirection.On);
            };
            context["given a hexadecimal code"] = () =>
            {
                before = () =>
                {
                    got_ec1 = parser.EventCodes.TryGetValue((UInt16)0xBEEF, out ec1);
                };
                it["Should convert the hex string to a UInt16"] = () => got_ec1.should_be_true();
                it["Should set the group properly"] = () => ec1.Group.Name.should_be("Group2");
            };
            context["given a group with no Event* attributes set"] = () =>
            {
                before = () =>
                {
                    got_ec1 = parser.EventCodes.TryGetValue((UInt16)0xBEEF, out ec1);
                };
                it["should have an unknown type"] = () => ec1.Type.should_be(EventType.Unknown);
                it["should have an unknown subject"] = () => ec1.Subject.should_be(EventSubject.Unknown);
                it["should have an unknown direction"] = () => ec1.Direction.should_be(EventDirection.Unknown);
            };
        }

        void describe_event_code_comparer() {
            context["when comparing EventCodes"] = () => {
                context["when the code property is the same"] = () => {
                    it["Should return true if the code property is the same"] = () => new EventCodeComparer().Equals(new EventCode("nothing", 1234, null), new EventCode("somethingElse", 1234, null)).should_be_true();
                };
                context["when the code property is different"] = () => {
                    it["Should return false if the code property is different"] = () => new EventCodeComparer().Equals(new EventCode("nothing", 1234, null), new EventCode("nothing", 5678, null)).should_be_false();                
                };
            };
        }

        void describe_event_group()
        {
            context["when adding new children to an EventGroup"] = () =>
            {
                before = () => {
                    orphan = new EventGroup("No Parent");
                    daddy = new EventGroup("Big Daddy");
                    kid = new EventGroup("The baby", daddy);
                };
                it["should add the child group to the parent's list of children"] = () =>
                {
                    daddy.AddChild(orphan);
                    daddy.Children.Contains(orphan).should_be_true();
                };
                context["when the child does not already have a parent set"] = () =>
                {
                    before = () =>
                    {
                        daddy.AddChild(orphan);
                        kidnapper = new EventGroup("ooh, scary");
                    };
                    it["should set the child's parent node"] = () => orphan.Parent.should_be(daddy);
                };
                context["when the child already has a parent set"] = () =>
                {
                    before = () =>
                    {
                        kidnapper = new EventGroup("ooh, scary");
                        kidnapper.AddChild(kid);
                    };
                    it["should remove the child from the previous parent's children"] = () => daddy.Children.should_not_contain(kid);
                    it["should set the child group's parent to the new one"] = () => kid.Parent.should_be(kidnapper);
                    it["should add the child to the new parent"] = () => kidnapper.Children.should_contain(kid);
                };
            };
            context["when setting an EventGroup's parent"] = () =>
            {
                before = () =>
                {
                    orphan = new EventGroup("No Parent");
                    daddy = new EventGroup("Big Daddy");
                    kid = new EventGroup("The baby", daddy);
                };
                context["when the group does not already have a parent"] = () =>
                {
                    before = () =>
                    {
                        orphan.Parent = daddy;
                    };
                    it["should add the child to the parent node"] = () => daddy.Children.should_contain(orphan);
                };
                context["when the group already has a parent"] = () =>
                {
                    before = () =>
                    {
                        kidnapper = new EventGroup("ooh, scary");
                        kid.Parent = kidnapper;
                    };
                    it["should remove the child from the previous parent's list"] = () => daddy.Children.should_not_contain(kid);
                    it["should add the child to the parent node"] = () => kidnapper.Children.should_contain(kid);
                };
            };
        }

        /* This one generates a really long list of tests, so to avoid clutter and reconfiguring the test runner, commenting it out for normal usage */
        /*
        void describe_event_masks()
        {
            context["given an event code with subject, type, and direction set"] = () =>
            {
                foreach(EventSubject subject in Enum.GetValues(typeof(EventSubject))) {
                    if (subject == EventSubject.Unknown) continue;
                    context[String.Format("when the subject is {0}",subject.ToString())] = () => {
                        before = () => {
                            daddy = new EventGroup { Subject=subject };
                            ec = new EventCode("Testing EventCode", 0x0001, daddy);
                        };
                        it["should mask off the subject"] = () => ((EventSubject)((UInt16)ec.Subject & EventParser.SUBJECT_MASK)).should_be(subject);
                        foreach(EventType type in Enum.GetValues(typeof(EventType))) {
                            if (type == EventType.Unknown) continue;
                            context[String.Format("when the type is {0}", type.ToString())] = () =>
                            {
                                before = () => daddy.Type = type;
                                it["should mask off the type"] = () => ((EventType)((UInt16)ec.Type & EventParser.TYPE_MASK)).should_be(type);
                                foreach (EventDirection direction in Enum.GetValues(typeof(EventDirection)))
                                {
                                    if (direction == EventDirection.Unknown) continue;
                                    context[String.Format("when the direction is {0}", direction.ToString())] = () =>
                                    {
                                        before = () => daddy.Direction = direction;
                                        it["should mask off the direction"] = () => ((EventDirection)((UInt16)ec.Direction & EventParser.DIRECTION_MASK)).should_be(direction);
                                    };
                                }
                            };
                        }
                    };
                }                
            };
        } */
    
        private EventGroup orphan,daddy,kid,kidnapper;
    }
    
}
