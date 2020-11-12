using System.Collections.Generic;

using FluidBehaviorTree.Editor.BehaviorTree.Printer.GraphNode;
using FluidBehaviorTree.Runtime.Tasks;
using FluidBehaviorTree.Tests.Editor.Builders;

using NSubstitute;

using NUnit.Framework;

using UnityEngine;

namespace FluidBehaviorTree.Tests.Editor.BehaviorTrees
{
    public class GraphNodeTest
    {
        public class SetPositionMethod
        {
            private IGraphNodePrinter _printer;
            private readonly Vector2 _size = new Vector2(50, 100);

            [SetUp]
            public void BeforeEach()
            {
                _printer = Substitute.For<IGraphNodePrinter>();
            }

            private GraphNode CreateNode(ITask task, GraphNodeOptions options = null)
            {
                if (options == null)
                {
                    options = new GraphNodeOptions();
                }

                options.Size = _size;
                return new GraphNode(task, _printer, options);
            }

            public class DefaultTests : SetPositionMethod
            {
                [Test]
                public void It_should_set_the_position()
                {
                    ITask task = A.TaskStub().Build();
                    GraphNode graphNode = CreateNode(task);

                    graphNode.SetPosition(Vector2.one);

                    Assert.AreEqual(Vector2.one, graphNode.Position);
                }

                [Test]
                public void It_should_offset_two_children_by_half_the_width()
                {
                    ITask task = A.TaskStub()
                                  .SetChildren(new List<ITask>
                                  {
                                      A.TaskStub().Build(),
                                      A.TaskStub().Build()
                                  })
                                  .Build();
                    GraphNode graphNode = CreateNode(task);
                    GraphNode childA = graphNode.Children[0];
                    GraphNode childB = graphNode.Children[1];
                    float shiftAmount = graphNode.Size.x * 0.5f;

                    graphNode.SetPosition(Vector2.zero);

                    Assert.AreEqual(new Vector2(graphNode.Position.x - shiftAmount, graphNode.Size.y), childA.Position);
                    Assert.AreEqual(new Vector2(graphNode.Position.x + shiftAmount, graphNode.Size.y), childB.Position);
                }

                [Test]
                public void It_should_offset_three_children_to_line_up_in_the_middle()
                {
                    ITask task = A.TaskStub()
                                  .SetChildren(new List<ITask>
                                  {
                                      A.TaskStub().Build(),
                                      A.TaskStub().Build(),
                                      A.TaskStub().Build()
                                  })
                                  .Build();
                    GraphNode graphNode = CreateNode(task);
                    GraphNode childA = graphNode.Children[0];
                    GraphNode childB = graphNode.Children[1];
                    GraphNode childC = graphNode.Children[2];
                    float shiftAmount = graphNode.Size.x * 1f;

                    graphNode.SetPosition(Vector2.zero);

                    Assert.AreEqual(new Vector2(graphNode.Position.x - shiftAmount, graphNode.Size.y), childA.Position);
                    Assert.AreEqual(new Vector2(graphNode.Position.x, graphNode.Size.y), childB.Position);
                    Assert.AreEqual(new Vector2(graphNode.Position.x + shiftAmount, graphNode.Size.y), childC.Position);
                }

                [Test]
                public void It_should_offset_four_children_to_line_up_in_the_middle()
                {
                    ITask task = A.TaskStub()
                                  .SetChildren(new List<ITask>
                                  {
                                      A.TaskStub().Build(),
                                      A.TaskStub().Build(),
                                      A.TaskStub().Build(),
                                      A.TaskStub().Build()
                                  })
                                  .Build();
                    GraphNode graphNode = CreateNode(task);
                    GraphNode childA = graphNode.Children[0];
                    GraphNode childB = graphNode.Children[1];
                    GraphNode childC = graphNode.Children[2];
                    GraphNode childD = graphNode.Children[3];
                    float shiftAmount = graphNode.Size.x;

                    graphNode.SetPosition(Vector2.zero);

                    Assert.AreEqual(new Vector2(graphNode.Position.x - shiftAmount * 1.5f, graphNode.Size.y),
                                    childA.Position);
                    Assert.AreEqual(new Vector2(graphNode.Position.x - shiftAmount * 0.5f, graphNode.Size.y),
                                    childB.Position);
                    Assert.AreEqual(new Vector2(graphNode.Position.x + shiftAmount * 0.5f, graphNode.Size.y),
                                    childC.Position);
                    Assert.AreEqual(new Vector2(graphNode.Position.x + shiftAmount * 1.5f, graphNode.Size.y),
                                    childD.Position);
                }
            }

            public class SingleChildTests : SetPositionMethod
            {
                private ITask _task;

                [SetUp]
                public void BeforeEachTest()
                {
                    _task = A.TaskStub()
                             .SetChildren(new List<ITask> {A.TaskStub().Build()})
                             .Build();
                }

                [Test]
                public void It_should_set_the_child_position_directly_below_the_parent()
                {
                    GraphNode graphNode = CreateNode(_task);
                    GraphNode child = graphNode.Children[0];

                    graphNode.SetPosition(Vector2.zero);

                    Assert.AreEqual(new Vector2(0, graphNode.Size.y), child.Position);
                }

                [Test]
                public void It_should_set_the_child_position_below_a_vertical_connector_bottom()
                {
                    GraphNode graphNode = CreateNode(_task, new GraphNodeOptions
                    {
                        VerticalConnectorBottomHeight = 10
                    });
                    GraphNode child = graphNode.Children[0];

                    graphNode.SetPosition(Vector2.zero);

                    Assert.AreEqual(
                        new Vector2(0, graphNode.Size.y + graphNode.VerticalConnectorBottomHeight),
                        child.Position);
                }

                [Test]
                public void It_should_set_the_child_position_below_a_vertical_bottom_divider_plus_horizontal_line()
                {
                    GraphNode graphNode = CreateNode(_task, new GraphNodeOptions
                    {
                        VerticalConnectorBottomHeight = 10,
                        HorizontalConnectorHeight = 1
                    });
                    GraphNode child = graphNode.Children[0];

                    graphNode.SetPosition(Vector2.zero);

                    float expectedY = graphNode.Size.y
                        + graphNode.VerticalConnectorBottomHeight
                        + graphNode.HorizontalConnectorHeight;

                    Assert.AreEqual(new Vector2(0, expectedY), child.Position);
                }

                [Test]
                public void It_should_set_the_child_position_below_a_vertical_connector_top()
                {
                    GraphNode graphNode = CreateNode(_task, new GraphNodeOptions
                    {
                        VerticalConnectorBottomHeight = 10,
                        HorizontalConnectorHeight = 1,
                        VerticalConnectorTopHeight = 3
                    });
                    GraphNode child = graphNode.Children[0];

                    graphNode.SetPosition(Vector2.zero);

                    float expectedY = graphNode.Size.y
                        + graphNode.VerticalConnectorBottomHeight
                        + graphNode.HorizontalConnectorHeight
                        + graphNode.VerticalConnectorTopHeight;

                    Assert.AreEqual(new Vector2(0, expectedY), child.Position);
                }

                [Test]
                public void It_should_make_the_child_inherit_the_container_size()
                {
                    GraphNode graphNode = CreateNode(_task, new GraphNodeOptions
                    {
                        VerticalConnectorBottomHeight = 10,
                        HorizontalConnectorHeight = 1,
                        VerticalConnectorTopHeight = 3
                    });
                    GraphNode child = graphNode.Children[0];

                    Assert.AreEqual(graphNode.ContainerHeight, child.ContainerHeight);
                }
            }
        }
    }
}
