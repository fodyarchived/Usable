using System;
using Custom.Decompiler.ILAst;

public abstract class ILNodeVisitor
{
    protected ILNodeVisitor()
    {
    }

	public virtual ILNode Visit(ILNode node)
    {
		if (node == null)
            return node;

		var block = node as ILBlock;
		if (block != null)
			return VisitBlock(block);

		var basicBlock = node as ILBasicBlock;
		if (basicBlock != null)
			return VisitBasicBlock(basicBlock);

		var label = node as ILLabel;
		if (label != null)
			return VisitLabel(label);

		var tryCatchBlock = node as ILTryCatchBlock;
		if (tryCatchBlock != null)
			return VisitTryCatchBlock(tryCatchBlock);

		var expression = node as ILExpression;
		if (expression != null)
			return VisitExpression(expression);

		var whileLoop = node as ILWhileLoop;
		if (whileLoop != null)
			return VisitWhileLoop(whileLoop);

		var condition = node as ILCondition;
		if (condition != null)
			return VisitCondition(condition);

		var switchStatement = node as ILSwitch;
		if (switchStatement != null)
			return VisitSwitch(switchStatement);

		var fixedStatement = node as ILFixedStatement;
		if (fixedStatement != null)
			return VisitFixedStatement(fixedStatement);

		throw new NotSupportedException();
	}

	protected virtual ILBlock VisitBlock(ILBlock block)
	{
		foreach (var child in block.GetChildren())
            Visit(child);
        return block;
	}

	protected virtual ILBasicBlock VisitBasicBlock(ILBasicBlock basicBlock)
	{
		foreach (var child in basicBlock.GetChildren())
            Visit(child);
        return basicBlock;
	}

	protected virtual ILLabel VisitLabel(ILLabel label)
	{
		foreach (var child in label.GetChildren())
            Visit(child);
        return label;
	}

	protected virtual ILTryCatchBlock VisitTryCatchBlock(ILTryCatchBlock tryCatchBlock)
	{
		foreach (var child in tryCatchBlock.GetChildren())
            Visit(child);
        return tryCatchBlock;
	}

	protected virtual ILExpression VisitExpression(ILExpression expression)
	{
		foreach (var child in expression.GetChildren())
            Visit(child);
        return expression;
	}

	protected virtual ILWhileLoop VisitWhileLoop(ILWhileLoop whileLoop)
	{
		foreach (var child in whileLoop.GetChildren())
            Visit(child);
        return whileLoop;
	}

	protected virtual ILCondition VisitCondition(ILCondition condition)
	{
		foreach (var child in condition.GetChildren())
            Visit(child);
        return condition;
	}

	protected virtual ILSwitch VisitSwitch(ILSwitch switchStatement)
	{
		foreach (var child in switchStatement.GetChildren())
            Visit(child);
        return switchStatement;
	}

	protected virtual ILFixedStatement VisitFixedStatement(ILFixedStatement fixedStatement)
	{
		foreach (var child in fixedStatement.GetChildren())
            Visit(child);
        return fixedStatement;
	}

}
