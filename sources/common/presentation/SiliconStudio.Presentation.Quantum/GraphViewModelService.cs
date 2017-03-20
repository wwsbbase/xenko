﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using System;
using System.Collections.Generic;
using SiliconStudio.Presentation.Quantum.Presenters;

namespace SiliconStudio.Presentation.Quantum
{
    /// <summary>
    /// A class that provides various services to <see cref="GraphViewModel"/> objects
    /// </summary>
    public class GraphViewModelService
    {
        private readonly List<IPropertyNodeUpdater> propertyNodeUpdaters = new List<IPropertyNodeUpdater>();
        private readonly List<INodePresenterUpdater> nodePresenterUpdaters = new List<INodePresenterUpdater>();

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphViewModelService"/> class.
        /// </summary>
        public GraphViewModelService()
        {
            CombinedNodeViewModelFactory = GraphViewModel.DefaultCombinedNodeViewModelFactory;
            NodePresenterFactory = new NodePresenterFactory(AvailableCommands);
        }

        public INodePresenterFactory NodePresenterFactory { get; set; }

        public INodeViewModelFactory NodeViewModelFactory { get; set; }

        public List<INodePresenterCommand> AvailableCommands { get; } = new List<INodePresenterCommand>();

        /// <summary>
        /// Gets or sets the combined node factory.
        /// </summary>
        [Obsolete]
        public CreateCombinedNodeDelegate CombinedNodeViewModelFactory { get; set; }

        /// <summary>
        /// Registers a <see cref="IPropertyNodeUpdater"/> to this service.
        /// </summary>
        /// <param name="propertyNodeUpdater">The node updater to register.</param>
        public void RegisterPropertyNodeUpdater(IPropertyNodeUpdater propertyNodeUpdater)
        {
            propertyNodeUpdaters.Add(propertyNodeUpdater);
        }

        /// <summary>
        /// Unregisters a <see cref="IPropertyNodeUpdater"/> from this service.
        /// </summary>
        /// <param name="propertyNodeUpdater">The node updater to unregister.</param>
        public void UnregisterPropertyNodeUpdater(IPropertyNodeUpdater propertyNodeUpdater)
        {
            propertyNodeUpdaters.Remove(propertyNodeUpdater);
        }

        internal void NotifyNodeInitialized(SingleNodeViewModel node)
        {
            foreach (var updater in propertyNodeUpdaters)
            {
                updater.UpdateNode(node);
            }
        }

        internal void NotifyNodePresenterChanged(INodePresenter node)
        {
            foreach (var updater in nodePresenterUpdaters)
            {
                updater.UpdateNode(node);
            }
        }
    }
}
