// Copyright (c) 2014 Silicon Studio Corp. (http://siliconstudio.co.jp)
// This file is distributed under GPL v3. See LICENSE.md for details.
using SiliconStudio.Shaders.Ast;

namespace SiliconStudio.Paradox.Shaders.Parser.Analysis
{
    internal class ExpressionNodeCouple
    {
        public Expression Expression;
        public Node Node;

        public ExpressionNodeCouple() : this(null, null) {}

        public ExpressionNodeCouple(Expression expression, Node node)
        {
            Expression = expression;
            Node = node;
        }
    }
}
