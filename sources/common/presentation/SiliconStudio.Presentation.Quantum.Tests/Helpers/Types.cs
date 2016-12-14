﻿using SiliconStudio.Presentation.Quantum;
using SiliconStudio.Quantum;
using SiliconStudio.Quantum.Contents;
using SiliconStudio.Quantum.References;

namespace SiliconStudio.Presentation.Tests.Helpers
{
    public static class Types
    {
        public class TestPropertiesProvider : IPropertiesProviderViewModel
        {
            private readonly IGraphNode rootNode;

            public TestPropertiesProvider(IGraphNode rootNode)
            {
                this.rootNode = rootNode;
            }
            public bool CanProvidePropertiesViewModel => true;

            public IGraphNode GetRootNode()
            {
                return rootNode;
            }

            public ExpandReferencePolicy ShouldExpandReference(MemberContent member, ObjectReference reference) => ExpandReferencePolicy.Full;

            public bool ShouldConstructMember(MemberContent content, ExpandReferencePolicy expandReferencePolicy) => expandReferencePolicy == ExpandReferencePolicy.Full;
        }

        public class SimpleObject
        {
            public string Name { get; set; }

            public string Nam { get; set; } // To test scenario when Name.StartsWith(Nam) is true
        }

        public class DependentPropertyContainer
        {
            public string Title { get; set; }

            public SimpleObject Instance { get; set; }
        }
    }
}
