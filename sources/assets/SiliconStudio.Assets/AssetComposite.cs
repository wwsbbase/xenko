﻿// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.

using System;
using System.Collections.Generic;
using System.Linq;
using SiliconStudio.Assets.Analysis;
using SiliconStudio.Core.Reflection;

namespace SiliconStudio.Assets
{
    /// <summary>
    /// Base class for an asset that supports inheritance by composition.
    /// </summary>
    public abstract class AssetComposite : Asset, IAssetComposite
    {
        /// <summary>
        /// Adds the given <see cref="AssetBase"/> to the <see cref="Asset.BaseParts"/> collection of this asset.
        /// </summary>
        /// <remarks>If the <see cref="Asset.BaseParts"/> collection already contains the argument. this method does nothing.</remarks>
        /// <param name="newBasePart">The base to add to the <see cref="Asset.BaseParts"/> collection.</param>
        [Obsolete("This method will be removed soon")]
        public void AddBasePart(AssetBase newBasePart)
        {
            if (newBasePart == null) throw new ArgumentNullException(nameof(newBasePart));

            if (BaseParts == null)
            {
                BaseParts = new List<AssetBase>();
            }

            if (BaseParts.All(x => x.Id != newBasePart.Id))
            {
                BaseParts.Add(newBasePart);
            }
        }

        public abstract IEnumerable<AssetPart> CollectParts();

        [Obsolete("This method will be removed soon")]
        public abstract void SetPart(Guid id, Guid baseId, Guid basePartInstanceId);

        public abstract bool ContainsPart(Guid id);

        /// <inheritdoc />
        public override void FixupReferences(bool clearMissingReferences = true)
        {
            // Fixup base
            Base?.Asset.FixupReferences(clearMissingReferences);
            // Fixup base parts
            if (BaseParts != null)
            {
                foreach (var basePart in BaseParts)
                {
                    basePart.Asset.FixupReferences(clearMissingReferences);
                }
            }

            var visitor = new AssetCompositePartReferenceCollector();
            visitor.VisitAsset(this);
            var references = visitor.Result;

            // Reverse the list, so that we can still properly update everything
            // (i.e. if we have a[0], a[1], a[1].Test, we have to do it from back to front to be valid at each step)
            // TODO: I don't think this is needed. Find a proper example or remove it.
            references.Reverse();

            foreach (var reference in references)
            {
                var realPart = ResolvePartReference(reference.AssetPart);
                if (realPart != reference.AssetPart)
                {
                    if (realPart != null || clearMissingReferences)
                    {
                        reference.Path.Apply(this, MemberPathAction.ValueSet, realPart);
                    }
                }
            }
        }

        /// <summary>
        /// Resolves the actual target of a reference to a part of this asset that is currently targeting the given <paramref name="referencedObject"/>.
        /// </summary>
        /// <param name="referencedObject">The object currently referenced by the part.</param>
        /// <returns></returns>
        /// <remarks>
        /// The <paramref name="referencedObject"/> can already be the actual target of the reference, but it can also be a proxy object,
        /// a temporary object, or an old version of the actual object. Implementations of this methods are supposed to identify this given object
        /// and retrieve the actual one from the asset itself to return it.
        /// </remarks>
        protected abstract object ResolvePartReference(object referencedObject);
    }
}
