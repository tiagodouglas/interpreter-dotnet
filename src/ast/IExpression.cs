namespace interpreter_dotnet.ast;

internal interface IExpression: INode
{
    void ExpressionNode();
    string TokenLiteral();
    string String();
}
